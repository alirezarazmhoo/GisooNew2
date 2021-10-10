using Gisoo.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service.Interface
{
   public interface ILineType
    {
       List<LineType> GetLineTypes();
       PagedList<LineType> GetLineTypes(int pageId = 1, string filterLink = "");
       int AddLineTypeFromAdmin(LineType LineType);
       int AddLineType(LineType LineType);
       void EditLineType(LineType LineType);
       LineType FindById(int id);
       Task<bool> Delete(int id);
    }
}
