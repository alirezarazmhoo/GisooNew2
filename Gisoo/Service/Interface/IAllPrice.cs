using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface IAllPrice
    {
       object GetAllPrices();
       PagedList<AllPrice> GetAllPrices(int pageId = 1, string filterName = "");

    }
}
