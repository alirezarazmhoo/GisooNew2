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
    public class EditNoticesController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;

        public EditNoticesController(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }
        
        [HttpPost]
        public object PutNotice()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

           // Notice Notice = jalbum.ToObject<Notice>();
            var notice = _context.Notices.Find(id);
            var httpRequest = HttpContext.Request;
            var hfc = HttpContext.Request.Form.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                if (hfc[0].Length > 1024 * 1024 * 1)
                {
                    return new { status = 1, message = "فایل ارسالی بزرگ تر از حد مجاز می باشد." };
                }
                var namefile = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(hfc[i].FileName).ToLower();
                var filePath = Path.Combine(environment.WebRootPath, "Notice/", namefile);
                if (hfc[i].FileName == "imageUpload1.jpg")
                {
                    string deletePath = environment.WebRootPath + notice.image1;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                    fileUrl1 = "/Notice/" + namefile;
                }
                if (hfc[i].FileName == "imageUpload2.jpg")
                {
                    string deletePath = environment.WebRootPath + notice.image2;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                    fileUrl2 = "/Notice/" + namefile;
                }
                if (hfc[i].FileName == "imageUpload3.jpg")
                {
                    string deletePath = environment.WebRootPath + notice.image3;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                    fileUrl3 = "/Notice/" + namefile;
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    hfc[i].CopyTo(stream);
                }
            }
            if (fileUrl1 != "")
                notice.image1 = fileUrl1;
            if (fileUrl2 != "")
                notice.image2 = fileUrl2;
            if (fileUrl3 != "")
                notice.image3 = fileUrl3;
            var dict = HttpContext.Request?.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            if (dict.ContainsKey("areaId"))
            {
                int areaId = Convert.ToInt32(HttpContext.Request?.Form["areaId"]);
                notice.areaId = areaId;
            }
            if (dict.ContainsKey("isBarber"))
            {
                bool isBarber = Convert.ToBoolean(HttpContext.Request?.Form["isBarber"]);
                notice.isBarber = isBarber;
            }
            if (dict.ContainsKey("title"))
            {
                string title = HttpContext.Request?.Form["title"];
                notice.title = title;
            }
            if (dict.ContainsKey("description"))
            {
                string description = HttpContext.Request?.Form["description"];
                notice.description = description;
            }

            if (dict.ContainsKey("cityId"))
            {
                int cityId = Convert.ToInt32(HttpContext.Request?.Form["cityId"]);
                notice.cityId = cityId;
            }
            if (dict.ContainsKey("provinceId"))
            {
                int provinceId = Convert.ToInt32(HttpContext.Request?.Form["provinceId"]);
                notice.provinceId = provinceId;
            }
            notice.adminConfirmStatus = EnumStatus.Pending;
            _context.Notices.Update(notice);
            _context.SaveChanges();
            return new { status = 0, message = "آگهی شما با موفقیت ویرایش گردید." };
        }
       

    }
}