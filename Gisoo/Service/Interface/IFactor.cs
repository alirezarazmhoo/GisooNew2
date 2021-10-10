using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface IFactor
    {
       PagedList<Factor> GetFactors(int pageId = 1);
   

    }
}
