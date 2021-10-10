using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface IClassRoomType
    {
       List<ClassRoomType> GetClassRoomTypes();
       PagedList<ClassRoomType> GetClassRoomTypes(int pageId = 1, string filterLink = "");
       int AddClassRoomTypeFromAdmin(ClassRoomType LineType);
       int AddClassRoomType(ClassRoomType LineType);
       void EditClassRoomType(ClassRoomType LineType);
       ClassRoomType FindById(int id);
       Task<bool> Delete(int id);
    }
}
