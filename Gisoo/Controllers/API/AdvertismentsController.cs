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
    public class AdvertismentsController : ControllerBase
    {

        private readonly Context _context;
        private IAdvertisment _Advertisment;
        private readonly IHostingEnvironment environment;

        public AdvertismentsController(Context context, IAdvertisment Advertisment, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Advertisment = Advertisment;
        }

        // GET: api/Advertisments
        //[HttpGet("{categoryId}/{page}/{pagesize}")]
        //public object GetAdvertisments([FromRoute]int categoryId,[FromRoute]int page,[FromRoute] int pagesize)
        //{
        //     var data = _Advertisment.GetAdvertisments(categoryId,page,pagesize);
        //    return data;
        //}
        [HttpGet("{page}/{pagesize}")]
        public object GetAdvertisments([FromRoute]int page, [FromRoute] int pagesize)
        {
            var data = _Advertisment.GetAdvertisments(page, pagesize);
            return data;
        }


        //    [HttpGet("{page}/{pageSize}/{isWorkshop}")]
        //public object GetAdvertisments([FromRoute]int page, [FromRoute] int pagesize,bool isWorkshop)
        //{
        //    IQueryable<Advertisment> result = _context.Advertisments.Where(x => x.expireDate >= DateTime.Now  && x.isWorkshop==isWorkshop && x.adminConfirmStatus==EnumStatus.Accept);

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



        //[HttpPost]
        //public object GetAdvertisments(AdvertismentSearch AdvertismentSearch)
        //{
        //    var result = _Advertisment.GetAdvertismentsByCatAndType(AdvertismentSearch);
        //    return result;
        //}

        [HttpPost("{id}")]
        public object ExtendedAdvertisment([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var advertisment = _context.Advertisments.Find(id);
                if (advertisment == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };
               // advertisment.expireDate = advertisment.expireDate.AddDays(15);
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
               var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", 1000);
                var res = payment.PaymentRequest($"پرداخت {advertisment.id} تمدید", "http://gisoooo.ir/api/ConfirmExtentAdvertisment/" + advertisment.id + "/" + factor.id, "", "");
                if (res.Result.Status == 100)
                {
                    trans.Commit();
                    return new { status = 0, message = "تمدید تبلیغات با موفقیت انجام شد.", url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority };
                }
                else
                {
                    trans.Rollback();
                    return new { status = 5, message = "خطا در تمدید تبلیغات." };
                }
            }
        }
        //[HttpPost("{page}")]
        //public object GetAdvertisments([FromRoute]int page)
        //{
        //    var data = _Advertisment.GetLastAdvertisments(page);
        //    return data;
        //}

        // GET: api/Advertisments/5
        [HttpGet("{id}")]
        public IActionResult GetAdvertisment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Advertisment = _context.Advertisments.Select(x => new { x.id, x.image1, x.image2, x.image3, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name, x.isWorkshop, x.code, x.cityId, x.areaId, x.provinceId, x.user.cellphone,isExpire=isExpire(x.expireDate) }).FirstOrDefault(x => x.id == id);
            if (Advertisment == null)
            {
                return NotFound();
            }

            return Ok(Advertisment);
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
                if(totalprice==0)
                Advertisment.isDeleted = false;
                else
                Advertisment.isDeleted = true;
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
                if (totalprice == 0)
                {
                    trans.Commit();
                    return new { status = 0, message = "تبلیغات شما با موفقیت ثبت گردید.", url = "" };
                }
                else
                {
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {Advertisment.id}", "http://gisoooo.ir/api/ConfirmAdvertisments/" + Advertisment.id + "/" + factor.id, "", "");
                    if (res.Result.Status == 100)
                    {
                        trans.Commit();
                        return new { status = 0, message = "تبلیغات شما با موفقیت ثبت گردید.", url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority };
                    }
                    else
                    {
                        trans.Rollback();
                        return new { status = 5, message = "خطا در ثبت تبلیغات." };
                    }
                }
            }
        }
        [HttpPut("{id}")]
        public object PutAdvertisment([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return new { status = 2, message = "مقادیر ارسالی معتبر نمی باشد." };
            string Token = HttpContext.Request?.Headers["Token"];
            var user = _context.Users.Where(p => p.token == Token).FirstOrDefault();
            if (user == null)
                return new { status = 3, message = "چنین کاربری وجود ندارد." };
            //string data = HttpContext.Request?.Form["data"];
            //JObject json = JObject.Parse(data);
            //JObject jalbum = json as JObject;
            var fileUrl1 = "";
            var fileUrl2 = "";
            var fileUrl3 = "";
            //Advertisment Advertisment = jalbum.ToObject<Advertisment>();
            var advertisment = _context.Advertisments.Find(id);
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
            if (fileUrl1 != "")
                advertisment.image1 = fileUrl1;
            if (fileUrl2 != "")
                advertisment.image2 = fileUrl2;
            if (fileUrl3 != "")
                advertisment.image3 = fileUrl3;
            var dict = HttpContext.Request?.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            if (dict.ContainsKey("areaId"))
            {
                int areaId = Convert.ToInt32(HttpContext.Request?.Form["areaId"]);
                advertisment.areaId = areaId;
            }
            if (dict.ContainsKey("title"))
            {
                string title = HttpContext.Request?.Form["title"];
                advertisment.title = title;
            }
            if (dict.ContainsKey("description"))
            {
                string description = HttpContext.Request?.Form["description"];
                advertisment.description = description;
            }

            if (dict.ContainsKey("cityId"))
            {
                int cityId = Convert.ToInt32(HttpContext.Request?.Form["cityId"]);
                advertisment.cityId = cityId;
            }
            if (dict.ContainsKey("provinceId"))
            {
                int provinceId = Convert.ToInt32(HttpContext.Request?.Form["provinceId"]);
                advertisment.provinceId = provinceId;
            }

            _context.Advertisments.Update(advertisment);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            { }
            return new { status = 0, message = "با موفقیت ویرایش گردید." };
        }

    }
}