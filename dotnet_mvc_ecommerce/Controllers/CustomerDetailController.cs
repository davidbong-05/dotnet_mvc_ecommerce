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
    public class CustomerDetailController : Controller
    {
        private readonly dotnet_mvc_ecommerceContext _context;

        public CustomerDetailController(dotnet_mvc_ecommerceContext context)
        {
            _context = context;
        }

        // GET: CustomerDetail
        public async Task<IActionResult> Index()
        {
            var dotnet_mvc_ecommerceContext = _context.CustomerDetail.Include(c => c.User);
            return View(await dotnet_mvc_ecommerceContext.ToListAsync());
        }

        // GET: CustomerDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerDetail == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetail
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerDetail == null)
            {
                return NotFound();
            }

            return View(customerDetail);
        }

        // GET: CustomerDetail/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: CustomerDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreditCard,UserId")] CustomerDetail customerDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerDetail.UserId);
            return View(customerDetail);
        }

        // GET: CustomerDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerDetail == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetail.FindAsync(id);
            if (customerDetail == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerDetail.UserId);
            return View(customerDetail);
        }

        // POST: CustomerDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreditCard,UserId")] CustomerDetail customerDetail)
        {
            if (id != customerDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerDetailExists(customerDetail.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerDetail.UserId);
            return View(customerDetail);
        }

        // GET: CustomerDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerDetail == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetail
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerDetail == null)
            {
                return NotFound();
            }

            return View(customerDetail);
        }

        // POST: CustomerDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerDetail == null)
            {
                return Problem("Entity set 'dotnet_mvc_ecommerceContext.CustomerDetail'  is null.");
            }
            var customerDetail = await _context.CustomerDetail.FindAsync(id);
            if (customerDetail != null)
            {
                _context.CustomerDetail.Remove(customerDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerDetailExists(int id)
        {
          return (_context.CustomerDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
