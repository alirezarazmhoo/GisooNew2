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

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditAdvertismentsController : ControllerBase
    {
        private readonly Context _context;
        private IAdvertisment _Advertisment;
        private readonly IHostingEnvironment environment;

        public EditAdvertismentsController(Context context, IAdvertisment Advertisment, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Advertisment = Advertisment;
        }

       
        [HttpPost]
        public object PutAdvertisment()
        {
            if (!ModelState.IsValid)
                return new { status = 2, message = "مقادیر ارسالی معتبر نمی باشد." };
            string Token = HttpContext.Request?.Headers["Token"];
            int id = Convert.ToInt32(HttpContext.Request?.Form["id"]);
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
                    string deletePath = environment.WebRootPath + advertisment.image1;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                    fileUrl1 = "/Advertisment/" + namefile;
                }
                if (hfc[i].FileName == "imageUpload2.jpg")
                {
                     string deletePath = environment.WebRootPath + advertisment.image2;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                    fileUrl2 = "/Advertisment/" + namefile;
                }
                if (hfc[i].FileName == "imageUpload3.jpg")
                {
                     string deletePath = environment.WebRootPath + advertisment.image3;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
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
            if (dict.ContainsKey("isWorkshop"))
            {
                bool isWorkshop = Convert.ToBoolean(HttpContext.Request?.Form["isWorkshop"]);
                advertisment.isWorkshop = isWorkshop;
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
            advertisment.adminConfirmStatus = EnumStatus.Pending;
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