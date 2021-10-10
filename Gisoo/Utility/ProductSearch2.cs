using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Utility
{
    public class ProductSearch2
    {
        public string title { get; set; }
        public int page { get; set; }
        public int? areaId { get; set; }
        public int? provinceId { get; set; }
        public int? cityId { get; set; }
        public bool? isBarber { get; set; }
        public bool? isWorkshop { get; set; }

    }
}
