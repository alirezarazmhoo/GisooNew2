using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.ViewModel
{
    public class AndroidVersionViewModel
    {
        public int id { get; set; }
        [MaxLength(70)]
        public string appAndroidUrl { get; set; }
        [MaxLength(20)]
        [Display(Name = "ورژن ")]

        public string currVersion { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = " apk انتخاب فایل")]
        public IFormFile files { get; set; }

    }
}
