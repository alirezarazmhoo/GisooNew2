using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class cityService : ICity
    {
        private Context _context;
        public cityService(Context context)
        {
            _context = context;
        }
        public object GetCities()
        {
            IQueryable<City> result = _context.Cities;
            List<City> res = result.OrderBy(u => u.id).ToList();
            return new { data = res, totalCount = result.Count() };
        }
         public object GetProvinces(int cityId)
        {
            IQueryable<Province> result = _context.Provinces;
            object res = result.Where(x=>x.cityId==cityId).OrderBy(u => u.id).Select( x=>new {x.id,x.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }
        public object GetAreas(int provinceId)
        {
            IQueryable<Area> result = _context.Areas;
            object res = result.Where(x=>x.provinceId==provinceId).OrderBy(u => u.id).Select( x=>new {x.id,x.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }

         public List<City> GetCitiesList()
        {
            List<City> result = _context.Cities.OrderBy(u => u.id).ToList();
            return result;
        }
         public List<Province> GetProvincesList(int cityId)
        {
            List<Province> result = _context.Provinces.Where(x=>x.cityId==cityId).OrderBy(u => u.id).ToList();
            return result;
        }
        public List<Area> GetAreasList(int provinceId)
        {
            List<Area> result = _context.Areas.Where(x=>x.provinceId==provinceId).OrderBy(u => u.id).ToList();
            return result;
        }
        public PagedList<City> GetCities(int pageId = 1, string filterName = "")
        {
            IQueryable<City> result = _context.Cities.OrderByDescending(x => x.id);
           if(!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(x => x.name.Contains(filterName));
            }
            PagedList<City> res = new PagedList<City>(result, pageId, 10);
            return res;
        }
    }
}
