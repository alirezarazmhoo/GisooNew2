using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Newtonsoft.Json.Linq;
using Gisoo.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Zarinpal;

namespace Gisoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmReserveLineFromWebSiteController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILineWeekDate _LineWeekDate;
        private readonly ILine _ILine;
        private readonly IUser _Iuser;
        
        private readonly IHostingEnvironment environment;

        public ConfirmReserveLineFromWebSiteController(IUser user, ILine line,Context context,ILineWeekDate LineWeekDate, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _LineWeekDate = LineWeekDate;
            _Iuser = user;
            _ILine = line;

        }

        [HttpGet("{allIds}/{lineId}/{userId}")]
        public object Get(string allIds, int lineId, int userId)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                 HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", 1000);
                var res = payment.Verification(authority).Result;
                if (res.Status != 100)
                {
                    return Redirect("http://gisoooo.ir/CustomerHome/Index?isRegisterSuccess=false&PaySuccess=%D9%BE%D8%B1%D8%AF%D8%A7%D8%AE%D8%AA-%D8%A8%D8%A7-%D8%B4%DA%A9%D8%B3%D8%AA-%D9%85%D9%88%D8%A7%D8%AC%D9%87-%D8%B4%D8%AF");
                }
                else
                {
                    var LineItem = _context.Lines.Include(s=>s.user).FirstOrDefault(s => s.id == lineId);
                    var UserItem = _context.Users.FirstOrDefault(s => s.id == userId);

                    _LineWeekDate.ReserveLine(allIds,LineItem,UserItem);

                    SendSms.CallSmSMethod(Convert.ToInt64(UserItem.cellphone), 51229, "UserName", UserItem.fullname);
                    SendSms.CallSmSMethod(Convert.ToInt64(LineItem.user.cellphone), 51231, "UserName", UserItem.fullname);

                    return Redirect("http://gisoooo.ir/CustomerHome/Index?isRegisterSuccess=false&PaySuccess=%D9%BE%D8%B1%D8%AF%D8%A7%D8%AE%D8%AA%20%D8%A8%D8%A7%20%D9%85%D9%88%D9%81%D9%82%DB%8C%D8%AA%20%D8%A7%D9%86%D8%AC%D8%A7%D9%85%20%D8%B4%D8%AF");
                }
            }
            else
            {
                return Redirect("http://gisoooo.ir/CustomerHome/Index?isRegisterSuccess=false&PaySuccess=%D9%BE%D8%B1%D8%AF%D8%A7%D8%AE%D8%AA-%D8%A8%D8%A7-%D8%B4%DA%A9%D8%B3%D8%AA-%D9%85%D9%88%D8%A7%D8%AC%D9%87-%D8%B4%D8%AF");

            }
        }


    }
}