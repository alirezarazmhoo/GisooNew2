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
    public class ConfirmAdvertismentsController : ControllerBase
    {
        private readonly Context _context;
        private INotice _notice;
        private readonly IHostingEnvironment environment;

        public ConfirmAdvertismentsController(Context context, INotice notice, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _notice = notice;
        }

        // GET: api/Notices/5
        [HttpGet("{id}/{factorId}")]
        public object GetNotice(int id, int factorId)
        {
            var Advertisment = _context.Advertisments.Include(x=>x.user).FirstOrDefault(x => x.id == id);
            if (HttpContext.Request.Query["Status"] != "" &&
                 HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (Advertisment.isWorkshop && allprice.isHasWorkshopPrice)
                    totalprice = allprice.workshopPrice;
                if (!Advertisment.isWorkshop && allprice.isHasAdvertismentPrice)
                    totalprice = allprice.advertismentPrice;
                var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                var res = payment.Verification(authority).Result;
                if (res.Status != 100)
                {
                    _context.Factors.Remove(_context.Factors.FirstOrDefault(x => x.id == factorId));
                    if (!String.IsNullOrEmpty(Advertisment.image1))
                    {
                        string deletePath = environment.WebRootPath + Advertisment.image1;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    if (!String.IsNullOrEmpty(Advertisment.image2))
                    {
                        string deletePath = environment.WebRootPath + Advertisment.image2;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    if (!String.IsNullOrEmpty(Advertisment.image3))
                    {
                        string deletePath = environment.WebRootPath + Advertisment.image3;
                        if (System.IO.File.Exists(deletePath))
                        {
                            System.IO.File.Delete(deletePath);
                        }
                    }
                    _context.Advertisments.Remove(Advertisment);
                    _context.SaveChanges();
                    return new { status = 1, message = "پرداخت نا موفق." };

                }
                else
                {
                    Advertisment.isDeleted = false;
                    _context.SaveChanges();
                    try
                    {
                        SendSms.CallSmSMethod(Convert.ToInt64(Advertisment.user.cellphone),41384, "AllNotice",Advertisment.title);
                    }
                    catch (Exception ex)
                    {
                        return new { status = 0, message = "پرداخت با موفقیت انجام شد پیامک ارسال نشد." };

                    }
                    return new { status = 0, message = "پرداخت با موفقیت انجام شد." };
                }
            }
            else
            {
                _context.Factors.Remove(_context.Factors.FirstOrDefault(x => x.id == factorId));
                if (!String.IsNullOrEmpty(Advertisment.image1))
                {
                    string deletePath = environment.WebRootPath + Advertisment.image1;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                if (!String.IsNullOrEmpty(Advertisment.image2))
                {
                    string deletePath = environment.WebRootPath + Advertisment.image2;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                if (!String.IsNullOrEmpty(Advertisment.image3))
                {
                    string deletePath = environment.WebRootPath + Advertisment.image3;
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                _context.Advertisments.Remove(Advertisment);
                _context.SaveChanges();
                return new { status = 1, message = "پرداخت نا موفق." };
                

            }
        }


    }
}