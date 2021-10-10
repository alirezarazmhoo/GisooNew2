using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Models.Enums;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class noticeService : INotice
    {
        private Context _context;
        public noticeService(Context context)
        {
            _context = context;
        }

        //public int AddNoticeFromAdmin(CreateNoticeViewModel Notice)
        //{
        //    Notice addNotice = new Notice();
        //    addNotice.description = Notice.description;
        //    addNotice.title = Notice.title;

        //    #region Save Image

        //    if (Notice.image != null)
        //    {
        //        string imagePath = "";
        //        addNotice.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Notice.image.FileName);
        //        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Notice", addNotice.image);
        //        using (var stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            Notice.image.CopyTo(stream);
        //        }
        //        addNotice.image = "/images/Notice/" + addNotice.image;
        //    }

        //    #endregion

        //    return AddNotice(addNotice);
        //}
        public int AddNotice(Notice Notice)
        {
            _context.Notices.Add(Notice);
            _context.SaveChanges();
            return Notice.id;
        }
        //public void EditNotice(CreateNoticeViewModel Notice)
        //{
        //    Notice _Notice = _context.Notices.Find(Notice.id);
        //    _Notice.title = Notice.title;
        //    _Notice.description = Notice.description;
        //    if (Notice.image != null)
        //    {
        //        if (_Notice.image != null)
        //        {
        //            string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Notice/", _Notice.image);
        //            if (File.Exists(deletePath))
        //            {
        //                File.Delete(deletePath);
        //            }
        //        }


        //        string imagePath = "";
        //        _Notice.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Notice.image.FileName);
        //        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Notice", _Notice.image);
        //        using (var stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            Notice.image.CopyTo(stream);
        //        }
        //        _Notice.image = "/images/Notice/" + _Notice.image;

        //    }

        //    _context.Notices.Update(_Notice);
        //    _context.SaveChanges();
        //}
        public object GetNotices(int page = 1)
        {
            IQueryable<Notice> result = _context.Notices;
            int skip = (page - 1) * 10;
            List<Notice> res = result.ToList();
            return new { data = res.OrderByDescending(u => u.id).Skip(skip).Take(10), totalCount = res.Count() };
        }
        public object GetNotices(int page = 1, int pagesize = 10)
        {
            IQueryable<Notice> result = _context.Notices.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept);
            int skip = (page - 1) * pagesize;
            var res = result.OrderByDescending(u => u.createDate).Skip(skip).Take(pagesize).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }

        public PagedList<Notice> GetNotices(ConditionEnum? filtercondition, bool? filterisBarber, int page = 1, string filterTitle = "")
        {
            IQueryable<Notice> result = _context.Notices.Where(x => x.isDeleted == false).OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            if (filtercondition != null)
            {
                result = result.Where(u => u.condition == filtercondition);
            }
            if (filterisBarber != null)
            {
                result = result.Where(u => u.isBarber == filterisBarber);
            }

            PagedList<Notice> res = new PagedList<Notice>(result, page, 20);
            return res;
        }

        public object GetLastNotices(int page = 1)
        {
            IQueryable<Notice> result = _context.Notices;
            int skip = (page - 1) * 10;
            var res = result.OrderByDescending(u => u.id).Skip(skip).Take(10).Select(x => new { x.id, x.image1, x.image2, x.image3, x.title, x.description }).ToList();
            return new { data = res, totalCount = result.Count() };
        }
        public int CreateOrUpdate(Notice2ViewModel model, IFormFile _File1, IFormFile _File2, IFormFile _File3, bool fromAdmin = false)
        {
            try
            {
                string img1 = "";
                string img2 = "";
                string img3 = "";
                if (model.id == 0)
                {
                    Notice notice = new Notice();

                    var propInfo = model.GetType().GetProperties();
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "cities" || item.Name == "provinces" || item.Name == "areas")
                            continue;
                        notice.GetType().GetProperty(item.Name).SetValue(notice, item.GetValue(model, null), null);
                    }
                    notice.createDate = DateTime.Now;
                    notice.expireDate = DateTime.Now.AddDays(15);
                    if (fromAdmin)
                        notice.adminConfirmStatus = EnumStatus.Accept;
                    else
                        notice.adminConfirmStatus = EnumStatus.Pending;
                    if (_context.Notices.Count() == 0)
                        notice.code = "1";
                    else
                        notice.code = (Convert.ToInt32(_context.Notices.LastOrDefault().code) + 1).ToString();
                    if (_File1 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File1.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File1.CopyTo(stream);
                            //notice.image1 = "/Notice/" + fileName;
                            img1 = "/Notice/" + fileName;
                        }

                    }
                    if (_File2 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File2.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File2.CopyTo(stream);
                            //notice.image2 = "/Notice/" + fileName;
                            img2 = "/Notice/" + fileName;
                        }

                    }
                    if (_File3 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File3.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File3.CopyTo(stream);
                            //notice.image3 = "/Notice/" + fileName;
                            img3 = "/Notice/" + fileName;
                        }

                    }
                    notice.image1 = img1;
                    notice.image2 = img2;
                    notice.image3 = img3;
                    _context.Notices.Add(notice);
                    _context.SaveChanges();

                    return notice.id;
                }
                else
                {
                    Notice notice = _context.Notices.Where(s => s.id == model.id).FirstOrDefault();
                    var propInfo = model.GetType().GetProperties();

                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "cities" || item.Name == "provinces" || item.Name == "areas")
                            continue;
                        notice.GetType().GetProperty(item.Name).SetValue(notice, item.GetValue(model, null), null);
                    }
                    if (notice != null)
                    {
                        if (_File1 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File1.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File1.CopyTo(stream);
                                notice.image1 = "/Notice/" + fileName;
                            }

                        }
                        if (_File2 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File2.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File2.CopyTo(stream);
                                notice.image2 = "/Notice/" + fileName;
                            }

                        }
                        if (_File3 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File3.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Notice", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File3.CopyTo(stream);
                                notice.image3 = "/Notice/" + fileName;
                            }

                        }
                        _context.Notices.Update(notice);


                    }
                }
                _context.SaveChanges();
                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public Notice GetById(int Id)
        {
            var Items = _context.Notices.Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public Notice GetByIdForDetail(int Id, string ip)
        {
            var Items = _context.Notices.Where(s => s.id == Id).Include(x => x.user).Include(x => x.city).Include(x => x.province).Include(x => x.area).FirstOrDefault();
            if (!_context.Visits.Any(x => x.anyNoticeId == Id && x.whichTableEnum == WhichTableEnum.Notice && x.Ip == ip))

            {
                Visit v = new Visit();
                v.date = DateTime.Now.Date;
                v.Ip = ip;
                v.whichTableEnum = WhichTableEnum.Notice;
                v.anyNoticeId = Id;
                _context.Visits.Add(v);
                if (Items.countView == null)
                {
                    Items.countView = 1;
                }
                else
                    Items.countView = Items.countView + 1;
            }

            _context.SaveChanges();
            return Items;
        }
        public List<Notice> GetRelated(int id)
        {

            var result = _context.Notices.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.id != id).OrderByDescending(x => x.id).Take(20).Include(x => x.user).ToList();
            return result;
        }
        public List<Notice> GetAll(int userId, SearchNoticeViewModel searchNoticeViewModel)
        {
            IQueryable<Notice> result = _context.Notices.Where(s => s.userId == userId).AsQueryable();
            if (!String.IsNullOrEmpty(searchNoticeViewModel.title))
                result = result.Where(x => x.title.Contains(searchNoticeViewModel.title));
            if (!String.IsNullOrEmpty(searchNoticeViewModel.registerDate))
            {
                result = result.Where(x => x.createDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchNoticeViewModel.registerDate)));
            }
            List<Notice> Notices = result.OrderByDescending(x => x.id).ToList();
            return Notices;
        }
        //public object GetNoticesByCatAndType(NoticeSearch NoticeSearch)
        //{
        //    var result = _context.Notices.AsQueryable();
        //    int skip = (NoticeSearch.page - 1) * 10;

        //    List<int> cats = new List<int>();
        //    cats.Add(Convert.ToInt32(NoticeSearch.category_id));
        //    int catId = 0;


        //    return new { data = result.OrderByDescending(p => p.id).Skip(10 * (NoticeSearch.page - 1)).Take(10).Select(x => new { x.id, x.image, x.title, x.description }).ToList(), totalCount = result.Count() };
        //}

        //public object GetNoticesByTitle(NoticeSearch2 NoticeSearch)
        //{
        //    var result = _context.Notices.AsQueryable();
        //    int skip = (NoticeSearch.page - 1) * 10;
        //    if (NoticeSearch.title != "")
        //        result = result.Where(x => x.title.Contains(NoticeSearch.title));
        //    return new { data = result.OrderByDescending(p => p.id).Skip(10 * (NoticeSearch.page - 1)).Take(10).Select(x => new { x.id, x.image,  x.title, x.description }).ToList(), totalCount = result.Count() };
        //}

        //public object AddNoticeToFactor(BuyNotice buyNotice)
        //{
        //    try
        //    {
        //        string Token = buyNotice.token;
        //        var user = _context.Users.Where(p => p.token == Token).FirstOrDefault();
        //        List<Factor> factors = new List<Factor>();
        //        List<FactorItem> FactorItems = new List<FactorItem>();

        //        //foreach (var item in buyNotice.Notices)
        //        //{
        //        //    var Notice = _context.Notices.FirstOrDefault(x => x.id == item);
        //        //    if (Notice == null)
        //        //    {
        //        //        return new { status = 1, title = "خطای ثبت فاکتور", message = "چنین محصولی یافت نشد" };

        //        //    }
        //        //    else
        //        //    {
        //        //        Factor factor = new Factor();
        //        //        factor.NoticeId = item;
        //        //        factor.isAdminCheck = false;
        //        //        factor.state = State.IsPay;
        //        //        factor.userId = user.id;
        //        //        factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);;
        //        //        factors.Add(factor);
        //        //    }
        //        //}
        //        //_context.Factors.AddRange(factors);
        //        long totalPrice = 0;


        //        Factor factor = new Factor();
        //        //factor.NoticeId = buyNotice.Notices.ToString();

        //        factor.state = State.IsPay;
        //        factor.userId = user.id;
        //        factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now); ;

        //        foreach (var item in buyNotice.Notices)
        //        {
        //            var Notice = _context.Notices.FirstOrDefault(x => x.id == item);
        //            if (Notice == null)
        //            {
        //                return new { status = 1, title = "خطای ثبت فاکتور", message = "چنین محصولی یافت نشد" };
        //            }
        //            //totalPrice += Notice.price;
        //        }
        //        factor.totalPrice = totalPrice;
        //        _context.Factors.Add(factor);
        //        foreach (var item in buyNotice.Notices)
        //        {
        //            var Notice = _context.Notices.FirstOrDefault(x => x.id == item);
        //            if (Notice == null)
        //            {
        //                return new { status = 1, title = "خطای ثبت فاکتور", message = "چنین محصولی یافت نشد" };
        //            }
        //            FactorItem factorItem = new FactorItem();
        //            factorItem.NoticeId = Notice.id;
        //            factorItem.FactorId = factor.id;
        //            FactorItems.Add(factorItem);
        //        }
        //        _context.FactorItems.AddRange(FactorItems);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { status = 2, title = "خطای ثبت فاکتور", message = "خطایی رخ داده است." };
        //    }
        //    return new { status = 0, title = " ثبت فاکتور", message = "ثبت فاکتور با موفقیت انجام شد." };

        //}

        //public object GetFactorOfUser(GetFactor getFactor)
        //{
        //    var user = _context.Users.Where(p => p.token == getFactor.token).FirstOrDefault();
        //    //List<int?> Noticeids = _context.Factors.Where(x => x.userId == user.id).Select(x => x.NoticeId).ToList();
        //    //var result = _context.Notices.Where(x => Noticeids.Contains(x.id)).AsQueryable();
        //    var result = _context.Factors.Where(x => x.userId == user.id).ToList().Select(x => new { x.id, x.createDatePersian, x.totalPrice }).AsQueryable();
        //    int skip = (getFactor.page - 1) * 10;
        //    var res = result.OrderByDescending(u => u.id).Skip(skip).Take(10).ToList();
        //    return new { data = res, totalCount = result.Count() };
        //}
        //public object GetFactorItems(AllFactorItem allFactorItem)
        //{
        //    List<int> NoticeIds = _context.FactorItems.Include(x => x.Factor).Where(x => x.FactorId == allFactorItem.factorId).Select(x=>x.NoticeId).ToList();
        //    var result = _context.Notices.Where(x => NoticeIds.Contains(x.id)).Select(x => new { x.id, x.image, x.title, x.description }).AsQueryable();
        //    int skip = (allFactorItem.page - 1) * 10;
        //    var res = result.OrderByDescending(u => u.id).Skip(skip).Take(10).ToList();
        //    return new { data = res, totalCount = result.Count() };
        //}

        //public object GetLinkOfNotices(GetLinkOfNotice getLinkOfNotice)
        //{
        //    var user = _context.Users.Where(p => p.token == getLinkOfNotice.token).FirstOrDefault();
        //    var factor = _context.Factors.FirstOrDefault(x => x.userId == user.id);
        //    if (factor == null)
        //    {
        //        return new { status = 1, title = "خطای دریافت لینک", message = "این لینک توسط شما خریداری نشده است." };
        //    }
        //    else
        //    {
        //        var Notice = _context.Notices.FirstOrDefault(x => x.id == getLinkOfNotice.NoticeId);
        //        return new { status = 0, title = "لینک محصول", message = "لینک محصول شما.", data = Notice };
        //    }

        //}

        public void Remove(Notice model)
        {
            var MainItem = _context.Notices.FirstOrDefault(s => s.id == model.id);
            if (File.Exists($"wwwroot/Notice/" + MainItem.image1))
            {
                File.Delete($"wwwroot/Notice/" + MainItem.image1);
            }
            if (File.Exists($"wwwroot/Notice/" + MainItem.image2))
            {
                File.Delete($"wwwroot/Notice/" + MainItem.image2);
            }
            if (File.Exists($"wwwroot/Notice/" + MainItem.image3))
            {
                File.Delete($"wwwroot/Notice/" + MainItem.image3);
            }
            _context.Notices.Remove(MainItem);
            _context.SaveChanges();
        }
        public List<Notice> GetAllHome()
        {
            return _context.Notices
                .Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber).OrderByDescending(x => x.id).Take(20).Include(x => x.user).ToList();
        }
        public List<Notice> GetAllHomeNotBarber()
        {
            return _context.Notices
                .Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber == false)
                .OrderByDescending(x => x.id)
                .Include(x => x.user)
                .Take(15)
                .ToList();
        }
        public List<Notice> GetAllNotBarber()
        {
            return _context.Notices
                .Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber == false)
                .OrderByDescending(x => x.id)
                .Include(x => x.user)
                .ToList();
        }

        public PagedList<Notice> GetNoticeList(int pageId = 1, bool isBarber = false)
        {
            IQueryable<Notice> result = _context.Notices.Where(x => x.isDeleted == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isBarber == isBarber).OrderByDescending(x => x.createDate);
            PagedList<Notice> res = new PagedList<Notice>(result, pageId, 10);
            return res;
        }

        public List<Notice> GetAllRegisterByUser(int userId)
        {
            List<Notice> result = _context.Notices.Where(x => x.isDeleted == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.userId == userId).ToList();
            return result;
        }
        public void ChageState(int Id, int UserId, string totalAmount)
        {
            var Item = _context.Notices.Find(Id);
            var UserItem = _context.Users.Find(UserId);
            long Price = Convert.ToInt64(totalAmount);
            if (Item != null)
            {
                Item.isDeleted = false;
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }
        public void Extent(int Id, int UserId, string totalAmount)
        {
            var Item = _context.Notices.Find(Id);
            var UserItem = _context.Users.Find(UserId);
            long Price = Convert.ToInt64(totalAmount);
            if (Item != null)
            {
                Item.expireDate = DateTime.Now.AddDays(15);
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }

        public void Ladder(int Id, int UserId, string totalAmount)
        {
            var Item = _context.Notices.Find(Id);
            var UserItem = _context.Users.Find(UserId);
            long Price = Convert.ToInt64(totalAmount);
            if (Item != null)
            {
                Item.createDate = DateTime.Now;
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }
        public void ReserveRegister(NoticeType type, int UserId, string totalAmount, int ItemId)
        {
            var UserItem = _context.Users.Find(UserId);
            Reserve reserve = new Reserve();
            Line LineItem = new Line();
            ClassRoom ClassRoomItem = new ClassRoom();
            Product ProductItem = new Product();

            if (type == NoticeType.Line)
            {
                LineItem = _context.Lines.Find(ItemId);
            }
            else if (type == NoticeType.ClassRoom)
            {
                ClassRoomItem = _context.ClassRooms.Find(ItemId);
            }
            else
            {
                ProductItem = _context.Products.Find(ItemId);
            }
            if (UserItem != null && !string.IsNullOrEmpty(totalAmount))
            {
                long Price = Convert.ToInt64(totalAmount);
                reserve.date = DateTime.Now;
                reserve.price = Price;
                reserve.userId = UserItem.id;
                if (type == NoticeType.Line)
                {
                    reserve.lineId = LineItem.id;


                }
                else if (type == NoticeType.ClassRoom)
                {
                    reserve.classroomId = ClassRoomItem.id;
                }
                else
                {
                    reserve.productId = ProductItem.id;
                }
                _context.Reserves.Add(reserve);
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }

        public List<Notice> PercentNotices()
        {
            return _context.Notices
                .Where(x => x.condition == ConditionEnum.Percent && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber).OrderByDescending(x => x.id)
                .Include(x => x.user).ToList();

        }

        public List<Notice> RentNotices()
        {
            return _context.Notices
              .Where(x => x.condition == ConditionEnum.Rent && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber).OrderByDescending(x => x.id)
              .Include(x => x.user).ToList();

        }
        public List<Notice> FixedSalaryNotices()
        {
            return _context.Notices
              .Where(x => x.condition == ConditionEnum.FixedSalary && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isBarber).OrderByDescending(x => x.id)
              .Include(x => x.user).ToList();

        }
    }
}
