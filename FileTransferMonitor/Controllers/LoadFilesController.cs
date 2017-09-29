//using FileTransferMonitor.GeneratorCodeDB;
using FileTransferMonitor.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileTransferMonitor.Controllers
{
    public class LoadFilesController : Controller
    {
        static string DirSaveFile = string.Empty;
        ApplicationDbContext dBc = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FilesToServer(string Url)
        {
            DirSaveFile = Request.PhysicalApplicationPath + "BoxFiles\\";
            if (!User.Identity.IsAuthenticated)
                ViewBag.Info = "Please register";
            return View();
        }
        [HttpPost]
        public ActionResult FilesToServer()
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { Message = "Requare registration!" });
            //return Redirect("Account/Login");
            string file_name = "";

            HttpPostedFileBase pfb = (Request.Files)[0];
            string path = DirSaveFile + pfb.FileName;
            if (null != dBc.FilesUploadNotes.FirstOrDefault(e => e.Name == pfb.FileName))
                return Json(new { Message = "The file already exists" });
            try
            {  
                dBc.FilesUploadNotes.Add(new FilesUploadNote
                {
                    UserName = User.Identity.Name,
                    Name = pfb.FileName,
                    Size = ((decimal)pfb.ContentLength) / 1024000,
                    Type = pfb.ContentType,
                    DataTimeUpload = DateTime.Now,
                    Path = path
                });
                pfb.SaveAs(path);
                dBc.SaveChanges();
                file_name = pfb.FileName;
            }
            catch (FileLoadException x)
            {
                return Json(new { Message = x.Message}); 
            }
            return Json(new { Message = "ok" });
        }
    }
}