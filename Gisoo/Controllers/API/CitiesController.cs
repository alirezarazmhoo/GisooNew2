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
    public class CitiesController : ControllerBase
    {
        private readonly Context _context;

        private ICity _City;

        public CitiesController(Context context,ICity City)
        {
            _context = context;
            _City = City;

        }

        // GET: api/AllPrices
        [HttpGet]
        public object GetCities()
        {
            var data = _City.GetCities();
            return data;
        }

        // GET: api/AllPrices/5
        
    }
}