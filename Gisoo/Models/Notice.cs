using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public enum ConditionEnum
    {
        Percent = 1,
        Rent = 2, 
        FixedSalary = 3
    }
    public class Notice
    {
        [Key]
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

        [Display(Name = "عنوان آگهی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }

        public ConditionEnum condition { get; set; }
        public bool isBarber { get; set; }
        [Display(Name = "دلیل رد")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string notConfirmDescription { get; set; }
       
        public EnumStatus adminConfirmStatus { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

        [ForeignKey("city")]
        public int cityId { get; set; }
        public virtual City city { get; set; }

        [ForeignKey("province")]
        public int provinceId { get; set; }
        public virtual Province province { get; set; }

        [ForeignKey("area")]
        public int areaId { get; set; }
        public virtual Area area { get; set; }

        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public DateTime createDate { get; set; }
         [Display(Name = "تاریخ انقضا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public DateTime expireDate { get; set; }
        public bool isDeleted { get; set; }
        [Display(Name = "آمار بازدید")]

        public int? countView { get; set; }

    }
}
