using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gisoo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Gisoo.Service.Interface;
using Gisoo.DAL;
using Microsoft.EntityFrameworkCore;
using Gisoo.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Gisoo.Utility;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Gisoo.Models.Enums;
using System.Data.SqlClient;
using PagedList.Core;
using Zarinpal;
using Microsoft.AspNetCore.Mvc.Routing;
using static DateTimeHelper;

namespace Gisoo.Controllers
{

    public class CustomerHomeController : Controller
    {
        private readonly IUser _Iuser;
        private readonly IRole _Irole;
        private readonly ILineType _ILineType;
        private readonly ILine _ILine;
        private readonly ICity _ICity;
        private readonly IAdvertisment _IAdvertisment;
        private readonly INotice _INotice;
        private readonly IClassRoom _IClassRoom;
        private readonly IClassRoomType _IClassRoomType;
        private readonly IProduct _IProduct;
        private readonly ISlider _ISlider;
        private readonly IBanner _IBanner;
        private readonly IVisit _IVisit;
        private readonly IHostingEnvironment environment;
        private readonly IActionContextAccessor _accessor;
        private readonly IContactUs _contactUs;
        private readonly IAboutUs _aboutUs;
        private readonly IRule _rule;
        private readonly IReserve _reserve;
        private readonly ILineWeekDate _LineWeekDate;
        private readonly IArticle _Article;
        private readonly Context _context;
        public CustomerHomeController(IUser user, IRole role, ILineType lineType, ILine line, ICity city, IAdvertisment advertisment, INotice notice, IClassRoom classRoom, IClassRoomType classRoomType, IProduct product, ISlider slider, IBanner banner, ILineWeekDate lineWeekDate, IActionContextAccessor accessor, IHostingEnvironment environment, IVisit Visit, IReserve reserve, IContactUs ContactUs, IAboutUs AboutUs, IRule Rule, IArticle Article, Context context)
        {
            this.environment = environment;
            _Iuser = user;
            _Irole = role;
            _ILineType = lineType;
            _ILine = line;
            _ICity = city;
            _IAdvertisment = advertisment;
            _INotice = notice;
            _IClassRoom = classRoom;
            _IClassRoomType = classRoomType;
            _IProduct = product;
            _ISlider = slider;
            _IBanner = banner;
            _accessor = accessor;
            _context = context;
            _IVisit = Visit;
            _aboutUs = AboutUs;
            _contactUs = ContactUs;
            _rule = Rule;
            _reserve = reserve;
            _Article = Article;
            _LineWeekDate = lineWeekDate;
        }
        public IActionResult Index(bool isRegisterSuccess = false, string PaySuccess = "")
        {

            if (isRegisterSuccess)
                ViewBag.isRegisterSuccess = true;
            ViewBag.PaySuccess = PaySuccess;
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Sliders = _ISlider.GetSlidersWebSite();
            //homeViewModel.Lines = _ILine.GetAllHome();
            //homeViewModel.LineIsService = _ILine.GetAllHomeIsService();
            homeViewModel.Products = _IProduct.GetAllHome();
            homeViewModel.Notices = _INotice.GetAllHome();
            homeViewModel.NoticeNotBarbers = _INotice.GetAllHomeNotBarber();
            homeViewModel.Advertisments = _IAdvertisment.GetAllHome();
            homeViewModel.AdvertismentNotWorkshops = _IAdvertisment.GetAllHomeisNotWorkshop();
            //homeViewModel.ClassRooms = _IClassRoom.GetAllHome();
            //homeViewModel.ClassRoomReserves = _IClassRoom.GetAllHomeReserve();
            homeViewModel.Banners = _IBanner.GetAllHome();
            homeViewModel.Articles = _Article.GetArticlesFirstPage();
            var user = _Iuser.GetAllUsers();
            homeViewModel.SalonOwnerUsers = _Iuser.GetByUserRole(user, "SalonOwner");
            homeViewModel.AcademyUsers = _Iuser.GetByUserRole(user, "Academy");
            homeViewModel.HairStylistUsers = _Iuser.GetByUserRole(user, "HairStylist");
            homeViewModel.MentorUsers = _Iuser.GetByUserRole(user, "Mentor");
            homeViewModel.StoreUsers = _Iuser.GetByUserRole(user, "Store");
            return View(homeViewModel);
        }
        public IActionResult SelectUserType()
        {

            return View(_Irole.Get());
        }
        public IActionResult Login(string returnUrl, string textRegister = "", int isNoticeShortcut = 0, int isAdverstitment = 0)
        {
            UserViewModel model = new UserViewModel();
            ViewBag.textRegister = textRegister;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                model.returnUrl = returnUrl;
            }
            model.isNoticeShortcut = isNoticeShortcut;
            model.isAdverstitment = isAdverstitment;
            return View(model);
        }
        [Route("LogoutCustomer")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/CustomerHome/Index");
        }
        //public IActionResult Register(int? type, int isnoticeshourtcut = 0)
        [HttpGet]
        public IActionResult Register(UserViewModel model)
        {
            if (model.roleId == 0)
            {
                return RedirectToAction(nameof(SelectUserType));
            }

            //ViewBag.UseType = type;

            //ViewBag.isnoticeshourtcut = isnoticeshourtcut;


            return View(model);
        }
        [HttpPost]
        public IActionResult Register(UserViewModel model, string a)
        {
            try
            {

                if (_Iuser.CheckMobile(model.cellphone))
                {
                    ModelState.AddModelError("", "شماره همراه تکراری است");
                    return View(model);
                }
                if (_Iuser.CheckNationalCode(model.nationalCode))
                {
                    ModelState.AddModelError("", "کد ملی تکراری است");
                    return View(model);
                }
                if (!String.IsNullOrEmpty(model.identifiecode))
                {
                    if (!_Iuser.CheckCodeIdentifie(model.identifiecode))
                    {
                        ModelState.AddModelError("", "چنین کد معرفی در سیستم وجود ندارد");
                        return View(model);
                    }
                }
                if (ModelState.IsValid)
                {
                    if (model.cellphone.Substring(1, 1) == "۹")
                        model.cellphone = DateChanger.PersianToEnglish(model.cellphone);
                    Tuple<bool, string> result = _Iuser.Register(model);
                    if (result.Item1 == false)
                        return RedirectToAction("Login", "CustomerHome", new { textRegister = "کاربری شما ثبت گردید ولی پیامک ارسال نشد لطفا مجددا درخواست ارسال پیامک دهید" });

                }

                return RedirectToAction("Login", "CustomerHome", new { textRegister = "کاربری شما ثبت گردید منتظر دریافت پیامک کد تایید باشید" });
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        public IActionResult Academy(bool isRegisterClassRoom = false, bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0)
        {
            ClassRoomViewModel classRoomViewModel = new ClassRoomViewModel();
            classRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
            ViewBag.isRegisterClassRoom = isRegisterClassRoom;
            ViewBag.subScore = subScore;
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            if (!UserItem.role.RoleNameEn.Equals("Academy"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.TotalCountRegisterClass = _IClassRoom.CountRegisterClass(UserItem.id);
                ViewBag.expireDate = UserItem.expireDateAccount;
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
                ViewBag.isProfileAccept = UserItem.isProfileAccept;
                ViewBag.isProfileComplete = UserItem.isProfileComplete;

            }
            return View(classRoomViewModel);
        }
        public IActionResult Member(bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0)
        {
            LineViewModel lineViewModel = new LineViewModel();
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            ViewBag.subScore = subScore;
            if (!(UserItem.role.RoleNameEn.Equals("Member")))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
            }
            return View(lineViewModel);
        }
        #region SalonOwner
        public IActionResult SalonOwner(bool isRegisterLine = false, bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0, bool isReserveWeek = false, int lineId = 0, bool ChargeFree = false)
        {
            LineViewModel lineViewModel = new LineViewModel();
            lineViewModel.LineTypes = _ILineType.GetLineTypes();
            ViewBag.isRegisterLine = isRegisterLine;
            ViewBag.ChargeFree = ChargeFree;
            ViewBag.subScore = subScore;
            ViewBag.isReserveWeek = isReserveWeek;
            ViewBag.lineId = lineId;
            var UserItem = _Iuser.GetUser(User.Identity.Name);

            if (!(UserItem.role.RoleNameEn.Equals("SalonOwner")))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.TotalCountRegisterLine = _ILine.CountRegisterLine(UserItem.id);
                ViewBag.expireDate = UserItem.expireDateAccount;
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
                ViewBag.isProfileAccept = UserItem.isProfileAccept;
                ViewBag.isProfileComplete = UserItem.isProfileComplete;
                var allprice = _context.AllPrices.FirstOrDefault();
                if (UserItem.isBuyOneMonth == true)
                    ViewBag.totalprice = Convert.ToInt64(allprice.threeSubscriptionPrice);
                else
                    ViewBag.totalprice = Convert.ToInt64(allprice.oneSubscriptionPrice);
            }
            return View(lineViewModel);
        }

        public IActionResult RegisterAdvertisment(int? Id)
        {
            AdvertismentViewModel AdvertismentViewModel = new AdvertismentViewModel();
            if (Id.HasValue)
            {
                Advertisment Item = _IAdvertisment.GetById(Id.Value);
                if (Item != null)
                {
                    AdvertismentViewModel.title = Item.title;
                    AdvertismentViewModel.description = Item.description;
                    AdvertismentViewModel.provinces = _ICity.GetProvincesList(Item.cityId);
                    AdvertismentViewModel.areas = _ICity.GetAreasList(Item.areaId);
                    AdvertismentViewModel.image1 = Item.image1;
                    AdvertismentViewModel.image2 = Item.image2;
                    AdvertismentViewModel.image3 = Item.image3;
                    AdvertismentViewModel.id = Item.id;
                    AdvertismentViewModel.cityId = Item.cityId;
                    AdvertismentViewModel.provinceId = Item.provinceId;
                    AdvertismentViewModel.areaId = Item.areaId;
                    AdvertismentViewModel.isWorkshop = Item.isWorkshop;
                    AdvertismentViewModel.isDeleted = Item.isDeleted;
                    AdvertismentViewModel.adminConfirmStatus = Item.adminConfirmStatus;
                }
            }

            AdvertismentViewModel.cities = _ICity.GetCitiesList();
            return PartialView("_RegisterAdvertismentPartial", AdvertismentViewModel);
        }
        public IActionResult RegisterNotice(int? Id)
        {
            Notice2ViewModel Notice2ViewModel = new Notice2ViewModel();
            if (Id.HasValue)
            {
                Notice Item = _INotice.GetById(Id.Value);
                if (Item != null)
                {
                    Notice2ViewModel.title = Item.title;
                    Notice2ViewModel.description = Item.description;
                    Notice2ViewModel.provinces = _ICity.GetProvincesList(Item.cityId);
                    Notice2ViewModel.areas = _ICity.GetAreasList(Item.areaId);
                    Notice2ViewModel.image1 = Item.image1;
                    Notice2ViewModel.image2 = Item.image2;
                    Notice2ViewModel.image3 = Item.image3;
                    Notice2ViewModel.id = Item.id;
                    Notice2ViewModel.cityId = Item.cityId;
                    Notice2ViewModel.provinceId = Item.provinceId;
                    Notice2ViewModel.areaId = Item.areaId;
                    Notice2ViewModel.isBarber = Item.isBarber;
                    Notice2ViewModel.isDeleted = Item.isDeleted;
                    Notice2ViewModel.adminConfirmStatus = Item.adminConfirmStatus;
                    Notice2ViewModel.condition = Item.condition;
                }
            }

            Notice2ViewModel.cities = _ICity.GetCitiesList();
            return PartialView("_RegisterNoticePartial", Notice2ViewModel);
        }
        public IActionResult RegisterLine(int? Id)
        {
            LineViewModel lineViewModel = new LineViewModel();
            var allPrice = _context.AllPrices.FirstOrDefault();
            long? minDiscount = allPrice.minDiscount == null ? 0 : allPrice.minDiscount;
            long? maxDiscount = allPrice.maxDiscount == null ? 0 : allPrice.maxDiscount;

            if (Id.HasValue)
            {
                Line Item = _ILine.GetById(Id.Value);

                if (Item != null)
                {
                    lineViewModel.title = Item.title;
                    lineViewModel.description = Item.description;
                    lineViewModel.lineLaw = Item.lineLaw;
                    lineViewModel.price = Item.price;
                    lineViewModel.LineImage = Item.LineImages.ToList();
                    lineViewModel.LineTypes = _ILineType.GetLineTypes();
                    lineViewModel.lineTypeId = Item.lineTypeId;

                }
            }
            else
            {
                lineViewModel.LineTypes = _ILineType.GetLineTypes();

            }
            lineViewModel.minDiscount = Convert.ToInt64(minDiscount);
            lineViewModel.maxDiscount = Convert.ToInt64(maxDiscount);
            return PartialView("_RegisterLinePartial", lineViewModel);
        }
        public IActionResult RegisterLineForReserve(int? Id)
        {
            LineViewModel lineViewModel = new LineViewModel();
            var allPrice = _context.AllPrices.FirstOrDefault();
            long? minDiscount = allPrice.minDiscount == null ? 0 : allPrice.minDiscount;
            long? maxDiscount = allPrice.maxDiscount == null ? 0 : allPrice.maxDiscount;

            if (Id.HasValue)
            {
                Line Item = _ILine.GetById(Id.Value);
                if (Item != null)
                {
                    lineViewModel.title = Item.title;
                    lineViewModel.description = Item.description;
                    lineViewModel.lineLaw = Item.lineLaw;
                    lineViewModel.price = Item.price;
                    lineViewModel.discountPrice = Item.discountPrice;
                    lineViewModel.LineImage = Item.LineImages.ToList();
                    lineViewModel.LineTypes = _ILineType.GetLineTypes();
                    lineViewModel.reserveHour = Item.reserveHour;
                    lineViewModel.reserveDate = Item.reserveDate.ToString();
                    lineViewModel.lineTypeId = Item.lineTypeId;
                    lineViewModel.lineTeacher = Item.lineTeacher;
                }
            }
            else
            {
                lineViewModel.LineTypes = _ILineType.GetLineTypes();
            }
            lineViewModel.minDiscount = Convert.ToInt64(minDiscount);
            lineViewModel.maxDiscount = Convert.ToInt64(maxDiscount);
            return PartialView("_RegisterLineForReservePartial", lineViewModel);
        }
        public string GetProvince(int cityId)
        {
            var provinces = _ICity.GetProvincesList(cityId);
            string str = "<option value=''>انتخاب کنید</option>";
            foreach (var item in provinces)
            {
                str += "<option value = " + item.id + "> " + item.name + "</option>";
            }
            return str;
        }
        public string GetArea(int provinceId)
        {
            var areas = _ICity.GetAreasList(provinceId);
            string str = "<option value=''>انتخاب کنید</option>";
            foreach (var item in areas)
            {
                str += "<option value = " + item.id + "> " + item.name + "</option>";
            }
            return str;
        }

        public IActionResult AllLine(SearchLineViewModel searchLineViewModel)
        {
            //SearchLineViewModel searchLineViewModel = new SearchLineViewModel();
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            searchLineViewModel.LineTypes = _ILineType.GetLineTypes();

            searchLineViewModel.Lines = _ILine.GetAll(userId, searchLineViewModel);
            return PartialView("_AllLinePartial", searchLineViewModel);
        }
        public IActionResult AllAdvertisment(SearchAdvertismentViewModel searchAdvertismentViewModel)
        {
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            searchAdvertismentViewModel.Advertisments = _IAdvertisment.GetAll(userId, searchAdvertismentViewModel);
            return PartialView("_AllAdvertismentPartial", searchAdvertismentViewModel);
        }
        public IActionResult AllNotice(SearchNoticeViewModel searchNoticeViewModel)
        {
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            searchNoticeViewModel.Notices = _INotice.GetAll(userId, searchNoticeViewModel);
            return PartialView("_AllNoticePartial", searchNoticeViewModel);
        }

        [HttpPost]
        public IActionResult RegisterLine(LineViewModel lineViewModel, IFormFile[] file)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            lineViewModel.userId = user.id;
            var compareDate = DateTime.Compare((DateTime)user.expireDateAccount, @DateTime.Now);
            int countLine = _ILine.CountRegisterLine(lineViewModel.userId);
            if (countLine > 20 || compareDate < 0)
                return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterLine = true });
            _ILine.CreateOrUpdate(lineViewModel, file);
            return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterLine = true });
        }
        [HttpPost]
        public IActionResult RegisterLineForReserve(LineViewModel lineViewModel, IFormFile[] file)
        {
            lineViewModel.userId = _Iuser.GetUser(User.Identity.Name).id;
            lineViewModel.lineLaw = EnumLineLaw.reserve;
            int lineId = _ILine.CreateOrUpdate(lineViewModel, file);
            if (lineViewModel.id != 0)
                return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterLine = true });
            else
                return RedirectToAction("SalonOwner", "CustomerHome", new { isReserveWeek = true, lineId });
        }
        public IActionResult ResearveWeek(int? lineId)
        {
            ViewBag.lineId = lineId;
            return PartialView("_ResearveWeekPartial");
        }

        [HttpPost]
        public IActionResult RegisterAdvertisment(AdvertismentViewModel advertismentViewModel, IFormFile file1, IFormFile file2, IFormFile file3)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            long totalprice = 0;
            var allprice = _context.AllPrices.FirstOrDefault();
            if (user != null)
            {
                advertismentViewModel.userId = user.id;
                if (advertismentViewModel.isWorkshop && allprice.isHasWorkshopPrice)
                {
                    totalprice = allprice.workshopPrice;
                }
                if (!advertismentViewModel.isWorkshop && allprice.isHasAdvertismentPrice)
                {
                    totalprice = allprice.advertismentPrice;
                }
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.isWorkshop = advertismentViewModel.isWorkshop;

                if (totalprice != 0)
                {
                    factorViewModel.shouldpurshe = true;
                }
                else
                {
                    factorViewModel.shouldpurshe = false;
                    factorViewModel.totalAmount = "رایگان";
                }
                if (totalprice > 0 && advertismentViewModel.id == 0)
                {
                    advertismentViewModel.isDeleted = true;
                    factorViewModel.ItemId = _IAdvertisment.CreateOrUpdate(advertismentViewModel, file1, file2, file3);
                    return RedirectToAction(nameof(FactorCreator), factorViewModel);
                }
                else
                {
                    advertismentViewModel.userId = user.id;
                    _IAdvertisment.CreateOrUpdate(advertismentViewModel, file1, file2, file3);
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterAdvertisment = true });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { isRegisterAdvertisment = true });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { isRegisterAdvertisment = true });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { isRegisterAdvertisment = true });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterAdvertisment = true });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { isRegisterAdvertisment = true });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterAdvertisment = true });
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult RegisterNotice(Notice2ViewModel notice2ViewModel, IFormFile file1, IFormFile file2, IFormFile file3)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            long totalprice = 0;
            var allprice = _context.AllPrices.FirstOrDefault();
            if (user != null)
            {
                notice2ViewModel.userId = user.id;
                if (notice2ViewModel.isBarber && allprice.isHasBarberPrice)
                    totalprice = allprice.barberPrice;
                if (!notice2ViewModel.isBarber && allprice.isHasSaloonPrice)
                    totalprice = allprice.saloonPrice;
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.isBarber = notice2ViewModel.isBarber;
                factorViewModel.isNotice = 1;

                if (totalprice != 0)
                {
                    factorViewModel.shouldpurshe = true;
                }
                else
                {
                    factorViewModel.shouldpurshe = false;
                    factorViewModel.totalAmount = "رایگان";
                }
                if (totalprice > 0 && notice2ViewModel.id == 0)
                {
                    notice2ViewModel.isDeleted = true;
                    factorViewModel.ItemId = _INotice.CreateOrUpdate(notice2ViewModel, file1, file2, file3);
                    return RedirectToAction(nameof(FactorCreator), factorViewModel);
                }
                else
                {

                    _INotice.CreateOrUpdate(notice2ViewModel, file1, file2, file3);
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { isRegisterNotice = true });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult LadderNotice(int NotId)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            var notice = _INotice.GetById(NotId);
            long totalprice = 0;
            var allprice = _context.AllPrices.FirstOrDefault();
            if (user != null)
            {
                if (notice.isBarber && allprice.isHasBarberPrice)
                    totalprice = allprice.barberPrice;
                if (!notice.isBarber && allprice.isHasSaloonPrice)
                    totalprice = allprice.saloonPrice;
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.isNotice = 1;
                factorViewModel.ItemId = NotId;

                if (totalprice == 0)
                {
                    notice.createDate = DateTime.Now;
                    _context.SaveChanges();
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { isRegisterNotice = true });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                }
                else
                {
                    _context.SaveChanges();

                    return RedirectToAction(nameof(FactorCreatorLadder), factorViewModel);

                }
            }
            else
                return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        public IActionResult LadderAdvertisment(int NotId)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            var advertisment = _IAdvertisment.GetById(NotId);
            long totalprice = 0;
            var allprice = _context.AllPrices.FirstOrDefault();
            if (user != null)
            {
                if (advertisment.isWorkshop && allprice.isHasWorkshopPrice)
                    totalprice = allprice.workshopPrice;
                if (!advertisment.isWorkshop && allprice.isHasAdvertismentPrice)
                    totalprice = allprice.advertismentPrice;
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;

                factorViewModel.ItemId = NotId;

                if (totalprice == 0)
                {
                    advertisment.createDate = DateTime.Now;
                    _context.SaveChanges();
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { isRegisterNotice = true });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { isRegisterNotice = true });
                }
                else
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(FactorCreatorLadder), factorViewModel);

                }
            }
            else
                return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        public IActionResult ExtentNotice(int NotiId, int isNotice)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);

            long totalprice = 1000;
            if (user != null)
            {
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.ItemId = NotiId;
                factorViewModel.isNotice = isNotice;
                return RedirectToAction(nameof(FactorCreatorExtent), factorViewModel);
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult HairStylist(bool isRegisterLine = false, bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0, bool isReserveWeek = false, int lineId = 0)
        {
            LineViewModel lineViewModel = new LineViewModel();
            lineViewModel.LineTypes = _ILineType.GetLineTypes();
            ViewBag.isRegisterLine = isRegisterLine;
            ViewBag.subScore = subScore;
            ViewBag.isReserveWeek = isReserveWeek;
            ViewBag.lineId = lineId;
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            if (!(UserItem.role.RoleNameEn.Equals("SalonOwner") || UserItem.role.RoleNameEn.Equals("HairStylist")))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.TotalCountRegisterLine = _ILine.CountRegisterLine(UserItem.id);
                ViewBag.expireDate = UserItem.expireDateAccount;
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
                ViewBag.isProfileAccept = UserItem.isProfileAccept;
                ViewBag.isProfileComplete = UserItem.isProfileComplete;

            }
            return View(lineViewModel);
        }


        [HttpPost]
        public JsonResult SignInConfirmCode(User user)
        {
            UserViewModel userItem = new UserViewModel();
            if (string.IsNullOrEmpty(user.cellphone))
            {
                return Json(new { success = false, responseText = "شماره همراه خالی است" });
            }
            if (user.cellphone.Substring(1, 1) == "۹")
                user.cellphone = DateChanger.PersianToEnglish(user.cellphone);
            var UserItem = _Iuser.GetUser(user.cellphone);
            if (UserItem == null)
            {
                userItem.cellphone = user.cellphone;
                userItem.roleId = 2;
                _Iuser.Register(userItem);
            }

            Tuple<bool, string> result = _Iuser.LoginUserConfirmCode(user.cellphone);
            return Json(new { success = result.Item1, responseText = result.Item2 });
        }
        [HttpPost]
        public IActionResult SignIn(User User, string returnUrl, int shouldnoticeshortcut, int shouldAdverstitmentshortcut)
        {
            if (User.cellphone.Substring(1, 1) == "۹")
                User.cellphone = DateChanger.PersianToEnglish(User.cellphone);
            var user = _Iuser.GetUser(User.cellphone);
            if (user == null)
            {
                return Json(new { success = false, responseText = "چنین کاربری یافت نشد." });
            }
            if (user.code != User.code)
            {
                return Json(new { success = false, responseText = "کد وارد شده نامعتبر است." });
            }
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.cellphone.ToString()),
                        new Claim(ClaimTypes.Name,user.cellphone),
                        new Claim(ClaimTypes.Role,user.role.RoleNameEn),

                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            HttpContext.SignInAsync(principal, properties);
            return Json(new { success = true, responseText = "", roleNameEn = user.role.RoleNameEn, returnUrl = returnUrl, hasnoticeshortcut = shouldnoticeshortcut, hasadverstitmentshortcut = shouldAdverstitmentshortcut });
        }

        public JsonResult RemoveFile(int Id)
        {
            try
            {
                _ILine.RemoveFile(Id);
                return Json(new { success = true, responseText = "Done" });

            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "Done" });

            }
        }

        public JsonResult Remove(int id)
        {
            var Item = _ILine.GetById(id);
            if (Item != null)
            {
                _ILine.Remove(Item);
            }
            return Json(new { success = true, responseText = "Done" });

        }
        public JsonResult RemoveAdvertisment(int id)
        {
            var Item = _IAdvertisment.GetById(id);
            if (Item != null)
            {
                _IAdvertisment.Remove(Item);
            }
            return Json(new { success = true, responseText = "Done" });

        }
        public JsonResult RemoveNotice(int id)
        {
            var Item = _INotice.GetById(id);
            if (Item != null)
            {
                _INotice.Remove(Item);
            }
            return Json(new { success = true, responseText = "Done" });

        }
        #endregion
        #region Academy
        public IActionResult RegisterClassRoom(int? Id)
        {
            ClassRoomViewModel ClassRoomViewModel = new ClassRoomViewModel();
            var allPrice = _context.AllPrices.FirstOrDefault();
            long? minDiscount = allPrice.minDiscount == null ? 0 : allPrice.minDiscount;
            long? maxDiscount = allPrice.maxDiscount == null ? 0 : allPrice.maxDiscount;

            if (Id.HasValue)
            {
                ClassRoom Item = _IClassRoom.GetById(Id.Value);
                if (Item != null)
                {
                    ClassRoomViewModel.title = Item.title;
                    ClassRoomViewModel.description = Item.description;
                    ClassRoomViewModel.classRoomLaw = Item.classRoomLaw;
                    ClassRoomViewModel.price = Item.price;
                    ClassRoomViewModel.ClassRoomImage = Item.ClassRoomImages.ToList();
                    ClassRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
                    ClassRoomViewModel.classRoomTypeId = Item.classRoomTypeId;
                }
            }
            else
            {
                ClassRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();

            }
            ClassRoomViewModel.minDiscount = Convert.ToInt64(minDiscount);
            ClassRoomViewModel.maxDiscount = Convert.ToInt64(maxDiscount);
            return PartialView("_RegisterClassRoomPartial", ClassRoomViewModel);
        }
        public IActionResult RegisterClassRoomForReserve(int? Id)
        {
            ClassRoomViewModel ClassRoomViewModel = new ClassRoomViewModel();
            var allPrice = _context.AllPrices.FirstOrDefault();
            long? minDiscount = allPrice.minDiscount == null ? 0 : allPrice.minDiscount;
            long? maxDiscount = allPrice.maxDiscount == null ? 0 : allPrice.maxDiscount;
            var user = _Iuser.GetUser(User.Identity.Name);
            if (Id.HasValue)
            {
                ClassRoom Item = _IClassRoom.GetById(Id.Value);
                if (Item != null)
                {
                    ClassRoomViewModel.title = Item.title;
                    ClassRoomViewModel.description = Item.description;
                    ClassRoomViewModel.classRoomLaw = Item.classRoomLaw;
                    ClassRoomViewModel.price = Item.price;
                    ClassRoomViewModel.discountPrice = Item.discountPrice;
                    ClassRoomViewModel.classRoomPeriod = Item.classRoomPeriod;
                    ClassRoomViewModel.classRoomTeacher = Item.classRoomTeacher;
                    ClassRoomViewModel.ClassRoomImage = Item.ClassRoomImages.ToList();
                    ClassRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
                    ClassRoomViewModel.reserveHour = Item.reserveHour;
                    ClassRoomViewModel.reserveDate = Item.reserveDate.ToString();
                    ClassRoomViewModel.classRoomTypeId = Item.classRoomTypeId;
                }
            }
            else
            {
                ClassRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
            }
            ClassRoomViewModel.minDiscount = Convert.ToInt64(minDiscount);
            ClassRoomViewModel.maxDiscount = Convert.ToInt64(maxDiscount);
            ClassRoomViewModel.userId = user.id;

            return PartialView("_RegisterClassRoomForReservePartial", ClassRoomViewModel);
        }
        public IActionResult AllClassRoom(SearchClassRoomViewModel searchClassRoomViewModel)
        {
            //SearchClassRoomViewModel searchClassRoomViewModel = new SearchClassRoomViewModel();
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            searchClassRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();

            searchClassRoomViewModel.ClassRooms = _IClassRoom.GetAll(userId, searchClassRoomViewModel);
            return PartialView("_AllClassRoomPartial", searchClassRoomViewModel);
        }
        [HttpPost]
        public IActionResult RegisterClassRoom(ClassRoomViewModel ClassRoomViewModel, IFormFile[] file)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            ClassRoomViewModel.userId = user.id;
            var compareDate = DateTime.Compare((DateTime)user.expireDateAccount, @DateTime.Now);
            int countClassroom = _IClassRoom.CountRegisterClass(ClassRoomViewModel.userId);
            if (user.role.RoleNameEn == "Academy")
            {
                if (countClassroom > 20 || compareDate < 0)
                    return RedirectToAction("Academy", "CustomerHome", new { isRegisterClassRoom = true });
                _IClassRoom.CreateOrUpdate(ClassRoomViewModel, file);
                return RedirectToAction("Academy", "CustomerHome", new { isRegisterClassRoom = true });
            }
            else
            {
                if (countClassroom > 20 || compareDate < 0)
                    return RedirectToAction("Mentor", "CustomerHome", new { isRegisterClassRoom = true });
                _IClassRoom.CreateOrUpdate(ClassRoomViewModel, file);
                return RedirectToAction("Mentor", "CustomerHome", new { isRegisterClassRoom = true });
            }
        }
        [HttpPost]
        public IActionResult RegisterClassRoomForReserve(ClassRoomViewModel ClassRoomViewModel, IFormFile[] file)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            ClassRoomViewModel.userId = user.id;
            ClassRoomViewModel.classRoomLaw = EnumLineLaw.reserve;
            _IClassRoom.CreateOrUpdate(ClassRoomViewModel, file);
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { isRegisterClassRoom = true });
            else
                return RedirectToAction("Mentor", "CustomerHome", new { isRegisterClassRoom = true });

        }
        public JsonResult RemoveFileClassRoom(int Id)
        {
            try
            {
                _IClassRoom.RemoveFile(Id);
                return Json(new { success = true, responseText = "Done" });

            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "Done" });

            }
        }
        public JsonResult RemoveClassRoom(int id)
        {
            var Item = _IClassRoom.GetById(id);
            if (Item != null)
            {
                _IClassRoom.Remove(Item);
            }
            return Json(new { success = true, responseText = "Done" });

        }
        #endregion
        #region Store
        public IActionResult Store(bool isRegisterProduct = false, bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            ViewBag.isRegisterProduct = isRegisterProduct;
            ViewBag.subScore = subScore;
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            if (!UserItem.role.RoleNameEn.Equals("Store"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.url = UserItem.url;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.TotalCountRegisterProduct = _IProduct.CountRegisterProduct(UserItem.id);
                ViewBag.expireDate = UserItem.expireDateAccount;
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
                ViewBag.isProfileAccept = UserItem.isProfileAccept;
                ViewBag.isProfileComplete = UserItem.isProfileComplete;


            }
            return View(productViewModel);
        }
        public IActionResult RegisterProduct(int? Id)
        {
            ProductViewModel ProductViewModel = new ProductViewModel();
            var allPrice = _context.AllPrices.FirstOrDefault();
            long? minDiscount = allPrice.minDiscount == null ? 0 : allPrice.minDiscount;
            long? maxDiscount = allPrice.maxDiscount == null ? 0 : allPrice.maxDiscount;

            if (Id.HasValue)
            {
                Product Item = _IProduct.GetById(Id.Value);
                if (Item != null)
                {
                    ProductViewModel.title = Item.title;
                    ProductViewModel.description = Item.description;
                    ProductViewModel.price = Item.price;
                    ProductViewModel.ProductImages = Item.ProductImages.ToList();

                }
            }
            ProductViewModel.minDiscount = Convert.ToInt64(minDiscount);
            ProductViewModel.maxDiscount = Convert.ToInt64(maxDiscount);
            return PartialView("_RegisterProductPartial", ProductViewModel);
        }
        public IActionResult AllProduct(SearchProductViewModel searchProductViewModel)
        {
            //SearchProductViewModel searchProductViewModel = new SearchProductViewModel();
            int userId = _Iuser.GetUser(User.Identity.Name).id;

            searchProductViewModel.Products = _IProduct.GetAll(userId, searchProductViewModel);
            return PartialView("_AllProductPartial", searchProductViewModel);
        }
        [HttpPost]
        public IActionResult RegisterProduct(ProductViewModel ProductViewModel, IFormFile[] file)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            ProductViewModel.userId = user.id;
            var compareDate = DateTime.Compare((DateTime)user.expireDateAccount, @DateTime.Now);
            int countProduct = _IProduct.CountRegisterProduct(ProductViewModel.userId);
            if (countProduct > 50 || compareDate < 0)
                return RedirectToAction("Store", "CustomerHome", new { isRegisterProduct = true });
            _IProduct.CreateOrUpdate(ProductViewModel, file);
            return RedirectToAction("Store", "CustomerHome", new { isRegisterProduct = true });
        }
        public JsonResult RemoveFileProduct(int Id)
        {
            try
            {
                _IProduct.RemoveFile(Id);
                return Json(new { success = true, responseText = "Done" });

            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "Done" });


            }
        }
        #endregion
        #region Students
        public IActionResult Student(bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0)
        {

            var UserItem = _Iuser.GetUser(User.Identity.Name);
            ViewBag.subScore = subScore;
            if (!UserItem.role.RoleNameEn.Equals("Student"))
            {
                return RedirectToAction(nameof(Login));
            }

            if (UserItem != null)
            {

                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;


            }
            return View();
        }
        public IActionResult ReservedSalowns(SearchReserveViewModel searchReserveViewModel)
        {
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            if (userId == 0)
            {
                return NotFound();
            }
            searchReserveViewModel.Reserve = _ILine.GetReservedLines(userId, searchReserveViewModel);
            searchReserveViewModel.LineTypes = _ILineType.GetLineTypes();
            return PartialView("StudentPartial/_AllReservedPartial", searchReserveViewModel);
        }
        public IActionResult ReservedClass(SearchClassRoomReserveViewModel searchReserveViewModel)
        {
            int userId = _Iuser.GetUser(User.Identity.Name).id;
            if (userId == 0)
            {
                return NotFound();
            }
            searchReserveViewModel.Reserve = _ILine.GetReservedClasses(userId, searchReserveViewModel);
            searchReserveViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
            return PartialView("StudentPartial/_AllReservedClasses", searchReserveViewModel);
        }
        #endregion
        #region ProfileManagment
        public IActionResult ProfileManagment(ProfileViewModel profileViewModel)
        {
            ProfileViewModel profile = new ProfileViewModel();
            var userItem = _Iuser.GetUser(User.Identity.Name);
            if (userItem == null)
            {
                return NotFound();
            }
            profile.address = userItem.address;
            profile.nationalCode = userItem.nationalCode;
            profile.fullname = userItem.fullname;
            profile.hasCertificate = userItem.hasCertificate;
            profile.latitude = userItem.latitude;
            profile.longitude = userItem.longitude;
            profile.shortDescription = userItem.shortDescription;
            profile.linkTelegram = userItem.linkTelegram;
            profile.linkInstagram = userItem.linkInstagram;
            profile.shebaNumber = userItem.shebaNumber;
            profile.longDescription = userItem.longDescription;
            profile.url = userItem.url;
            profile.sexuality = userItem.sexuality;
            profile.UserDocumentImages = userItem.UserDocumentImages.ToList();

            profile.workingHours = userItem.workingHours;
            profile.rolename = userItem.role.RoleNameEn;
            switch (profile.rolename)
            {
                case ("Student"):
                    ViewBag.ReturnUrl = "Student";
                    break;
                case ("SalonOwner"):
                    ViewBag.ReturnUrl = "SalonOwner";
                    break;
                case ("Store"):
                    ViewBag.ReturnUrl = "Store";
                    break;
                case ("Academy"):
                    ViewBag.ReturnUrl = "Academy";
                    break;
                case ("Mentor"):
                    ViewBag.ReturnUrl = "Mentor";
                    break;
                case ("HairStylist"):
                    ViewBag.ReturnUrl = "HairStylist";
                    break;
                case ("Member"):
                    ViewBag.ReturnUrl = "Member";
                    break;
            }
            return PartialView("ProfilePartialFolder/ProfilePartial", profile);
        }
        [HttpPost]
        public IActionResult UpdateProfile(ProfileViewModel profileViewModel)
        {
            var userItem = _Iuser.GetUser(User.Identity.Name);
            if (userItem == null)
            {
                return NotFound();
            }
            profileViewModel.id = userItem.id;
            _Iuser.Update(profileViewModel);
            if (userItem.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome");
            if (userItem.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome");
            if (userItem.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome");
            if (userItem.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome");
            if (userItem.role.RoleNameEn == "Student")
                return RedirectToAction("Student", "CustomerHome");
            if (userItem.role.RoleNameEn == "HairStylist")
                return RedirectToAction("HairStylist", "CustomerHome");
            if (userItem.role.RoleNameEn == "Member")
                return RedirectToAction("Member", "CustomerHome");
            else
                return RedirectToAction("Student", "CustomerHome");
        }
        #endregion
        public JsonResult RemoveProduct(int id)
        {
            var Item = _IProduct.GetById(id);
            if (Item != null)
            {
                _IProduct.Remove(Item);
            }
            return Json(new { success = true, responseText = "Done" });

        }
        #region Details
        [Route("CustomerHome/NoticeDetail/{id}/{title}")]
        public IActionResult NoticeDetail(int id, string title)
        {
            NoticeDetailViewModel noticeDetailViewModel = new NoticeDetailViewModel();
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            noticeDetailViewModel.Notice = _INotice.GetByIdForDetail(id, ip);
            noticeDetailViewModel.OtherNotice = _INotice.GetRelated(id);
            noticeDetailViewModel.Banner = _IBanner.GetFirst();
            return View(noticeDetailViewModel);
        }
        [Route("CustomerHome/AdvertismentDetail/{id}/{title}")]

        public IActionResult AdvertismentDetail(int id, string title)
        {
            AdvertismentDetailViewModel advertismentDetailViewModel = new AdvertismentDetailViewModel();
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            advertismentDetailViewModel.Advertisment = _IAdvertisment.GetByIdForDetail(id, ip);
            advertismentDetailViewModel.OtherAdvertisment = _IAdvertisment.GetRelated(id);
            advertismentDetailViewModel.Banner = _IBanner.GetFirst();
            return View(advertismentDetailViewModel);
        }
        [Route("CustomerHome/ClassRoomDetail/{id}/{title}")]
        public IActionResult ClassRoomDetail(int id, string title, bool isReserved = false)
        {
            if (isReserved)
                ViewBag.isReserved = isReserved;
            ClassRoomDetailViewModel classRoomDetailViewModel = new ClassRoomDetailViewModel();
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            classRoomDetailViewModel.ClassRoom = _IClassRoom.GetByIdForDetail(id, ip);
            classRoomDetailViewModel.OtherClassRoom = _IClassRoom.GetRelated(id);
            classRoomDetailViewModel.Banner = _IBanner.GetFirst();
            return View(classRoomDetailViewModel);
        }


        [Route("CustomerHome/LineDetail/{id}/{title}")]
        public IActionResult LineDetail(int id, string title, bool isReserved = false)
        {
            LineDetailViewModel lineDetailViewModel = new LineDetailViewModel();
            if (isReserved)
                ViewBag.isReserved = isReserved;
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            lineDetailViewModel.Line = _ILine.GetByIdForDetail(id, ip);
            lineDetailViewModel.OtherLine = _ILine.GetRelated(id);
            lineDetailViewModel.LineWeekDates = _LineWeekDate.GetForEdit(id, "", "");
            lineDetailViewModel.Banner = _IBanner.GetFirst();
            return View(lineDetailViewModel);
        }
        [Route("CustomerHome/ProductDetail/{id}/{title}")]

        public IActionResult ProductDetail(int id, string title)
        {
            ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel();
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            productDetailViewModel.Product = _IProduct.GetByIdForDetail(id, ip);
            productDetailViewModel.OtherProduct = _IProduct.GetRelated(id);
            productDetailViewModel.Banner = _IBanner.GetFirst();
            return View(productDetailViewModel);
        }
        #endregion
        #region Lists
        public IActionResult LineList(int page = 1, bool LineIsService = false)
        {
            var model = _ILine.GetLines(page, LineIsService);
            ViewBag.LineIsService = LineIsService;
            LineListViewModel lineListViewModel = new LineListViewModel();
            lineListViewModel.Lines = model;
            lineListViewModel.CheapestLines = _ILine.CheapestLines();
            lineListViewModel.Banner = _IBanner.GetFirst();

            return View(lineListViewModel);

        }
        public IActionResult LineListPartial(int page = 1, bool LineIsService = false)
        {
            var model = _ILine.GetLines(page, LineIsService);
            LineListViewModel lineListViewModel = new LineListViewModel();
            lineListViewModel.Lines = model;
            return PartialView("LineListPartial", lineListViewModel);


        }
        public IActionResult ClassRoomList(int page = 1, bool isReserves = false)
        {
            var model = _IClassRoom.GetClassRooms(page, isReserves);
            ViewBag.isReserves = isReserves;

            ClassRoomListViewModel ClassRoomListViewModel = new ClassRoomListViewModel();
            ClassRoomListViewModel.ClassRooms = model;
            ClassRoomListViewModel.CheapestClassRooms = _IClassRoom.CheapestClassRooms();
            ClassRoomListViewModel.Banner = _IBanner.GetFirst();

            return View(ClassRoomListViewModel);

        }
        public IActionResult ClassRoomListPartial(int page = 1, bool isReserves = false)
        {
            var model = _IClassRoom.GetClassRooms(page, isReserves);
            ClassRoomListViewModel ClassRoomListViewModel = new ClassRoomListViewModel();
            ClassRoomListViewModel.ClassRooms = model;
            return PartialView("ClassRoomListPartial", ClassRoomListViewModel);
        }
        public IActionResult ProductList(int page = 1)
        {
            var model = _IProduct.GetProducts(page);
            ProductListViewModel ProductListViewModel = new ProductListViewModel();
            ProductListViewModel.Products = model;
            ProductListViewModel.CheapestProducts = _IProduct.CheapestProducts();
            ProductListViewModel.Banner = _IBanner.GetFirst();
            return View(ProductListViewModel);
        }
        public IActionResult ProductListPartial(int page = 1)
        {
            var model = _IProduct.GetProducts(page);
            ProductListViewModel ProductListViewModel = new ProductListViewModel();
            ProductListViewModel.Products = model;
            return PartialView("ProductListPartial", ProductListViewModel);

        }

        [Route("/Ad_List")]
        public IActionResult Ad_List()
        {

            AdViewModel homeViewModel = new AdViewModel();
            homeViewModel.Notices = _INotice.GetAllHome();
            homeViewModel.NoticeNotBarbers = _INotice.GetAllNotBarber();

            return View(homeViewModel);
        }

        public IActionResult NoticeList(int page = 1, bool isBarber = false)
        {
            //var model = _INotice.GetNoticeList(page, isBarber);


            ViewBag.isBarber = isBarber;
            NoticeListViewModel NoticeListViewModel = new NoticeListViewModel();

            //NoticeListViewModel.Notices = model;

            if (isBarber == true)
            {
                NoticeListViewModel.NoticeNotBarbers = _INotice.GetAllNotBarber();
            }
            else
            {
                NoticeListViewModel.PercentNotices = _INotice.PercentNotices();
                NoticeListViewModel.RentNotices = _INotice.RentNotices();
                NoticeListViewModel.FixedSalaryNotices = _INotice.FixedSalaryNotices();
                NoticeListViewModel.Banner = _IBanner.GetFirst();
            }

            return View(NoticeListViewModel);
        }
        public IActionResult NoticeListPartial(int page = 1, bool isBarber = false)
        {
            var model = _INotice.GetNoticeList(page, isBarber);
            NoticeListViewModel NoticeListViewModel = new NoticeListViewModel();
            NoticeListViewModel.Notices = model;

            return PartialView("NoticeListPartial", NoticeListViewModel);

        }
        public IActionResult AdvertismentList(int page = 1, bool isWorkshop = false)
        {
            var model = _IAdvertisment.GetAdvertismentList(page, isWorkshop);
            ViewBag.isWorkshop = isWorkshop;
            AdvertismentListViewModel AdvertismentListViewModel = new AdvertismentListViewModel();
            AdvertismentListViewModel.Advertisments = model;
            AdvertismentListViewModel.IsWorkshopAdvertisments = _IAdvertisment.IsWorkshopAdvertisments(isWorkshop);
            AdvertismentListViewModel.Banner = _IBanner.GetFirst();
            return View(AdvertismentListViewModel);
        }
        public IActionResult AdvertismentListPartial(int page = 1, bool isWorkshop = false)
        {
            var model = _IAdvertisment.GetAdvertismentList(page, isWorkshop);
            AdvertismentListViewModel AdvertismentListViewModel = new AdvertismentListViewModel();
            AdvertismentListViewModel.Advertisments = model;
            return PartialView("AdvertismentListPartial", AdvertismentListViewModel);

        }
        #endregion
        #region ProfileUserSingle
        [Route("CustomerHome/ProfileUserSingle/{userId}/{fullname}")]

        public IActionResult ProfileUserSingle(int userId, string fullname)
        {
            ProfileUserSingleViewModel ProfileUserSingleViewModel = new ProfileUserSingleViewModel();
            ProfileUserSingleViewModel.User = _Iuser.GetByIdUser(userId);
            ProfileUserSingleViewModel.Banner = _IBanner.GetFirst();
            ProfileUserSingleViewModel.Products = _IProduct.GetAllRegisterByUser(userId);
            ProfileUserSingleViewModel.Lines = _ILine.GetAllRegisterByUser(userId);
            ProfileUserSingleViewModel.ClassRooms = _IClassRoom.GetAllRegisterByUser(userId);
            ProfileUserSingleViewModel.Advertisments = _IAdvertisment.GetAllRegisterByUser(userId);
            ProfileUserSingleViewModel.Notices = _INotice.GetAllRegisterByUser(userId);
            return View(ProfileUserSingleViewModel);
        }
        #endregion
        #region FactorAndPaymentActions
        public IActionResult FactorCreator(FactorViewModel model)
        {

            var userItem = _Iuser.GetUser(User.Identity.Name);
            if (userItem != null)
            {
                switch (userItem.role.RoleNameEn)
                {
                    case ("Student"):
                        ViewBag.ReturnUrl = "Student";
                        break;
                    case ("SalonOwner"):
                        ViewBag.ReturnUrl = "SalonOwner";
                        break;
                    case ("Store"):
                        ViewBag.ReturnUrl = "Store";
                        break;
                    case ("Academy"):
                        ViewBag.ReturnUrl = "Academy";
                        break;
                    case ("Mentor"):
                        ViewBag.ReturnUrl = "Mentor";
                        break;
                    case ("HairStylist"):
                        ViewBag.ReturnUrl = "HairStylist";
                        break;
                    case ("Member"):
                        ViewBag.ReturnUrl = "Member";
                        break;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult FactorCreatorLadder(FactorViewModel model)
        {

            var userItem = _Iuser.GetUser(User.Identity.Name);
            if (userItem != null)
            {
                switch (userItem.role.RoleNameEn)
                {
                    case ("Student"):
                        ViewBag.ReturnUrl = "Student";
                        break;
                    case ("SalonOwner"):
                        ViewBag.ReturnUrl = "SalonOwner";
                        break;
                    case ("Store"):
                        ViewBag.ReturnUrl = "Store";
                        break;
                    case ("Academy"):
                        ViewBag.ReturnUrl = "Academy";
                        break;
                    case ("Mentor"):
                        ViewBag.ReturnUrl = "Mentor";
                        break;
                    case ("HairStylist"):
                        ViewBag.ReturnUrl = "HairStylist";
                        break;
                    case ("Member"):
                        ViewBag.ReturnUrl = "Member";
                        break;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult FactorCreatorExtent(FactorViewModel model)
        {

            var userItem = _Iuser.GetUser(User.Identity.Name);
            if (userItem != null)
            {
                switch (userItem.role.RoleNameEn)
                {
                    case ("Student"):
                        ViewBag.ReturnUrl = "Student";
                        break;
                    case ("SalonOwner"):
                        ViewBag.ReturnUrl = "SalonOwner";
                        break;
                    case ("Store"):
                        ViewBag.ReturnUrl = "Store";
                        break;
                    case ("Academy"):
                        ViewBag.ReturnUrl = "Academy";
                        break;
                    case ("Mentor"):
                        ViewBag.ReturnUrl = "Mentor";
                        break;
                    case ("HairStylist"):
                        ViewBag.ReturnUrl = "HairStylist";
                        break;
                    case ("Member"):
                        ViewBag.ReturnUrl = "Member";
                        break;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult FactorCreatorExtent(FactorViewModel model, string x)
        {
            long totalprice = 1000;
            bool isRegisterAdvertisment = false;
            bool isRegisterNotice = false;
            if (model.isNotice == 1)
                isRegisterNotice = true;
            else isRegisterAdvertisment = true;
            var user = _Iuser.GetUser(User.Identity.Name);
            if (model.PursheType == 2)
            {
                if (user.score < Convert.ToInt64(model.totalAmount))
                {
                    ViewData["Error"] = "امتیاز شما کافی نیست";
                    return View(model);
                }
                else
                {
                    if (model.isNotice == 1)
                    {
                        _INotice.Extent(model.ItemId, user.id, model.totalAmount);
                    }
                    else
                    {
                        _IAdvertisment.Extent(model.ItemId, user.id, model.totalAmount);
                    }
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                }
            }
            else
            {
                if (model.isNotice == 1)
                {
                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                    factor.noticeId = model.ItemId;
                    factor.factorKind = FactorKind.Extend;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);
                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmExtentNoticesFromWeb/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }
                else
                {
                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                    factor.advertismentId = model.ItemId;
                    factor.factorKind = FactorKind.Extend;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);
                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmExtentAdvertismentFromWeb/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }

            }
            if (user.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Student")
                return RedirectToAction("Student", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Member")
                return RedirectToAction("Member", "CustomerHome", new { });
            else
                return RedirectToAction("Student", "CustomerHome", new { });
        }
        [HttpPost]
        public IActionResult FactorCreatorLadder(FactorViewModel model, string x)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            var allprice = _context.AllPrices.FirstOrDefault();
            long totalprice = 0;
            bool isRegisterAdvertisment = false;
            bool isRegisterNotice = false;
            if (model.isNotice == 1)
            {

                if (model.isBarber && allprice.isHasBarberPrice)
                    totalprice = allprice.barberPrice;
                if (!model.isBarber && allprice.isHasSaloonPrice)
                    totalprice = allprice.saloonPrice;
                isRegisterNotice = true;
            }
            else
            {
                if (model.isWorkshop && allprice.isHasWorkshopPrice)
                    totalprice = allprice.workshopPrice;
                if (!model.isWorkshop && allprice.isHasAdvertismentPrice)
                    totalprice = allprice.advertismentPrice;
                isRegisterAdvertisment = true;
            }
            if (model.PursheType == 2)
            {
                if (user.score < Convert.ToInt64(model.totalAmount))
                {
                    ViewData["Error"] = "امتیاز شما کافی نیست";
                    return View(model);
                }
                else
                {
                    if (model.isNotice == 1)
                    {
                        _INotice.Ladder(model.ItemId, user.id, model.totalAmount);
                    }
                    else
                    {
                        _IAdvertisment.Ladder(model.ItemId, user.id, model.totalAmount);
                    }
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                }
            }
            else
            {

                if (model.isNotice == 1)
                {


                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                    factor.noticeId = model.ItemId;
                    factor.factorKind = FactorKind.Ladder;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);
                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmLadderNoticesFromWeb/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }
                else
                {


                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                    factor.advertismentId = model.ItemId;
                    factor.factorKind = FactorKind.Ladder;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);
                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmLadderAdvertismentFromWeb/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }

            }
            if (user.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Student")
                return RedirectToAction("Student", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Member")
                return RedirectToAction("Member", "CustomerHome", new { });
            else
                return RedirectToAction("Student", "CustomerHome", new { });
        }
        public IActionResult CancellFactor(int Id, int isNotice)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            bool isRegisterAdvertisment = false; bool isRegisterNotice = false;
            if (isNotice == 1)
            {
                isRegisterNotice = true;
                var NoticeItem = _INotice.GetById(Id);
                if (NoticeItem != null)
                {
                    _INotice.Remove(NoticeItem);
                }
            }
            else if (isNotice == 0)
            {
                isRegisterAdvertisment = true;
                var Item = _IAdvertisment.GetById(Id);
                if (Item != null)
                {
                    _IAdvertisment.Remove(Item);
                }
            }
            _context.SaveChanges();
            if (user.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            if (user.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            if (user.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            if (user.role.RoleNameEn == "Student")
                return RedirectToAction("Student", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            if (user.role.RoleNameEn == "Member")
                return RedirectToAction("Member", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
            else
                return RedirectToAction("Student", "CustomerHome", new { isRegisterAdvertisment, isRegisterNotice });
        }
        [HttpPost]
        public IActionResult FactorCreator(FactorViewModel model, string x)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            bool isRegisterAdvertisment = false;
            bool isRegisterNotice = false;
            if (model.isNotice == 1)
                isRegisterNotice = true;
            else isRegisterAdvertisment = true;
            if (model.PursheType == 2)
            {
                if (user.score < Convert.ToInt64(model.totalAmount))
                {
                    ViewData["Error"] = "امتیاز شما کافی نیست";
                    return View(model);
                }
                else
                {
                    if (model.isNotice == 1)
                    {
                        _INotice.ChageState(model.ItemId, user.id, model.totalAmount);
                    }
                    else
                    {
                        _IAdvertisment.ChangeStatus(model.ItemId, user.id, model.totalAmount);
                    }
                    if (user.role.RoleNameEn == "SalonOwner")
                        return RedirectToAction("SalonOwner", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Store")
                        return RedirectToAction("Store", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Academy")
                        return RedirectToAction("Academy", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Mentor")
                        return RedirectToAction("Mentor", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Student")
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    if (user.role.RoleNameEn == "Member")
                        return RedirectToAction("Member", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });
                    else
                        return RedirectToAction("Student", "CustomerHome", new { subScore = true, isRegisterAdvertisment, isRegisterNotice });

                }
            }
            else
            {
                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (model.isNotice == 1)
                {
                    if (model.isBarber && allprice.isHasBarberPrice)
                        totalprice = allprice.barberPrice;
                    if (!model.isBarber && allprice.isHasSaloonPrice)
                        totalprice = allprice.saloonPrice;
                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);
                    factor.noticeId = model.ItemId;
                    factor.factorKind = FactorKind.Add;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);

                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmNoticesFromWebSite/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }
                else
                {
                    if (model.isWorkshop && allprice.isHasWorkshopPrice)
                        totalprice = allprice.workshopPrice;
                    if (!model.isWorkshop && allprice.isHasAdvertismentPrice)
                        totalprice = allprice.advertismentPrice;
                    Factor factor = new Factor();
                    factor.state = State.IsPay;
                    factor.userId = user.id;
                    factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);
                    factor.advertismentId = model.ItemId;
                    factor.factorKind = FactorKind.Add;
                    factor.totalPrice = totalprice;
                    _context.Factors.Add(factor);

                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmAdvertismentsFromWebSite/" + model.ItemId + "/" + factor.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }
                }

            }
            if (user.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Student")
                return RedirectToAction("Student", "CustomerHome", new { });
            if (user.role.RoleNameEn == "Member")
                return RedirectToAction("Member", "CustomerHome", new { });
            else
                return RedirectToAction("Student", "CustomerHome", new { });
        }
        #endregion
        #region Visitr
        public IActionResult VisitAll(int? Id, WhichTableEnum whichTableEnum)
        {
            VisitViewModel Item = _IVisit.GetByAllNoticeId(Id.Value, whichTableEnum);
            return PartialView("_VisitPartial", Item);
        }
        #endregion

        #region Razmjoo

        public IActionResult CustomerFactorCreator(FactorViewModel model)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "CustomerHome", new { returnUrl = $"/CustomerHome/LineDetail?id={model.ItemId}" });
            }
            else
            {
                var CustomerItem = _Iuser.GetUser(User.Identity.Name);
                model.fullName = CustomerItem.fullname;
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult CustomerFactorCreator(FactorViewModel model, int x)
        {
            var user = _Iuser.GetUser(User.Identity.Name);

            if (model.PursheType == 2)
            {
                if (user.score < Convert.ToInt64(model.totalAmount))
                {
                    ViewData["Error"] = "امتیاز شما کافی نیست";
                    return View(model);
                }
                else
                {
                    if (model.NoticeType == NoticeType.Line)
                    {
                        _INotice.ReserveRegister(NoticeType.Line, user.id, model.totalAmount, model.ItemId);
                    }
                    else if (model.NoticeType == NoticeType.ClassRoom)
                    {
                        _INotice.ReserveRegister(NoticeType.ClassRoom, user.id, model.totalAmount, model.ItemId);
                    }
                    else
                    {
                        _INotice.ReserveRegister(NoticeType.Store, user.id, model.totalAmount, model.ItemId);
                    }
                }
                return RedirectToAction("Index", "CustomerHome", new { isRegisterSuccess = true });

            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ShortcutToNotice()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "CustomerHome", new { isNoticeShortcut = 1 });
            }
            else
            {
                var user = _Iuser.GetUser(User.Identity.Name);
                string _redirectUrl = string.Empty;
                if (user != null)
                {
                    if (user.role.RoleNameEn == "SalonOwner")
                    {
                        _redirectUrl = "/CustomerHome/SalonOwner";
                        return RedirectToAction("SalonOwner", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Store")
                    {
                        _redirectUrl = "/CustomerHome/Store";
                        return RedirectToAction("Store", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });
                    }
                    if (user.role.RoleNameEn == "Academy")
                    {
                        _redirectUrl = "/CustomerHome/Academy";
                        return RedirectToAction("Academy", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Mentor")
                    {
                        _redirectUrl = "/CustomerHome/Mentor";
                        return RedirectToAction("Mentor", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Student")
                    {
                        _redirectUrl = "/CustomerHome/Student";
                        return RedirectToAction("Student", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });
                    }
                    if (user.role.RoleNameEn == "Member")
                    {
                        _redirectUrl = "/CustomerHome/Member";
                        return RedirectToAction("Member", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    else
                        _redirectUrl = "/CustomerHome/Index";
                    return RedirectToAction("Login", "CustomerHome", new { returnUrl = _redirectUrl });
                }
                else
                {

                    return RedirectToAction("Login", "Register");

                }

            }
        }
        public IActionResult ShortcutToAdverstitment()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "CustomerHome", new { isAdverstitment = 1 });
            }
            else
            {
                var user = _Iuser.GetUser(User.Identity.Name);
                string _redirectUrl = string.Empty;
                if (user != null)
                {
                    if (user.role.RoleNameEn == "SalonOwner")
                    {
                        _redirectUrl = "/CustomerHome/SalonOwner";
                        return RedirectToAction("SalonOwner", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Store")
                    {
                        _redirectUrl = "/CustomerHome/Store";
                        return RedirectToAction("Store", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });
                    }
                    if (user.role.RoleNameEn == "Academy")
                    {
                        _redirectUrl = "/CustomerHome/Academy";
                        return RedirectToAction("Academy", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Mentor")
                    {
                        _redirectUrl = "/CustomerHome/Mentor";
                        return RedirectToAction("Mentor", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    if (user.role.RoleNameEn == "Student")
                    {
                        _redirectUrl = "/CustomerHome/Student";
                        return RedirectToAction("Student", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });
                    }
                    if (user.role.RoleNameEn == "Member")
                    {
                        _redirectUrl = "/CustomerHome/Member";
                        return RedirectToAction("Member", "CustomerHome", new { returnUrl = _redirectUrl, isNoticeShortcut = 1 });

                    }
                    else
                        _redirectUrl = "/CustomerHome/Index";
                    return RedirectToAction("Login", "CustomerHome", new { returnUrl = _redirectUrl });
                }
                else
                {

                    return RedirectToAction("Login", "Register");

                }

            }
        }
        #endregion
        #region Nasr
        public JsonResult RemoveFileUserDocument(int Id)
        {
            try
            {
                _Iuser.RemoveFile(Id);
                return Json(new { success = true, responseText = "Done" });

            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "Done" });


            }
        }
        [HttpGet]
        public IActionResult AllSearch(string title, int page = 1)
        {
            IQueryable<AllSearchDetailViewModel> AllSearchDetailViewModels = _context.AllSearchDetailViewModels.FromSql("EXEC	 [dbo].[SearchAll] @param1 = N'" + title + "'").AsQueryable();
            PagedList<AllSearchDetailViewModel> res = new PagedList<AllSearchDetailViewModel>(AllSearchDetailViewModels, page, 10);
            ViewBag.title = title;
            return View(res);
        }
        public IActionResult AllSearchPartial(string title, int page = 1)
        {
            IQueryable<AllSearchDetailViewModel> AllSearchDetailViewModels = _context.AllSearchDetailViewModels.FromSql("EXEC	 [dbo].[SearchAll] @param1 = N'" + title + "'").AsQueryable();
            PagedList<AllSearchDetailViewModel> res = new PagedList<AllSearchDetailViewModel>(AllSearchDetailViewModels, page, 10);
            ViewBag.title = title;
            return PartialView("AllSearchPartial", res);

        }
        [HttpGet]
        public IActionResult AllSearchWithType(string title, int page = 1)
        {
            IQueryable<AllSearchDetailViewModel> AllSearchDetailViewModels = _context.AllSearchDetailViewModels.FromSql("EXEC	 [dbo].[SearchAll] @param1 = N'" + title + "'").AsQueryable();
            PagedList<AllSearchDetailViewModel> res = new PagedList<AllSearchDetailViewModel>(AllSearchDetailViewModels, page, 10);
            ViewBag.title = title;
            return PartialView("AllSearchWithType", res);

        }
        public IActionResult AllSearchWithTypePartial(string title, int page = 1)
        {
            IQueryable<AllSearchDetailViewModel> AllSearchDetailViewModels = _context.AllSearchDetailViewModels.FromSql("EXEC	 [dbo].[SearchAll] @param1 = N'" + title + "'").AsQueryable();
            PagedList<AllSearchDetailViewModel> res = new PagedList<AllSearchDetailViewModel>(AllSearchDetailViewModels, page, 10);
            ViewBag.title = title;
            return PartialView("AllSearchWithTypePartial", res);
        }
        public IActionResult UserList(string userRolaNameEng, int page = 1, string userType = "آرایشگاه ها")
        {
            ViewData["userType"] = userType;
            var model = _Iuser.GetUserList(userRolaNameEng, page);

            // for add HairStylist in Academy
            var model_HairStylist = _Iuser.GetUserList("HairStylist", page);


            UserListViewModel UserListViewModel = new UserListViewModel();
            UserListViewModel.Users = model;



            UserListViewModel.Banner = _IBanner.GetFirst();
            UserListViewModel.userRolaNameEng = userRolaNameEng;
            if (userRolaNameEng == "SalonOwner")
            {
                UserListViewModel.UserTypeName = "آرایشگاه ها";

                model_HairStylist = _Iuser.GetUserList("HairStylist", page);
                UserListViewModel.RoleNameListLeftFa = "آرایشگر ها";

            }
            else if (userRolaNameEng == "Academy")
            {
                UserListViewModel.UserTypeName = "آموزشگاه ها";

                model_HairStylist = _Iuser.GetUserList("Mentor", page);
                UserListViewModel.RoleNameListLeftFa = "مربی ها";

            }
            else if (userRolaNameEng == "Store")
            {
                UserListViewModel.UserTypeName = "فروشگاه ها";
                UserListViewModel.RoleNameListLeftFa = "فروشگاه ها";

            }

            UserListViewModel.HairStylist = model_HairStylist;
            return View(UserListViewModel);
        }
        public IActionResult UserListPartial(string userRolaNameEng, int page = 1)
        {
            var model = _Iuser.GetUserList(userRolaNameEng, page);
            UserListViewModel UserListViewModel = new UserListViewModel();
            UserListViewModel.Users = model;
            return PartialView("UserListPartial", UserListViewModel);

        }
        public IActionResult ContactUs()
        {
            var model = _contactUs.Get();
            return View(model);
        }
        public IActionResult AboutUs()
        {
            var model = _aboutUs.Get();

            return View(model);
        }
        public IActionResult Rule()
        {
            var model = _rule.Get();

            return View(model);

        }
        public IActionResult AllReserveThisUser(SearchAllReserveViewModel searchAllReserveViewModel)
        {

            var userItem = _Iuser.GetUser(User.Identity.Name);
            var model = _reserve.Get(userItem.id, searchAllReserveViewModel);
            SearchAllReserveViewModel SearchAllReserveViewModel = new SearchAllReserveViewModel();
            SearchAllReserveViewModel.Reserves = model;
            return PartialView("_AllReserveThisUserPatial", SearchAllReserveViewModel);

        }
        public IActionResult FactorChargeAccount()
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            long totalprice = 0;
            var allprice = _context.AllPrices.FirstOrDefault();
            if (user != null)
            {
                if (user.isBuyOneMonth == true)
                    totalprice = Convert.ToInt64(allprice.threeSubscriptionPrice);
                else
                    totalprice = Convert.ToInt64(allprice.oneSubscriptionPrice);

                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.expireDateAccount = user.expireDateAccount;
                factorViewModel.isBuyOneMonth = user.isBuyOneMonth;
                if (totalprice != 0)
                {
                    factorViewModel.shouldpurshe = true;
                }
                else
                {
                    factorViewModel.shouldpurshe = false;
                    factorViewModel.totalAmount = "رایگان";
                }
                return View(factorViewModel);

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult FactorChargeAccountFree(FactorViewModel model)
        {
            var user = _Iuser.GetUser(User.Identity.Name);
            DateTime expireDate = DateTime.Now.Date.AddMonths(1).AddDays(8);
            user.expireDateAccount = expireDate;
            user.isBuyOneMonth = true;
            _context.SaveChanges();
            if (user.role.RoleNameEn == "SalonOwner" || user.role.RoleNameEn == "HairStylist")
            {
                var lines = _context.Lines.Where(x => x.userId == user.id);
                foreach (var item in lines)
                {
                    item.expireDate = expireDate;
                }
            }
            if (user.role.RoleNameEn == "Store")
            {
                var products = _context.Products.Where(x => x.userId == user.id);
                foreach (var item in products)
                {
                    item.expireDate = expireDate;
                }
            }
            if (user.role.RoleNameEn == "Academy" || user.role.RoleNameEn == "Mentor")
            {
                var classrooms = _context.ClassRooms.Where(x => x.userId == user.id);
                foreach (var item in classrooms)
                {
                    item.expireDate = expireDate;
                }
            }

            if (user.role.RoleNameEn == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome", new { ChargeFree = true });
            if (user.role.RoleNameEn == "Store")
                return RedirectToAction("Store", "CustomerHome", new { ChargeFree = true });
            if (user.role.RoleNameEn == "Academy")
                return RedirectToAction("Academy", "CustomerHome", new { ChargeFree = true });
            if (user.role.RoleNameEn == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome", new { ChargeFree = true });
            if (user.role.RoleNameEn == "HairStylist")
                return RedirectToAction("HairStylist", "CustomerHome", new { ChargeFree = true });
            else
                return RedirectToAction("Member", "CustomerHome");
        }
        [HttpPost]
        public IActionResult FactorChargeAccount(FactorViewModel model)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            var allprice = _context.AllPrices.FirstOrDefault();
            long totalPrice = 0;
            if (model.mounthCount == null)
                model.mounthCount = 1;
            if (user.isBuyOneMonth == true)
                totalPrice = Convert.ToInt32(model.mounthCount) * Convert.ToInt64(allprice.threeSubscriptionPrice);
            else
                totalPrice = Convert.ToInt64(allprice.oneSubscriptionPrice);
            model.isBuyOneMonth = user.isBuyOneMonth;
            Factor factor = new Factor();
            factor.state = State.IsPay;
            factor.userId = user.id;
            factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);
            factor.factorKind = FactorKind.ChargeUser;
            factor.totalPrice = totalPrice;
            _context.Factors.Add(factor);
            _context.SaveChanges();
            if (user != null)
            {
                if (model.PursheType == 2)
                {
                    if (user.score < Convert.ToInt64(totalPrice))
                    {
                        ViewData["Error"] = "امتیاز شما کافی نیست";
                        return View(model);
                    }
                    else
                    {
                        DateTime expireDate = DateTime.Now.AddMonths(Convert.ToInt32(model.mounthCount)).AddDays(8);
                        user.expireDateAccount = expireDate;
                        user.isBuyOneMonth = true;
                        user.score -= totalPrice;
                        _context.SaveChanges();
                        if (user.role.RoleNameEn == "SalonOwner" || user.role.RoleNameEn == "HairStylist")
                        {
                            var lines = _context.Lines.Where(x => x.userId == user.id);
                            foreach (var item in lines)
                            {
                                item.expireDate = expireDate;
                            }
                        }
                        if (user.role.RoleNameEn == "Store")
                        {
                            var products = _context.Products.Where(x => x.userId == user.id);
                            foreach (var item in products)
                            {
                                item.expireDate = expireDate;
                            }
                        }
                        if (user.role.RoleNameEn == "Academy" || user.role.RoleNameEn == "Mentor")
                        {
                            var classrooms = _context.ClassRooms.Where(x => x.userId == user.id);
                            foreach (var item in classrooms)
                            {
                                item.expireDate = expireDate;
                            }
                        }

                        if (user.role.RoleNameEn == "SalonOwner")
                            return RedirectToAction("SalonOwner", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "Store")
                            return RedirectToAction("Store", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "Academy")
                            return RedirectToAction("Academy", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "Mentor")
                            return RedirectToAction("Mentor", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "Student")
                            return RedirectToAction("Student", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "Member")
                            return RedirectToAction("Member", "CustomerHome", new { subScore = true });
                        if (user.role.RoleNameEn == "HairStylist")
                            return RedirectToAction("HairStylist", "CustomerHome", new { subScore = true });
                        else
                            return RedirectToAction("Student", "CustomerHome", new { subScore = true });
                    }
                }
                var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalPrice);
                var res = payment.PaymentRequest($"پرداخت {user.id}", "http://gisoooo.ir/api/ChargeUserFromWebSite/" + user.id + "/" + factor.id + "/" + Convert.ToInt32(model.mounthCount), "", "");
                string url = "";
                if (res.Result.Status == 100)
                {
                    url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                    return Redirect(url);
                }
                else
                {
                    return RedirectToAction(nameof(Index));

                }

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult AddReserveForLine(List<string> allShanbeh, List<string> allyekShanbeh, List<string> alldoShanbeh, List<string> allseShanbeh, List<string> allcharShanbeh, List<string> allpanjShanbeh, List<string> alljome, int lineId, string dateReserve, int mounthCount, bool isFromAdmin)
        {
            _LineWeekDate.Create(allShanbeh, allyekShanbeh, alldoShanbeh, allseShanbeh, allcharShanbeh, allpanjShanbeh, alljome, lineId, dateReserve, mounthCount);
            if (isFromAdmin)
                return Json(new { redirectToUrl = Url.Action("Index", "Line") });
            else
                return Json(new { redirectToUrl = Url.Action("SalonOwner", "CustomerHome", new { isRegisterLine = true }) });
        }
        public IActionResult ReserveWeekEdit(int lineId, string FromdateReserve, string TodateReserve)
        {
            List<LineWeekDate> Item = _LineWeekDate.GetForEdit(lineId, FromdateReserve, TodateReserve);
            ViewBag.lineId = lineId;
            if (String.IsNullOrEmpty(FromdateReserve))
                ViewBag.FromdateReserve = DateTime.Now;
            else
                ViewBag.FromdateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(FromdateReserve));
            if (String.IsNullOrEmpty(TodateReserve))
                ViewBag.TodateReserve = DateTime.Now.AddDays(31);
            else
                ViewBag.TodateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(TodateReserve));
            return PartialView("_WeekDateEditPartial", Item);
        }
        public IActionResult ReserveWeekByDate(int lineId, string FromdateReserve, string TodateReserve)
        {
            List<LineWeekDate> Item = _LineWeekDate.GetForEdit(lineId, FromdateReserve, TodateReserve);
            ViewBag.lineId = lineId;
            if (String.IsNullOrEmpty(FromdateReserve))
                ViewBag.FromdateReserve = DateTime.Now;
            else
                ViewBag.FromdateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(FromdateReserve));
            if (String.IsNullOrEmpty(TodateReserve))
                ViewBag.TodateReserve = DateTime.Now.AddDays(31);
            else
                ViewBag.TodateReserve = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(TodateReserve));
            return PartialView("_WeekDatePartial", Item);
        }
        [HttpPost]
        public IActionResult DeleteLineWeekDate(string LineWeekDateId)
        {
            int lineId = _LineWeekDate.Delete(LineWeekDateId);
            return Json(new { success = true, responseText = "Done" });

        }
        public IActionResult AddReserveLine(string allIds, int lineId)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            var line = _ILine.GetById(lineId);
            string[] values = allIds.Split(',');
            long price = 0;
            if (line.discountPrice != null && line.discountPrice != 0)
                price = Convert.ToInt64(line.discountPrice);
            else
                price = Convert.ToInt64(line.price);
            long totalprice = values.Count() * price;

            if (user != null)
            {
                factorViewModel.totalAmount = totalprice.ToString();
                factorViewModel.amount = totalprice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.lineId = lineId;
                factorViewModel.allIds = allIds;
                factorViewModel.title = line.title;
                return View("FactorCreatorForReserveClient", factorViewModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult FactorCreatorForReserveClient(FactorViewModel model, string s = "")

        {
            var user = _Iuser.GetUser(User.Identity.Name);
            if (model.lineId != null)
            {
                var line = _ILine.GetById(Convert.ToInt32(model.lineId));
                string[] values = model.allIds.Split(',');
                long price = 0;
                if (line.discountPrice != null && line.discountPrice != 0)
                    price = Convert.ToInt64(line.discountPrice);
                else
                    price = Convert.ToInt64(line.price);
                long totalprice = values.Count() * price;
                if (model.PursheType == 2)
                {
                    if (user.score < Convert.ToInt64(model.totalAmount))
                    {
                        ViewData["Error"] = "امتیاز شما کافی نیست";
                        return View(model);
                    }
                    else
                    {
                        _LineWeekDate.ReserveLine(model.allIds, line, user, true);

                    }

                }
                else
                {
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmReserveLineFromWebSite/" + model.allIds + "/" + line.id + "/" + user.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;

                        return Redirect(url);
                    }

                }
                return RedirectToAction("LineDetail", "CustomerHome", new { line.id, line.title, isReserved = true });
            }
            else
            {
                var classRoom = _IClassRoom.GetById(Convert.ToInt32(model.classRoomId));
                long totalprice = 0;
                if (classRoom.discountPrice != null && classRoom.discountPrice != 0)
                    totalprice = Convert.ToInt64(classRoom.discountPrice);
                else
                    totalprice = Convert.ToInt64(classRoom.price);
                if (model.PursheType == 2)
                {
                    if (user.score < Convert.ToInt64(model.totalAmount))
                    {
                        ViewData["Error"] = "امتیاز شما کافی نیست";
                        return View(model);
                    }
                    else
                    {
                        _IClassRoom.Reserve(classRoom, user, true);
                    }

                }
                else
                {
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {model.ItemId}", "http://gisoooo.ir/api/ConfirmReserveClassRoomFromWebSite/" + classRoom.id + "/" + user.id, "", "");
                    string url = "";
                    if (res.Result.Status == 100)
                    {
                        url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority;
                        return Redirect(url);
                    }

                }
                return RedirectToAction("ClassRoomDetail", "CustomerHome", new { classRoom.id, classRoom.title, isReserved = true });

            }

        }

        public IActionResult AddReserveClassRoom(int classRoomId)
        {
            FactorViewModel factorViewModel = new FactorViewModel();
            var user = _Iuser.GetUser(User.Identity.Name);
            var classRoom = _IClassRoom.GetById(classRoomId);
            long totalPrice = 0;
            if (classRoom.discountPrice != null && classRoom.discountPrice != 0)
                totalPrice = Convert.ToInt64(classRoom.discountPrice);
            else
                totalPrice = Convert.ToInt64(classRoom.price);

            if (user != null)
            {
                factorViewModel.totalAmount = totalPrice.ToString();
                factorViewModel.amount = totalPrice;
                factorViewModel.date = DateTime.Now;
                factorViewModel.fullName = user.fullname;
                factorViewModel.classRoomId = classRoomId;
                factorViewModel.title = classRoom.title;
                return View("FactorCreatorForReserveClient", factorViewModel);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Mentor(bool isRegisterClassRoom = false, bool isRegisterAdvertisment = false, bool isRegisterNotice = false, bool subScore = false, int isNoticeShortcut = 0, int isadverstitmentShortcut = 0)
        {
            ClassRoomViewModel classRoomViewModel = new ClassRoomViewModel();
            classRoomViewModel.ClassRoomTypes = _IClassRoomType.GetClassRoomTypes();
            ViewBag.isRegisterClassRoom = isRegisterClassRoom;
            ViewBag.subScore = subScore;
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            if (!UserItem.role.RoleNameEn.Equals("Mentor"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (UserItem != null)
            {
                ViewBag.isRegisterAdvertisment = isRegisterAdvertisment;
                ViewBag.isRegisterNotice = isRegisterNotice;
                ViewBag.UserName = UserItem.fullname;
                ViewBag.identifyCode = UserItem.identifiecodeOwner;
                ViewBag.url = UserItem.url;
                ViewBag.UserScore = UserItem.score;
                ViewBag.TotalAdvertisments = _Iuser.TotalAdvertisments(UserItem.id);
                ViewBag.TotalNotice = _Iuser.TotalNotice(UserItem.id);
                ViewBag.TotalReserve = _Iuser.TotalReserve(UserItem.id);
                ViewBag.TotalCountRegisterClass = _IClassRoom.CountRegisterClass(UserItem.id);
                ViewBag.expireDate = UserItem.expireDateAccount;
                ViewBag.isNoticeShortcut = isNoticeShortcut;
                ViewBag.hasadverstitmentshortcut = isadverstitmentShortcut;
                ViewBag.isProfileAccept = UserItem.isProfileAccept;
                ViewBag.isProfileComplete = UserItem.isProfileComplete;

            }
            return View(classRoomViewModel);
        }
        public IActionResult ChangeRole()
        {
            return PartialView("ChangeRolePartial", _Irole.Get());
        }
        [HttpPost]
        public IActionResult ChangeRole(string roleNameEng)
        {
            var UserItem = _Iuser.GetUser(User.Identity.Name);
            _Iuser.ChangeRole(UserItem, roleNameEng);
            if (roleNameEng == "SalonOwner")
                return RedirectToAction("SalonOwner", "CustomerHome");
            if (roleNameEng == "Store")
                return RedirectToAction("Store", "CustomerHome");
            if (roleNameEng == "Academy")
                return RedirectToAction("Academy", "CustomerHome");
            if (roleNameEng == "Mentor")
                return RedirectToAction("Mentor", "CustomerHome");
            if (roleNameEng == "Student")
                return RedirectToAction("Student", "CustomerHome");
            if (roleNameEng == "HairStylist")
                return RedirectToAction("HairStylist", "CustomerHome");

            else
                return RedirectToAction("Student", "CustomerHome");
        }
        public IActionResult ArticleList(int page = 1)
        {
            var model = _Article.GetArticles(page);
            ArticleListViewModel ArticleListViewModel = new ArticleListViewModel();
            ArticleListViewModel.Articles = model;
            ArticleListViewModel.Banner = _IBanner.GetFirst();
            return View(ArticleListViewModel);
        }
        public IActionResult ArticleListPartial(int page = 1)
        {
            var model = _Article.GetArticles(page);
            ArticleListViewModel ArticleListViewModel = new ArticleListViewModel();
            ArticleListViewModel.Articles = model;
            return PartialView("ArticleListPartial", ArticleListViewModel);
        }
        [Route("CustomerHome/ArticleDetail/{id}/{title}")]

        public IActionResult ArticleDetail(int id, string title)
        {
            ArticleDetailViewModel articleDetailViewModel = new ArticleDetailViewModel();
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            articleDetailViewModel.Article = _Article.GetByIdForDetail(id);
            articleDetailViewModel.OtherArticle = _Article.GetRelated(id);
            articleDetailViewModel.Banner = _IBanner.GetFirst();
            return View(articleDetailViewModel);
        }
        #endregion
    }

}
