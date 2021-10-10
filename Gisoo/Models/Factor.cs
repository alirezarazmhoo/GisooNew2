using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public enum State
    {
        IsPay = 1,
        NotPay = 2,


    }
    public enum FactorKind
    {
        Ladder=1,
        Extend=2,
        Add=3,
        ChargeUser=4
            
    }
    public class Factor
    {
        [Key]
        public int id { get; set; }

        //[ForeignKey("product")]
        //public int? productId { get; set; }
        //public virtual Product product { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

        [ForeignKey("advertisment")]
        public int? advertismentId { get; set; }
        public virtual Advertisment advertisment { get; set; }

        [ForeignKey("notice")]
        public int? noticeId { get; set; }
        public virtual Notice notice { get; set; }

        public State state { get; set; }
        public FactorKind factorKind { get; set; }

        
        [Display(Name = "تاریخ ایجاد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string createDatePersian { get; set; }

        [Display(Name = "قیمت کل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long totalPrice { get; set; }

         [ForeignKey("line")]
        public int? lineId { get; set; }
        public virtual Line line { get; set; }
         [ForeignKey("classRoom")]
        public int? classRoomId { get; set; }
        public virtual ClassRoom classRoom { get; set; }
         [ForeignKey("product")]
        public int? productId { get; set; }
        public virtual Product product { get; set; }
		public string LineWeekDateId { get; set; }

    }
}
