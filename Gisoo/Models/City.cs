using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public class City
    {
         public int id { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
    }
}
