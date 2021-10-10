using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Gisoo.Controllers
{
    [Authorize]

    public class ArticleController : Controller
    {
        private readonly Context _context;
        private IArticle _Article;
        private readonly IHostingEnvironment environment;

        public ArticleController(Context context,IArticle Article, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _Article = Article;
        }

        // GET: Articles
        public IActionResult Index(int page = 1, string filterLink = "",bool isSuccess=true)
        {
            var model = _Article.GetArticles(page, filterLink);
            if (isSuccess == false)
                ViewData["IsError"]= "خطایی رخ داده است.";
            return View(model);
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Article = _Article.FindById(id);
            if (Article == null)
            {
                return NotFound();
            }

            return View(Article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,image,imageUrl,title,description")] Article Article)
        {
            if (ModelState.IsValid)
            {
              int productId = _Article.AddArticleFromAdmin(Article);

                return RedirectToAction(nameof(Index));
            }
            return View(Article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Article = _Article.FindById(id);
            
            if (Article == null)
            {
                return NotFound();
            }
            return View(Article);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,image,imageUrl,title,description")] Article Article)
        {
            if (id != Article.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 _Article.EditArticle(Article);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(Article.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var Article = _Article.FindById(id);
            if (Article == null)
            {
                return NotFound();
            }

            return View(Article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if(await _Article.Delete(id)==true)
            return RedirectToAction(nameof(Index));
            else
            return RedirectToAction(nameof(Index), new {isSuccess=false });

        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.id == id);
        }
    }
}
