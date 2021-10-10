using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class visitService : IVisit
    {
        private Context _context;
        public visitService(Context context)
        {
            _context = context;
        }

        public VisitViewModel GetByAllNoticeId(int noticeId, WhichTableEnum whichTableEnum)
        {
            List<Visit> visits = _context.Visits.Where(x => x.anyNoticeId == noticeId && x.whichTableEnum == whichTableEnum).ToList();
            VisitViewModel visitViewModel = new VisitViewModel();
            visitViewModel.datecount1 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-9)) == 0).Count();
            visitViewModel.datecount2 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-8)) == 0).Count();
            visitViewModel.datecount3 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-7)) == 0).Count();
            visitViewModel.datecount4 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-6)) == 0).Count();
            visitViewModel.datecount5 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-5)) == 0).Count();
            visitViewModel.datecount6 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-4)) == 0).Count();
            visitViewModel.datecount7 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-3)) == 0).Count();
            visitViewModel.datecount8 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-2)) == 0).Count();
            visitViewModel.datecount9 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) == 0).Count();
            visitViewModel.datecount10 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date) == 0).Count();
            if (whichTableEnum == WhichTableEnum.Advertisment)
                visitViewModel.viewTotalCount = _context.Advertisments.FirstOrDefault(x => x.id == noticeId).countView;
            if (whichTableEnum == WhichTableEnum.ClssRoom)
                visitViewModel.viewTotalCount = _context.ClassRooms.FirstOrDefault(x => x.id == noticeId).countView;
            if (whichTableEnum == WhichTableEnum.Notice)
                visitViewModel.viewTotalCount = _context.Notices.FirstOrDefault(x => x.id == noticeId).countView;
            if (whichTableEnum == WhichTableEnum.Line)
                visitViewModel.viewTotalCount = _context.Lines.FirstOrDefault(x => x.id == noticeId).countView;
            if (whichTableEnum == WhichTableEnum.Product)
                visitViewModel.viewTotalCount = _context.Products.FirstOrDefault(x => x.id == noticeId).countView;
            return visitViewModel;
        }
         public VisitViewModel GetByAllNoticeIdForAdmin(int noticeId, WhichTableEnum whichTableEnum)
        {
            List<Visit> visits = _context.Visits.Where(x => x.anyNoticeId == noticeId && x.whichTableEnum == whichTableEnum).ToList();
            VisitViewModel visitViewModel = new VisitViewModel();
            visitViewModel.datecount1 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-9)) == 0).Count();
            visitViewModel.datecount2 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-8)) == 0).Count();
            visitViewModel.datecount3 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-7)) == 0).Count();
            visitViewModel.datecount4 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-6)) == 0).Count();
            visitViewModel.datecount5 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-5)) == 0).Count();
            visitViewModel.datecount6 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-4)) == 0).Count();
            visitViewModel.datecount7 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-3)) == 0).Count();
            visitViewModel.datecount8 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-2)) == 0).Count();
            visitViewModel.datecount9 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) == 0).Count();
            visitViewModel.datecount10 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date) == 0).Count();
            if(whichTableEnum==WhichTableEnum.Notice)
            visitViewModel.Notice = _context.Notices.Include(x => x.city).Include(x => x.province).Include(x => x.area).Include(x => x.user).FirstOrDefault(m => m.id == noticeId);
            if(whichTableEnum==WhichTableEnum.Advertisment)
            visitViewModel.Advertisment = _context.Advertisments.Include(x => x.city).Include(x => x.province).Include(x => x.area).Include(x => x.user).FirstOrDefault(m => m.id == noticeId);
            return visitViewModel;
        }
    }
}
