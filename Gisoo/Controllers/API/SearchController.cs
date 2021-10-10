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
    public class SearchController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;

        public SearchController(Context context,INotice notice)
        {
            _context = context;
            _notice = notice;
        }

        // GET: api/Search
        //[HttpGet("{categoryId}/{page}/{pagesize}")]
        //public object GetSearch([FromRoute]int categoryId,[FromRoute]int page,[FromRoute] int pagesize)
        //{
        //     var data = _product.GetSearch(categoryId,page,pagesize);
        //    return data;
        //}


        [HttpPost]
        public object GetSearch(ProductSearch2 searchNotice)
        {
            int page = 1;
            int pageSize = 10;
            page = searchNotice.page;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            var result = _context.Notices.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept).AsQueryable();
            if(!String.IsNullOrEmpty(searchNotice.title))
             result = result.Where(x => x.title.Contains(searchNotice.title)|| x.description.Contains(searchNotice.title));
            if(searchNotice.areaId!=null)
             result = result.Where(x => x.areaId == searchNotice.areaId);
            if(searchNotice.cityId!=null)
             result = result.Where(x => x.cityId == searchNotice.cityId);
            if(searchNotice.provinceId!=null)
             result = result.Where(x => x.provinceId == searchNotice.provinceId);
            if(searchNotice.isBarber!=null)
             result = result.Where(x => x.isBarber == searchNotice.isBarber);
              return new { data = result.OrderByDescending(x => x.expireDate).Skip(10 * (page - 1)).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name  }), totalCount = result.Count() };
        }


    }
}