using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Controllers.AcceptManagment
{
    public class AcceptManagementController : Controller
    {

        private readonly Context _context;
        private ILine _line;


        public AcceptManagementController(Context context, ILine line)
        {

            _context = context;
            _line = line;
        }

        public IActionResult Lines()
        {
            return View(_line.GetAll());
        }
        [HttpPost]
        public IActionResult ChangeStatusToSuccess(int id)
        {
           if(id != 0)
            {
                _line.ChangeStatusToSuccess(id);
                return RedirectToAction(nameof(Lines));

            }
            return RedirectToAction(nameof(Lines));

        }
        [HttpGet]
        public JsonResult GetLine(int id)
        {
            var Line = new Line();
        

            if (id != 0)
            {
                Line = _line.GetById(id);

                return Json(Line);
            }
            else
            {
                return Json(Line);
            }
        }
    }
}