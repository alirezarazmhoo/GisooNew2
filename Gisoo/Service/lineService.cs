using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.Utility;
using System.Globalization;

namespace Gisoo.Service
{
    public class lineService : ILine
    {
        private Context _context;
        private readonly IUser _Iuser;

        private readonly IHostingEnvironment environment;

        public lineService(Context context, IHostingEnvironment environment, IUser user)
        {
            this.environment = environment;
            _context = context;
            _Iuser = user;

        }
        public int CreateOrUpdate(LineViewModel model, IFormFile[] _File)
        {
            try
            {
                var user = _Iuser.GetByIdUser(model.userId);

                if (model.id == 0)
                {
                    Line line = new Line();


                    var propInfo = model.GetType().GetProperties();
                    if (!String.IsNullOrEmpty(model.reserveDate))
                        line.reserveDate = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(model.reserveDate));
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "LineTypes" || item.Name == "LineImage" || item.Name == "reserveDate" || item.Name == "minDiscount" || item.Name == "maxDiscount")
                            continue;
                        line.GetType().GetProperty(item.Name).SetValue(line, item.GetValue(model, null), null);
                    }
                    line.registerDate = DateTime.Now;
                    line.expireDate = (DateTime)user.expireDateAccount;
                    line.adminConfirmStatus = EnumStatus.Pending;
                    _context.Lines.Add(line);
                    if (_File != null && _File.Count() > 0)
                    {
                        foreach (var item in _File)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Line\File", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                                _context.LineImages.Add(new LineImage()
                                {
                                    url = fileName,
                                    lineId = line.id,
                                });
                            }
                        }
                    }
                    _context.SaveChanges();
                    return line.id;
                }
                else
                {
                    Line line = _context.Lines.Where(s => s.id == model.id).FirstOrDefault();
                    var propInfo = model.GetType().GetProperties();
                    if (!String.IsNullOrEmpty(model.reserveDate))
                        line.reserveDate = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(model.reserveDate));
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "LineTypes" || item.Name == "LineImage" || item.Name == "reserveDate" || item.Name == "minDiscount" || item.Name == "maxDiscount")
                            continue;
                        line.GetType().GetProperty(item.Name).SetValue(line, item.GetValue(model, null), null);
                    }
                    if (line != null)
                    {
                        if (_File != null && _File.Count() > 0)
                        {
                            foreach (var item in _File)
                            {
                                var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Line\File", fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                    _context.LineImages.Add(new LineImage()
                                    {
                                        url = fileName,
                                        lineId = line.id,

                                    });
                                }
                            }
                        }
                        line.adminConfirmStatus = EnumStatus.Pending;

                        _context.Lines.Update(line);
                        _context.SaveChanges();

                    }
                    return line.id;

                }

            }
            catch (Exception)
            {
                return 0;
            }
        }
        public Line GetById(int Id)
        {
            var Items = _context.Lines.Include(s => s.LineImages).Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public void Remove(Line model)
        {
            var MainItem = _context.Lines.FirstOrDefault(s => s.id == model.id);

            if (MainItem.LineImages.Count > 0)
            {
                foreach (var item in MainItem.LineImages)
                {
                    File.Delete($"wwwroot/Line/File/{item.url}");
                }
                _context.RemoveRange(MainItem.LineImages);
            }

            _context.Lines.Remove(MainItem);
            _context.SaveChanges();
        }
        public void RemoveFile(int Id)
        {
            var Item = _context.LineImages.FirstOrDefault(s => s.id == Id);
            if (Item != null)
            {
                File.Delete($"wwwroot/Line/File/{Item.url}");
            }

            _context.LineImages.Remove(Item);
            _context.SaveChanges();
        }
        public List<Line> GetAll(int userId, SearchLineViewModel searchLineViewModel)
        {
            IQueryable<Line> result = _context.Lines.Include(s => s.LineImages).Where(s => s.userId == userId).AsQueryable();
            if (searchLineViewModel.lineLaw != 0)
                result = result.Where(x => x.lineLaw == searchLineViewModel.lineLaw);
            if (searchLineViewModel.lineTypeId != 0)
                result = result.Where(x => x.lineTypeId == searchLineViewModel.lineTypeId);
            if (!String.IsNullOrEmpty(searchLineViewModel.title))
                result = result.Where(x => x.title.Contains(searchLineViewModel.title));
            if (!String.IsNullOrEmpty(searchLineViewModel.registerDate))
            {
                result = result.Where(x => x.registerDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchLineViewModel.registerDate)));
            }
            List<Line> lines = result.OrderByDescending(x => x.id).ToList();
            return lines;
        }
        public List<Line> GetAllHome()
        {
            Random rand = new Random();
            var result = _context.Lines.Where(x => x.adminConfirmStatus == EnumStatus.Accept && x.expireDate >= DateTime.Now && x.reserveHour == null).Include(x => x.LineImages).Include(x => x.user).ToList();
            List<int> getAllId = new List<int>();
            List<Line> HelperList = new List<Line>();
            int reapet = result.Count() > 20 ? 20 : result.Count();
            List<int> listNumbers = new List<int>();
            int number;
            for (int i = 0; i < reapet; i++)
            {
                do
                {
                    number = rand.Next(0, result.Count());
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                if (result.ElementAt(number) != null)
                    HelperList.Add(result.ElementAt(number));
            }
            //return _context.Lines.Take(20).Include(s => s.LineImages).Include(x => x.user).ToList();
            return HelperList.Take(20).ToList();
        }
        public List<Line> GetAllHomeIsService()
        {
            Random rand = new Random();
            var result = _context.Lines.Where(x => x.adminConfirmStatus == EnumStatus.Accept && x.expireDate >= DateTime.Now && x.reserveHour != null).Include(x => x.LineImages).Include(x => x.user).ToList();
            List<int> getAllId = new List<int>();
            List<Line> HelperList = new List<Line>();
            int reapet = result.Count() > 20 ? 20 : result.Count();
            List<int> listNumbers = new List<int>();
            int number;
            for (int i = 0; i < reapet; i++)
            {
                do
                {
                    number = rand.Next(0, result.Count());
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                if (result.ElementAt(number) != null)
                    HelperList.Add(result.ElementAt(number));
            }
            //return _context.Lines.Take(20).Include(s => s.LineImages).Include(x => x.user).ToList();
            return HelperList.Take(20).ToList();
        }

        public List<Reserve> GetReservedLines(int userId, SearchReserveViewModel searchReserveViewModel)
        {
            IQueryable<Reserve> result = _context.Reserves.Include(s => s.line).Include(s => s.user).Include(s => s.line.lineType).Include(s => s.line.LineImages).Where(s => s.userId == userId && s.classroomId == null);
            if (!String.IsNullOrEmpty(searchReserveViewModel.title))
            {
                result = result.Where(x => x.line.title.Contains(searchReserveViewModel.title));
            }
            if (!String.IsNullOrEmpty(searchReserveViewModel.registerDate))
            {
                result = result.Where(x => x.line.registerDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchReserveViewModel.registerDate)));
            }
            if (searchReserveViewModel.lineTypeId != 0)
            {
                result = result.Where(x => x.line.lineTypeId == searchReserveViewModel.lineTypeId);
            }
            List<Reserve> items = result.OrderByDescending(x => x.id).ToList();
            return items;
        }
        public List<Reserve> GetReservedClasses(int userId, SearchClassRoomReserveViewModel searchClassRoomReserveViewModel)
        {
            IQueryable<Reserve> result = _context.Reserves.Include(s => s.classroom).Include(s => s.user).Include(s => s.classroom.classRoomType).Include(s => s.classroom.ClassRoomImages).Where(s => s.userId == userId && s.lineId == null);
            if (!String.IsNullOrEmpty(searchClassRoomReserveViewModel.title))
            {
                result = result.Where(x => x.classroom.title.Contains(searchClassRoomReserveViewModel.title));
            }
            if (!String.IsNullOrEmpty(searchClassRoomReserveViewModel.registerDate))
            {
                result = result.Where(x => x.classroom.registerDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchClassRoomReserveViewModel.registerDate)));
            }
            if (searchClassRoomReserveViewModel.ClassRoomTypesId != 0)
            {
                result = result.Where(x => x.classroom.classRoomTypeId == searchClassRoomReserveViewModel.ClassRoomTypesId);
            }
            List<Reserve> items = result.OrderByDescending(x => x.id).ToList();
            return items;
        }
        public List<Line> GetAll()
        {
            var Items = _context.Lines.Where(s => s.adminConfirmStatus == EnumStatus.Pending || s.adminConfirmStatus == EnumStatus.NotAccept).ToList();
            return Items;

        }

        public void ChangeStatusToSuccess(int id)
        {
            var item = _context.Lines.Find(GetById(id));
            if (item != null)
            {
                item.adminConfirmStatus = EnumStatus.Accept;
                _context.SaveChanges();
            }

        }
        public Line GetByIdForDetail(int Id, string ip)
        {
            var Items = _context.Lines.Where(s => s.id == Id).Include(x => x.user).Include(x => x.lineType).Include(x => x.LineImages).FirstOrDefault();
            if (!_context.Visits.Any(x => x.anyNoticeId == Id && x.whichTableEnum == WhichTableEnum.Line && x.Ip == ip))

            {
                Visit v = new Visit();
                v.date = DateTime.Now.Date;
                v.Ip = ip;
                v.whichTableEnum = WhichTableEnum.Line;
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
        public List<Line> GetRelated(int id)
        {

            var result = _context.Lines.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.id != id).OrderByDescending(x => x.id).Take(20).Include(x => x.user).Include(x => x.LineImages).ToList();
            return result;
        }
        public PagedList<Line> GetLines(int pageId = 1, bool LineIsService = false)
        {
            IQueryable<Line> result = null;
            if (LineIsService)
                result = _context.Lines.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.reserveHour != null).Include(x => x.LineImages).Include(x => x.lineType).OrderByDescending(x => x.registerDate);
            else
                result = _context.Lines.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.reserveHour == null).Include(x => x.LineImages).Include(x => x.lineType).OrderByDescending(x => x.registerDate);

            PagedList<Line> res = new PagedList<Line>(result, pageId, 10);
            return res;
        }
        public List<Line> CheapestLines()
        {
            List<Line> result = _context.Lines.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept).Include(x => x.LineImages).Include(x => x.lineType).OrderBy(x => x.price).Take(3).ToList();
            return result;
        }
        public List<Line> GetAllRegisterByUser(int userId)
        {
            List<Line> result = _context.Lines.Where(x => x.userId == userId && x.adminConfirmStatus == EnumStatus.Accept).Include(x => x.LineImages).ToList();
            return result;
        }
        public PagedList<Line> GetLines(int page = 1, string filterTitle = "")
        {
            IQueryable<Line> result = _context.Lines.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            PagedList<Line> res = new PagedList<Line>(result, page, 20);
            return res;
        }
        public int CreateOrUpdateFromAdmin(LineViewModelAdmin lineViewModelAdmin, IFormFile[] file)
        {
            if (lineViewModelAdmin.line.id != 0)
            {
                var Line = _context.Lines.Find(lineViewModelAdmin.line.id);
                var propInfo = lineViewModelAdmin.line.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.Name == "id" || item.Name == "userId")
                        continue;
                    Line.GetType().GetProperty(item.Name).SetValue(Line, item.GetValue(lineViewModelAdmin.line, null), null);
                }
                if (lineViewModelAdmin.expireDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(lineViewModelAdmin.expireDate1.Substring(0, 4));
                    int month = Convert.ToInt32(lineViewModelAdmin.expireDate1.Substring(5, 2));
                    int day = Convert.ToInt32(lineViewModelAdmin.expireDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    Line.expireDate = dt;
                }
                if (lineViewModelAdmin.reserveDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(0, 4));
                    int month = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(5, 2));
                    int day = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    Line.reserveDate = dt;
                }
                if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Line\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.LineImages.Add(new LineImage()
                            {
                                url = fileName,
                                lineId = Line.id,
                            });
                        }
                    }
                }
                _context.SaveChanges();
                return Line.id;
            }
            else
            {
                Line Line = new Line();
                var propInfo = lineViewModelAdmin.line.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.Name == "id")
                        continue;
                    Line.GetType().GetProperty(item.Name).SetValue(Line, item.GetValue(lineViewModelAdmin.line, null), null);

                }

                if (lineViewModelAdmin.reserveDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(0, 4));
                    int month = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(5, 2));
                    int day = Convert.ToInt32(lineViewModelAdmin.reserveDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    Line.reserveDate = dt;
                }
                Line.registerDate = DateTime.Now;
                Line.expireDate = DateTime.Now.AddDays(15);
                Line.adminConfirmStatus = EnumStatus.Accept;
                _context.Lines.Add(Line);
                if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Line\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.LineImages.Add(new LineImage()
                            {
                                url = fileName,
                                lineId = Line.id,
                            });
                        }
                    }
                }
                _context.SaveChanges();
                return Line.id;

            }

        }
        public LineViewModelAdmin GetForDetailPage(int id)
        {
            var Line = _context.Lines.Include(x => x.user).Include(x => x.LineImages).FirstOrDefault(x => x.id == id);
            LineViewModelAdmin lineViewModelAdmin = new LineViewModelAdmin();
            lineViewModelAdmin.line = Line;
            lineViewModelAdmin.lineTypes = _context.LineTypes.ToList();
            List<Visit> visits = _context.Visits.Where(x => x.anyNoticeId == id && x.whichTableEnum == WhichTableEnum.Line).ToList();
            lineViewModelAdmin.datecount1 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-9)) == 0).Count();
            lineViewModelAdmin.datecount2 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-8)) == 0).Count();
            lineViewModelAdmin.datecount3 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-7)) == 0).Count();
            lineViewModelAdmin.datecount4 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-6)) == 0).Count();
            lineViewModelAdmin.datecount5 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-5)) == 0).Count();
            lineViewModelAdmin.datecount6 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-4)) == 0).Count();
            lineViewModelAdmin.datecount7 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-3)) == 0).Count();
            lineViewModelAdmin.datecount8 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-2)) == 0).Count();
            lineViewModelAdmin.datecount9 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) == 0).Count();
            lineViewModelAdmin.datecount10 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date) == 0).Count();
            return lineViewModelAdmin;
        }

        public int CountRegisterLine(int userId)
        {
            int count = _context.Lines.Where(x => x.userId == userId).Count();
            return count;
        }
    }
}

