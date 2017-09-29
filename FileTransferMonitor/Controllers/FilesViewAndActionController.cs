using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FileTransferMonitor.Models;
using FileTransferMonitor.App_View;
using System.IO;
using Microsoft.AspNet.Identity;

namespace FileTransferMonitor.Controllers
{
    //[ValidateAntiForgeryToken]
    public class FilesViewAndActionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: FilesViewAndAction
        public async Task<ActionResult> Index(int PageNumber = 1, int PageSize = 10)
        {
            PageInfo pageInfo = new PageInfo{
                PageNumber = PageNumber, PageSize = PageSize,
                TotalItems = await db.FilesUploadNotes.CountAsync()};
            IEnumerable<FilesUploadNote> listItem =
                await  db.FilesUploadNotes.OrderBy(i=>i.Id)
                         .Skip((PageNumber - 1) * PageSize)
                         .Take(PageSize).ToListAsync();
             VMUploadFile vmup = new VMUploadFile
                { FilesUploadNote = listItem, pageInfo = pageInfo };
            return View(vmup);
        }
        // GET: FilesViewAndAction/Details/5
        public async Task<JsonResult> Details(long? id)
        {
            if (id == null)
            {
                return Json(new { result = "nul" }, 0);
            }
            //FilesUploadNote filesUpload = db.FilesDownloadNotes.Include(f => f.FilesUploadNote).FirstOrDefault(x => x.FilesUploadNote.Id == id).FilesUploadNote;
            FilesUploadNote fun = await db.FilesUploadNotes.FindAsync(id);
            if ( fun == null )
            {
                return Json(new { result = "null" }, 0);
            }
            return Json(new { Name = fun.Name, Type = fun.Type, Size = fun.Size,
                              DataTimeUpload = fun.DataTimeUpload, UserName = fun.UserName }, 0);
         }
        // GET: FilesViewAndAction/Create
        public async Task<ActionResult> Edit (long? id, string moniker)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { response = "No User Identities" }, 0);
            if (null == id || null == moniker)
                return Json(new { response = "Server error" }, 0);
 
            FilesUploadNote fun = await db.FilesUploadNotes.FirstOrDefaultAsync(x => x.Id == id);
            if(null==fun)
                return Json(new { response = "File not found" }, 0);

            string path = Request.PhysicalApplicationPath + "BoxFiles\\";
            FileInfo fi = new FileInfo(path + fun.Name);
            if(!fi.Exists)
                return Json(new { response = "File not found" }, 0);
            
            try
            {
                string ff_name = path + moniker + fi.Extension;
                fi.MoveTo(ff_name);
                fun.Path = ff_name;
                fun.Name = moniker + fi.Extension;
                db.Entry(fun).State = EntityState.Modified;

                db.FilesDownloadNotes.Add(new FilesDownloadNote { DateTimeDownload = DateTime.Now,
                                                                  ApplicationUserId = User.Identity.GetUserId(),
                                                                  FilesUploadNoteId = fun.Id,
                                                                  FileOperations = "Rename"});
                await db.SaveChangesAsync();
                return Json(new { response = moniker + fi.Extension }, 0);
            }
            catch
            {
                return Json(new { response = " Bad Request" }, 0);
            }
            
        }

        public JsonResult Preload(long? id, string moniker)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { answer = "Registration required" }, JsonRequestBehavior.AllowGet);

            if (id == null)
                return Json(new { answer = "Request value - null" }, JsonRequestBehavior.AllowGet);

            string path = Request.PhysicalApplicationPath + "BoxFiles\\" + moniker;
            if (!System.IO.File.Exists(path))
                return Json(new { answer = "File not found" }, JsonRequestBehavior.AllowGet);

            return Json(new { answer = "Request handled" }, JsonRequestBehavior.AllowGet);
        } 
        public async Task<FileResult> Download(long? id)
        {
            FilesUploadNote filesUploadNote = await db.FilesUploadNotes.FirstOrDefaultAsync(x => x.Id == id);
                string path = Request.PhysicalApplicationPath + "BoxFiles\\" + filesUploadNote.Name;
                db.FilesDownloadNotes.Add(new FilesDownloadNote()
                {
                    ApplicationUserId = User.Identity.GetUserId(),
                    FilesUploadNoteId = filesUploadNote.Id,
                    DateTimeDownload = DateTime.Now,
                    FileOperations = "Download"
                });
                await db.SaveChangesAsync();
                //return Json(new { answer = "Request handled" }, JsonRequestBehavior.AllowGet);
                return File(path, filesUploadNote.Type, filesUploadNote.Name);
        }
        public async Task<JsonResult> Delete(long? id)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { result = "Registration required" }, 0);
            if (id == null)
                return Json(new { result = "Request value null" }, 0);
 
            FilesUploadNote filesUploadNote = await db.FilesUploadNotes.FindAsync(id);
                if (null == filesUploadNote) return Json(new { result = "The file was deleted" }, 0);
            FileInfo fi = new FileInfo(Request.PhysicalApplicationPath + "BoxFiles\\" + filesUploadNote.Name);
                if (!fi.Exists)return Json(new { result = "File not found" }, 0);
                    
            long index = filesUploadNote.Id;
            db.FilesUploadNotes.Remove(filesUploadNote);
            await db.SaveChangesAsync();
            fi.Delete();
                 return Json(new { result = index}, 0);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
