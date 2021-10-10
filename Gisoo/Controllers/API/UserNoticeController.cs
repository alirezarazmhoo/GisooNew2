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
    public class UserNoticeController : ControllerBase
    {
        private readonly Context _context;

        public UserNoticeController(Context context)
        {
            _context = context;
        }

        [HttpGet("{page}/{pagesize}")]
        public object UserNotice([FromRoute]int page, [FromRoute] int pagesize)
        {
            string Token = HttpContext.Request?.Headers["Token"];
            var user = _context.Users.Where(p => p.token == Token).FirstOrDefault();
            if (user == null)
                return new { status = 3, message = "چنین کاربری وجود ندارد." };
            IQueryable<Notice> result = _context.Notices.Where(x => x.userId == user.id);
            int skip = (page - 1) * pagesize;
            var res = result.OrderByDescending(u => u.createDate).Skip(skip).Take(pagesize).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name, x.adminConfirmStatus,x.expireDate, x.isBarber,isExpire=isExpire(x.expireDate),x.notConfirmDescription }).ToList();
            return new { data = res, totalCount = result.Count() };
        }

        private bool isExpire(DateTime expireDate)
        {

            if (expireDate < DateTime.Now)
                return true;
            else return false;
        }

    }
}