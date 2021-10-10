using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SmsIrRestfulNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zarinpal;

namespace Gisoo.Service
{
    public class userService : IUser
    {
        private readonly Context _context;
        private readonly IHostingEnvironment environment;
        public userService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;

            _context = context;
        }
        public IQueryable<User> GetAllUsers()
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(x => x.userStatus).Include(x => x.role);
            return result;
        }
        public List<User> GetByUserRole(IQueryable<User> users, string roleNameEnglish)
        {
            Random rand = new Random();
            List<User> result = users.Where(x => x.role.RoleNameEn == roleNameEnglish && x.userStatus).ToList();

            List<int> getAllId = new List<int>();
            List<User> HelperList = new List<User>();
            int reapet = result.Count() > 20 ? 20 : result.Count();
            List<int> listNumbers = new List<int>();
            int number;
            for (int i = 0; i < reapet; i++)
            {
                do
                {
                    number = rand.Next(0, result.Count());
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                if (result.ElementAt(number) != null)
                    HelperList.Add(result.ElementAt(number));
            }
            return HelperList.Take(20).ToList();
        }
        public PagedList<User> GetUsers(string rolenameEn, int pageId = 1,bool isProfileComplete=false, string filtercellphone = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(x => x.role.RoleNameEn == rolenameEn).OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filtercellphone))
            {
                result = result.Where(x => x.cellphone.Contains(filtercellphone));
            }
            if(isProfileComplete==true)
                result = result.Where(x => x.isProfileComplete==true);

            PagedList<User> res = new PagedList<User>(result, pageId, 10);
            return res;
        }
        public User GetUser(string cellphone)
        {
            User result = _context.Users.Include(x => x.role).Include(x => x.UserDocumentImages).IgnoreQueryFilters().FirstOrDefault(x => x.role.RoleNameEn != "Admin" && x.cellphone == cellphone);
            return result;
        }
        public User GetByIdUser(int userId)
        {
            User result = _context.Users.Include(x => x.role).IgnoreQueryFilters().FirstOrDefault(x => x.role.RoleNameEn != "Admin" && x.id == userId);
            return result;
        }
        public Tuple<bool, string> LoginUserConfirmCode(string cellphone)
        {
            User u = GetUser(cellphone);
            if (u == null)
            {
                return Tuple.Create(false, "چنین شماره همراهی در سیستم وجود ندارد");
            }
            //if (u.userStatus == false)
            //{
            //    return Tuple.Create(false, "کاربری شما تایید نشده است");
            //}
            Random random = new Random();
            string code = random.Next(1000, 9999).ToString();
            u.code = code;
            _context.SaveChanges();
            try
            {
                SendSms.CallSmSMethod(Convert.ToInt64(u.cellphone), 17464, "VerificationCode", u.code);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "خطا در ارسال پیامک");

            }
            return Tuple.Create(true, "");
        }

        public Tuple<bool, string> Register(UserViewModel model)
        {
            User user = new User();
            var propInfo = model.GetType().GetProperties();
            foreach (var item in propInfo)
            {
                if (item.Name == "id" || item.Name == "returnUrl" || item.Name == "isNoticeShortcut" || item.Name == "isAdverstitment")
                    continue;
                user.GetType().GetProperty(item.Name).SetValue(user, item.GetValue(model, null), null);
            }
            user.token = Guid.NewGuid().ToString().Replace('-', '0');
            if(user.roleId==7 || user.roleId==2)
            user.userStatus = true;
            else
            user.userStatus = false;
            Random random = new Random();
            user.identifiecodeOwner = random.Next(100000, 999999).ToString();
            var allPrice = _context.AllPrices.FirstOrDefault();
            if (!String.IsNullOrEmpty(model.identifiecode))
            {
                if (allPrice.scoreWithInturducer != null)
                {
                    user.score = user.score + Convert.ToInt64(allPrice.scoreWithInturducer);
                    var userInturducer = _context.Users.FirstOrDefault(x => x.identifiecodeOwner == user.identifiecode);
                    userInturducer.score = userInturducer.score + Convert.ToInt64(allPrice.scoreWithInturducer);
                    user.isscored = true;
                }
            }
            string code = random.Next(1000, 9999).ToString();
            user.code = code;
            user.expireDateAccount = DateTime.Now.AddDays(-1);
            user.sexuality = model.sexuality;
            _context.Users.Add(user);
            _context.SaveChanges();
            try
            {
                SendSms.CallSmSMethod(Convert.ToInt64(user.cellphone), 17464, "VerificationCode", user.code);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "خطا در ارسال پیامک");
            }
            return Tuple.Create(true, "");
        }

        public bool CheckMobile(string item)
        {
            if (_context.Users.Any(s => s.cellphone == item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCodeIdentifie(string item)
        {
            if (_context.Users.Any(s => s.identifiecodeOwner == item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckNationalCode(string item)
        {
            if (_context.Users.Any(s => s.nationalCode == item))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int TotalNotice(int Id)
        {

            if (Id != 0)
            {
                return _context.Notices.Where(s => s.userId == Id).Count();
            }
            else
            {
                return 0;
            }


        }
        public int TotalAdvertisments(int Id)
        {
            if (Id != 0)
            {
                return _context.Advertisments.Where(s => s.userId == Id).Count();
            }
            else
            {
                return 0;
            }
        }
        public int TotalLines(int Id)
        {
            if (Id != 0)
            {
                return _context.Lines.Where(s => s.userId == Id).Count();
            }
            else
            {
                return 0;
            }
        }

        public int TotalClassRooms(int Id)
        {
            if (Id != 0)
            {
                return _context.ClassRooms.Where(s => s.userId == Id).Count();
            }
            else
            {
                return 0;
            }
        }


        public int TotalReserve(int Id)
        {
            if (Id != 0)
            {
                return _context.Reserves.Where(s => s.userId == Id).Count();
            }
            else
            {
                return 0;
            }
        }
        public void Update(ProfileViewModel model)
        {
            var UserIitem = _context.Users.FirstOrDefault(s => s.id == model.id);
            if (UserIitem != null)
            {
                UserIitem.fullname = model.fullname;
                UserIitem.nationalCode = model.nationalCode;
                UserIitem.address = model.address;
                UserIitem.hasCertificate = model.hasCertificate;
                UserIitem.longitude = model.longitude;
                UserIitem.latitude = model.latitude;
                UserIitem.shortDescription = model.shortDescription;
                UserIitem.longDescription = model.longDescription;
                UserIitem.workingHours = model.workingHours;
                UserIitem.linkInstagram = model.linkInstagram;
                UserIitem.linkTelegram = model.linkTelegram;
                UserIitem.shebaNumber = model.shebaNumber;
                UserIitem.sexuality = model.sexuality;
                UserIitem.isProfileComplete = true;
                if (model.imageUrl != null)
                {
                    string deletePath = environment.WebRootPath + UserIitem.url;
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                    string imagePath = "";
                    UserIitem.url = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(model.imageUrl.FileName);
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Profile", UserIitem.url);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        model.imageUrl.CopyTo(stream);
                    }
                    UserIitem.url = "/images/Profile/" + UserIitem.url;
                }
                if (model.imageUrldocuments != null && model.imageUrldocuments.Count() > 0)
                {
                    foreach (var item in model.imageUrldocuments)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\Profile", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.UserDocumentImages.Add(new UserDocumentImage()
                            {
                                url = fileName,
                                userId = model.id

                            });
                        }
                    }
                }
                _context.SaveChanges();
            }
        }

        public void RemoveFile(int Id)
        {
            var Item = _context.UserDocumentImages.FirstOrDefault(s => s.id == Id);
            if (Item != null)
            {
                File.Delete($"wwwroot/images/Profile/{Item.url}");
            }
            _context.UserDocumentImages.Remove(Item);
            _context.SaveChanges();
        }
        public PagedList<User> GetUserList(string userRolaNameEng, int pageId = 1)
        {
            IQueryable<User> result = _context.Users.Where(x => x.userStatus && x.role.RoleNameEn == userRolaNameEng).OrderByDescending(x => x.id);
            PagedList<User> res = new PagedList<User>(result, pageId, 10);
            return res;
        }
        public void ChangeRole(User UserItem, string roleNameEng)
        {
            int roleId = _context.Roles.FirstOrDefault(x => x.RoleNameEn == roleNameEng).Id;
            UserItem.roleId = roleId;
            UserItem.expireDateAccount = DateTime.Now;
            _context.SaveChanges();
        }



    }
}
