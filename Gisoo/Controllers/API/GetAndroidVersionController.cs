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
    public class GetAndroidVersionController : ControllerBase
    {
        private readonly Context _context;

        public GetAndroidVersionController(Context context)
        {
            _context = context;

        }

        [HttpGet]
        public object GetAndroidVersion()
        {
           var AndroidVersion = new AndroidVersion();
            AndroidVersion = _context.AndroidVersions.FirstOrDefault();
            return new
            {
                data = AndroidVersion,
                Status = 1
            };
        }
    }
}