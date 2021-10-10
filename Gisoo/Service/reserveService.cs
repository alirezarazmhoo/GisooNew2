using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class reserveService : IReserve
    {
        private Context _context;
        public reserveService(Context context)
        {
            _context = context;
        }
        
        public List<Reserve> Get(int userIdNoticeOwner,SearchAllReserveViewModel searchAllReserveViewModel)
        {
            IQueryable<Reserve> result = _context.Reserves.Where(x=>x.userIdNoticeOwner==userIdNoticeOwner).Include(x=>x.user).Include(x=>x.product).Include(x=>x.line).Include(x=>x.classroom).Include(x=>x.lineWeekDate).AsQueryable();
            if (!String.IsNullOrEmpty(searchAllReserveViewModel.userFullName))
                result = result.Where(x => x.user.fullname.Contains(searchAllReserveViewModel.userFullName));
            if (!String.IsNullOrEmpty(searchAllReserveViewModel.userMobile))
                result = result.Where(x => x.user.cellphone==searchAllReserveViewModel.userMobile);

            if (!String.IsNullOrEmpty(searchAllReserveViewModel.registerDate))
            {
                result = result.Where(x => x.date.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchAllReserveViewModel.registerDate)));
            }
             List<Reserve> Reserves = result.OrderByDescending(x => x.id).ToList();
            return Reserves;
           
        }
    }
}
