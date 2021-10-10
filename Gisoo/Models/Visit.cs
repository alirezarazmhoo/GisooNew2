using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public enum WhichTableEnum
    {
        Notice = 1,
        Advertisment = 2, ClssRoom= 3,Line=4,Product=5
    }
    public class Visit
    {
         [Key]
        public int id { get; set; }
       
        public int anyNoticeId { get; set; }
      
		public string Ip { get; set; }
        [Display(Name = "تاریخ ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public DateTime date { get; set; }
        public WhichTableEnum whichTableEnum { get; set; }

    }
}
