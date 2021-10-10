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
    public class NoticeController : Controller
    {
        private readonly Context _context;
        private INotice _Notice;
        private IVisit _IVisit;
        private readonly IHostingEnvironment environment;

        public NoticeController(Context context, INotice Notice, IVisit visit, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Notice = Notice;
            _IVisit = visit;
        }

        // GET: Products
        public IActionResult Index(ConditionEnum? filtercondition, bool? filterisBarber, int page = 1, string filterTitle = "")
        {
            var model = _Notice.GetNotices(filtercondition, filterisBarber, page, filterTitle);
            return View(model);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VisitViewModel visitViewModel = _IVisit.GetByAllNoticeIdForAdmin(id.Value, WhichTableEnum.Notice);
            return View(visitViewModel);
        }
        [HttpPost]
        public ActionResult InActive(NoticeViewModel page)
        {
            try
            {
                var Notice = _context.Notices.Include(x => x.user).FirstOrDefault(x => x.id == page.id);
                Notice.adminConfirmStatus = page.adminConfirmStatus;
                Notice.notConfirmDescription = page.notConfirmDescription;
                if (page.adminConfirmStatus == EnumStatus.NotAccept)
                {
                    try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "NoticeTitle", ParameterValue = Notice.title});
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue = page.notConfirmDescription});
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(Notice.user.cellphone), 41387,Parameter);
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
                        
                        SendSms.CallSmSMethod(Convert.ToInt64(Notice.user.cellphone), 41386,"NoticeTitle",Notice.title);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Notice = await _context.Notices
                .FirstOrDefaultAsync(m => m.id == id);
            if (Notice == null)
            {
                return NotFound();
            }

            return View(Notice);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Notice = await _context.Notices.FindAsync(id);
            if (Notice.image1 != null)
            {

                string deletePath = environment.WebRootPath + Notice.image1;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            if (Notice.image3 != null)
            {

                string deletePath = environment.WebRootPath + Notice.image3;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            if (Notice.image2 != null)
            {

                string deletePath = environment.WebRootPath + Notice.image2;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            var factor = _context.Factors.Where(x => x.noticeId == id);
            foreach (var item in factor)
            {
                _context.Factors.Remove(item);
            }
            _context.Notices.Remove(Notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public JsonResult GetNotice(int id)
        {
            var notice = new Notice();

            if (id != 0)
            {
                notice = _context.Notices.Find(id);

                return Json(notice);
            }
            else
            {
                return Json(notice);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var notice = await _context.Notices.Include(x => x.user).FirstOrDefaultAsync(x => x.id == id);
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name", notice.cityId);
            ViewData["provinceId"] = new SelectList(_context.Provinces.Where(x => x.cityId == notice.cityId), "id", "name", notice.provinceId);
            ViewData["areaId"] = new SelectList(_context.Areas.Where(x => x.provinceId == notice.provinceId), "id", "name", notice.areaId);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string expireDate1, bool isBarber, string description, int areaId, int provinceId, int cityId, ConditionEnum condition, string title)
        {


            if (ModelState.IsValid)
            {
                try
                {


                    var notice = _context.Notices.Find(id);
                    if (expireDate1 != null)
                    {
                        PersianCalendar pc = new PersianCalendar();
                        int year = Convert.ToInt32(expireDate1.Substring(0, 4));
                        int month = Convert.ToInt32(expireDate1.Substring(5, 2));
                        int day = Convert.ToInt32(expireDate1.Substring(8, 2));
                        DateTime dt = new DateTime(year, month, day, pc);
                        notice.expireDate = dt;
                    }
                    notice.areaId = areaId;
                    notice.description = description;

                    notice.provinceId = provinceId;
                    notice.cityId = cityId;
                    notice.title = title;
                    notice.isBarber = isBarber;
                    notice.condition = condition;

                    //_context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name", cityId);
            ViewData["provinceId"] = new SelectList(_context.Provinces.Where(x => x.cityId == cityId), "id", "name", provinceId);
            ViewData["areaId"] = new SelectList(_context.Areas.Where(x => x.provinceId == provinceId), "id", "name", areaId);
            return RedirectToAction("Edit", new { id = id });

        }
        public string LoadProvince(int cityId)
        {
            var provinces = _context.Provinces.Where(x => x.cityId == cityId).ToList();
            string str = "";
            str+="<option value=''>انتخاب کنید</option>";

            foreach (var item in provinces)
            {
                str += "<option value=" + item.id + "> " + item.name + " </option>";
            }
            return str;
        }
        public string LoadArea(int provinceId)
        {
            var areas = _context.Areas.Where(x => x.provinceId == provinceId).ToList();
            string str = "";
            str+="<option value=''>انتخاب کنید</option>";

            foreach (var item in areas)
            {
                str += "<option value=" + item.id + "> " + item.name + " </option>";
            }
            return str;
        }
        [HttpPost]
        public ActionResult GoToAdvertisment(int id, bool isWorkShop)
        {
            try
            {
                var Notice = _context.Notices.Find(id);
                Advertisment advertisment = new Advertisment();
                advertisment.adminConfirmStatus = Notice.adminConfirmStatus;
                advertisment.areaId = Notice.areaId;
                advertisment.cityId = Notice.cityId;
                advertisment.provinceId = Notice.provinceId;
                advertisment.createDate = Notice.createDate;
                advertisment.description = Notice.description;
                advertisment.image1 = Notice.image1;
                advertisment.image2 = Notice.image2;
                advertisment.image3 = Notice.image3;
                advertisment.title = Notice.title;
                advertisment.userId = Notice.userId;
                advertisment.notConfirmDescription = Notice.notConfirmDescription;
                advertisment.isWorkshop = isWorkShop;
                advertisment.expireDate = Notice.expireDate;
                if (_context.Advertisments.Count() == 0)
                    advertisment.code = "1";
                else
                    advertisment.code = (Convert.ToInt32(_context.Advertisments.LastOrDefault().code) + 1).ToString();
                _context.Advertisments.Add(advertisment);
                var factors = _context.Factors.Where(x => x.noticeId == id).ToList();
                foreach (var item in factors)
                {
                    var factor = _context.Factors.Find(item.id);
                    factor.advertismentId = advertisment.id;
                }
                _context.Notices.Remove(Notice);
                _context.SaveChanges();
                return Json("Done");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }
         public IActionResult Create()
        {
            
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name");
            ViewData["provinceId"] = new SelectList(_context.Provinces, "id", "name");
            ViewData["areaId"] = new SelectList(_context.Areas, "id", "name");
            ViewData["userId"] = new SelectList(_context.Users.Where(x=>x.role.RoleNameEn!="Admin"), "id", "cellphone");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notice2ViewModel notice2ViewModel, IFormFile file1,IFormFile file2,IFormFile file3)
        {
            ModelState.Remove("id");
            if (ModelState.IsValid)
            {
                try
                {
                    _Notice.CreateOrUpdate(notice2ViewModel, file1, file2, file3,true);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name", notice2ViewModel.cityId);
            ViewData["provinceId"] = new SelectList(_context.Provinces.Where(x => x.cityId == notice2ViewModel.cityId), "id", "name", notice2ViewModel.provinceId);
            ViewData["areaId"] = new SelectList(_context.Areas.Where(x => x.provinceId == notice2ViewModel.provinceId), "id", "name", notice2ViewModel.areaId);
            return RedirectToAction("Create");
        }
    }
}