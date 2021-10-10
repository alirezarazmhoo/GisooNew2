using Gisoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.ViewModel
{
    public class NoticeViewModel
    {
        public int id { get; set; }
        public string notConfirmDescription { get; set; }
        public EnumStatus adminConfirmStatus { get; set; }
    }
}
