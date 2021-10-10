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
using Gisoo.Models.Enums;
using Gisoo.ViewModel;

namespace Gisoo.Controllers
{
    public class UserController : Controller
    {
        private readonly Context _context;
        private IUser _user;
        //private readonly UserManager<User> _userManager;
        private IHostingEnvironment _hostingEnvironment;

        public UserController(Context context, IUser user, IHostingEnvironment hostingEnvironment)
        //,UserManager<User> userManager)
        {
            //_userManager = userManager;
            _context = context;
            _user = user;
            _hostingEnvironment = hostingEnvironment;
        }
        //       [HttpGet]
        //public async Task<int> GetCurrentUserId()
        //{
        //	User usr = await GetCurrentUserAsync();
        //	return usr.id;
        //}

        //   private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public IActionResult Index(string rolenameEn, int page = 1,bool isProfileComplete=false, string filtercellphone = "")
        {
            var model = _user.GetUsers(rolenameEn, page,isProfileComplete, filtercellphone);
            ViewBag.rolenameEn = rolenameEn;
            return View(model);
        }
        public async Task<IActionResult> ExportToExcel(string rolenameEn, string filtercellphone = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(x => x.role.RoleNameEn == rolenameEn).OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filtercellphone))
            {
                result = result.Where(x => x.cellphone.Contains(filtercellphone));
            }
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ردیف");
                row.CreateCell(1).SetCellValue("شماره همراه");
                int count = 1;
                foreach (var item in result)
                {
                    row = excelSheet.CreateRow(count);
                    row.CreateCell(0).SetCellValue(count);
                    row.CreateCell(1).SetCellValue(item.cellphone);
                    count++;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);

        }
        //[Route("User/Login")]

        public ActionResult Login()
        {
            return View();
        }
        //[Route("get-captcha-image")]
        //public IActionResult GetCaptchaImage()
        //{
        //    int width = 100;
        //    int height = 36;
        //    var captchaCode = Captcha.GenerateCaptchaCode();
        //    var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
        //    HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
        //    Stream s = new MemoryStream(result.CaptchaByteData);
        //    return new FileStreamResult(s, "image/png");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string captchaAttemptedValue, LoginUserAdmin user)
        {
            //const string captchaName = "myCaptchaName";

            //var captchaJson = this.HttpContext.Request.Cookies[captchaName];

            //if (string.IsNullOrEmpty(captchaJson))
            //{
            //    return this.BadRequest(new { error = "noCaptcha" });
            //}
            //CaptchaHelper captchaHelper = new CaptchaHelper();
            //var validatedCaptcha = captchaHelper.ValidateCaptcha(JsonConvert.DeserializeObject<Captcha>(captchaJson), captchaAttemptedValue);

            //if (validatedCaptcha.AttemptFailed)
            //{
            //    this.SetCaptcha(validatedCaptcha, captchaName);

            //    ModelState.Clear();
            //    ModelState.AddModelError("", "کپچا اشتباه است.");
            //    return View("Login");
            //}


            //this.DeleteCaptcha(captchaName);
            if (user.password == "" || user.password == null || user.cellphone == "" || user.cellphone == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "شماره همراه یا رمز عبور صحیح نیست");
                return View("Login");
            }
            var u = _context.Users.Include(x => x.role).Where(p => p.cellphone == user.cellphone && p.role.RoleNameEn == "Admin").FirstOrDefault();
            if (u == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "شماره همراه یا رمز عبور صحیح نیست");
                return View("Login");
            }
            if (!BCrypt.Net.BCrypt.Verify(user.password, u.password))
            {
                ModelState.Clear();
                ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
                return View("Login");
            }
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.cellphone.ToString()),
                        new Claim(ClaimTypes.Name,user.cellphone)
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = user.RememberMe
            };
            HttpContext.SignInAsync(principal, properties);

            return Redirect("/Home/Index");



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
        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/User/Login");
        }

        #endregion

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.Include(x => x.UserDocumentImages).FirstOrDefaultAsync(x => x.id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult AdminUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUser(UserAdmin userAdmin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.Include(x => x.role).FirstOrDefault(x => x.role.RoleNameEn == "Admin" && x.passwordShow == userAdmin.passwordShow);
                    if (user == null)
                    {
                        ModelState.AddModelError("passwordShow", "رمز عبور فعلی نا معتبر است");
                        return View();
                    }
                    else
                    {
                        user.passwordShow = userAdmin.newPasswordShow;
                        user.password = BCrypt.Net.BCrypt.HashPassword(userAdmin.newPasswordShow, BCrypt.Net.BCrypt.GenerateSalt());
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                ViewData["passwordChanged"] = "رمز عبور تغییر یافت";
                return View();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Confirm(User User)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.id == User.id);
                //if (User.userStatus == true && user.userStatus == false)
                //{
                //    var allPrice = _context.AllPrices.FirstOrDefault();
                //    if (allPrice.scoreWithInturducer != null && user.isscored == false)
                //    {
                //        user.score = user.score + Convert.ToInt64(allPrice.scoreWithInturducer);
                //        var userInturducer = _context.Users.FirstOrDefault(x => x.identifiecodeOwner == user.identifiecode);
                //        userInturducer.score = userInturducer.score + Convert.ToInt64(allPrice.scoreWithInturducer);
                //        user.isscored = true;
                //    }
                //    try
                //    {
                //        SendSms.CallSmSMethod(Convert.ToInt64(user.cellphone), 41382, "phoneNumber", user.cellphone);
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json("Fail");
                       
                //    }
                //}
                if(User.isProfileAccept)
                {
                    try
                    {
                        SendSms.CallSmSMethod(Convert.ToInt64(user.cellphone), 41382, "phoneNumber", user.cellphone);
                    }
                    catch (Exception ex)
                    {
                        return Json("Fail");

                    }
                }
                else
                {
                    try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "phoneNumber", ParameterValue = user.cellphone });
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue =User.notConfirmDes });
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(user.cellphone), 42320, Parameter);
                    }
                    catch (Exception ex)
                    {
                        return Json("Fail");

                    }
                }
                user.userStatus = User.userStatus;
                user.isProfileAccept = User.isProfileAccept;
                user.notConfirmDes = User.notConfirmDes;
                _context.SaveChanges();
                return Json("Done");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.id == id);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> GetUser(int id)
        {
            var User = await _context.Users.FindAsync(id);
            return Json(User);
        }
    }
}