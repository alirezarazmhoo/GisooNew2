using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface ICity
    {
       object GetCities();
       PagedList<City> GetCities(int pageId = 1, string filterName = "");
        object GetProvinces(int cityId);
         object GetAreas(int provinceId);
        List<City> GetCitiesList();

        List<Province> GetProvincesList(int cityId);

         List<Area> GetAreasList(int provinceId);

    }
}
