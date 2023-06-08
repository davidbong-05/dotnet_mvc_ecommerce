using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_mvc_ecommerce.Data;
using dotnet_mvc_ecommerce.Models;

namespace dotnet_mvc_ecommerce.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private readonly dotnet_mvc_ecommerceContext _context;

        public ShoppingBasketController(dotnet_mvc_ecommerceContext context)
        {
            _context = context;
        }

        // GET: ShoppingBasket
        public async Task<IActionResult> Index()
        {
            var dotnet_mvc_ecommerceContext = _context.ShoppingBasket.Include(s => s.User);
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
    }
}
