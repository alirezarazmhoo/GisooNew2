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
    public class classRoomService : IClassRoom
    {
        private Context _context;
        private readonly IHostingEnvironment environment;
        private readonly IUser _Iuser;

        public classRoomService(Context context, IHostingEnvironment environment, IUser user)
        {
            this.environment = environment;
            _context = context;
            _Iuser = user;

        }
        public void CreateOrUpdate(ClassRoomViewModel model, IFormFile[] _File)
        {
            try
            {
                var user = _Iuser.GetByIdUser(model.userId);
                if (model.id == 0)
                {
                    
                    ClassRoom classRoom = new ClassRoom();

                    var propInfo = model.GetType().GetProperties();
                    if (!String.IsNullOrEmpty(model.reserveDate))
                        classRoom.reserveDate = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(model.reserveDate));
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "ClassRoomTypes" || item.Name == "ClassRoomImage" || item.Name == "reserveDate" || item.Name == "minDiscount" || item.Name == "maxDiscount")
                            continue;
                        classRoom.GetType().GetProperty(item.Name).SetValue(classRoom, item.GetValue(model, null), null);
                    }
                    classRoom.registerDate = DateTime.Now;
                    classRoom.expireDate = (DateTime)user.expireDateAccount;
                    classRoom.adminConfirmStatus = EnumStatus.Pending;
                    _context.ClassRooms.Add(classRoom);
                    if (_File != null && _File.Count() > 0)
                    {
                        foreach (var item in _File)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClassRoom\File", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                                _context.ClassRoomImages.Add(new ClassRoomImage()
                                {
                                    url = fileName,

                                    classRoomId = classRoom.id,
                                });
                            }
                        }
                    }
                }
                else
                {
                    ClassRoom classRoom = _context.ClassRooms.Where(s => s.id == model.id).FirstOrDefault();
                    var propInfo = model.GetType().GetProperties();
                    if (!String.IsNullOrEmpty(model.reserveDate))
                        classRoom.reserveDate = DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(model.reserveDate));
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "ClassRoomTypes" || item.Name == "ClassRoomImage" || item.Name == "reserveDate" || item.Name == "minDiscount" || item.Name == "maxDiscount")
                            continue;
                        classRoom.GetType().GetProperty(item.Name).SetValue(classRoom, item.GetValue(model, null), null);
                    }
                    if (classRoom != null)
                    {
                        if (_File != null && _File.Count() > 0)
                        {
                            foreach (var item in _File)
                            {
                                var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClassRoom\File", fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                    _context.ClassRoomImages.Add(new ClassRoomImage()
                                    {
                                        url = fileName,
                                        classRoomId = classRoom.id,

                                    });
                                }
                            }
                        }
                    classRoom.adminConfirmStatus = EnumStatus.Pending;

                        _context.ClassRooms.Update(classRoom);

                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public ClassRoom GetById(int Id)
        {
            var Items = _context.ClassRooms.Include(s => s.ClassRoomImages).Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public void Remove(ClassRoom model)
        {
            var MainItem = _context.ClassRooms.FirstOrDefault(s => s.id == model.id);

            if (MainItem.ClassRoomImages.Count > 0)
            {
                foreach (var item in MainItem.ClassRoomImages)
                {
                    File.Delete($"wwwroot/ClassRoom/File/{item.url}");
                }
                _context.RemoveRange(MainItem.ClassRoomImages);
            }

            _context.ClassRooms.Remove(MainItem);
            _context.SaveChanges();
        }
        public void RemoveFile(int Id)
        {
            var Item = _context.ClassRoomImages.FirstOrDefault(s => s.id == Id);
            if (Item != null)
            {
                File.Delete($"wwwroot/ClassRoom/File/{Item.url}");
            }

            _context.ClassRoomImages.Remove(Item);
            _context.SaveChanges();
        }
        public List<ClassRoom> GetAll(int userId, SearchClassRoomViewModel searchClassRoomViewModel)
        {
            IQueryable<ClassRoom> result = _context.ClassRooms.Include(s => s.ClassRoomImages).Where(s => s.userId == userId).AsQueryable();
            if (searchClassRoomViewModel.classRoomLaw != 0)
                result = result.Where(x => x.classRoomLaw == searchClassRoomViewModel.classRoomLaw);
            if (searchClassRoomViewModel.classRoomTypeId != 0)
                result = result.Where(x => x.classRoomTypeId == searchClassRoomViewModel.classRoomTypeId);
            if (!String.IsNullOrEmpty(searchClassRoomViewModel.title))
                result = result.Where(x => x.title.Contains(searchClassRoomViewModel.title));
            if (!String.IsNullOrEmpty(searchClassRoomViewModel.registerDate))
            {
                result = result.Where(x => x.registerDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchClassRoomViewModel.registerDate)));
            }
            List<ClassRoom> ClassRooms = result.OrderByDescending(x => x.id).ToList();
            return ClassRooms;
        }
        public List<ClassRoom> GetAllHome()
        {
            Random rand = new Random();
            var result = _context.ClassRooms.Where(x => x.adminConfirmStatus == EnumStatus.Accept && x.expireDate >= DateTime.Now && x.reserveHour == null).Include(x => x.ClassRoomImages).Include(x => x.user).ToList();
            List<int> getAllId = new List<int>();
            List<ClassRoom> HelperList = new List<ClassRoom>();
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
            return HelperList.Take(20).ToList();

        }
        public List<ClassRoom> GetAllHomeReserve()
        {
            Random rand = new Random();
            var result = _context.ClassRooms.Where(x => x.adminConfirmStatus == EnumStatus.Accept && x.expireDate >= DateTime.Now && x.reserveHour != null).Include(x => x.ClassRoomImages).Include(x => x.user).ToList();
            List<int> getAllId = new List<int>();
            List<ClassRoom> HelperList = new List<ClassRoom>();
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
            return HelperList.Take(20).ToList();

        }
        public ClassRoom GetByIdForDetail(int Id, string ip)
        {
            var Items = _context.ClassRooms.Where(s => s.id == Id).Include(x => x.user).Include(x => x.classRoomType).Include(x => x.ClassRoomImages).FirstOrDefault();
            if (!_context.Visits.Any(x => x.anyNoticeId == Id && x.whichTableEnum == WhichTableEnum.ClssRoom && x.Ip == ip))

            {
                Visit v = new Visit();
                v.date = DateTime.Now.Date;
                v.Ip = ip;
                v.whichTableEnum = WhichTableEnum.ClssRoom;
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
        public List<ClassRoom> GetRelated(int id)
        {

            var result = _context.ClassRooms.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.id != id).OrderByDescending(x => x.id).Take(20).Include(x => x.user).Include(x => x.ClassRoomImages).ToList();
            return result;
        }
        public PagedList<ClassRoom> GetClassRooms(int pageId = 1, bool isReserves = false)
        {
            IQueryable<ClassRoom> result = null;
            if (isReserves)
                result = _context.ClassRooms.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.reserveHour != null).Include(x => x.ClassRoomImages).Include(x => x.classRoomType).OrderByDescending(x => x.registerDate);
            else
                result = _context.ClassRooms.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.reserveHour == null).Include(x => x.ClassRoomImages).Include(x => x.classRoomType).OrderByDescending(x => x.registerDate);
            PagedList<ClassRoom> res = new PagedList<ClassRoom>(result, pageId, 10);
            return res;
        }
        public List<ClassRoom> CheapestClassRooms()
        {
            List<ClassRoom> result = _context.ClassRooms.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept).Include(x => x.ClassRoomImages).Include(x => x.classRoomType).Where(x => x.adminConfirmStatus == EnumStatus.Accept).OrderBy(x => x.price).Take(3).ToList();
            return result;
        }
        public List<ClassRoom> GetAllRegisterByUser(int userId)
        {
            List<ClassRoom> result = _context.ClassRooms.Where(x => x.userId == userId && x.adminConfirmStatus == EnumStatus.Accept).Include(x => x.ClassRoomImages).ToList();
            return result;
        }
        public PagedList<ClassRoom> GetClassRooms(int page = 1, string filterTitle = "")
        {
            IQueryable<ClassRoom> result = _context.ClassRooms.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            PagedList<ClassRoom> res = new PagedList<ClassRoom>(result, page, 20);
            return res;
        }
        public void CreateOrUpdateFromAdmin(ClassRoomViewModelAdmin ClassRoomViewModelAdmin, IFormFile[] file)
        {
            if (ClassRoomViewModelAdmin.ClassRoom.id != 0)
            {
                var ClassRoom = _context.ClassRooms.Find(ClassRoomViewModelAdmin.ClassRoom.id);
                var propInfo = ClassRoomViewModelAdmin.ClassRoom.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.Name == "id" || item.Name == "userId")
                        continue;
                    ClassRoom.GetType().GetProperty(item.Name).SetValue(ClassRoom, item.GetValue(ClassRoomViewModelAdmin.ClassRoom, null), null);
                }
                if (ClassRoomViewModelAdmin.expireDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(ClassRoomViewModelAdmin.expireDate1.Substring(0, 4));
                    int month = Convert.ToInt32(ClassRoomViewModelAdmin.expireDate1.Substring(5, 2));
                    int day = Convert.ToInt32(ClassRoomViewModelAdmin.expireDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    ClassRoom.expireDate = dt;
                }
                if (ClassRoomViewModelAdmin.reserveDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(0, 4));
                    int month = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(5, 2));
                    int day = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    ClassRoom.reserveDate = dt;
                }
                ClassRoom.adminConfirmStatus = EnumStatus.Accept;
                if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClassRoom\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.ClassRoomImages.Add(new ClassRoomImage()
                            {
                                url = fileName,
                                classRoomId = ClassRoom.id,
                            });
                        }
                    }
                }
            }
            else
            {
                ClassRoom classRoom = new ClassRoom();

                var propInfo = ClassRoomViewModelAdmin.ClassRoom.GetType().GetProperties();

                foreach (var item in propInfo)
                {
                    if (item.Name == "id")
                        continue;
                    classRoom.GetType().GetProperty(item.Name).SetValue(classRoom, item.GetValue(ClassRoomViewModelAdmin.ClassRoom, null), null);
                }
                if (ClassRoomViewModelAdmin.reserveDate1 != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    int year = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(0, 4));
                    int month = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(5, 2));
                    int day = Convert.ToInt32(ClassRoomViewModelAdmin.reserveDate1.Substring(8, 2));
                    DateTime dt = new DateTime(year, month, day, pc);
                    classRoom.reserveDate = dt;
                }
                classRoom.registerDate = DateTime.Now;
                classRoom.expireDate = DateTime.Now.AddDays(15);
                classRoom.adminConfirmStatus = EnumStatus.Accept;
                _context.ClassRooms.Add(classRoom);
                if (file != null && file.Count() > 0)
                {
                    foreach (var item in file)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(item.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClassRoom\File", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            _context.ClassRoomImages.Add(new ClassRoomImage()
                            {
                                url = fileName,
                                classRoomId = classRoom.id,
                            });
                        }
                    }
                }
            }

        }
        public ClassRoomViewModelAdmin GetForDetailPage(int id)
        {
            var ClassRoom = _context.ClassRooms.Include(x => x.user).Include(x => x.ClassRoomImages).FirstOrDefault(x => x.id == id);
            ClassRoomViewModelAdmin ClassRoomViewModelAdmin = new ClassRoomViewModelAdmin();
            ClassRoomViewModelAdmin.ClassRoom = ClassRoom;
            ClassRoomViewModelAdmin.ClassRoomTypes = _context.ClassRoomTypes.ToList();
            List<Visit> visits = _context.Visits.Where(x => x.anyNoticeId == id && x.whichTableEnum == WhichTableEnum.ClssRoom).ToList();
            ClassRoomViewModelAdmin.datecount1 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-9)) == 0).Count();
            ClassRoomViewModelAdmin.datecount2 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-8)) == 0).Count();
            ClassRoomViewModelAdmin.datecount3 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-7)) == 0).Count();
            ClassRoomViewModelAdmin.datecount4 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-6)) == 0).Count();
            ClassRoomViewModelAdmin.datecount5 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-5)) == 0).Count();
            ClassRoomViewModelAdmin.datecount6 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-4)) == 0).Count();
            ClassRoomViewModelAdmin.datecount7 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-3)) == 0).Count();
            ClassRoomViewModelAdmin.datecount8 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-2)) == 0).Count();
            ClassRoomViewModelAdmin.datecount9 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date.AddDays(-1)) == 0).Count();
            ClassRoomViewModelAdmin.datecount10 = visits.Where(x => x.date.Date.CompareTo(DateTime.Now.Date) == 0).Count();
            return ClassRoomViewModelAdmin;
        }
        public int CountRegisterClass(int userId)
        {
            int count = _context.ClassRooms.Where(x => x.userId == userId).Count();
            return count;
        }


        public void Reserve(ClassRoom classRoom, User user, bool ifFromKif = false)
        {
            var userClassRoomOwner = _Iuser.GetByIdUser(classRoom.userId);

            long totalprice = 0;
            if (classRoom.discountPrice != null && classRoom.discountPrice != 0)
                totalprice = Convert.ToInt64(classRoom.discountPrice);
            else
                totalprice = Convert.ToInt64(classRoom.price);
            Reserve reserve = new Reserve();
            reserve.date = DateTime.Now;
            reserve.price = totalprice;
            reserve.userId = user.id;
                reserve.userIdNoticeOwner =userClassRoomOwner.id ;

            reserve.classroomId = classRoom.id;
            Factor factor = new Factor();
            factor.state = State.IsPay;
            factor.userId = user.id;
            factor.classRoomId = classRoom.id;
            factor.createDatePersian = PersianCalendarDate.PersianCalendarResult(DateTime.Now);
            factor.factorKind = FactorKind.Add;
            factor.totalPrice = totalprice;
            if (ifFromKif)
            {
                user.score -= totalprice;
            }
            userClassRoomOwner.score += totalprice;
            _context.Factors.Add(factor);
            _context.Reserves.Add(reserve);
            _context.SaveChanges();
        }
    }
}

