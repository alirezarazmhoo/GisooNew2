using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class aboutUsService : IAboutUs
    {
        private Context _context;
        public aboutUsService(Context context)
        {
            _context = context;
        }
        
        public AboutUs Get()
        {
            AboutUs result = _context.AboutUss.FirstOrDefault();
            return result;
        }
    }
}
