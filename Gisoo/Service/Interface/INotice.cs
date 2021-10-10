using Gisoo.Models;
using Gisoo.Models.Enums;
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
    public interface INotice
    {
        PagedList<Notice> GetNotices(ConditionEnum? filtercondition,bool? filterisBarber,int pageId = 1, string filterTitle = "");

        object GetNotices(int page = 1);
        //object GetNoticesByCatAndType(NoticeSearch NoticeSearch);
        object GetLastNotices(int page = 1);
        //int AddNoticeFromAdmin(CreateNoticeViewModel Notice);
        int AddNotice(Notice Notice);
        //void EditNotice(CreateNoticeViewModel Notice);
        object GetNotices(int page = 1, int pagesize = 10);
        int CreateOrUpdate(Notice2ViewModel model, IFormFile _File1, IFormFile _File2, IFormFile _File3, bool fromAdmin = false);

        Notice GetById(int Id);
        List<Notice> GetAll(int userId, SearchNoticeViewModel searchNoticeViewModel);
        void Remove(Notice model);
         List<Notice> GetAllHome();
        Notice GetByIdForDetail(int Id,string ip);
        List<Notice> GetRelated(int id);
        PagedList<Notice> GetNoticeList(int pageId = 1,bool isBarber=false);
        List<Notice> PercentNotices();
        List<Notice> RentNotices();
        List<Notice> FixedSalaryNotices();
        List<Notice> GetAllRegisterByUser(int userId);
        void ChageState(int Id, int UserId, string totalAmount);

        void ReserveRegister(NoticeType type , int UserId , string totalAmount ,  int ItemId);
        void Extent(int Id, int UserId, string totalAmount);
        void Ladder(int Id, int UserId, string totalAmount);
        List<Notice> GetAllHomeNotBarber();
        List<Notice> GetAllNotBarber();

        //object GetNoticesByTitle(NoticeSearch2 NoticeSearch);
        //object AddNoticeToFactor(BuyNotice buyNotice);
        //object GetFactorOfUser(GetFactor getFactor);
        //object GetLinkOfNotices(GetLinkOfNotice getLinkOfNotice);
        //object GetFactorItems(AllFactorItem allFactorItem);
    }
}
