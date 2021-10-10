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
    public interface ILine
    {
        int CreateOrUpdate(LineViewModel model, IFormFile[] _File);
        Line GetById(int Id);
        void Remove(Line model);
        void RemoveFile(int Id);
        List<Line> GetAll();
        List<Line> GetAll(int userId,SearchLineViewModel searchLineViewModel);
        List<Reserve> GetReservedLines(int userId, SearchReserveViewModel  searchReserveViewModel);
        List<Reserve> GetReservedClasses(int userId, SearchClassRoomReserveViewModel  searchClassRoomReserveViewModel);

        List<Line> GetAllHome();
        Line GetByIdForDetail(int Id, string ip);

        void  ChangeStatusToSuccess(int id);
        List<Line> GetRelated(int id);
         PagedList<Line> GetLines(int pageId = 1, bool LineIsService = false);

        List<Line> CheapestLines();
        List<Line> GetAllRegisterByUser(int userId);

        PagedList<Line> GetLines(int page = 1, string filterTitle = "");
        int CreateOrUpdateFromAdmin(LineViewModelAdmin lineViewModelAdmin, IFormFile[] file);
        LineViewModelAdmin GetForDetailPage(int id);
        int CountRegisterLine(int userId);
        List<Line> GetAllHomeIsService();

    }
}
