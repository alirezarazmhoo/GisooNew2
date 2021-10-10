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
    public class ConfirmReserveClassRoomFromWebSiteController : ControllerBase
    {
        private readonly Context _context;
        private readonly IClassRoom _ClassRoom;
        
        private readonly IHostingEnvironment environment;

        public ConfirmReserveClassRoomFromWebSiteController(Context context,IClassRoom ClassRoom, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _ClassRoom = ClassRoom;
        }

        [HttpGet("{classRoomId}/{userId}")]
        public object Get(int? classRoomId, int? userId)
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
                    var classRoomItem = _context.ClassRooms.Include(s=>s.user).FirstOrDefault(s => s.id == classRoomId.Value);
                    var userItem = _context.Users.FirstOrDefault(s => s.id == userId.Value);
                     _ClassRoom.Reserve(classRoomItem, userItem);

                    SendSms.CallSmSMethod(Convert.ToInt64(userItem.cellphone), 51229, "UserName", userItem.fullname);
                    SendSms.CallSmSMethod(Convert.ToInt64(classRoomItem.user.cellphone), 51230, "UserName", userItem.fullname);
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