using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
	public enum EnumLineLaw
    {
		hire=1,
		percent=2,
		law=3,
		reserve=4
    }
	public class Line
	{
		
		public int id { get; set; }
		[Display(Name = "عنوان")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set;}
		[Display(Name = "توضیحات")]
		[MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string description { get; set; }
		public long price { get; set;}
		public int userId { get; set;}
		public User user { get; set;}
		public int lineTypeId { get; set; }
		public LineType lineType { get; set; }
		public EnumLineLaw lineLaw { get; set; }
		public DateTime registerDate { get; set; }
        [Display(Name = "تاریخ رزرو")]

        public DateTime? reserveDate { get; set; }
        [Display(Name = "ساعت رزرو")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string reserveHour { get; set; }
        public ICollection<LineImage> LineImages { get; set; }
		public EnumStatus adminConfirmStatus { get; set; }
		[Display(Name = "آمار بازدید")]

        public int? countView { get; set; }
		[Display(Name = "تاریخ انقضا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
       
        public DateTime expireDate { get; set; }
		[Display(Name = "دلیل رد")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string notConfirmDescription { get; set; }
        public long? discountPrice { get; set; }

		[Display(Name = "نام و نام خانوادگی مدرس")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string lineTeacher { get; set; }

	}
}
