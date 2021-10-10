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
    public class FactorItemController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;

        public FactorItemController(Context context,INotice notice)
        {
            _context = context;
            _notice = notice;
        }

        // [HttpPost]
        //public object FactorItem(AllFactorItem allFactorItem)
        //{
        //    var result = _product.GetFactorItems(allFactorItem);
        //    return result;
        //}
       


    }
}