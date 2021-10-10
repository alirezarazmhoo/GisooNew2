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

    public class LineTypeController : Controller
    {
        private readonly Context _context;
        private ILineType _LineType;
        private readonly IHostingEnvironment environment;

        public LineTypeController(Context context,ILineType LineType, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _LineType = LineType;
        }

        // GET: LineTypes
        public IActionResult Index(int page = 1, string filtername = "",bool isSuccess=true)
        {
            var model = _LineType.GetLineTypes(page, filtername);
            if (isSuccess == false)
                ViewData["IsError"]= "خطایی رخ داده است.";
            return View(model);
        }

        // GET: LineTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var LineType = _LineType.FindById(id);
            if (LineType == null)
            {
                return NotFound();
            }

            return View(LineType);
        }

        // GET: LineTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LineType LineType)
        {
            if (ModelState.IsValid)
            {
              int productId = _LineType.AddLineTypeFromAdmin(LineType);

                return RedirectToAction(nameof(Index));
            }
            return View(LineType);
        }

        // GET: LineTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var LineType = _LineType.FindById(id);
            
            if (LineType == null)
            {
                return NotFound();
            }
            return View(LineType);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  LineType LineType)
        {
            if (id != LineType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 _LineType.EditLineType(LineType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineTypeExists(LineType.id))
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
            return View(LineType);
        }

        // GET: LineTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var LineType = _LineType.FindById(id);
            if (LineType == null)
            {
                return NotFound();
            }

            return View(LineType);
        }

        // POST: LineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if(await _LineType.Delete(id)==true)
            return RedirectToAction(nameof(Index));
            else
            return RedirectToAction(nameof(Index), new {isSuccess=false });

        }

        private bool LineTypeExists(int id)
        {
            return _context.LineTypes.Any(e => e.id == id);
        }
    }
}
