using Gisoo.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }        

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string cellphone { get; set; }

        [Display(Name = "پسورد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string password { get; set; }

        [Display(Name = "پسورد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string passwordShow { get; set; }

       
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string token { get; set; }
        [ForeignKey("role")]
        public int roleId { get; set; }
        public Role role { get; set; }

        [Display(Name = "کد")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string code { get; set; }
        [Display(Name = "کدملی")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string nationalCode { get; set; }
        public long score { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string fullname { get; set; }
        public bool  userStatus { get; set; }
        public string url { get; set; }
         [Display(Name = "توضیحات مختصر")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string shortDescription { get; set; }
         [Display(Name = "توضیحات تکمیلی")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string longDescription { get; set; }
         [Display(Name = "آدرس")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string address { get; set; }
        [Display(Name = "ساعت کاری")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string workingHours { get; set; }
        [Display(Name = "مجوز دارد")]
        public bool hasCertificate { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
		public ICollection<UserDocumentImage> UserDocumentImages { get; set; }
         [Display(Name = "لینک تلگرام")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string linkTelegram { get; set; }
        [Display(Name = "لینک اینستاگرام")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string linkInstagram { get; set; }
        [Display(Name = "شماره شبا")]
        [MaxLength(16, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string shebaNumber { get; set; }
        [Display(Name = "کد معرف")]
        [MaxLength(8, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        
        public string identifiecode { get; set; }
         [Display(Name = "کد شخص برای معرفی")]
        [MaxLength(8, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        
        public string identifiecodeOwner { get; set; }
        public bool isscored { get; set; }

		public DateTime? expireDateAccount { get; set; }
		public bool isProfileAccept { get; set; }
		public bool isProfileComplete { get; set; }
         [Display(Name = "دلیل رد پروفایل")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string notConfirmDes { get; set; }
		public bool isBuyOneMonth { get; set; }
		public bool sexuality { get; set; }

    }
}
