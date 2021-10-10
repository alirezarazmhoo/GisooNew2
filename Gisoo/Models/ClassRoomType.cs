using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
	public class ClassRoomType
	{
		public int id { get; set; }
        [Display(Name = "عنوان")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string name { get; set; }
	}
}
