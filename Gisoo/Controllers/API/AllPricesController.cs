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

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllPricesController : ControllerBase
    {
        private readonly Context _context;

        private IAllPrice _AllPrice;

        public AllPricesController(Context context,IAllPrice AllPrice)
        {
            _context = context;
            _AllPrice = AllPrice;

        }

        // GET: api/AllPrices
        [HttpGet]
        public object GetAllPrices()
        {
            var data = _AllPrice.GetAllPrices();
            return data;
        }

        // GET: api/AllPrices/5
        
    }
}