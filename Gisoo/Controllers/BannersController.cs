using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Gisoo.Controllers
{
    [Authorize]

    public class BannersController : Controller
    {
        private readonly Context _context;
        private IBanner _Banner;
        private readonly IHostingEnvironment environment;

        public BannersController(Context context,IBanner Banner, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Banner = Banner;
        }

        // GET: Banners
        public IActionResult Index(int page = 1, string filterLink = "",bool isSuccess=true)
        {
            var model = _Banner.GetBanners(page, filterLink);
            if (isSuccess == false)
                ViewData["IsError"]= "خطایی رخ داده است.";
            return View(model);
        }

        // GET: Banners/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Banner = _Banner.FindById(id);
            if (Banner == null)
            {
                return NotFound();
            }

            return View(Banner);
        }

        // GET: Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,image,movie,link,imageUrl,movieUrl,isForWebSite,titleForWebSite")] Banner Banner)
        {
            if (ModelState.IsValid)
            {
              int productId = _Banner.AddBannerFromAdmin(Banner);

                return RedirectToAction(nameof(Index));
            }
            return View(Banner);
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Banner = _Banner.FindById(id);
            
            if (Banner == null)
            {
                return NotFound();
            }
            return View(Banner);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,image,movie,link,imageUrl,movieUrl,isForWebSite,titleForWebSite")] Banner Banner)
        {
            if (id != Banner.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 _Banner.EditBanner(Banner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(Banner.id))
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
            return View(Banner);
        }

        // GET: Banners/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var Banner = _Banner.FindById(id);
            if (Banner == null)
            {
                return NotFound();
            }

            return View(Banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if(await _Banner.Delete(id)==true)
            return RedirectToAction(nameof(Index));
            else
            return RedirectToAction(nameof(Index), new {isSuccess=false });

        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.id == id);
        }
    }
}
