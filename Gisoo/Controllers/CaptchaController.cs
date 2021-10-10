using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gisoo.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gisoo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gisoo.Controllers
{
    [Produces("application/json")]
    [Route("api/captcha")]
    public class CaptchaController : Controller
    {
        //private readonly CaptchaHelper _captchaHelper;

        //public CaptchaController(CaptchaHelper captchaHelper)
        //{
        //    this._captchaHelper = captchaHelper;
        //}

        [HttpGet]

        public IActionResult GetCaptchaActionResult(string captchaName)
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            var newCaptcha = captchaHelper.CreateNewCaptcha(5);
            var newCaptchaImage = captchaHelper.CreateCaptchaImage(newCaptcha);

            //Return the captcha cookie
            this.SetCaptcha(newCaptcha, captchaName);

            return this.Ok(newCaptchaImage);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteCaptchaActionResult(string captchaName)
        {
            this.DeleteCaptcha(captchaName);

            return this.Ok();
        }

        private void SetCaptcha(Captcha captcha, string captchaName)
        {
            //I used Newtonsoft JSON.net to serialize
            this.Response.Cookies.Append(captchaName, JsonConvert.SerializeObject(captcha, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        private void DeleteCaptcha(string captchaName)
        {
            this.Response.Cookies.Delete(captchaName);
        }
    }
}