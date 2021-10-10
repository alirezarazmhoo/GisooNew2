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
    public class articleService : IArticle
    {
        private Context _context;
        private readonly IHostingEnvironment environment;

        public articleService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        public object GetArticles()
        {
            IQueryable<Article> result = _context.Articles;
            List<Article> res = result.OrderByDescending(u => u.id).ToList();
            return new { data = res };
        }
        public List<Article> GetArticlesFirstPage()
        {
            return _context.Articles.OrderByDescending(x => x.id).Take(3).ToList();

        }
        public Article FindById(int id)
        {
            return _context.Articles.FirstOrDefault(m => m.id == id);
        }
        public PagedList<Article> GetArticles(int page = 1, string filterTitle = "")
        {
            IQueryable<Article> result = _context.Articles.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            PagedList<Article> res = new PagedList<Article>(result, page, 20);
            return res;
        }
        public PagedList<Article> GetArticles(int pageId = 1)
        {
            IQueryable<Article> result = _context.Articles.OrderByDescending(x => x.id);
            PagedList<Article> res = new PagedList<Article>(result, pageId, 10);
            return res;
        }
        public int AddArticleFromAdmin(Article Article)
        {
            #region Save Image
            if (Article.imageUrl != null)
            {
                string imagePath = "";
                Article.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Article.imageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Article", Article.image);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Article.imageUrl.CopyTo(stream);
                }
                Article.image = "/images/Article/" + Article.image;
            }


            #endregion

            return AddArticle(Article);
        }
        public int AddArticle(Article Article)
        {
            _context.Articles.Add(Article);
            _context.SaveChanges();
            return Article.id;
        }
        public void EditArticle(Article Article)
        {
            Article _Article = _context.Articles.Find(Article.id);
            _Article.title = Article.title;
            _Article.description = Article.description;


            if (Article.imageUrl != null)
            {
                string deletePath = environment.WebRootPath + _Article.image;
                if (File.Exists(deletePath))
                {
                    File.Delete(deletePath);
                }
                string imagePath = "";
                _Article.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Article.imageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Article", _Article.image);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Article.imageUrl.CopyTo(stream);
                }
                _Article.image = "/images/Article/" + _Article.image;

            }

            _context.Articles.Update(_Article);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var Article = FindById(id);
                if (Article.image != null)
                {

                    string deletePath = environment.WebRootPath + Article.image;

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }

                _context.Articles.Remove(Article);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public Article GetByIdForDetail(int Id)
        {
            var Items = _context.Articles.Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public List<Article> GetRelated(int id)
        {
            var result = _context.Articles.Where(x => x.id != id).OrderByDescending(x => x.id).Take(20).ToList();
            return result;
        }
    }
}
