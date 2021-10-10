using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmsIrRestfulNetCore;

namespace Gisoo.Controllers
{
    public class ProductController : Controller
    {
        private readonly Context _context;
        private IProduct _Product;
        private IVisit _IVisit;
        private readonly IHostingEnvironment environment;

        public ProductController(Context context, IProduct Product, IVisit visit, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Product = Product;
            _IVisit = visit;

        }

        // GET: Products
        public IActionResult Index(int page = 1, string filterTitle = "")
        {
            var model = _Product.GetProducts(page, filterTitle);
            return View(model);
        }


        [HttpPost]
        public ActionResult InActive(NoticeViewModel page)
        {
            try
            {
                var Product = _context.Products.Include(x => x.user).FirstOrDefault(x => x.id == page.id);
                Product.adminConfirmStatus = page.adminConfirmStatus;
                Product.notConfirmDescription = page.notConfirmDescription;
                if (page.adminConfirmStatus == EnumStatus.NotAccept)
                {
                    try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "NoticeTitle", ParameterValue = Product.title});
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue = page.notConfirmDescription});
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(Product.user.cellphone), 41387,Parameter);
                    }
                    catch (Exception ex)
                    {
                return Json("Failed");
                        
                    }
                }
                else
                {
                    try
                    {
                        
                        SendSms.CallSmSMethod(Convert.ToInt64(Product.user.cellphone), 41386,"NoticeTitle",Product.title);
                    }
                    catch (Exception ex)
                    {
                return Json("Failed");
                        
                    }
                }
                _context.SaveChanges();
                return Json("Done");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }
        [HttpGet]
        public JsonResult GetProduct(int id)
        {
            var Product = new Product();

            if (id != 0)
            {
                Product = _context.Products.Find(id);

                return Json(Product);
            }
            else
            {
                return Json(Product);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Products
                .FirstOrDefaultAsync(m => m.id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Product = await _context.Products.FindAsync(id);
            var ProductImage = _context.ProductImages.Where(x => x.productId == id).ToList();
            if (ProductImage != null)
            {
                foreach (var item in ProductImage)
                {
                    string deletePath = environment.WebRootPath + item.url;

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }

            }
            var factor = _context.Factors.Where(x => x.productId == id);
            var reserves = _context.Reserves.Where(x => x.productId == id);
            _context.Reserves.RemoveRange(reserves);
            _context.Factors.RemoveRange(factor);

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Product = await _context.Products.Include(x => x.user).Include(x=>x.ProductImages).FirstOrDefaultAsync(x => x.id == id);
            ProductViewModelAdmin ProductViewModelAdmin = new ProductViewModelAdmin();
            ProductViewModelAdmin.Product = Product;
            if (Product == null)
            {
                return NotFound();
            }
            return View(ProductViewModelAdmin);
        }

         public async Task<IActionResult> Details(int id)
        {
            
            ProductViewModelAdmin ProductViewModelAdmin =   _Product.GetForDetailPage(id);
            return View(ProductViewModelAdmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModelAdmin ProductViewModelAdmin, IFormFile[] file)
        {
            if (ModelState.IsValid)
            {
                try
                {
            _Product.CreateOrUpdateFromAdmin(ProductViewModelAdmin, file);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Edit", new { id = ProductViewModelAdmin.Product.id });

        }
        public IActionResult Create()
        {
            ProductViewModelAdmin ProductViewModelAdmin = new ProductViewModelAdmin();
            ProductViewModelAdmin.Users = _context.Users.Where(x => x.userStatus == true).ToList();
            return View(ProductViewModelAdmin);
        }
    }
}