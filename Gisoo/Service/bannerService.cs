using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class bannerService : IBanner
    {
        private Context _context;
        private readonly IHostingEnvironment environment;

        public bannerService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        public object GetBanners()
        {
            IQueryable<Banner> result = _context.Banners;
            List<Banner> res = result.OrderByDescending(u => u.id).ToList();
            return new { data = res };
        }
         public Banner FindById(int id)
        {
            return   _context.Banners.FirstOrDefault(m => m.id == id);
        }
        public PagedList<Banner> GetBanners(int page = 1, string filterLink = "")
        {
            IQueryable<Banner> result = _context.Banners.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterLink))
            {
                result = result.Where(u => u.link.Contains(filterLink));
            }
            PagedList<Banner> res = new PagedList<Banner>(result, page, 20);
            return res;
        }

        public int AddBannerFromAdmin(Banner Banner)
        {
            #region Save Image
            if (Banner.imageUrl != null)
            {
                string imagePath = "";
                Banner.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Banner.imageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Banner", Banner.image);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Banner.imageUrl.CopyTo(stream);
                }
                Banner.image = "/images/Banner/" + Banner.image;
            }

           
            #endregion

            return AddBanner(Banner);
        }
        public int AddBanner(Banner Banner)
        {
            _context.Banners.Add(Banner);
            _context.SaveChanges();
            return Banner.id;
        }
        public void EditBanner(Banner Banner)
        {
            Banner _Banner = _context.Banners.Find(Banner.id);
            _Banner.link = Banner.link;
           

            if (Banner.imageUrl != null)
            {
               string deletePath = environment.WebRootPath + _Banner.image;
                if (File.Exists(deletePath))
                {
                    File.Delete(deletePath);
                }
                string imagePath = "";
                _Banner.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Banner.imageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Banner", _Banner.image);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Banner.imageUrl.CopyTo(stream);
                }
                _Banner.image = "/images/Banner/" + _Banner.image;

            }
            
            _context.Banners.Update(_Banner);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var Banner =  FindById(id);
                if (Banner.image != null)
                {

                    string deletePath = environment.WebRootPath + Banner.image;

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                   }
                }

                _context.Banners.Remove(Banner);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public List<Banner> GetAllHome()
        {
            return  _context.Banners.OrderByDescending(x=>x.id).Take(20).ToList();
        }
        public Banner GetFirst()
        {
            return _context.Banners.FirstOrDefault();
        }

    }
}
