using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
	
	public class ClassRoom
	{
		public int id { get; set; }
		[Display(Name = "عنوان کلاس")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set;}
		[Display(Name = "توضیحات")]
		[MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string description { get; set; }
       
		public long price { get; set;}
		public int userId { get; set;}
		public User user { get; set;}
		public int classRoomTypeId { get; set; }
		public ClassRoomType classRoomType { get; set; }
		public EnumLineLaw classRoomLaw { get; set; }
		public DateTime registerDate { get; set; }
		[Display(Name = "تاریخ رزرو")]

		public DateTime? reserveDate { get; set; }
		[Display(Name = "ساعت رزرو")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string reserveHour { get; set; }
		public ICollection<ClassRoomImage> ClassRoomImages { get; set; }
		public EnumStatus adminConfirmStatus { get; set; }
		[Display(Name = "آمار بازدید")]

        public int? countView { get; set; }
		[Display(Name = "تاریخ انقضا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
       
        public DateTime expireDate { get; set; }
		[Display(Name = "دلیل رد")]
        [MaxLength(1000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string notConfirmDescription { get; set; }
		
        public long discountPrice { get; set; }

		[Display(Name = "نام و نام خانوادگی مدرس")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string classRoomTeacher { get; set; }
		[Display(Name = "طول دوره")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string classRoomPeriod { get; set; }
	}
}
