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
    public class ruleService : IRule
    {
        private Context _context;
        public ruleService(Context context)
        {
            _context = context;
        }
        
        public Rule Get()
        {
            Rule result = _context.Rules.FirstOrDefault();
            return result;
        }
    }
}
