using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Gisoo.Controllers
{
    [Authorize]

    public class AllPriceController : Controller
    {
        private readonly Context _context;

        public AllPriceController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.AllPrices.FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(AllPrice allPrice)
        {
            ModelState.Remove("id");
            if (ModelState.IsValid)
            {
                if (allPrice.id == 0)
                {
                    _context.Add(allPrice);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    try
                    {
                        _context.Update(allPrice);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));


        }
    }
}