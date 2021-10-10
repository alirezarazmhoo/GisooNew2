using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteNoticeImageController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHostingEnvironment environment;

        public DeleteNoticeImageController(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        [HttpPost]
        public object DeleteNoticeImage()
        {
            string imageName = HttpContext.Request?.Form["imageName"];
            int id = Convert.ToInt32(HttpContext.Request?.Form["id"]);
            var notice = _context.Notices.Find(id);
            if (imageName == "imageUpload1")
            {
                string deletePath = environment.WebRootPath + notice.image1;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                notice.image1 = "";

            }
            if (imageName == "imageUpload2")
            {
                string deletePath = environment.WebRootPath + notice.image2;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                notice.image2 = "";

            }
            if (imageName == "imageUpload3")
            {
                string deletePath = environment.WebRootPath + notice.image3;
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                notice.image3 = "";
            }
            _context.SaveChanges();
            return new { status = 0, message = "عکس آگهی حذف گردید." };



        }

    }
}