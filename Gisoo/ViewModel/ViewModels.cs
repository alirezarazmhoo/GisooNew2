using Gisoo.Models;
using Gisoo.Models.Enums;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.ViewModel
{
    #region SalonOwner
    public class LineViewModel
    {
        public int id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]

        public string title { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }
        public long price { get; set; }
        //[Range(10, 1000, 
        //ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public long? discountPrice { get; set; }
        public long minDiscount { get; set; }
        public long maxDiscount { get; set; }
        public int lineTypeId { get; set; }
        public int userId { get; set; }
        public List<LineType> LineTypes { get; set; }
        public List<LineImage> LineImage { get; set; }
        [Display(Name = "نوع هزینه")]

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public EnumLineLaw lineLaw { get; set; }

        [Display(Name = "تاریخ رزرو")]

        public string reserveDate { get; set; }
        [Display(Name = "ساعت رزرو")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string reserveHour { get; set; }
        [Display(Name = "نام و نام خانوادگی مدرس")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string lineTeacher { get; set; }
    }
    public class SearchLineViewModel
    {
        public string title { get; set; }
        public EnumLineLaw lineLaw { get; set; }
        public string registerDate { get; set; }
        public int lineTypeId { get; set; }
        public List<LineType> LineTypes { get; set; }
        public List<Line> Lines { get; set; }

    }
    public class AdvertismentViewModel
    {
        public int id { get; set; }

        [Display(Name = "تصویر اول ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image1 { get; set; }
        [Display(Name = "تصویر دوم ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image2 { get; set; }
        [Display(Name = "تصویر سوم ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image3 { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000
            , ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]

        public string description { get; set; }

        public EnumStatus adminConfirmStatus { get; set; }
        public bool isWorkshop { get; set; }
        [Display(Name = "کاربر")]

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public int userId { get; set; }
        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int cityId { get; set; }
        [Display(Name = "شهرستان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int provinceId { get; set; }
        [Display(Name = "محدوده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int areaId { get; set; }

        [Display(Name = "کد")]


        public string code { get; set; }

        public bool isDeleted { get; set; }
        public List<City> cities { get; set; }
        public List<Province> provinces { get; set; }
        public List<Area> areas { get; set; }

    }
    public class SearchAdvertismentViewModel
    {
        public string title { get; set; }
        public string registerDate { get; set; }
        public List<Advertisment> Advertisments { get; set; }

    }
    public class Notice2ViewModel
    {
        public int id { get; set; }

        [Display(Name = "تصویر اول ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image1 { get; set; }
        [Display(Name = "تصویر دوم ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image2 { get; set; }
        [Display(Name = "تصویر سوم ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image3 { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }

        public EnumStatus adminConfirmStatus { get; set; }
        public ConditionEnum condition { get; set; }
        public bool isBarber { get; set; }

        public int userId { get; set; }
        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int cityId { get; set; }
        [Display(Name = "شهرستان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int provinceId { get; set; }
        [Display(Name = "محدوده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int areaId { get; set; }

        [Display(Name = "کد")]
        public string code { get; set; }

        public bool isDeleted { get; set; }
        public List<City> cities { get; set; }
        public List<Province> provinces { get; set; }
        public List<Area> areas { get; set; }

    }
    public class SearchNoticeViewModel
    {
        public string title { get; set; }
        public string registerDate { get; set; }
        public List<Notice> Notices { get; set; }
    }
    #endregion
    #region Academy
    public class ClassRoomViewModel
    {
        public int id { get; set; }
        [Display(Name = "عنوان کلاس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }

        public long price { get; set; }
        public long? discountPrice { get; set; }
        public long minDiscount { get; set; }
        public long maxDiscount { get; set; }
        public int classRoomTypeId { get; set; }
        public int userId { get; set; }
        public List<ClassRoomType> ClassRoomTypes { get; set; }
        public List<ClassRoomImage> ClassRoomImage { get; set; }
        public EnumLineLaw classRoomLaw { get; set; }

        [Display(Name = "تاریخ رزرو")]

        public string reserveDate { get; set; }
        [Display(Name = "ساعت رزرو")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public string reserveHour { get; set; }
        [Display(Name = "نام و نام خانوادگی مدرس")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string classRoomTeacher { get; set; }
		[Display(Name = "طول دوره")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string classRoomPeriod { get; set; }
    }
    public class SearchClassRoomViewModel
    {
        public string title { get; set; }
        public EnumLineLaw classRoomLaw { get; set; }
        public string registerDate { get; set; }
        public int classRoomTypeId { get; set; }
        public List<ClassRoomType> ClassRoomTypes { get; set; }
        public List<ClassRoom> ClassRooms { get; set; }

    }
    public class SearchAllReserveViewModel
    {
        public string userMobile { get; set; }
        public string userFullName { get; set; }
        public string registerDate { get; set; }
        public List<Reserve> Reserves { get; set; }

    }

    #endregion
    #region Store
    public class ProductViewModel
    {
        public int id { get; set; }
        [Display(Name = "عنوان کلاس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }

        public long price { get; set; }
        public long? discountPrice { get; set; }
        public long minDiscount { get; set; }
        public long maxDiscount { get; set; }
        public int userId { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        [Display(Name = "تاریخ ثبت")]

        public string createDate { get; set; }

    }
    public class SearchProductViewModel
    {
        public string title { get; set; }
        public string createDate { get; set; }
        public List<Product> Products { get; set; }

    }
    #endregion
    #region Home
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Advertisment> Advertisments { get; set; }
        public List<Advertisment> AdvertismentNotWorkshops { get; set; }
        public List<Notice> Notices { get; set; }
        public List<Notice> NoticeNotBarbers { get; set; }
        public List<Line> Lines { get; set; }
        public List<Line> LineIsService { get; set; }
        public List<Article> Articles { get; set; }
        public List<Banner> Banners { get; set; }
        public List<ClassRoom> ClassRooms { get; set; }
        public List<ClassRoom> ClassRoomReserves { get; set; }
        public List<Product> Products { get; set; }
        public List<User> SalonOwnerUsers { get; set; }
        public List<User> AcademyUsers { get; set; }
        public List<User> HairStylistUsers { get; set; }
        public List<User> MentorUsers { get; set; }
        public List<User> StoreUsers { get; set; }
    }
    #endregion

    #region Student
    public class SearchReserveViewModel
    {
        public string title { get; set; }
        public string registerDate { get; set; }
        public List<Reserve> Reserve { get; set; }
        public List<LineType> LineTypes { get; set; }
        public int lineTypeId { get; set; }

    }
    public class SearchClassRoomReserveViewModel
    {
        public string title { get; set; }
        public string registerDate { get; set; }
        public List<Reserve> Reserve { get; set; }
        public List<ClassRoomType> ClassRoomTypes { get; set; }
        public int ClassRoomTypesId { get; set; }

    }

    #endregion
    #region Details
    public class NoticeDetailViewModel
    {
        public Notice Notice { get; set; }
        public List<Notice> OtherNotice { get; set; }
        public Banner Banner { get; set; }

    }
    public class AdvertismentDetailViewModel
    {
        public Advertisment Advertisment { get; set; }
        public List<Advertisment> OtherAdvertisment { get; set; }
        public Banner Banner { get; set; }

    }
    public class ClassRoomDetailViewModel
    {
        public ClassRoom ClassRoom { get; set; }
        public List<ClassRoom> OtherClassRoom { get; set; }
        public Banner Banner { get; set; }

    }
    public class LineDetailViewModel
    {
        public Line Line { get; set; }
        public List<Line> OtherLine { get; set; }
        public List<LineWeekDate> LineWeekDates { get; set; }

        public Banner Banner { get; set; }

    }
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product> OtherProduct { get; set; }
        public Banner Banner { get; set; }

    }
     public class ArticleDetailViewModel
    {
        public Article Article { get; set; }
        public List<Article> OtherArticle { get; set; }
        public Banner Banner { get; set; }

    }
    #endregion


    #region Profile
    public class ProfileViewModel
    {
        public int id { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string fullname { get; set; }
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]

        public string nationalCode { get; set; }
        public string url { get; set; }

        public string shortDescription { get; set; }
        public string longDescription { get; set; }
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string address { get; set; }
        public string workingHours { get; set; }
        public bool hasCertificate { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string rolename { get; set; }
        public IFormFile imageUrl { get; set; }


        public IFormFile[] imageUrldocuments { get; set; }
        public List<UserDocumentImage> UserDocumentImages { get; set; }
        [Display(Name = "لینک تلگرام")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string linkTelegram { get; set; }
        [Display(Name = "لینک اینستاگرام")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string linkInstagram { get; set; }
        [Display(Name = "شماره شبا")]
        [MaxLength(16, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string shebaNumber { get; set; }
		public bool sexuality { get; set; }


    }

    #endregion
    #region List
    public class LineListViewModel
    {
        public PagedList<Line> Lines { get; set; }
        public List<Line> CheapestLines { get; set; }
        public Banner Banner { get; set; }

    }
    public class ClassRoomListViewModel
    {
        public PagedList<ClassRoom> ClassRooms { get; set; }
        public List<ClassRoom> CheapestClassRooms { get; set; }
        public Banner Banner { get; set; }

    }
    public class ProductListViewModel
    {
        public PagedList<Product> Products { get; set; }
        public List<Product> CheapestProducts { get; set; }
        public Banner Banner { get; set; }

    }
    public class ArticleListViewModel
    {
        public PagedList<Article> Articles { get; set; }
        public Banner Banner { get; set; }
    }
    public class NoticeListViewModel
    {
        public PagedList<Notice> Notices { get; set; }
        public List<Notice> PercentNotices { get; set; }
        public List<Notice> RentNotices { get; set; }
        public List<Notice> FixedSalaryNotices { get; set; }
        public Banner Banner { get; set; }

        public List<Notice> NoticeNotBarbers { get; set; }

    }
    public class UserListViewModel
    {
        public PagedList<User> Users { get; set; }
        public Banner Banner { get; set; }
        public string userRolaNameEng { get; set; }
        public string UserTypeName { get; set; }


        public string RoleNameListLeftFa { get; set; }
        public PagedList<User> HairStylist { get; set; }

    }
    public class AdvertismentListViewModel
    {
        public PagedList<Advertisment> Advertisments { get; set; }
        public List<Advertisment> IsWorkshopAdvertisments { get; set; }
        public Banner Banner { get; set; }

    }
    #endregion


    #region FactorAndPaygment
    public class FactorViewModel
    {
        public string fullName { get; set; }
        public DateTime date { get; set; }
        public long amount { get; set; }
        public string totalAmount { get; set; }
        public int type { get; set; }
        public bool isWorkshop { get; set; }
        public bool isBarber { get; set; }

        public bool shouldpurshe { get; set; }

        public int ItemId { get; set; }

        public int isNotice { get; set; }

        public int PursheType { get; set; }
        public int? mounthCount { get; set; }
        public int mode { get; set; }
        public NoticeType NoticeType { get; set; }

        public DateTime reserveDate { get; set; }
        public DateTime? expireDateAccount { get; set; }
        public bool isBuyOneMonth { get; set; }
        public string allIds { get; set; }
        public int? lineId { get; set; }
        public int? classRoomId { get; set; }
        public string title { get; set; }

    }
    #endregion

    #region ProfileUserSingle
    public class ProfileUserSingleViewModel
    {
        public User User { get; set; }
        public Banner Banner { get; set; }
        public List<Advertisment> Advertisments { get; set; }
        public List<Notice> Notices { get; set; }
        public List<ClassRoom> ClassRooms { get; set; }
        public List<Line> Lines { get; set; }
        public List<Product> Products { get; set; }
    }
    #endregion
    #region Visit
    public class VisitViewModel
    {
        public long datecount1 { get; set; }
        public long datecount2 { get; set; }
        public long datecount3 { get; set; }
        public long datecount4 { get; set; }
        public long datecount5 { get; set; }
        public long datecount6 { get; set; }
        public long datecount7 { get; set; }
        public long datecount8 { get; set; }
        public long datecount9 { get; set; }
        public long datecount10 { get; set; }
        public long? viewTotalCount { get; set; }
        public Notice Notice { get; set; }
        public Advertisment Advertisment { get; set; }
        public ClassRoom ClassRoom { get; set; }
        public Line Line { get; set; }
        public Product Product { get; set; }

    }
    #endregion
    public class UserDocumentImageViewModel
    {
        public int id { get; set; }

        public string url { get; set; }


    }

    public class LineViewModelAdmin
    {
        public Line line { get; set; }
        public List<LineType> lineTypes { get; set; }
        public List<User> Users { get; set; }
        public string expireDate1 { get; set; }
        public string reserveDate1 { get; set; }
        public long datecount1 { get; set; }
        public long datecount2 { get; set; }
        public long datecount3 { get; set; }
        public long datecount4 { get; set; }
        public long datecount5 { get; set; }
        public long datecount6 { get; set; }
        public long datecount7 { get; set; }
        public long datecount8 { get; set; }
        public long datecount9 { get; set; }
        public long datecount10 { get; set; }
        public long? viewTotalCount { get; set; }

    }
    public class ClassRoomViewModelAdmin
    {
        public ClassRoom ClassRoom { get; set; }
        public List<ClassRoomType> ClassRoomTypes { get; set; }
        public List<User> Users { get; set; }
        public string expireDate1 { get; set; }
        public string reserveDate1 { get; set; }
        public long datecount1 { get; set; }
        public long datecount2 { get; set; }
        public long datecount3 { get; set; }
        public long datecount4 { get; set; }
        public long datecount5 { get; set; }
        public long datecount6 { get; set; }
        public long datecount7 { get; set; }
        public long datecount8 { get; set; }
        public long datecount9 { get; set; }
        public long datecount10 { get; set; }
        public long? viewTotalCount { get; set; }

    }
    public class ProductViewModelAdmin
    {
        public Product Product { get; set; }
        public string expireDate1 { get; set; }
        public List<User> Users { get; set; }

        public long datecount1 { get; set; }
        public long datecount2 { get; set; }
        public long datecount3 { get; set; }
        public long datecount4 { get; set; }
        public long datecount5 { get; set; }
        public long datecount6 { get; set; }
        public long datecount7 { get; set; }
        public long datecount8 { get; set; }
        public long datecount9 { get; set; }
        public long datecount10 { get; set; }
        public long? viewTotalCount { get; set; }

    }
    public class AdvertismentViewModelAdmin
    {
        public Advertisment Advertisment { get; set; }
        public List<User> Users { get; set; }
        public List<City> Cities { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Area> Areas { get; set; }
        public long? viewTotalCount { get; set; }

    }
    #region Search
    public enum EnumTableType
    {
        Notice = 1,
        Advertisment = 2,
        Line = 3,
        Product = 4,
        ClassRoom = 5
    }
    public class AllSearchDetailViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image1 { get; set; }
        public int userId { get; set; }
        public EnumTableType tabletype { get; set; }
    }
    #endregion

    public class AdViewModel
    {
        public List<Notice> Notices { get; set; }
        public List<Notice> NoticeNotBarbers { get; set; }
    }
}
