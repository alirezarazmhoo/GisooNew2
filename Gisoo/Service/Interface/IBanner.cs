using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
    public interface IBanner
    {
        object GetBanners();
        PagedList<Banner> GetBanners(int pageId = 1, string filterLink = "");
        int AddBannerFromAdmin(Banner Banner);
        int AddBanner(Banner Banner);
        void EditBanner(Banner Banner);
        Banner FindById(int id);
        Task<bool> Delete(int id);
        List<Banner> GetAllHome();
        Banner GetFirst();

    }
}
