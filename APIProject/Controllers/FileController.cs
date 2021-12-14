using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using APIProject.App_Start;
using Data.DB;

namespace APIProject.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthenticationFilter]
        public ActionResult UploadFiles()
        {
            string FileName = "";
            string filename = "";
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";    

                HttpPostedFileBase file = files[i];

                string fname;

                // Checking for Internet Explorer    
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                    FileName = file.FileName;

                }
                var timestamp = DateTime.Now.ToFileTime();
                string ext = "files";
                if (Path.GetExtension(fname).Equals("jpg") || Path.GetExtension(fname).Equals("png") || Path.GetExtension(fname).Equals("bmp"))
                {
                    ext = "images";
                }
                fname =  timestamp.ToString()+Path.GetExtension(fname);
                 filename = "/Uploads/"+ext+"/"+ fname ;
                // Get the complete folder path and store the file inside it.    
                fname = Server.MapPath("/Uploads/"+ext+"/")+ fname;
                file.SaveAs(fname);
            }
            return Json(new { FileName = FileName, Path = filename }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList(int id)
        {
            try
            {
                var data = (from u in Context.GroupMedias
                           where u.IsActive.Equals(1) && u.GroupID.Equals(id) && u.IsReject.Value.Equals(0)
                           select new
                           {
                               u.path,
                               u.Type
                           }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

    }
}