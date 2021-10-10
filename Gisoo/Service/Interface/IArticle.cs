using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface IArticle
    {
       object GetArticles();
       PagedList<Article> GetArticles(int pageId = 1, string filterLink = "");
       int AddArticleFromAdmin(Article Article);
       int AddArticle(Article Article);
       void EditArticle(Article Article);
       Article FindById(int id);
       Task<bool> Delete(int id);
       List<Article> GetArticlesFirstPage();
       PagedList<Article> GetArticles(int pageId = 1);
        Article GetByIdForDetail(int Id);
        List<Article> GetRelated(int id);
    }
}
