using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public class AndroidVersion
    {
        [Key]
        public int id { get; set; }
        [MaxLength(70)]
        public string appAndroidUrl { get; set; }
        [MaxLength(20)]
        [Display(Name = "ورژن ")]

        public string currVersion { get; set; }
    }
}
