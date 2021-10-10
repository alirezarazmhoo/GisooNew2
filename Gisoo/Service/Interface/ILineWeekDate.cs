using Gisoo.Models;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
    public interface ILineWeekDate
    {
        void Create(List<string> allShanbeh, List<string> allyekShanbeh, List<string> alldoShanbeh, List<string> allseShanbeh, List<string> allcharShanbeh, List<string> allpanjShanbeh, List<string> alljome, int lineId, string dateReserve, int mounthCount);
        List<LineWeekDate> GetForEdit(int lineId,string FromdateReserve,string TodateReserve);
        int Delete(string lineWeekDateId);
        void ReserveLine(string allIds, Line line, User user,bool ifFromKif=false);
        LineWeekDate GetById(string Id);

    }
}
