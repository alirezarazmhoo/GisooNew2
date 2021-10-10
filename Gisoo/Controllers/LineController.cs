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
    public class LineController : Controller
    {
        private readonly Context _context;
        private ILine _Line;
        private IVisit _IVisit;
         private readonly ILineType _ILineType;
        private readonly ILineWeekDate _LineWeekDate;
        
        private readonly IHostingEnvironment environment;

        public LineController(Context context, ILine Line, IVisit visit,ILineType LineType,ILineWeekDate lineWeekDate, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Line = Line;
            _IVisit = visit;
            _ILineType = LineType;
            _LineWeekDate = lineWeekDate;

        }

        // GET: Products
        public IActionResult Index(int page = 1, string filterTitle = "")
        {
            var model = _Line.GetLines(page, filterTitle);
            return View(model);
        }


        [HttpPost]
        public ActionResult InActive(NoticeViewModel page)
        {
            try
            {
                var Line = _context.Lines.Include(x => x.user).FirstOrDefault(x => x.id == page.id);
                Line.adminConfirmStatus = page.adminConfirmStatus;
                Line.notConfirmDescription = page.notConfirmDescription;
                if (page.adminConfirmStatus == EnumStatus.NotAccept)
                {
                    try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "NoticeTitle", ParameterValue = Line.title});
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue = page.notConfirmDescription});
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(Line.user.cellphone), 41387,Parameter);
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
                        
                        SendSms.CallSmSMethod(Convert.ToInt64(Line.user.cellphone), 41386,"NoticeTitle",Line.title);
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
        public JsonResult GetLine(int id)
        {
            var Line = new Line();

            if (id != 0)
            {
                Line = _context.Lines.Find(id);

                return Json(Line);
            }
            else
            {
                return Json(Line);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Line = await _context.Lines
                .FirstOrDefaultAsync(m => m.id == id);
            if (Line == null)
            {
                return NotFound();
            }

            return View(Line);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Line = await _context.Lines.FindAsync(id);
            var lineImage = _context.LineImages.Where(x => x.lineId == id).ToList();
            if (lineImage != null)
            {
                foreach (var item in lineImage)
                {
                    string deletePath = environment.WebRootPath + item.url;

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }

            }
            var factor = _context.Factors.Where(x => x.lineId == id);
            var reserves = _context.Reserves.Where(x => x.lineId == id);
            _context.Factors.RemoveRange(factor);
            _context.Reserves.RemoveRange(reserves);
            _context.Lines.Remove(Line);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Line = await _context.Lines.Include(x => x.user).Include(x=>x.LineImages).FirstOrDefaultAsync(x => x.id == id);
            LineViewModelAdmin lineViewModelAdmin = new LineViewModelAdmin();
            lineViewModelAdmin.line = Line;
            lineViewModelAdmin.lineTypes = _context.LineTypes.ToList();
            if (Line == null)
            {
                return NotFound();
            }
            return View(lineViewModelAdmin);
        }

         public async Task<IActionResult> Details(int id)
        {
            
            LineViewModelAdmin lineViewModelAdmin =   _Line.GetForDetailPage(id);
            return View(lineViewModelAdmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LineViewModelAdmin lineViewModelAdmin, IFormFile[] file)
        {
            var lineId = 0;
            if (ModelState.IsValid)
            {
                try
                {
            lineId= _Line.CreateOrUpdateFromAdmin(lineViewModelAdmin, file);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                if(lineViewModelAdmin.line.id ==0)
                return RedirectToAction("RegisterLineForReserve",new {id=lineId,userId=lineViewModelAdmin.line.userId});
                else
                return RedirectToAction("Index","Line");
            }
            return RedirectToAction("Edit", new { id = lineViewModelAdmin.line.id });

        }
        public IActionResult Create()
        {
            LineViewModelAdmin LineViewModelAdmin = new LineViewModelAdmin();
            LineViewModelAdmin.lineTypes = _context.LineTypes.ToList();
            LineViewModelAdmin.Users = _context.Users.Include(x=>x.role).Where(x => x.userStatus == true && (x.role.RoleNameEn=="HairStylist"||x.role.RoleNameEn=="SalonOwner")).ToList();
            return View(LineViewModelAdmin);
        }
        public IActionResult RegisterLineForReserve(int? id,int userId)
        {
            ViewBag.lineId = id;
            return View();
        }
         public IActionResult ReserveWeekEdit(int lineId, string FromdateReserve, string TodateReserve)
        {
            List<LineWeekDate> Item = _LineWeekDate.GetForEdit(lineId, FromdateReserve, TodateReserve);
            ViewBag.lineId = lineId;
            if (String.IsNullOrEmpty(FromdateReserve))
                ViewBag.FromdateReserve = DateTime.Now;
            else
                ViewBag.FromdateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(FromdateReserve));
            if (String.IsNullOrEmpty(TodateReserve))
                ViewBag.TodateReserve = DateTime.Now.AddDays(31);
            else
                ViewBag.TodateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(TodateReserve));
            return View(Item);
        }

    }
}