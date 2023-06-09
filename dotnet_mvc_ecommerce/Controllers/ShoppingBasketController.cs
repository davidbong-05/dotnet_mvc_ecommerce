using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_mvc_ecommerce.Data;
using dotnet_mvc_ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_mvc_ecommerce.Controllers
{
    [Authorize]
    public class ShoppingBasketController : Controller
    {
        private readonly dotnet_mvc_ecommerceContext _context;
        private readonly UserManager<User> _userManager;

        public ShoppingBasketController(dotnet_mvc_ecommerceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShoppingBasket
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var dotnet_mvc_ecommerceContext = _context.ShoppingBasket
                .Include(s => s.User)
                .Include(s => s.ShoppingBasket_Products)
                    .ThenInclude(sp => sp.Product)
                .Where(s => s.UserId == user.Id);
            return View(await dotnet_mvc_ecommerceContext.ToListAsync());
        }

        // GET: ShoppingBasket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingBasket == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBasket
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            return View(shoppingBasket);
        }

        // GET: ShoppingBasket/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ShoppingBasket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] ShoppingBasket shoppingBasket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBasket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingBasket.UserId);
            return View(shoppingBasket);
        }

        // GET: ShoppingBasket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingBasket == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBasket.FindAsync(id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingBasket.UserId);
            return View(shoppingBasket);
        }

        // POST: ShoppingBasket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId")] ShoppingBasket shoppingBasket)
        {
            if (id != shoppingBasket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingBasket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingBasketExists(shoppingBasket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shoppingBasket.UserId);
            return View(shoppingBasket);
        }

        // GET: ShoppingBasket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingBasket == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBasket
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            return View(shoppingBasket);
        }

        // POST: ShoppingBasket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingBasket == null)
            {
                return Problem("Entity set 'dotnet_mvc_ecommerceContext.ShoppingBasket'  is null.");
            }
            var shoppingBasket = await _context.ShoppingBasket.FindAsync(id);
            if (shoppingBasket != null)
            {
                _context.ShoppingBasket.Remove(shoppingBasket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBasketExists(int id)
        {
          return (_context.ShoppingBasket?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost, ActionName("remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var product = await _context.Product.FindAsync(id);
            var shoppingBasket = await _context.ShoppingBasket.FirstOrDefaultAsync(m => m.UserId == user.Id);
            var quantity = 1;
            if (product != null)
            {
                var shoppingBasketProduct = await _context.ShoppingBasket_Product.FirstOrDefaultAsync(p =>
                    p.ShoppingBasketId == shoppingBasket.Id && p.ProductId == product.Id);

                if (shoppingBasketProduct.Quantity >= 0)
                {
                    shoppingBasketProduct.Quantity -= quantity;
                }

                if (shoppingBasketProduct.Quantity == 0)
                {
                    _context.ShoppingBasket_Product.Remove(shoppingBasketProduct);
                }

                await _context.SaveChangesAsync();
                TempData["success"] = "Product added to basket";
            }



            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> add(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var product = await _context.Product.FindAsync(id);
            var shoppingBasket = await _context.ShoppingBasket.FirstOrDefaultAsync(m => m.UserId == user.Id);
            var quantity = 1;

            if (product != null)
            {
                var shoppingBasketProduct = await _context.ShoppingBasket_Product.FirstOrDefaultAsync(p =>
                    p.ShoppingBasketId == shoppingBasket.Id && p.ProductId == product.Id);

                if (shoppingBasketProduct.Quantity >= 1)
                {
                    shoppingBasketProduct.Quantity += quantity;
                }

                await _context.SaveChangesAsync();
                TempData["success"] = "Product added to basket";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(string password, string[] ProductNames, int[] Quantities, int ShoppingBasketId)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = await _userManager.FindByEmailAsync(user.Email);

            if (password != null && await _userManager.CheckPasswordAsync(username, password))
            {
                var orderDetails = new List<string>();

            for (int i = 0; i < ProductNames.Length; i++)
            {
                var orderDetail = $"{ProductNames[i]} - {Quantities[i]}";
                orderDetails.Add(orderDetail);
            }

            var combinedOrderDetails = string.Join(", ", orderDetails);

            var order = new Order
            {
                OrderDetails = combinedOrderDetails,
                UserId = user.Id
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

             
            var shoppingBasket = await _context.ShoppingBasket.FindAsync(ShoppingBasketId);
            if (shoppingBasket != null)
            {
                _context.ShoppingBasket.Remove(shoppingBasket);
                await _context.SaveChangesAsync();
            }

                TempData["success"] = "Order added to basket";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Order failed";
            return RedirectToAction(nameof(Index));

        }


    }
}
