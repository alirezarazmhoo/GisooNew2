using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsIrRestfulNetCore;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly Context _context;

        public RegisterController(Context context)
        {
            _context = context;
        }
        [HttpPost]

        public object Register(RegisterUser u)
        {

            SmsIrRestfulNetCore.Token tokenInstance = new SmsIrRestfulNetCore.Token();
            var token = tokenInstance.GetToken("82f4b59d36b1b5178b76b59d", "&Gisoo!!");
            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(u.cellphone),
                TemplateId = 17464,
                ParameterArray = new List<UltraFastParameters>()
                      {
                    new UltraFastParameters()
                          {
                          Parameter = "VerificationCode" , ParameterValue = u.code

                       }
                     }.ToArray()

            };
            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful==false)
            {
                return new { status = 1, title = "خطای ارسال پیامک", message = "پیامک ارسال نشد." };

            }
            return new { status = 0,title = "ارسال پیامک",  message = "کد با موفقیت ارسال شد." };
        }

       
    }
}