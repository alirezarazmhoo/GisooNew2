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
    public class AdvertismentController : Controller
    {
        private readonly Context _context;
        private IAdvertisment _Advertisment;
        private IVisit _IVisit;
        private readonly IHostingEnvironment environment;
        
        public AdvertismentController(Context context, IAdvertisment Advertisment, IVisit visit, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Advertisment = Advertisment;
            _IVisit = visit;

        }

        // GET: Products
        public IActionResult Index(bool? filterisWorkshop, int page = 1, string filterTitle = "")
        {
            var model = _Advertisment.GetAdvertisments(filterisWorkshop, page, filterTitle);
            return View(model);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }
            VisitViewModel visitViewModel = _IVisit.GetByAllNoticeIdForAdmin(id.Value, WhichTableEnum.Advertisment);
            return View(visitViewModel);
        }
        [HttpPost]
        public ActionResult InActive(NoticeViewModel page)
        {
            try
            {
                var Advertisment = _context.Advertisments.Include(x => x.user).FirstOrDefault(x => x.id == page.id);
                Advertisment.adminConfirmStatus = page.adminConfirmStatus;
                Advertisment.notConfirmDescription = page.notConfirmDescription;
                if (page.adminConfirmStatus == EnumStatus.NotAccept)
                {
                    try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "NoticeTitle", ParameterValue = Advertisment.title });
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue = page.notConfirmDescription });
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(Advertisment.user.cellphone), 41387, Parameter);
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

                        SendSms.CallSmSMethod(Convert.ToInt64(Advertisment.user.cellphone), 41386, "NoticeTitle", Advertisment.title);
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
        public JsonResult GetAdvertisment(int id)
        {
            var Advertisment = new Advertisment();

            if (id != 0)
            {
                Advertisment = _context.Advertisments.Find(id);

                return Json(Advertisment);
            }
            else
            {
                return Json(Advertisment);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Advertisment = await _context.Advertisments
                .FirstOrDefaultAsync(m => m.id == id);
            if (Advertisment == null)
            {
                return NotFound();
            }

            return View(Advertisment);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Advertisment = await _context.Advertisments.FindAsync(id);
            if (Advertisment.image1 != null)
            {

                string deletePath = environment.WebRootPath + Advertisment.image1;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            if (Advertisment.image3 != null)
            {

                string deletePath = environment.WebRootPath + Advertisment.image3;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            if (Advertisment.image2 != null)
            {

                string deletePath = environment.WebRootPath + Advertisment.image2;

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
            }
            var factor = _context.Factors.Where(x => x.advertismentId == id);
            foreach (var item in factor)
            {
                _context.Factors.Remove(item);
            }
            _context.Advertisments.Remove(Advertisment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var advertisment = await _context.Advertisments.Include(x => x.user).FirstOrDefaultAsync(x => x.id == id);
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name", advertisment.cityId);
            ViewData["provinceId"] = new SelectList(_context.Provinces.Where(x => x.cityId == advertisment.cityId), "id", "name", advertisment.provinceId);
            ViewData["areaId"] = new SelectList(_context.Areas.Where(x => x.provinceId == advertisment.provinceId), "id", "name", advertisment.areaId);
            if (advertisment == null)
            {
                return NotFound();
            }
            return View(advertisment);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string expireDate1, bool isWorkshop, string description, int areaId, int provinceId, int cityId, ConditionEnum condition, string title)
        {


            if (ModelState.IsValid)
            {
                try
                {


                    var advertisment = _context.Advertisments.Find(id);
                    if (expireDate1 != null)
                    {
                        PersianCalendar pc = new PersianCalendar();
                        int year = Convert.ToInt32(expireDate1.Substring(0, 4));
                        int month = Convert.ToInt32(expireDate1.Substring(5, 2));
                        int day = Convert.ToInt32(expireDate1.Substring(8, 2));
                        DateTime dt = new DateTime(year, month, day, pc);
                        advertisment.expireDate = dt;
                    }
                    advertisment.areaId = areaId;
                    advertisment.description = description;

                    advertisment.provinceId = provinceId;
                    advertisment.cityId = cityId;
                    advertisment.title = title;
                    advertisment.isWorkshop = isWorkshop;

                    //_context.Update(advertisment);
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
        public ActionResult GoToNotice(int id, bool isBarber, ConditionEnum condition)
        {
            try
            {
                var Advertisment = _context.Advertisments.Find(id);
                Notice notice = new Notice();
                notice.adminConfirmStatus = Advertisment.adminConfirmStatus;
                notice.areaId = Advertisment.areaId;
                notice.cityId = Advertisment.cityId;
                notice.provinceId = Advertisment.provinceId;
                notice.createDate = Advertisment.createDate;
                notice.description = Advertisment.description;
                notice.image1 = Advertisment.image1;
                notice.image2 = Advertisment.image2;
                notice.image3 = Advertisment.image3;
                notice.title = Advertisment.title;
                notice.userId = Advertisment.userId;
                notice.notConfirmDescription = Advertisment.notConfirmDescription;
                notice.isBarber = isBarber;
                notice.condition = condition;
                notice.expireDate = Advertisment.expireDate;
                if (_context.Notices.Count() == 0)
                    notice.code = "1";
                else
                    notice.code = (Convert.ToInt32(_context.Notices.LastOrDefault().code) + 1).ToString();
                _context.Notices.Add(notice);
                var factors = _context.Factors.Where(x => x.advertismentId == id).ToList();
                foreach (var item in factors)
                {
                    var factor = _context.Factors.Find(item.id);
                    factor.noticeId = notice.id;
                }
                _context.Advertisments.Remove(Advertisment);
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
        public async Task<IActionResult> Create(AdvertismentViewModel advertismentViewModel, IFormFile file1,IFormFile file2,IFormFile file3)
        {
            ModelState.Remove("id");

            if (ModelState.IsValid)
            {
                try
                {
                    _Advertisment.CreateOrUpdate(advertismentViewModel, file1, file2, file3,true);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cityId"] = new SelectList(_context.Cities, "id", "name", advertismentViewModel.cityId);
            ViewData["provinceId"] = new SelectList(_context.Provinces.Where(x => x.cityId == advertismentViewModel.cityId), "id", "name", advertismentViewModel.provinceId);
            ViewData["areaId"] = new SelectList(_context.Areas.Where(x => x.provinceId == advertismentViewModel.provinceId), "id", "name", advertismentViewModel.areaId);
            return RedirectToAction("Create");
        }
    }
}