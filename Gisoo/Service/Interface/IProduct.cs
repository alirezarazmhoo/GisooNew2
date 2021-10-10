using Gisoo.Models;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
    public interface IProduct
    {
        void CreateOrUpdate(ProductViewModel model, IFormFile[] _File);
        Product GetById(int Id);
        void Remove(Product model);
        void RemoveFile(int Id);
        List<Product> GetAll(int userId,SearchProductViewModel searchProductViewModel);
        List<Product> GetAllHome();
        Product GetByIdForDetail(int Id, string ip);
        List<Product> GetRelated(int id);
        PagedList<Product> GetProducts(int pageId = 1);
        List<Product> CheapestProducts();
        List<Product> GetAllRegisterByUser(int userId);
        PagedList<Product> GetProducts(int page = 1, string filterTitle = "");
        void CreateOrUpdateFromAdmin(ProductViewModelAdmin ProductViewModelAdmin, IFormFile[] file);
        ProductViewModelAdmin GetForDetailPage(int id);
        int CountRegisterProduct(int userId);

    }
}
