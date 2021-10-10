using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }
         [HttpPost]

        public object Login(LoginUser user)
        {
            var data = _context.Users.Where(p => p.role.RoleNameEn == "Member").FirstOrDefault(p => p.cellphone == user.cellphone);
            if (data == null)
            {
                return new { status = 1, title = "خطای ورود", message = "کاربری با این شماره موبایل ثبت نشده است." };
            }
            //if (!BCrypt.Net.BCrypt.Verify(user.password, data.password))
            //{
            //  return new { status = 2, title = "خطای ورود", message = "پسورد یا موبایل نامعتبر است." };
            //}
            return new { status = 0, title = "ورود", message = "خوش آمدید.", token = data.token };

        }
    }
}