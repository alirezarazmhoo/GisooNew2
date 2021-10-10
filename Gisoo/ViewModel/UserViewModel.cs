using Gisoo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string cellphone { get; set; }
        [Display(Name = "کدملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد .")]
        public string nationalCode { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string fullname { get; set; }
        public int roleId { get; set; }
        [Display(Name = "کد")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string code { get; set; }

        public string returnUrl { get; set; }

        public string identifiecode { get; set; }
        public int isNoticeShortcut { get; set; }

        public int isAdverstitment { get; set; }
		public bool sexuality { get; set; }
      

    }
}
