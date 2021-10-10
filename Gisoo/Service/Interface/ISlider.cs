using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface ISlider
    {
       object GetSliders();
       List<Slider> GetSlidersWebSite();
       PagedList<Slider> GetSliders(int pageId = 1, string filterLink = "");
       int AddSliderFromAdmin(Slider Slider);
       int AddSlider(Slider Slider);
       void EditSlider(Slider Slider);
    }
}
