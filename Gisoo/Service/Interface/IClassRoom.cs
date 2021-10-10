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
    public interface IClassRoom
    {
        void CreateOrUpdate(ClassRoomViewModel model, IFormFile[] _File);
        ClassRoom GetById(int Id);
        void Remove(ClassRoom model);
        void RemoveFile(int Id);
        List<ClassRoom> GetAll(int userId, SearchClassRoomViewModel searchClassRoomViewModel);
         List<ClassRoom> GetAllHome();
        ClassRoom GetByIdForDetail(int Id, string ip);
        List<ClassRoom> GetRelated(int id);
        PagedList<ClassRoom> GetClassRooms(int pageId = 1, bool isReserves = false);
        List<ClassRoom> CheapestClassRooms();
        List<ClassRoom> GetAllRegisterByUser(int userId);
        PagedList<ClassRoom> GetClassRooms(int page = 1, string filterTitle = "");
        void CreateOrUpdateFromAdmin(ClassRoomViewModelAdmin ClassRoomViewModelAdmin, IFormFile[] file);
        ClassRoomViewModelAdmin GetForDetailPage(int id);
        int CountRegisterClass(int userId);
        List<ClassRoom> GetAllHomeReserve();
        void Reserve(ClassRoom ClassRoom, User user, bool ifFromKif = false);


    }
}
