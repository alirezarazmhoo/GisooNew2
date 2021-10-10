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
    public class AllFirstPageController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;

        public AllFirstPageController(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }
        [HttpGet]
        public object GetAllFirstPage()
        {
            var advertismentIsworkshop = _context.Advertisments.Where(x => x.isWorkshop && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            var advertismentIsNotworkshop = _context.Advertisments.Where(x => x.isWorkshop == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            var notice = _context.Notices.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            var noticeIsBarber = _context.Notices.Where(x => x.isBarber && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            var noticeIsNotBarber = _context.Notices.Where(x => x.isBarber == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus==EnumStatus.Accept).OrderByDescending(x => x.createDate).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            var slider = _context.Sliders.ToList();
            return new { advertismentIsworkshop, advertismentIsNotworkshop, notice, noticeIsBarber, noticeIsNotBarber, slider };

        }
       
    }
}