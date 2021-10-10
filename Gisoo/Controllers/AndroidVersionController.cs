using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Gisoo.Controllers
{
    [Authorize]

    public class AndroidVersionController : Controller
    {
        private readonly Context _context;
        private readonly IHostingEnvironment environment;

        public AndroidVersionController(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(bool isSuccess=false)
        {
            var model = _context.AndroidVersions.FirstOrDefault();
            AndroidVersionViewModel AndroidVersionViewModel = new AndroidVersionViewModel();
           if (model != null)
            {
                AndroidVersionViewModel.appAndroidUrl = model.appAndroidUrl;
                AndroidVersionViewModel.currVersion = model.currVersion;
                AndroidVersionViewModel.id = model.id;             
            }
            if (isSuccess)
                ViewBag.success = "ورژن شما با موفقیت ثبت شد اگر مایل هستید نسخه جدید آپلود نمایید.";
            return View(AndroidVersionViewModel);
        }
        [HttpPost("UploadFiles")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(AndroidVersionViewModel page)
        {
            ModelState.Remove("id");
            if (ModelState.IsValid)
            {
                if (page.id == 0)
                {
                    var AndroidVersion = new AndroidVersion();
                    String FileExt = Path.GetExtension(page.files.FileName).ToUpper();
                    if (FileExt == ".APK")
                    {
                        var name = page.files.FileName;
                        var filePath = Path.Combine(environment.WebRootPath, "AndroidApp/", name);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            page.files.CopyTo(stream);
                        }
                        AndroidVersion.appAndroidUrl = "wwwroot/AndroidApp/"+ name;
                        AndroidVersion.currVersion = page.currVersion;
                        _context.AndroidVersions.Add(AndroidVersion);
                    }
                    else
                    {
                        return Json(new { Data = "Error" });

                    }
                    await _context.SaveChangesAsync();
                     return RedirectToAction("Index", new {  @isSuccess = true });

                }
                else
                {
                    try
                    {
                        var AndroidVersion = _context.AndroidVersions.Find(page.id);
                        string deletePath = environment.WebRootPath + AndroidVersion.appAndroidUrl;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                        String FileExt = Path.GetExtension(page.files.FileName).ToUpper();
                        if (FileExt == ".APK")
                        {
                            var name = page.files.FileName;
                            var filePath = Path.Combine(environment.WebRootPath, "AndroidApp/", name);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                page.files.CopyTo(stream);
                            }
                            AndroidVersion.appAndroidUrl = "wwwroot/AndroidApp/"+ name;;
                            AndroidVersion.currVersion = page.currVersion;
                            _context.AndroidVersions.Update(AndroidVersion);
                        }
                        else
                        {
                            return Json(new { Data = "Error" });

                        }
                        await _context.SaveChangesAsync();
                     return RedirectToAction("Index", new {  @isSuccess = true });

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