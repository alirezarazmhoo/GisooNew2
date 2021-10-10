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
    public class Notices2Controller : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;

        public Notices2Controller(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }
        [HttpPost("{id}")]
        public object ExtendedNotice([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var Notice = _context.Notices.Find(id);
                if (Notice == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };
                Notice.expireDate = Notice.expireDate.AddDays(15);
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
                    trans.Commit();
                    return new { status = 0, message = "تمدید آگهی با موفقیت انجام شد." };
                
            }

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
                //if(totalprice==0)
                notice.isDeleted = false;
                //else
                //notice.isDeleted = true;

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
               trans.Commit();
                return new { status = 0, message = "آگهی شما با موفقیت ثبت گردید.", url = "" };
               
            }
        }

    }
}