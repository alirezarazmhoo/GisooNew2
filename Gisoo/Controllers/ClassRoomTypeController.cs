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

    public class ClassRoomTypeController : Controller
    {
        private readonly Context _context;
        private IClassRoomType _ClassRoomType;
        private readonly IHostingEnvironment environment;

        public ClassRoomTypeController(Context context,IClassRoomType ClassRoomType, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _ClassRoomType = ClassRoomType;
        }

        // GET: ClassRoomTypes
        public IActionResult Index(int page = 1, string filtername = "",bool isSuccess=true)
        {
            var model = _ClassRoomType.GetClassRoomTypes(page, filtername);
            if (isSuccess == false)
                ViewData["IsError"]= "خطایی رخ داده است.";
            return View(model);
        }

        // GET: ClassRoomTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClassRoomType = _ClassRoomType.FindById(id);
            if (ClassRoomType == null)
            {
                return NotFound();
            }

            return View(ClassRoomType);
        }

        // GET: ClassRoomTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClassRoomType ClassRoomType)
        {
            if (ModelState.IsValid)
            {
              int productId = _ClassRoomType.AddClassRoomTypeFromAdmin(ClassRoomType);

                return RedirectToAction(nameof(Index));
            }
            return View(ClassRoomType);
        }

        // GET: ClassRoomTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClassRoomType = _ClassRoomType.FindById(id);
            
            if (ClassRoomType == null)
            {
                return NotFound();
            }
            return View(ClassRoomType);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ClassRoomType ClassRoomType)
        {
            if (id != ClassRoomType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 _ClassRoomType.EditClassRoomType(ClassRoomType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassRoomTypeExists(ClassRoomType.id))
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
            return View(ClassRoomType);
        }

        // GET: ClassRoomTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var ClassRoomType = _ClassRoomType.FindById(id);
            if (ClassRoomType == null)
            {
                return NotFound();
            }

            return View(ClassRoomType);
        }

        // POST: ClassRoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if(await _ClassRoomType.Delete(id)==true)
            return RedirectToAction(nameof(Index));
            else
            return RedirectToAction(nameof(Index), new {isSuccess=false });

        }

        private bool ClassRoomTypeExists(int id)
        {
            return _context.ClassRoomTypes.Any(e => e.id == id);
        }
    }
}
