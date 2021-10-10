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

namespace Gisoo.Controllers
{
    [Authorize]

    public class FactorController : Controller
    {
        private readonly Context _context;
        private IFactor _factor;

        public FactorController(Context context, IFactor factor)
        {
            _context = context;
            _factor = factor;
        }

        // GET: Information
        public IActionResult Index(int page = 1, string filterTitle = "")
        {
            var model = _factor.GetFactors(page);
            return View(model);
        }
        public IActionResult HistoryItems(int factorId)
        {
            var model = _context.FactorItems.Include(x=>x.Product).Include(x=>x.Factor).Where(x => x.FactorId == factorId).ToList();

            return View(model);
        }
        public IActionResult AdvisorDetail(int factorId)
        {
            var model = _context.Factors.Include(x=>x.notice).FirstOrDefault(x=>x.id==factorId);
            return View(model);
        }
        
    }
}
