using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FileTransferMonitor.Models;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using FileTransferMonitor.App_View;

namespace FileTransferMonitor.Controllers
{
    public class FilesDownloadNotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FilesDownloadNotes
        public async Task<ActionResult> Index()
        {
            var filesDownloadNotes = db.FilesDownloadNotes.Include(f => f.ApplicationUser).Include(f => f.FilesUploadNote);
            return View(await filesDownloadNotes.ToListAsync());
        }
        public ViewResult IndexGroup()
        {
            IQueryable<IGrouping<string, FilesDownloadNote>> groupByName = db.FilesDownloadNotes.Include(f => f.ApplicationUser)
                                    .Include(f => f.FilesUploadNote).GroupBy(g => g.ApplicationUser.UserName);
            return View(groupByName);
        }

        public async Task<FileResult> CreateSendExcellFile()
        {
            List<Report> report = await db.FilesDownloadNotes.Include(f => f.ApplicationUser).Include(f => f.FilesUploadNote)
                                         .Select(x => new Report
                                         {
                                             FileName = x.FilesUploadNote.Name,
                                             FileFileSize = x.FilesUploadNote.Size,
                                             FileType = x.FilesUploadNote.Type,
                                             DataUploaded = x.FilesUploadNote.DataTimeUpload,
                                             WhoUploaded = x.FilesUploadNote.UserName,
                                             TypeOperations = x.FileOperations,
                                             DataOperations = x.DateTimeDownload,
                                             WhoOperations = x.ApplicationUser.UserName
                                         }).ToListAsync();
 
            GridView grid = new GridView();
            grid.DataSource = report;
            grid.DataBind();
            StringWriter sw = new StringWriter(); 
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            string r_name = "Report_" + DateTime.Now.ToString() + "_.xls";

            return File(Encoding.UTF8.GetBytes(sw.ToString()), "application/ms-excel", r_name);
        }
        // GET: FilesDownloadNotes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesDownloadNote filesDownloadNote = await db.FilesDownloadNotes.FindAsync(id);
            if (filesDownloadNote == null)
            {
                return HttpNotFound();
            }
            return View(filesDownloadNote);
        }

        // GET: FilesDownloadNotes/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.FilesUploadNoteId = new SelectList(db.FilesUploadNotes, "Id", "Name");
            return View();
        }

        // POST: FilesDownloadNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DateTimeDownload,FileOperations,ApplicationUserId,FilesUploadNoteId")] FilesDownloadNote filesDownloadNote)
        {
            if (ModelState.IsValid)
            {
                db.FilesDownloadNotes.Add(filesDownloadNote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", filesDownloadNote.ApplicationUserId);
            ViewBag.FilesUploadNoteId = new SelectList(db.FilesUploadNotes, "Id", "Name", filesDownloadNote.FilesUploadNoteId);
            return View(filesDownloadNote);
        }

        // GET: FilesDownloadNotes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesDownloadNote filesDownloadNote = await db.FilesDownloadNotes.FindAsync(id);
            if (filesDownloadNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", filesDownloadNote.ApplicationUserId);
            ViewBag.FilesUploadNoteId = new SelectList(db.FilesUploadNotes, "Id", "Name", filesDownloadNote.FilesUploadNoteId);
            return View(filesDownloadNote);
        }

        // POST: FilesDownloadNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DateTimeDownload,FileOperations,ApplicationUserId,FilesUploadNoteId")] FilesDownloadNote filesDownloadNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filesDownloadNote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", filesDownloadNote.ApplicationUserId);
            ViewBag.FilesUploadNoteId = new SelectList(db.FilesUploadNotes, "Id", "Name", filesDownloadNote.FilesUploadNoteId);
            return View(filesDownloadNote);
        }

        // GET: FilesDownloadNotes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilesDownloadNote filesDownloadNote = await db.FilesDownloadNotes.FindAsync(id);
            if (filesDownloadNote == null)
            {
                return HttpNotFound();
            }
            return View(filesDownloadNote);
        }

        // POST: FilesDownloadNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            FilesDownloadNote filesDownloadNote = await db.FilesDownloadNotes.FindAsync(id);
            db.FilesDownloadNotes.Remove(filesDownloadNote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
