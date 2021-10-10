using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
         private readonly Context _context;

        public ContactUsController(Context context)
        {
            _context = context;

        }
        [HttpGet]

        public object GetContactUss()
        {
            var data = new ContactUs();
            if( _context.ContactUss.Any())
              data=  _context.ContactUss.FirstOrDefault();
            return data;
        }
    }
}