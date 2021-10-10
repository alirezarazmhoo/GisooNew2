using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string description { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime createDate { get; set; }
        [Display(Name = "قیمت")]
		public long price { get; set;}
		public ICollection<ProductImage> ProductImages { get; set; }
        public int userId { get; set;}
		public User user { get; set;}
		public EnumStatus adminConfirmStatus { get; set; }
        [Display(Name = "آمار بازدید")]

        public int? countView { get; set; }
        [Display(Name = "تاریخ انقضا")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public DateTime expireDate { get; set; }
		[Display(Name = "دلیل رد")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string notConfirmDescription { get; set; }
        public long? discountPrice { get; set; }

    }
}
