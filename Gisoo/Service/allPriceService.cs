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
    public class allPriceService : IAllPrice
    {
        private Context _context;
        public allPriceService(Context context)
        {
            _context = context;
        }
        public object GetAllPrices()
        {
            IQueryable<AllPrice> result = _context.AllPrices;
            List<AllPrice> res = result.OrderBy(u => u.id).ToList();
            return new { data = res, totalCount = result.Count() };
        }
        public PagedList<AllPrice> GetAllPrices(int pageId = 1, string filterName = "")
        {
            IQueryable<AllPrice> result = _context.AllPrices.OrderByDescending(x => x.id);
           
            PagedList<AllPrice> res = new PagedList<AllPrice>(result, pageId, 10);
            return res;
        }
    }
}
