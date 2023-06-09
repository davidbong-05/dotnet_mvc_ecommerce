using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_mvc_ecommerce.Data;
using dotnet_mvc_ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography.Xml;
using dotnet_mvc_ecommerce.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http;

namespace dotnet_mvc_ecommerce.Controllers
{
    [Authorize]
    public class CustomerDetailController : Controller
    {
        private readonly dotnet_mvc_ecommerceContext _context;
        private readonly UserManager<User> _userManager;

        public CustomerDetailController(dotnet_mvc_ecommerceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CustomerDetail
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(this.User);
            var dotnet_mvc_ecommerceContext = _context.CustomerDetail.Include(c => c.User).Where(c => c.UserId == user.Id);

            var customerDetails = await dotnet_mvc_ecommerceContext.ToListAsync();
            string decryptedCreditCard;
            byte[] encryptedCreaditCard;
            foreach (var item in customerDetails) {

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes("mysmallkey123456");
                    aes.IV = Encoding.UTF8.GetBytes(user.Id.Substring(0, 16));

                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    encryptedCreaditCard = Convert.FromBase64String(item.CreditCard);
                    using (MemoryStream memoryStream = new MemoryStream(encryptedCreaditCard))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                decryptedCreditCard = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                item.CreditCard = decryptedCreditCard;
            }
            return View(customerDetails);
        }

        // GET: CustomerDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id == null || _context.CustomerDetail == null)
            {
                return NotFound();
            }

            var customerDetail = await _context.CustomerDetail
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            CustomerDetail decryptedCustomerDetail;

            if (customerDetail == null)
            {
                return NotFound();
            }
            if(customerDetail.User.Id!= user.Id)
            {
                return Forbid();
            }
            else
            {
                string decryptedCreditCard;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes("mysmallkey123456");
                    aes.IV = Encoding.UTF8.GetBytes(user.Id.Substring(0, 16));

                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    byte[] encryptedCreaditCard = Convert.FromBase64String(customerDetail.CreditCard);
                    using (MemoryStream memoryStream = new MemoryStream(encryptedCreaditCard))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                decryptedCreditCard = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                decryptedCustomerDetail = new CustomerDetail(decryptedCreditCard, customerDetail.User);
            }
            return View(decryptedCustomerDetail);
        }

        // GET: CustomerDetail/Create
        public async Task<IActionResult> Create()
        {         
            return View();
        }

        // POST: CustomerDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string CreditCard)
        {

            byte[] encryptedCreditCard;
            var user = await _userManager.GetUserAsync(User);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("mysmallkey123456");
                aes.IV = Encoding.UTF8.GetBytes(user.Id.Substring(0, 16));

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(CreditCard);

                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();

                        encryptedCreditCard = memoryStream.ToArray();
                    }
                }
            }

            var customerDetail = new CustomerDetail(Convert.ToBase64String(encryptedCreditCard), user);

            if (ModelState.IsValid)
            {
                _context.Add(customerDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Customer"))
            {
                var filteredUsers = _context.Users
                .Where(u => u.Id == user.Id)
                .ToList();

                ViewData["UserId"] = new SelectList(filteredUsers, "Id", "UserName");
            }

            if (User.IsInRole("Shop Assistant"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            }
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
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Customer"))
            {
                var filteredUsers = _context.Users
                .Where(u => u.Id == user.Id)
                .ToList();

                ViewData["UserId"] = new SelectList(filteredUsers, "Id", "UserName");
            }

            if (User.IsInRole("Shop Assistant"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            }
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
