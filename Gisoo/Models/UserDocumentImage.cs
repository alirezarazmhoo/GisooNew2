using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
	public class UserDocumentImage
	{
		public int id { get; set; }
		[MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
		public string url { get; set; }
		public int userId { get; set; }
		public User  user { get; set; }

	}
}
