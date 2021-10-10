using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
   
    public class Article
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "تصویر ")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string image { get; set; }
        [NotMapped]
        [Display(Name = "تصویر ")]
        public IFormFile imageUrl { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string title { get; set; }
        [Display(Name = "توضیحات")]
        public string description { get; set; }
    }
}
