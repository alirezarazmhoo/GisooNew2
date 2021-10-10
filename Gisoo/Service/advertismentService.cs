using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Service
{
    public class advertismentService : IAdvertisment
    {
        private Context _context;
        public advertismentService(Context context)
        {
            _context = context;
        }

        //public int AddAdvertismentFromAdmin(CreateAdvertismentViewModel Advertisment)
        //{
        //    Advertisment addAdvertisment = new Advertisment();
        //    addAdvertisment.description = Advertisment.description;
        //    addAdvertisment.title = Advertisment.title;

        //    #region Save Image

        //    if (Advertisment.image != null)
        //    {
        //        string imagePath = "";
        //        addAdvertisment.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Advertisment.image.FileName);
        //        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Advertisment", addAdvertisment.image);
        //        using (var stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            Advertisment.image.CopyTo(stream);
        //        }
        //        addAdvertisment.image = "/images/Advertisment/" + addAdvertisment.image;
        //    }

        //    #endregion

        //    return AddAdvertisment(addAdvertisment);
        //}
        public int AddAdvertisment(Advertisment Advertisment)
        {
            _context.Advertisments.Add(Advertisment);
            _context.SaveChanges();
            return Advertisment.id;
        }
        //public void EditAdvertisment(CreateAdvertismentViewModel Advertisment)
        //{
        //    Advertisment _Advertisment = _context.Advertisments.Find(Advertisment.id);
        //    _Advertisment.title = Advertisment.title;
        //    _Advertisment.description = Advertisment.description;
        //    if (Advertisment.image != null)
        //    {
        //        if (_Advertisment.image != null)
        //        {
        //            string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Advertisment/", _Advertisment.image);
        //            if (File.Exists(deletePath))
        //            {
        //                File.Delete(deletePath);
        //            }
        //        }


        //        string imagePath = "";
        //        _Advertisment.image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Advertisment.image.FileName);
        //        imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Advertisment", _Advertisment.image);
        //        using (var stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            Advertisment.image.CopyTo(stream);
        //        }
        //        _Advertisment.image = "/images/Advertisment/" + _Advertisment.image;

        //    }

        //    _context.Advertisments.Update(_Advertisment);
        //    _context.SaveChanges();
        //}
        public object GetAdvertisments(int page = 1)
        {
            IQueryable<Advertisment> result = _context.Advertisments;
            int skip = (page - 1) * 10;
            List<Advertisment> res = result.ToList();
            return new { data = res.OrderByDescending(u => u.createDate).Skip(skip).Take(10), totalCount = res.Count() };
        }
        public object GetAdvertisments(int page = 1, int pagesize = 10)
        {
            IQueryable<Advertisment> result = _context.Advertisments.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept);
            int skip = (page - 1) * pagesize;
            var res = result.OrderByDescending(u => u.createDate).Skip(skip).Take(pagesize).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }

        public PagedList<Advertisment> GetAdvertisments(bool? filterisWorkshop, int page = 1, string filterTitle = "")
        {
            IQueryable<Advertisment> result = _context.Advertisments.Where(x => x.isDeleted == false).OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(u => u.title.Contains(filterTitle));
            }
            if (filterisWorkshop != null)
            {
                result = result.Where(u => u.isWorkshop == filterisWorkshop);
            }

            PagedList<Advertisment> res = new PagedList<Advertisment>(result, page, 20);
            return res;
        }

        public object GetLastAdvertisments(int page = 1)
        {
            IQueryable<Advertisment> result = _context.Advertisments;
            int skip = (page - 1) * 10;
            var res = result.OrderByDescending(u => u.createDate).Skip(skip).Take(10).Select(x => new { x.id, x.image1, x.title, x.description, areaName = x.area.name, cityName = x.city.name, provinceName = x.province.name }).ToList();
            return new { data = res, totalCount = result.Count() };
        }
        public Advertisment GetById(int Id)
        {
            var Items = _context.Advertisments.Where(s => s.id == Id).FirstOrDefault();
            return Items;
        }
        public int CreateOrUpdate(AdvertismentViewModel model, IFormFile _File1, IFormFile _File2, IFormFile _File3, bool fromAdmin = false)
        {
            try
            {
                string img1 = "";
                    string img2 = "";
                    string img3 = "";
                if (model.id == 0)
                {
                    
                    Advertisment advertisment = new Advertisment();

                    var propInfo = model.GetType().GetProperties();
                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "cities" || item.Name == "provinces" || item.Name == "areas")
                            continue;
                        advertisment.GetType().GetProperty(item.Name).SetValue(advertisment, item.GetValue(model, null), null);
                    }
                    advertisment.createDate = DateTime.Now;
                        advertisment.expireDate = DateTime.Now.AddDays(15);


                    if (fromAdmin)
                    {
                        advertisment.adminConfirmStatus = EnumStatus.Accept;
                    }
                    else
                    {
                        
                        advertisment.adminConfirmStatus = EnumStatus.Pending;
                    }
                    if (_context.Advertisments.Count() == 0)
                        advertisment.code = "1";
                    else
                        advertisment.code = (Convert.ToInt32(_context.Advertisments.LastOrDefault().code) + 1).ToString();
                    if (_File1 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File1.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File1.CopyTo(stream);
                            //advertisment.image1 = "/Advertisment/" + fileName;
                            img1 = "/Advertisment/" + fileName;
                        }

                    }
                    if (_File2 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File2.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File2.CopyTo(stream);
                            //advertisment.image2 = "/Advertisment/" + fileName;
                            img2 = "/Advertisment/" + fileName;
                        }

                    }
                    if (_File3 != null)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File3.FileName).ToLower();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            _File3.CopyTo(stream);
                            //advertisment.image3 = "/Advertisment/" + fileName;
                            img3 = "/Advertisment/" + fileName;
                        }

                    }
                    advertisment.image1 = img1;
                    advertisment.image2 = img2;
                    advertisment.image3 = img3;
                    _context.Advertisments.Add(advertisment);
                    _context.SaveChanges();
                    return advertisment.id;
                }
                else
                {
                    Advertisment advertisment = _context.Advertisments.Where(s => s.id == model.id).FirstOrDefault();
                    var propInfo = model.GetType().GetProperties();

                    foreach (var item in propInfo)
                    {
                        if (item.Name == "id" || item.Name == "cities" || item.Name == "provinces" || item.Name == "areas")
                            continue;
                        advertisment.GetType().GetProperty(item.Name).SetValue(advertisment, item.GetValue(model, null), null);
                    }
                    if (advertisment != null)
                    {
                        if (_File1 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File1.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File1.CopyTo(stream);
                                advertisment.image1 = "/Advertisment/" + fileName;
                            }

                        }
                        if (_File2 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File2.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File2.CopyTo(stream);
                                advertisment.image2 = "/Advertisment/" + fileName;
                            }

                        }
                        if (_File3 != null)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace('-', '0') + Path.GetExtension(_File3.FileName).ToLower();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Advertisment", fileName);
                            File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                _File3.CopyTo(stream);
                                advertisment.image3 = "/Advertisment/" + fileName;
                            }

                        }
                        _context.Advertisments.Update(advertisment);



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

        public List<Advertisment> GetAll(int userId, SearchAdvertismentViewModel searchAdvertismentViewModel)
        {
            IQueryable<Advertisment> result = _context.Advertisments.Where(s => s.userId == userId).AsQueryable();
            if (!String.IsNullOrEmpty(searchAdvertismentViewModel.title))
                result = result.Where(x => x.title.Contains(searchAdvertismentViewModel.title));
            if (!String.IsNullOrEmpty(searchAdvertismentViewModel.registerDate))
            {
                result = result.Where(x => x.createDate.Date == DateChanger.ToGeorgianDateTime(DateChanger.PersianToEnglish(searchAdvertismentViewModel.registerDate)));
            }
            List<Advertisment> advertisments = result.OrderByDescending(x => x.id).ToList();
            return advertisments;
        }
        public void RemoveFile(int Id, string imageName)
        {
            var advertisment = _context.Advertisments.FirstOrDefault(s => s.id == Id);
            if (advertisment.image1 == imageName)
                advertisment.image1 = null;
            if (advertisment.image2 == imageName)
                advertisment.image2 = null;
            if (advertisment.image3 == imageName)
                advertisment.image3 = null;
            if (advertisment != null)
            {
                File.Delete($"wwwroot/Advertisment/{imageName}");
            }

            _context.SaveChanges();
        }
        public void Remove(Advertisment model)
        {
            var MainItem = _context.Advertisments.FirstOrDefault(s => s.id == model.id);
            if (File.Exists($"wwwroot/Advertisment/" + MainItem.image1))
            {
                File.Delete($"wwwroot/Advertisment/" + MainItem.image1);
            }
            if (File.Exists($"wwwroot/Advertisment/" + MainItem.image2))
            {
                File.Delete($"wwwroot/Advertisment/" + MainItem.image2);
            }
            if (File.Exists($"wwwroot/Advertisment/" + MainItem.image3))
            {
                File.Delete($"wwwroot/Advertisment/" + MainItem.image3);
            }
            _context.Advertisments.Remove(MainItem);
            _context.SaveChanges();
        }
        public List<Advertisment> GetAllHome()
        {
            return _context.Advertisments.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isWorkshop).OrderByDescending(x => x.id).Take(20).Include(x => x.user).ToList();
        }
        public List<Advertisment> GetAllHomeisNotWorkshop()
        {
            return _context.Advertisments.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isDeleted == false && x.isWorkshop == false).OrderByDescending(x => x.id).Take(20).Include(x => x.user).ToList();
        }
        public Advertisment GetByIdForDetail(int Id, string ip)
        {
            var Items = _context.Advertisments.Where(s => s.id == Id).Include(x => x.user).Include(x => x.city).Include(x => x.province).Include(x => x.area).FirstOrDefault();
            if (!_context.Visits.Any(x => x.anyNoticeId == Id && x.whichTableEnum == WhichTableEnum.Advertisment && x.Ip == ip))

            {
                Visit v = new Visit();
                v.date = DateTime.Now.Date;
                v.Ip = ip;
                v.whichTableEnum = WhichTableEnum.Advertisment;
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
        public List<Advertisment> GetRelated(int id)
        {

            var result = _context.Advertisments.Where(x => x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.id != id).OrderByDescending(x => x.id).Take(20).Include(x => x.user).ToList();
            return result;
        }
        public PagedList<Advertisment> GetAdvertismentList(int pageId = 1, bool isWorkshop = false)
        {
            IQueryable<Advertisment> result = _context.Advertisments.Where(x => x.isDeleted == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isWorkshop == isWorkshop).OrderByDescending(x => x.createDate);
            PagedList<Advertisment> res = new PagedList<Advertisment>(result, pageId, 10);
            return res;
        }
        public List<Advertisment> IsWorkshopAdvertisments(bool isWorkshop = false)
        {
            List<Advertisment> result = _context.Advertisments.Where(x => x.isDeleted == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.isWorkshop != isWorkshop).OrderByDescending(x => x.createDate).Take(3).ToList();
            return result;
        }
        public List<Advertisment> GetAllRegisterByUser(int userId)
        {
            List<Advertisment> result = _context.Advertisments.Where(x => x.isDeleted == false && x.expireDate >= DateTime.Now && x.adminConfirmStatus == EnumStatus.Accept && x.userId == userId).ToList();
            return result;
        }
        public void ChangeStatus(int Id, int UserId, string totalAmount)
        {
            var Item = _context.Advertisments.Find(Id);
            var UserItem = _context.Users.Find(UserId);
            long Price = Convert.ToInt64(totalAmount);
            if (Item != null && UserItem != null)
            {
                Item.isDeleted = false;
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }
        public void Extent(int Id, int UserId, string totalAmount)
        {
            var Item = _context.Advertisments.Find(Id);
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
            var Item = _context.Advertisments.Find(Id);
            var UserItem = _context.Users.Find(UserId);
            long Price = Convert.ToInt64(totalAmount);
            if (Item != null)
            {
                Item.createDate = DateTime.Now;
                UserItem.score -= Price;
                _context.SaveChanges();
            }
        }
    }
}
