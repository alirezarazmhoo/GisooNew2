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

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchNoticeController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;

        public SearchNoticeController(Context context,INotice notice)
        {
            _context = context;
            _notice = notice;
        }

        [HttpPost]
        public object GetNotices(ProductSearch2 searchNotice)
        {
            IQueryable<Notice> result = _context.Notices.Where(x => x.expireDate >= DateTime.Now  && x.adminConfirmStatus==EnumStatus.Accept);
           int page = 1;
            int pageSize = 10;
            page = searchNotice.page;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            if (!String.IsNullOrEmpty(searchNotice.title))
                result = result.Where(x => x.title.Contains(searchNotice.title) || x.description.Contains(searchNotice.title));
            if (searchNotice.areaId != 0)
                result = result.Where(x => x.areaId == searchNotice.areaId);
            if (searchNotice.cityId != 0)
                result = result.Where(x => x.cityId == searchNotice.cityId);
            if (searchNotice.provinceId != 0)
                result = result.Where(x => x.provinceId == searchNotice.provinceId);
            if (searchNotice.isBarber != null)
                result = result.Where(x => x.isBarber == searchNotice.isBarber);
            var res = result.OrderByDescending(x => x.createDate).Skip(10 * (page - 1)).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }

    }
}