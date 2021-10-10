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
    public class DeleteAdvertismentController : ControllerBase
    {
        private readonly Context _context;
        private readonly IHostingEnvironment environment;

        public DeleteAdvertismentController(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        [HttpPost]
        public object DeleteAdvertisment()
        {
           
            int id =Convert.ToInt32(HttpContext.Request?.Form["id"]);
            var Advertisment = _context.Advertisments.Find(id);
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
            _context.SaveChanges();
            return new { status = 0, message = "تبلیغ حذف گردید." };



        }



    }
}