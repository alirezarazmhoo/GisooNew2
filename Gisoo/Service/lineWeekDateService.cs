using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.Utility;
using System.Globalization;
using static DateTimeHelper;

namespace Gisoo.Service
{
    public class lineWeekDateService : ILineWeekDate
    {
        private Context _context;
        private readonly IUser _Iuser;

        private readonly IHostingEnvironment environment;

        public lineWeekDateService(Context context, IHostingEnvironment environment, IUser user)
        {
            this.environment = environment;
            _context = context;
            _Iuser = user;

        }
        public void Create(List<string> allShanbeh, List<string> allyekShanbeh, List<string> alldoShanbeh, List<string> allseShanbeh, List<string> allcharShanbeh, List<string> allpanjShanbeh, List<string> alljome, int lineId, string dateReserve, int mounthCount)
        {
            List<LineWeekDate> lineWeekDates = new List<LineWeekDate>();
            for (int i = 0; i < 30 * mounthCount; i++)
            {
                var a = DateTimeHelper.PersionDayOfWeek(DateTime.Now.AddDays(i));

                if (a == PersianDayOfWeek.Shanbe)
                {
                    for (int j = 0; j < allShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = allShanbeh[j];
                        lineWeekDate.toTime = allShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Yekshanbe)
                {
                    for (int j = 0; j < allyekShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = allyekShanbeh[j];
                        lineWeekDate.toTime = allyekShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Doshanbe)
                {
                    for (int j = 0; j < alldoShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = alldoShanbeh[j];
                        lineWeekDate.toTime = alldoShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Seshanbe)
                {
                    for (int j = 0; j < allseShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = allseShanbeh[j];
                        lineWeekDate.toTime = allseShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Charshanbe)
                {
                    for (int j = 0; j < allcharShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = allcharShanbeh[j];
                        lineWeekDate.toTime = allcharShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Panjshanbe)
                {
                    for (int j = 0; j < allpanjShanbeh.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = allpanjShanbeh[j];
                        lineWeekDate.toTime = allpanjShanbeh[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
                if (a == PersianDayOfWeek.Jome)
                {
                    for (int j = 0; j < alljome.Count(); j += 2)
                    {
                        LineWeekDate lineWeekDate = new LineWeekDate();
                        lineWeekDate.lineId = lineId;
                        DateTime date = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(dateReserve));
                        lineWeekDate.date = date.AddDays(i);
                        lineWeekDate.fromTime = alljome[j];
                        lineWeekDate.toTime = alljome[j + 1];
                        lineWeekDates.Add(lineWeekDate);
                    }
                }
            }
            _context.LineWeekDates.AddRange(lineWeekDates);
            _context.SaveChanges();

        }
        public LineWeekDate GetById(string Id)
        {
            var Items = _context.LineWeekDates.Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }

        public List<LineWeekDate> GetForEdit(int lineId, string FromdateReserve, string TodateReserve)
        {
            IQueryable<LineWeekDate> result = _context.LineWeekDates.Where(x => x.lineId == lineId).OrderBy(x => x.date).ThenBy(x=>x.fromTime);
            if (!String.IsNullOrEmpty(FromdateReserve))
            {
                result = result.Where(x => x.date.Date >= DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(FromdateReserve)));
            }
            else
                result = result.Where(x => x.date.Date >= DateTime.Now.Date);

            if (!String.IsNullOrEmpty(TodateReserve))
            {
                result = result.Where(x => x.date.Date <= DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(TodateReserve)));
            }
            else
                result = result.Where(x => x.date.Date <= DateTime.Now.AddDays(31).Date);
            return result.ToList();
        }

        public int Delete(string lineWeekDateId)
        {
            var lineWeekDate = _context.LineWeekDates.FirstOrDefault(x => x.id == lineWeekDateId);
            _context.LineWeekDates.Remove(lineWeekDate);
            _context.SaveChanges();
            return lineWeekDate.lineId;
        }
        public void ReserveLine(string allIds, Line line, User user, bool ifFromKif = false)
        {
            var userLineOwner = _Iuser.GetByIdUser(line.userId);
            string[] values = allIds.Split(',');
            long price = 0;
            if (line.discountPrice != null && line.discountPrice != 0)
                price = Convert.ToInt64(line.discountPrice);
            else
                price = Convert.ToInt64(line.price);
            long totalprice = values.Count() * price;
            List<Reserve> reserves = new List<Reserve>();
            foreach (var item in values)
            {
                var lineWeekDate = GetById(item);
                lineWeekDate.isReserved = true;
                Reserve reserve = new Reserve();
                reserve.date = DateTime.Now;
                reserve.LineWeekDateId = item;
                reserve.price = price;
                reserve.userId = user.id;
                reserve.userIdNoticeOwner =userLineOwner.id ;
                reserves.Add(reserve);
            }
            Factor factor = new Factor();
            factor.state = State.IsPay;
            factor.userId = user.id;
            factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);
            factor.factorKind = FactorKind.Add;
            factor.totalPrice = totalprice;
            factor.LineWeekDateId = allIds;
            if (ifFromKif)
            {
                user.score -= totalprice;
            }
            userLineOwner.score += totalprice;
            _context.Factors.Add(factor);
            _context.Reserves.AddRange(reserves);
            _context.SaveChanges();
        }
    }
}

