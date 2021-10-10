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
    public class AboutUsController : ControllerBase
    {
        private readonly Context _context;

        public AboutUsController(Context context)
        {
            _context = context;

        }

        // GET: api/Information
        [HttpGet]

        public object GetAboutUss()
        {
            var data = new AboutUs();
            if( _context.AboutUss.Any())
              data=  _context.AboutUss.FirstOrDefault();
            return data;
        }
    }
}