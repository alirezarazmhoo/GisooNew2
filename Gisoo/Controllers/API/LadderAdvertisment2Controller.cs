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
    public class LadderAdvertisment2Controller : ControllerBase
    {
        private readonly Context _context;

        public LadderAdvertisment2Controller(Context context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public object LadderAdvertisment2([FromRoute]int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                var Advertisment = _context.Advertisments.Find(id);
                if (Advertisment == null)
                    return new { status = 1, message = "چنین درخواستی یافت نشد." };
                Advertisment.createDate = DateTime.Now;
                var user = _context.Users.Find(Advertisment.userId);
                var allprice = _context.AllPrices.FirstOrDefault();
                long totalprice = 0;
                if (Advertisment.isWorkshop && allprice.isHasWorkshopPrice)
                    totalprice = allprice.workshopPrice;
                if (!Advertisment.isWorkshop && allprice.isHasAdvertismentPrice)
                    totalprice = allprice.advertismentPrice;
                Factor factor = new Factor();
                factor.state = State.IsPay;
                factor.userId = user.id;
                factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;
                factor.advertismentId = id;
                factor.factorKind = FactorKind.Ladder;
                factor.totalPrice = totalprice;
                _context.Factors.Add(factor);
                _context.SaveChanges();
                    trans.Commit();
                    return new { status = 0, message = "نردبان تبلیغات با موفقیت انجام شد.", url = "" };
            }
        }

    }
}