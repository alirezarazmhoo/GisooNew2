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
    public class roleService : IRole
    {
        private Context _context;
        public roleService(Context context)
        {
            _context = context;
        }
        
        public List<Role> Get()
        {
            List<Role> result = _context.Roles.ToList();
            return result;
        }
    }
}
