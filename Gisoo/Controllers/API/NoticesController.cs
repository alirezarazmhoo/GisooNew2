using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Newtonsoft.Json.Linq;
using Gisoo.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Zarinpal;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticesController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;

        public NoticesController(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }
        //[HttpGet]
        //public object GetAllFirstPage()
        //{
        //    var advertismentIsworkshop = _context.Advertisments.Where(x => x.isWorkshop && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    var advertismentIsNotworkshop = _context.Advertisments.Where(x => x.isWorkshop == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    var notice = _context.Notices.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    var noticeIsBarber = _context.Notices.Where(x => x.isBarber && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    var noticeIsNotBarber = _context.Notices.Where(x => x.isBarber == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    var slider = _context.Sliders.ToList();
        //    return new { advertismentIsworkshop, advertismentIsNotworkshop, notice, noticeIsBarber, noticeIsNotBarber, slider };

        //}
        // GET: api/Notices
        //[HttpGet("{categoryId}/{page}/{pagesize}")]
        //public object GetNotices([FromRoute]int categoryId,[FromRoute]int page,[FromRoute] int pagesize)
        //{
        //     var data = _notice.GetNotices(categoryId,page,pagesize);
        //    return data;
        //}
        [HttpGet("{page}/{pagesize}")]
        public object GetNotices([FromRoute]int page, [FromRoute] int pagesize)
        {


            var data = _notice.GetNotices(page, pagesize);
            return data;
        }

        //[HttpGet("{page}/{pageSize}/{isBarber}")]
        //public object GetNotices([FromRoute]int page, [FromRoute] int pagesize,bool isBarber)
        //{
        //    IQueryable<Notice> result = _context.Notices.Where(x => x.expireDate >= DateTime.Now  && x.isBarber==isBarber && x.adminConfirmStatus==EnumStatus.Accept);

        //    //if (!String.IsNullOrEmpty(searchNotice.title))
        //    //    result = result.Where(x => x.title.Contains(searchNotice.title) || x.description.Contains(searchNotice.title));
        //    //if (searchNotice.areaId != null)
        //    //    result = result.Where(x => x.areaId == searchNotice.areaId);
        //    //if (searchNotice.cityId != null)
        //    //    result = result.Where(x => x.cityId == searchNotice.cityId);
        //    //if (searchNotice.provinceId != null)
        //    //    result = result.Where(x => x.provinceId == searchNotice.provinceId);
        //    //if (searchNotice.isBarber != null)
        //    //    result = result.Where(x => x.isBarber == searchNotice.isBarber);
        //    var res = result.OrderByDescending(x => x.expireDate).Skip(10 * (page - 1)).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
        //    return new { data = res, totalCount = result.Count() };
        //}








        [HttpPost("{id}")]
        public object ExtendedNotice([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var Notice = _context.Notices.Find(id);
                if (Notice == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };
                //Notice.expireDate = Notice.expireDate.AddDays(15);
                var user = _context.Users.Find(Notice.userId);

                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.noticeId = id;
                factor.factorKind = FactorKind.Extend;
                factor.totalPrice = 1000;
                _context.Factors.Add(factor);
                _context.SaveChanges();
                 var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", 1000);
                var res = payment.PaymentRequest($"پرداخت {Notice.id} تمدید", "http://gisoooo.ir/api/ConfirmExtentNotice/" + Notice.id + "/" + factor.id, "", "");
                if (res.Result.Status == 100)
                {
                    trans.Commit();
                    return new { status = 0, message = "تمدید آگهی با موفقیت انجام شد.", url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority };
                }
                else
                {
                    trans.Rollback();
                    return new { status = 5, message = "خطا در تمدید آگهی." };
                }
            }

        }






        //[HttpPost]
        //public object GetNotices(NoticeSearch NoticeSearch)
        //{b
        //    var result = _notice.GetNoticesByCatAndType(NoticeSearch);
        //    return result;
        //}
        //[HttpPost("{page}")]
        //public object GetNotices([FromRoute]int page)
        //{
        //    var data = _notice.GetLastNotices(page);
        //    return data;
        //}

        // GET: api/Notices/5
        [HttpGet("{id}")]
        public IActionResult GetNotice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Notice = _context.Notices.Select(x => new { x.id, x.image1, x.image2, x.image3, x.title, x.description, areaName = x.area.name, cityName = x.city.name, x.code, x.condition, x.createDate, x.isBarber, provinceName = x.province.name, x.cityId, x.areaId, x.provinceId, x.user.cellphone,isExpire=isExpire(x.expireDate) }).FirstOrDefault(x => x.id == id);
            if (Notice == null)
            {
                return NotFound();
            }

            return Ok(Notice);
        }
        private bool isExpire(DateTime expireDate)
        {

            if (expireDate < DateTime.Now)
                return true;
            else return false;
        }
        [HttpPost]
        public object PostNotice()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var trans = _context.Database.BeginTransaction())
            {
                string Token = HttpContext.Request?.Headers["Token"];
                var user = _context.Users.Where(p => p.token == Token).FirstOrDefault();
                if (user == null)
                    return new { status = 3, message = "چنین کاربری وجود ندارد." };

                string title = HttpContext.Request?.Form["title"];
                string description = HttpContext.Request?.Form["description"];
                ConditionEnum condition = (ConditionEnum)Enum.Parse(typeof(ConditionEnum), HttpContext.Request?.Form["condition"], true);

                bool isBarber = Convert.ToBoolean(HttpContext.Request?.Form["isBarber"]);
                int cityId = Convert.ToInt32(HttpContext.Request?.Form["cityId"]);
                int provinceId = Convert.ToInt32(HttpContext.Request?.Form["provinceId"]);
                int areaId = Convert.ToInt32(HttpContext.Request?.Form["areaId"]);

                //string data = HttpContext.Request?.Form["data"];

                //JObject json = JObject.Parse(data);
                //JObject jalbum = json as JObject;
                var fileUrl1 = "";
                var fileUrl2 = "";
                var fileUrl3 = "";

                //Notice notice = jalbum.ToObject<Notice>();
                var httpRequest = HttpContext.Request;
                var hfc = HttpContext.Request.Form.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    if (hfc[i].Length > 1024 * 1024 * 10)
                    {
                        return new { status = 1, message = "فایل ارسالی بزرگ تر از حد مجاز می باشد." };
                    }
                    var namefile = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(hfc[i].FileName).ToLower();
                    var filePath = Path.Combine(environment.WebRootPath, "Notice/", namefile);
                    if (hfc[i].FileName == "imageUpload1.jpg")
                    {
                        fileUrl1 = "/Notice/" + namefile;
                    }
                    if (hfc[i].FileName == "imageUpload2.jpg")
                    {
                        fileUrl2 = "/Notice/" + namefile;
                    }
                    if (hfc[i].FileName == "imageUpload3.jpg")
                    {
                        fileUrl3 = "/Notice/" + namefile;
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        hfc[i].CopyTo(stream);
                    }
                }
                Notice notice = new Notice();
                notice.areaId = areaId;
                notice.cityId = cityId;
                notice.provinceId = provinceId;
                notice.title = title;
                notice.description = description;
                notice.condition = condition;
                notice.isBarber = isBarber;
                notice.createDate = DateTime.Now;
                notice.expireDate = DateTime.Now.AddDays(15);
                notice.image1 = fileUrl1;
                notice.image2 = fileUrl2;
                notice.image3 = fileUrl3;
                notice.adminConfirmStatus = EnumStatus.Pending;
                notice.userId = user.id;
                if (_context.Notices.Count() == 0)
                    notice.code = "1";
                else
                    notice.code = (Convert.ToInt32(_context.Notices.LastOrDefault().code) + 1).ToString();
                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (notice.isBarber && allprice.isHasBarberPrice)
                    totalprice = allprice.barberPrice;
                if (!notice.isBarber && allprice.isHasSaloonPrice)
                    totalprice = allprice.saloonPrice;
                if(totalprice==0)
                notice.isDeleted = false;
                else
                notice.isDeleted = true;

                _context.Notices.Add(notice);

                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.noticeId = notice.id;
                factor.factorKind = FactorKind.Add;
                factor.totalPrice = totalprice;
                _context.Factors.Add(factor);
               
                    _context.SaveChanges();
               
                if (totalprice == 0)
                {
                        trans.Commit();
                        return new { status = 0, message = "آگهی شما با موفقیت ثبت گردید.", url = "" };
                }
                else
                {
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {notice.id}", "http://gisoooo.ir/api/ConfirmNotices/" + notice.id + "/" + factor.id, "", "");
                    if (res.Result.Status == 100)
                    {
                        trans.Commit();
                        return new { status = 0, message = "آگهی شما با موفقیت ثبت گردید.", url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority };
                    }
                    else
                    {
                        trans.Rollback();
                        return new { status = 5, message = "خطا در ثبت آگهی." };
                    }
                }
            }
        }

    }
}