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

    public class InformationController : Controller
    {
        private readonly Context _context;

        public InformationController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(bool isSuccess=false)
        {
            if(isSuccess)
            ViewBag.success = "اطلاعیه شما با موفقیت ثبت شد در صورت نیاز با تغییر موارد زیر میتوانید اطلاعیه جدید ثبت نمایید";
            var model = _context.Informations.FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(Information Information)
        {
            ModelState.Remove("id");
            if (ModelState.IsValid)
            {
                var information = _context.Informations.FirstOrDefault();
                if(information!=null)
                _context.Informations.Remove(information);
                    _context.Add(Information);
                     _context.SaveChanges();
            return RedirectToAction("Index", new {@isSuccess=true});
                   
                
            }
            return RedirectToAction(nameof(Index));


        }
    }
}