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
    public class DeleteNoticeController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHostingEnvironment environment;

        public DeleteNoticeController(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        [HttpPost]
        public object DeleteNotice()
        {
            int id = Convert.ToInt32(HttpContext.Request?.Form["id"]);
            var Notice = _context.Notices.Find(id);
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
            _context.SaveChanges();
            return new { status = 0, message = " آگهی حذف گردید." };
        }

    }
}