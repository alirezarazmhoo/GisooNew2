using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Models
{
    public class AllPrice
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "قیمت برای آرایشگران")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long barberPrice { get; set; }

        [Display(Name = "قیمت برای ورک شاپ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long workshopPrice { get; set; }

        [Display(Name = "قیمت برای تبلیغات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long advertismentPrice { get; set; }

        [Display(Name = "قیمت برای آرایشگاه ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long saloonPrice { get; set; }
        [Display(Name = "قیمت برای اشتراک سه ماهه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? threeSubscriptionPrice { get; set; }
        [Display(Name = "قیمت برای اشتراک یک ماهه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? oneSubscriptionPrice { get; set; }

        public bool isHasSaloonPrice { get; set; }
        public bool isHasAdvertismentPrice { get; set; }
        public bool isHasWorkshopPrice { get; set; }
        public bool isHasBarberPrice { get; set; }
         [Display(Name = "حداقل تخفیف به درصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? minDiscount { get; set; }
        [Display(Name = "حداکثر تخفیف به درصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long? maxDiscount { get; set; }
         [Display(Name = "امتیاز اختصاص داده شده به شخصی که از طریق کدمعرف ثبت نام میکند")]
        public int? scoreWithInturducer { get; set; }

    }
}
