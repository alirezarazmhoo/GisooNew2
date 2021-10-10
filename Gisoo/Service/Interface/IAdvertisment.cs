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
    public interface IAdvertisment
    {
        PagedList<Advertisment> GetAdvertisments(bool? filterisWorkshop,int pageId = 1, string filterTitle = "");
        
        object GetAdvertisments(int page = 1);
        //object GetAdvertismentsByCatAndType(AdvertismentSearch AdvertismentSearch);
        object GetLastAdvertisments(int page = 1);
        //int AddAdvertismentFromAdmin(CreateAdvertismentViewModel Advertisment);
        int AddAdvertisment(Advertisment Advertisment);
        //void EditAdvertisment(CreateAdvertismentViewModel Advertisment);
        object GetAdvertisments(int page = 1, int pagesize = 10);
        Advertisment GetById(int Id);
        int CreateOrUpdate(AdvertismentViewModel model, IFormFile _File1, IFormFile _File2, IFormFile _File3, bool fromAdmin = false);

        List<Advertisment> GetAll(int userId, SearchAdvertismentViewModel searchAdvertismentViewModel);
        void RemoveFile(int Id, string imageName);
        void Remove(Advertisment model);
        List<Advertisment> GetAllHome();
        Advertisment GetByIdForDetail(int Id, string ip);
        List<Advertisment> GetRelated(int id);
        PagedList<Advertisment> GetAdvertismentList(int pageId = 1,bool isWorkshop=false);
        List<Advertisment> IsWorkshopAdvertisments(bool isWorkshop=false);
        List<Advertisment> GetAllRegisterByUser(int userId);
        void ChangeStatus(int Id, int UserId, string totalAmount);
        void Extent(int Id, int UserId, string totalAmount);
        void Ladder(int Id, int UserId, string totalAmount);
        List<Advertisment> GetAllHomeisNotWorkshop();

    }
}
