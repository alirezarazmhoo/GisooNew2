using Gisoo.Models;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface IUser
    {
        PagedList<User> GetUsers(string rolenameEn,int pageId = 1,bool isProfileComplete=false, string filterFullName = "");
        Tuple<bool, string> Register(UserViewModel model);

        bool CheckMobile(string item);
        bool CheckNationalCode(string item);
        User GetUser(string cellphone);
        Tuple<bool, string> LoginUserConfirmCode(string cellphone);
        int TotalNotice(int Id);
        int TotalAdvertisments(int Id);
        int TotalLines(int Id);
        int TotalClassRooms(int Id);
        int TotalReserve(int Id);
        User GetByIdUser(int userId);

        void Update(ProfileViewModel model);
        void RemoveFile(int Id);
        bool CheckCodeIdentifie(string item);
        IQueryable<User> GetAllUsers();
        List<User> GetByUserRole(IQueryable<User> users, string roleNameEnglish);
        PagedList<User> GetUserList(string userRolaNameEng, int pageId = 1);
        //void SubScore(User user, long totalPrice);
        //void ChargeAccount(User userId, int mounthCount);

        void ChangeRole(User UserItem, string roleNameEng);

    }
}
