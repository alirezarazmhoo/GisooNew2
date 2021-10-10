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
    public class LadderNoticeController : ControllerBase
    {
        private readonly Context _context;

        public LadderNoticeController(Context context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public object LadderNotice([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var Notice = _context.Notices.Find(id);
                if (Notice == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };

                // Notice.createDate = DateTime.Now;
                var user = _context.Users.Find(Notice.userId);
                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (Notice.isBarber && allprice.isHasBarberPrice)
                    totalprice = allprice.barberPrice;
                if (!Notice.isBarber && allprice.isHasSaloonPrice)
                    totalprice = allprice.saloonPrice;
                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.noticeId = id;
                factor.factorKind = FactorKind.Ladder;
                factor.totalPrice = totalprice;
                _context.Factors.Add(factor);
                if (totalprice == 0)
                {
                    Notice.createDate = DateTime.Now;
                    _context.SaveChanges();
                    trans.Commit();
                    return new { status = 0, message = "نردبان آگهی با موفقیت انجام شد.", url = "" };
                }
                else
                {
                    _context.SaveChanges();
                    var payment = new Payment("e67a20c2-f81e-11e9-913d-000c295eb8fc", (int)totalprice);
                    var res = payment.PaymentRequest($"پرداخت {Notice.id} نردبان", "http://gisoooo.ir/api/ConfirmLadderNotices/" + Notice.id + "/" + factor.id, "", "");
                    if (res.Result.Status == 100)
                    {
                        trans.Commit();
                        return new { status = 0, message = "نردبان آگهی با موفقیت انجام شد.", url = "https://www.zarinpal.com/pg/StartPay/" + res.Result.Authority };
                    }
                    else
                    {
                        trans.Rollback();
                        return new { status = 5, message = "خطا در نردبان آگهی." };
                    }
                }
            }

        }

    }
}