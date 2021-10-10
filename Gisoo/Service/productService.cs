using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.Utility;
using System.Globalization;

namespace Gisoo.Service
{
    public class productService : IProduct
    {
        private Context _context;
        private readonly IHostingEnvironment environment;

        public productService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        public void CreateOrUpdate(ProductViewModel model, IFormFile[] _File)
        {
            try
            {
                if (model.id == 0)
                {
                    Product Product = new Product();
                    var propInfo = model.GetType().GetProperties();
                    
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id"  || item.Name == "ProductImage" || item.Name=="minDiscount"||item.Name=="maxDiscount")
                            continue;
                        Product.GetType().GetProperty(item.Name).SetValue(Product, item.GetValue(model, null), null);
                    }
                    Product.createDate = DateTime.Now;
                    Product.expireDate = DateTime.Now.AddDays(15);

                    _context.Products.Add(Product);
                    if (_File != null && _File.Count() > 0)
                    {
                        foreach (var item in _File)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Product\File", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                                _context.ProductImages.Add(new ProductImage()
                                {
                                    url = fileName,
                                    productId = Product.id,
                                });
                            }
                        }
                    }
                }
                else
                {
                    Product Product =  _context.Products.Where(s => s.id == model.id).FirstOrDefault();
                    var propInfo = model.GetType().GetProperties();
                     
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "ProductImage" || item.Name == "createDate"|| item.Name=="minDiscount"||item.Name=="maxDiscount")
                            continue;
                        Product.GetType().GetProperty(item.Name).SetValue(Product, item.GetValue(model, null), null);
                    }
                    if (Product != null)
                    {
                        if (_File != null && _File.Count() > 0)
                        {
                            foreach (var item in _File)
                            {
                                var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Product\File", fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                    _context.ProductImages.Add(new ProductImage()
                                    {
                                        url = fileName,
                                        productId = Product.id,

                                    });
                                }
                            }
                        }

                        _context.Products.Update(Product);

                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            
            }
        }
        public Product GetById(int Id)
        {
            var Items = _context.Products.Include(s => s.ProductImages).Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public void Remove(Product model)
        {
            var MainItem = _context.Products.FirstOrDefault(s => s.id == model.id);

            if (MainItem.ProductImages.Count > 0)
            {
                foreach (var item in MainItem.ProductImages)
                {
                    File.Delete($"wwwroot/Product/File/{item.url}");
                }
                _context.RemoveRange(MainItem.ProductImages);
            }
            
            _context.Products.Remove(MainItem);
            _context.SaveChanges();
        }
        public void RemoveFile(int Id)
        {
            var Item = _context.ProductImages.FirstOrDefault(s=>s.id == Id);
            if (Item != null)
            {
                File.Delete($"wwwroot/Product/File/{Item.url}");
            }

            _context.ProductImages.Remove(Item);
            _context.SaveChanges();
        }
        public List<Product> GetAll(int userId, SearchProductViewModel searchProductViewModel)
        {
            IQueryable<Product> result = _context.Products.Include(s => s.ProductImages).Where(s => s.userId == userId).AsQueryable();
            
            
            if (!String.IsNullOrEmpty(searchProductViewModel.title))
                result = result.Where(x => x.title.Contains(searchProductViewModel.title));
            if (!String.IsNullOrEmpty(searchProductViewModel.createDate))
            {
                result = result.Where(x => x.createDate.Date== DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchProductViewModel.createDate)));
            }
            List<Product> Products=result.OrderByDescending(x=>x.id).ToList();
            return Products;
        }
        public List<Product> GetAllHome()
        {
             Random rand = new Random();
            var result=_context.Products.Where(x =>  x.adminConfirmStatus==EnumStatus.Accept).Include(x=>x.ProductImages).Include(x=>x.user).ToList();
            List<int> getAllId = new List<int>();
            List<Product> HelperList = new List<Product>();
            int reapet = result.Count() > 20 ? 20 : result.Count();
            List<int> listNumbers = new List<int>();
            int number;
            for (int i = 0; i < reapet; i++)
            {
                do
                {
                    number = rand.Next(0, result.Count());
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                if(result.ElementAt(number) !=null)
                HelperList.Add(result.ElementAt(number));
            }
            return HelperList.Take(20).ToList();
        }
         public Product GetByIdForDetail(int Id, string ip)
        {
            var Items = _context.Products.Where(s => s.id == Id).Include(x=>x.user).Include(x=>x.ProductImages).FirstOrDefault();
           if(!_context.Visits.Any(x => x.anyNoticeId == Id && x.whichTableEnum == WhichTableEnum.Product && x.Ip==ip))
            
            {
                Visit v = new Visit();
                v.date = DateTime.Now.Date;
                v.Ip = ip;
                v.whichTableEnum = WhichTableEnum.Product;
                v.anyNoticeId = Id;
                _context.Visits.Add(v);
                if (Items.countView == null)
                {
                    Items.countView = 1;
                }
                else
                    Items.countView = Items.countView + 1;
            }
            _context.SaveChanges();
            return Items;
        }
        public List<Product>  GetRelated(int id)
        {

            var result = _context.Products.Where(x=> x.id!=id).OrderByDescending(x=>x.id).Take(20).Include(x=>x.user).Include(x=>x.ProductImages).ToList();
            return result;
        }
         public PagedList<Product> GetProducts(int pageId = 1)
        {
            IQueryable<Product> result = _context.Products.Include(x=>x.ProductImages).OrderByDescending(x => x.createDate);
            PagedList<Product> res = new PagedList<Product>(result, pageId, 10);
            return res;
        }
        public List<Product> CheapestProducts()
        {
            List<Product> result = _context.Products.Include(x=>x.ProductImages).OrderBy(x => x.price).Take(3).ToList();
            return result;
        }
        public List<Product> GetAllRegisterByUser(int userId)
        {
            List<Product> result = _context.Products.Where(x=>x.userId==userId).Include(x=>x.ProductImages).Include(x=>x.user).ToList();
            return result;
        }
         public PagedList<Product> GetProducts(int page = 1, string filterTitle = "")
        {
            IQueryable<Product> result = _context.Products.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            PagedList<Product> res = new PagedList<Product>(result, page, 20);
            return res;
        }
        public void CreateOrUpdateFromAdmin(ProductViewModelAdmin ProductViewModelAdmin, IFormFile[] file)
        {
            if (ProductViewModelAdmin.Product.id != 0)
            {
                var Product = _context.Products.Find(ProductViewModelAdmin.Product.id);
                var propInfo = ProductViewModelAdmin.Product.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.Name == "id" || item.Name == "userId")
                        continue;
                    Product.GetType().GetProperty(item.Name).SetValue(Product, item.GetValue(ProductViewModelAdmin.Product, null), null);
                }
                if (ProductViewModelAdmin.expireDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(ProductViewModelAdmin.expireDate1.Substring(0, 4));
                    int month = Convert.ToInt32(ProductViewModelAdmin.expireDate1.Substring(5, 2));
                    int day = Convert.ToInt32(ProductViewModelAdmin.expireDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    Product.expireDate = dt;
                }

                if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Product\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.ProductImages.Add(new ProductImage()
                            {
                                url = fileName,
                                productId = Product.id,
                            });
                        }
                    }
                }
            }
            else
            {
                Product Product = new Product();
                var propInfo = ProductViewModelAdmin.Product.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.Name == "id")
                        continue;
                    Product.GetType().GetProperty(item.Name).SetValue(Product, item.GetValue(ProductViewModelAdmin.Product, null), null);
                }
                Product.createDate = DateTime.Now;
                Product.expireDate = DateTime.Now.AddDays(15);
                Product.adminConfirmStatus = EnumStatus.Accept;
                _context.Products.Add(Product);
                 if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Product\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.ProductImages.Add(new ProductImage()
                            {
                                url = fileName,
                                productId = Product.id,
                            });
                        }
                    }
                }
            }
            
        }
        public ProductViewModelAdmin GetForDetailPage(int id)
        {
             var Product =  _context.Products.Include(x => x.user).Include(x=>x.ProductImages).FirstOrDefault(x => x.id == id);
            ProductViewModelAdmin ProductViewModelAdmin = new ProductViewModelAdmin();
            ProductViewModelAdmin.Product = Product;
             List<Visit> visits = _context.Visits.Where(x => x.anyNoticeId == id && x.whichTableEnum == WhichTableEnum.Product).ToList();
            ProductViewModelAdmin.datecount1 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-9)) == 0).Count();
            ProductViewModelAdmin.datecount2 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-8)) == 0).Count();
            ProductViewModelAdmin.datecount3 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-7)) == 0).Count();
            ProductViewModelAdmin.datecount4 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-6)) == 0).Count();
            ProductViewModelAdmin.datecount5 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-5)) == 0).Count();
            ProductViewModelAdmin.datecount6 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-4)) == 0).Count();
            ProductViewModelAdmin.datecount7 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-3)) == 0).Count();
            ProductViewModelAdmin.datecount8 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-2)) == 0).Count();
            ProductViewModelAdmin.datecount9 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) == 0).Count();
            ProductViewModelAdmin.datecount10 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date) == 0).Count();
            return ProductViewModelAdmin;
        }
        public int CountRegisterProduct(int userId)
        {
          int count=  _context.Products.Where(x=>x.userId==userId).Count();
            return count;
        }
        
    }
}

