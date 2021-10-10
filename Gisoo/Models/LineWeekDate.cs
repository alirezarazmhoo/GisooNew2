using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
	
	public class LineWeekDate
	{
		public string id { get; set; }
		
		public int lineId { get; set;}
		public Line line { get; set;}
		[Display(Name = "تاریخ ")]
		public DateTime date { get; set; }
		[Display(Name = "از ساعت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string fromTime { get; set; }
		[Display(Name = "تا ساعت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string toTime { get; set; }
        public bool isReserved{ get; set; }
	}
}
