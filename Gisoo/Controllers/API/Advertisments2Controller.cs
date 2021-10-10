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
    public class Advertisments2Controller : ControllerBase
    {

        private readonly Context _context;
        private IAdvertisment _Advertisment;
        private readonly IHostingEnvironment environment;

        public Advertisments2Controller(Context context, IAdvertisment Advertisment, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Advertisment = Advertisment;
        }

        
      

        [HttpPost("{id}")]
        public object ExtendedAdvertisment([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var advertisment = _context.Advertisments.Find(id);
                if (advertisment == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };
               advertisment.expireDate = advertisment.expireDate.AddDays(15);
                var user = _context.Users.Find(advertisment.userId);

                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.advertismentId = id;
                factor.factorKind = FactorKind.Extend;
                factor.totalPrice = 1000;
                _context.Factors.Add(factor);
                _context.SaveChanges();
              
                    trans.Commit();
                    return new { status = 0, message = "تمدید تبلیغات با موفقیت انجام شد." };
              
            }
        }
       
       
        private bool isExpire(DateTime expireDate)
        {

            if (expireDate < DateTime.Now)
                return true;
            else return false;
        }
        [HttpPost]
        public object PostAdvertisment()
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

                bool isWorkshop = Convert.ToBoolean(HttpContext.Request?.Form["isWorkshop"]);
                int cityId = Convert.ToInt32(HttpContext.Request?.Form["cityId"]);
                int provinceId = Convert.ToInt32(HttpContext.Request?.Form["provinceId"]);
                int areaId = Convert.ToInt32(HttpContext.Request?.Form["areaId"]);
                //string data = HttpContext.Request?.Form["data"];
                //JObject json = JObject.Parse(data);
                //JObject jalbum = json as JObject;
                var fileUrl1 = "";
                var fileUrl2 = "";
                var fileUrl3 = "";
                //Advertisment Advertisment = jalbum.ToObject<Advertisment>();
                var httpRequest = HttpContext.Request;
                var hfc = HttpContext.Request.Form.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    if (hfc[0].Length > 1024 * 1024 * 1)
                    {
                        return new { status = 1, message = "فایل ارسالی بزرگ تر از حد مجاز می باشد." };
                    }
                    var namefile = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(hfc[i].FileName).ToLower();
                    var filePath = Path.Combine(environment.WebRootPath, "Advertisment/", namefile);
                    if (hfc[i].FileName == "imageUpload1.jpg")
                    {
                        fileUrl1 = "/Advertisment/" + namefile;
                    }
                    if (hfc[i].FileName == "imageUpload2.jpg")
                    {
                        fileUrl2 = "/Advertisment/" + namefile;
                    }
                    if (hfc[i].FileName == "imageUpload3.jpg")
                    {
                        fileUrl3 = "/Advertisment/" + namefile;
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        hfc[i].CopyTo(stream);
                    }
                }
                Advertisment Advertisment = new Advertisment();
                Advertisment.areaId = areaId;
                Advertisment.cityId = cityId;
                Advertisment.provinceId = provinceId;
                Advertisment.title = title;
                Advertisment.description = description;
                Advertisment.isWorkshop = isWorkshop;
                Advertisment.createDate = DateTime.Now;
                Advertisment.expireDate = DateTime.Now.AddDays(15);
                Advertisment.isDeleted = true;

                Advertisment.image1 = fileUrl1;
                Advertisment.image2 = fileUrl2;
                Advertisment.image3 = fileUrl3;
                Advertisment.adminConfirmStatus = EnumStatus.Pending;
                Advertisment.userId = user.id;
                if (_context.Advertisments.Count() == 0)
                    Advertisment.code = "1";
                else
                    Advertisment.code = (Convert.ToInt32(_context.Advertisments.LastOrDefault().code) + 1).ToString();

                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (Advertisment.isWorkshop && allprice.isHasWorkshopPrice)
                    totalprice = allprice.workshopPrice;
                if (!Advertisment.isWorkshop && allprice.isHasAdvertismentPrice)
                    totalprice = allprice.advertismentPrice;
                //if(totalprice==0)
                Advertisment.isDeleted = false;
                //else
                //Advertisment.isDeleted = true;
                _context.Advertisments.Add(Advertisment);

                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.advertismentId = Advertisment.id;
                factor.factorKind = FactorKind.Add;
                factor.totalPrice = totalprice;
                _context.Factors.Add(factor);
                _context.SaveChanges();
               
                        trans.Commit();
                    return new { status = 0, message = "تبلیغات شما با موفقیت ثبت گردید.", url = "" };
               
            }
        }
      

    }
}