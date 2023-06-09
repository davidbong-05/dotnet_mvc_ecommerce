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
    public class ShoppingBasket_ProductController : Controller
    {
        private readonly dotnet_mvc_ecommerceContext _context;
        private readonly UserManager<User> _userManager;

        public ShoppingBasket_ProductController(dotnet_mvc_ecommerceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShoppingBasket_Product
        public async Task<IActionResult> Index()
        {
            var dotnet_mvc_ecommerceContext = _context.ShoppingBasket_Product.Include(s => s.Product).Include(s => s.ShoppingBasket);
            return View(await dotnet_mvc_ecommerceContext.ToListAsync());
        }

        [HttpPost, ActionName("remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var product = await _context.Product.FindAsync(id);
            var shoppingBasket = await _context.ShoppingBasket.FirstOrDefaultAsync(m => m.UserId == user.Id);
            var quantity = 1;



            //}
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



            //}
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
    }
}
