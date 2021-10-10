using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorController : ControllerBase
    {
        private readonly Context _context;

        public FactorController(Context context)
        {
            _context = context;
        }

        // [HttpPost]
        //public object Factor(BuyProduct buyProduct)
        //{
        //    string Token = HttpContext.Request?.Headers["Token"];
        //    buyProduct.token = Token;
        //    var result = _product.AddProductToFactor(buyProduct);
        //    return result;
        //}
        //[HttpGet("{page}")]
        //public object Factor([FromRoute] int page)
        //{
        //    string Token = HttpContext.Request?.Headers["Token"];
        //    GetFactor getFactor = new GetFactor();
        //    getFactor.page = page;
        //    getFactor.token = Token;
        //    var result = _product.GetFactorOfUser(getFactor);
        //    return result;
        //}
        


    }
}