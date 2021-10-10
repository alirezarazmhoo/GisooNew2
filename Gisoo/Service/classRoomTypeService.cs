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
    public class classRoomTypeService : IClassRoomType
    {
        private Context _context;
        private readonly IHostingEnvironment environment;

        public classRoomTypeService(Context context, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        public List<ClassRoomType> GetClassRoomTypes()
        {
            IQueryable<ClassRoomType> result = _context.ClassRoomTypes;
            List<ClassRoomType> res = result.OrderByDescending(u => u.id).ToList();
            return   res ;
        }
         public ClassRoomType FindById(int id)
        {
            return   _context.ClassRoomTypes.FirstOrDefault(m => m.id == id);
        }
        public PagedList<ClassRoomType> GetClassRoomTypes(int page = 1, string filterName = "")
        {
            IQueryable<ClassRoomType> result = _context.ClassRoomTypes.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(u => u.name.Contains(filterName));
            }
            PagedList<ClassRoomType> res = new PagedList<ClassRoomType>(result, page, 20);
            return res;
        }

        public int AddClassRoomTypeFromAdmin(ClassRoomType ClassRoomType)
        {
            return AddClassRoomType(ClassRoomType);
        }
        public int AddClassRoomType(ClassRoomType ClassRoomType)
        {
            _context.ClassRoomTypes.Add(ClassRoomType);
            _context.SaveChanges();
            return ClassRoomType.id;
        }
        public void EditClassRoomType(ClassRoomType ClassRoomType)
        {
            ClassRoomType _ClassRoomType = _context.ClassRoomTypes.Find(ClassRoomType.id);
            _ClassRoomType.name = ClassRoomType.name;
            _context.ClassRoomTypes.Update(_ClassRoomType);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var ClassRoomType =  FindById(id);
                _context.ClassRoomTypes.Remove(ClassRoomType);
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
