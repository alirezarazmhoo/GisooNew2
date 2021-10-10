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
    public class contactUsService : IContactUs
    {
        private Context _context;
        public contactUsService(Context context)
        {
            _context = context;
        }
        
        public ContactUs Get()
        {
            ContactUs result = _context.ContactUss.FirstOrDefault();
            return result;
        }
    }
}
