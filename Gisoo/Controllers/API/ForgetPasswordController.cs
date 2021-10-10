using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Utility;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly Context _context;

        public ForgetPasswordController(Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Users/5
        [HttpPost]
        public object GetUser(LoginUser user1)
        {
            
           // string cellphone = HttpContext.Request?.Headers["cellphone"];
            if (!_context.Users.Any(p => p.cellphone == user1.cellphone))
            {
                return new { status = 1, title = "خطای دریافت رمز عبور", message = "این شماره قبلا ثبت نشده است لطفا اول ثبت نام کنید." };

            }
            else
            {
                var user = _context.Users.Where(p => p.cellphone == user1.cellphone).FirstOrDefault();
                //var msg = "به اصفهان من خوش آمدید." + "\n" + "نام کاربری:" + user.userNamee + "\n" + "کلمه عبور:" + user.passwordShow;
                //var client = new RestClient("http://37.130.202.188/class/sms/webservice/send_url.php?from=100020400&to="+user.mobile+"&msg="+msg+"&uname=f09370138848&pass=tmry");
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("Content-Type", "application/json");
                //IRestResponse response = client.Execute(request);
                return new { status = 0, title = "دریافت رمز عبور", message = "رمز عبور شما از طریق پیامک ارسال شد." };
            }
            
        }

      
    }
}