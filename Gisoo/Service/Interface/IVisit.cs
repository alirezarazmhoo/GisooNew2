using Gisoo.Models;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
    public interface IVisit
    {

       VisitViewModel GetByAllNoticeId(int noticeId, WhichTableEnum whichTableEnum);
        VisitViewModel GetByAllNoticeIdForAdmin(int noticeId, WhichTableEnum whichTableEnum);
        
    }
}
