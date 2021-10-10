using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class lineTypeService : ILineType
    {
        private Context _context;
        private readonly IHostingEnvironment environment;

        public lineTypeService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        public List<LineType> GetLineTypes()
        {
            IQueryable<LineType> result = _context.LineTypes;
            List<LineType> res = result.OrderByDescending(u => u.id).ToList();
            return   res ;
        }
         public LineType FindById(int id)
        {
            return   _context.LineTypes.FirstOrDefault(m => m.id == id);
        }
        public PagedList<LineType> GetLineTypes(int page = 1, string filterName = "")
        {
            IQueryable<LineType> result = _context.LineTypes.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(u => u.name.Contains(filterName));
            }
            PagedList<LineType> res = new PagedList<LineType>(result, page, 20);
            return res;
        }

        public int AddLineTypeFromAdmin(LineType LineType)
        {
            return AddLineType(LineType);
        }
        public int AddLineType(LineType LineType)
        {
            _context.LineTypes.Add(LineType);
            _context.SaveChanges();
            return LineType.id;
        }
        public void EditLineType(LineType LineType)
        {
            LineType _LineType = _context.LineTypes.Find(LineType.id);
            _LineType.name = LineType.name;
            _context.LineTypes.Update(_LineType);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var LineType =  FindById(id);
                _context.LineTypes.Remove(LineType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

    }
}
