using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class factorService : IFactor
    {
        private Context _context;
        public factorService(Context context)
        {
            _context = context;
        }

       

        public PagedList<Factor> GetFactors(int pageId = 1)
        {
            IQueryable<Factor> result = _context.Factors.Include(x=>x.advertisment).Include(x=>x.notice).Include(x=>x.user).OrderByDescending(x=>x.id);
            PagedList<Factor> res = new PagedList<Factor>(result, pageId, 10);
            return res;
        }
    }
}
