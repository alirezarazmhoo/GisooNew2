using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.DAL;
using Gisoo.Models;
using Gisoo.Service.Interface;
using Gisoo.Utility;
using Gisoo.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmsIrRestfulNetCore;

namespace Gisoo.Controllers
{
    public class ClassRoomController : Controller
    {
        private readonly Context _context;
        private IClassRoom _ClassRoom;
        private IVisit _IVisit;
        private readonly IHostingEnvironment environment;

        public ClassRoomController(Context context, IClassRoom ClassRoom, IVisit visit, IHostingEnvironment environment)
        {
            this.environment = environment;
            _context = context;
            _ClassRoom = ClassRoom;
            _IVisit = visit;

        }

        // GET: Products
        public IActionResult Index(int page = 1, string filterTitle = "")
        {
            var model = _ClassRoom.GetClassRooms(page, filterTitle);
            return View(model);
        }


        [HttpPost]
        public ActionResult InActive(NoticeViewModel page)
        {
            try
            {
                var ClassRoom = _context.ClassRooms.Include(x => x.user).FirstOrDefault(x => x.id == page.id);
                ClassRoom.adminConfirmStatus = page.adminConfirmStatus;
                ClassRoom.notConfirmDescription = page.notConfirmDescription;
                if (page.adminConfirmStatus == EnumStatus.NotAccept)
                {
                     try
                    {
                        List<SmsParameters> Parameter = new List<SmsParameters>();
                        Parameter.Add(new SmsParameters() { Parameter = "NoticeTitle", ParameterValue = ClassRoom.title});
                        Parameter.Add(new SmsParameters() { Parameter = "reason", ParameterValue = page.notConfirmDescription});
                        SendSms.CallSmSMethodAdvanced(Convert.ToInt64(ClassRoom.user.cellphone), 41387,Parameter);
                    }
                    catch (Exception ex)
                    {
                return Json("Failed");
                        
                    }
                }
                else
                {
                    try
                    {
                        
                        SendSms.CallSmSMethod(Convert.ToInt64(ClassRoom.user.cellphone), 41386,"NoticeTitle",ClassRoom.title);
                    }
                    catch (Exception ex)
                    {
                return Json("Failed");
                        
                    }
                }
                _context.SaveChanges();
                return Json("Done");
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }
        [HttpGet]
        public JsonResult GetClassRoom(int id)
        {
            var ClassRoom = new ClassRoom();

            if (id != 0)
            {
                ClassRoom = _context.ClassRooms.Find(id);

                return Json(ClassRoom);
            }
            else
            {
                return Json(ClassRoom);
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClassRoom = await _context.ClassRooms
                .FirstOrDefaultAsync(m => m.id == id);
            if (ClassRoom == null)
            {
                return NotFound();
            }

            return View(ClassRoom);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ClassRoom = await _context.ClassRooms.FindAsync(id);
            var ClassRoomImage = _context.ClassRoomImages.Where(x => x.classRoomId == id).ToList();
            if (ClassRoomImage != null)
            {
                foreach (var item in ClassRoomImage)
                {
                    string deletePath = environment.WebRootPath + item.url;

                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }

            }
            var factor = _context.Factors.Where(x => x.classRoomId == id);
             var reserves = _context.Reserves.Where(x => x.classroomId == id);
            _context.Reserves.RemoveRange(reserves);
            _context.Factors.RemoveRange(factor);
            _context.ClassRooms.Remove(ClassRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ClassRoom = await _context.ClassRooms.Include(x => x.user).Include(x=>x.ClassRoomImages).FirstOrDefaultAsync(x => x.id == id);
            ClassRoomViewModelAdmin ClassRoomViewModelAdmin = new ClassRoomViewModelAdmin();
            ClassRoomViewModelAdmin.ClassRoom = ClassRoom;
            ClassRoomViewModelAdmin.ClassRoomTypes = _context.ClassRoomTypes.ToList();
            if (ClassRoom == null)
            {
                return NotFound();
            }
            return View(ClassRoomViewModelAdmin);
        }

         public async Task<IActionResult> Details(int id)
        {
            
            ClassRoomViewModelAdmin ClassRoomViewModelAdmin =   _ClassRoom.GetForDetailPage(id);
            return View(ClassRoomViewModelAdmin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassRoomViewModelAdmin ClassRoomViewModelAdmin, IFormFile[] file)
        {
            if (ModelState.IsValid)
            {
                try
                {
            _ClassRoom.CreateOrUpdateFromAdmin(ClassRoomViewModelAdmin, file);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Edit", new { id = ClassRoomViewModelAdmin.ClassRoom.id });

        }
        public IActionResult Create()
        {
            ClassRoomViewModelAdmin ClassRoomViewModelAdmin = new ClassRoomViewModelAdmin();
            ClassRoomViewModelAdmin.ClassRoomTypes = _context.ClassRoomTypes.ToList();
            ClassRoomViewModelAdmin.Users = _context.Users.Include(x=>x.role).Where(x => x.userStatus == true  && (x.role.RoleNameEn=="Academy"||x.role.RoleNameEn=="Mentor")).ToList();
            return View(ClassRoomViewModelAdmin);
        }
        
    }
}