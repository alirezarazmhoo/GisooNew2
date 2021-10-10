using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gisoo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Gisoo.Service.Interface;
using Gisoo.DAL;
using Microsoft.EntityFrameworkCore;

namespace Gisoo.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;
        public HomeController(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }
        public IActionResult Index()
        {
            foreach (var item in _context.Notices.Where(x => x.isDeleted == true))
            {
                _context.Factors.RemoveRange(_context.Factors.Where(x => x.noticeId == item.id));
                if (!String.IsNullOrEmpty(item.image1))
                {
                    string deletePath = environment.WebRootPath + item.image1;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                if (!String.IsNullOrEmpty(item.image2))
                {
                    string deletePath = environment.WebRootPath + item.image2;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                if (!String.IsNullOrEmpty(item.image3))
                {
                    string deletePath = environment.WebRootPath + item.image3;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                _context.Notices.Remove(item);
            }
            foreach (var item in _context.Advertisments.Where(x => x.isDeleted == true))
            {
                _context.Factors.RemoveRange(_context.Factors.Where(x => x.advertismentId == item.id));
                    if (!String.IsNullOrEmpty(item.image1))
                    {
                        string deletePath = environment.WebRootPath + item.image1;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    if (!String.IsNullOrEmpty(item.image2))
                    {
                        string deletePath = environment.WebRootPath + item.image2;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    if (!String.IsNullOrEmpty(item.image3))
                    {
                        string deletePath = environment.WebRootPath + item.image3;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    _context.Advertisments.Remove(item);
            }
            _context.SaveChanges();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
    }
}
