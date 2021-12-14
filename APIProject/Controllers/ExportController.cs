using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Word;
using APIProject.Models;
using System.IO;
using APIProject.Util;
using Data.Utils;
using Data.Model.APIWeb;
using OfficeOpenXml;
using System.ComponentModel;
using Data.DB;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace APIProject.Controllers
{
    public class ExportController : BaseController
    {
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Convert(int C)
        {
            var listRecordID = (from g in Context.GroupRecords
                                join r in Context.Records on g.RecordID equals r.ID
                                where g.GroupID.Equals(C) && g.IsActive.Equals(SystemParam.ACTIVE) && r.IsActive.Equals(SystemParam.ACTIVE)
                                select r).ToList();
            List<string> str = new List<string>();
            foreach(var r in listRecordID)
            {
                string s = "Nguyên quán: " + r.Address + ", " + (r.ProvinceID.HasValue ? r.Province.Name : "");
                s = Data.Utils.Util.ConvertsExportFile(s);
                str.Add(s);
            }
            return Json(new { code = 200, data = str }, JsonRequestBehavior.AllowGet);
        }
        public FileResult exportRecordByGroupIDNew(int GroupID)
        {

            var listRecordID = (from g in Context.GroupRecords
                                join r in Context.Records on g.RecordID equals r.ID
                                where g.GroupID.Equals(GroupID) && g.IsActive.Equals(SystemParam.ACTIVE) && r.IsActive.Equals(SystemParam.ACTIVE)
                                select r).ToList();
            //thuc hien cat tat ca
            List<PaperModel> listItem = new List<PaperModel>();
            foreach (var r in listRecordID)
            {
                PaperModel item = new PaperModel();
                //Lay trong DB
                //thu hien fill vao item
                string s = !String.IsNullOrEmpty(r.MartyrsName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(r.MartyrsName.ToLower()) : "";
                item.name = s;
                item.des1 = r.PositionID.HasValue ? (r.Position.IsArmy == 1 ? (r.Position.Name + " Quân đội nhân dân Việt Nam") : (r.Position.IsGuerrilla == 1 ? (r.Position.Name + " du kích"):r.Position.Name)) : "";
                item.des2 = "Nguyên quán: " + r.Address + ", " + (r.ProvinceID.HasValue ? r.Province.Name : "");
                item.des3 = r.PeriodID.HasValue ? r.Period.PrintingContent : "";
                item.num = r.DecitionNumber.HasValue?r.DecitionNumber.Value.ToString():"";
                item.symbol = r.DecitionCodeID.HasValue ? r.DecisionCode.Code : "";
                item.dd = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Day.ToString() : "";
                item.mm = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Month.ToString() : "";
                item.yyyy = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Year.ToString() : "";
                item.dd2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Day.ToString() : "";
                item.mm2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Month.ToString() : "";
                item.yyyy2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Year.ToString() : "";
                item.dd3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Day.ToString() : "";
                item.mm3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Month.ToString() : "";
                item.yyyy3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Year.ToString() : "";
                string code = r.ObjectID.HasValue?r.Object.Code:"" ;
                string cpm = r.PeriodID.HasValue ? r.Period.Code : "";
                item.bt = code+cpm;
                item.id = (!String.IsNullOrEmpty(r.CerfiticateCode)?r.CerfiticateCode:"")+ " " + (r.CerfiticateNumber.HasValue?r.CerfiticateNumber.Value.ToString():"");
                item.position = "THỦ TƯỚNG";
                item.signed = "Nguyễn Xuân Phúc";
                listItem.Add(item);
            }
            return ExportBangKhen(listItem,1);
        }
        public FileResult exportRecordByGroupIDReNew(int GroupID)
        {

            var listRecordID = (from g in Context.GroupRecords
                                join r in Context.Records on g.RecordID equals r.ID
                                where g.GroupID.Equals(GroupID) && g.IsActive.Equals(SystemParam.ACTIVE) && r.IsActive.Equals(SystemParam.ACTIVE)
                                select r).ToList();
            //thuc hien cat tat ca
            List<PaperModel> listItem = new List<PaperModel>();
            Group group = Context.Groups.Find(GroupID);
            foreach (var r in listRecordID)
            {
                PaperModel item = new PaperModel();
                //Lay trong DB
                //thu hien fill vao item
                
                string s = !String.IsNullOrEmpty(r.MartyrsName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(r.MartyrsName) : "";
                item.name = s;
                item.des1 = r.PositionID.HasValue ? (r.Position.IsArmy == 1 ? (r.Position.Name + " Quân đội nhân dân Việt Nam") : (r.Position.IsGuerrilla == 1 ? (r.Position.Name + " du kích") : r.Position.Name)) : "";
                item.des2 = "Nguyên quán: " + r.Address + ", " + (r.ProvinceID.HasValue ? r.Province.Name : "");
                item.des3 = r.PeriodID.HasValue ? r.Period.PrintingContent : "";
                item.num = r.DecitionNumber.HasValue ? r.DecitionNumber.Value.ToString() : "";
                item.symbol = r.DecitionCodeID.HasValue ? r.DecisionCode.Code : "";
                item.dd = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Day.ToString() : "";
                item.mm = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Month.ToString() : "";
                item.yyyy = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Year.ToString() : "";
                item.dd2 = group.DecisionDate.HasValue ? group.DecisionDate.Value.Day.ToString() : "";
                item.mm2 = group.DecisionDate.HasValue ? group.DecisionDate.Value.Month.ToString() : "";
                item.yyyy2 = group.DecisionDate.HasValue ? group.DecisionDate.Value.Year.ToString() : "";
                item.dd3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Day.ToString() : "";
                item.mm3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Month.ToString() : "";
                item.yyyy3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Year.ToString() : "";
                string code = r.ObjectID.HasValue ? r.Object.Code : "";
                string cpm = r.PeriodID.HasValue ? r.Period.Code : "";
                item.bt = code + cpm;
                item.id = (!String.IsNullOrEmpty(r.CerfiticateCode) ? r.CerfiticateCode : "") + " " + (r.CerfiticateNumber.HasValue ? r.CerfiticateNumber.Value.ToString() : ""); ;
                item.position = "THỦ TƯỚNG";
                item.signed = "Nguyễn Xuân Phúc";
                listItem.Add(item);
            }
            return ExportBangKhen(listItem,2);
        }

        public FileResult ExportRecord(string strListID)
        {
            //thuc hien cat tat ca
            string[] listStr = strListID.Split(',');
            List<PaperModel> listItem = new List<PaperModel>();
            foreach (var str in listStr)
            {
                int RID = 0;
                if (int.TryParse(str, out RID))
                {
                    PaperModel item = new PaperModel();
                    //Lay trong DB
                    Record r = Context.Records.Find(RID);
                    //thu hien fill vao item
                    item.name = r.MartyrsName;
                    item.des1 = r.Position.Name;
                    item.des2 = "Nguyên quán: " + r.Village.Name + ", " + r.District.Name + ", " + r.Province.Name;
                    item.des3 = r.Period.PrintingContent;
                    item.num = r.CerfiticateNumber.ToString();
                    item.symbol = r.Period.Code.ToString();
                    item.dd = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Day.ToString() : "";
                    item.mm = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Month.ToString() : "";
                    item.yyyy = r.SacrificeDate.HasValue ? r.SacrificeDate.Value.Year.ToString() : "";
                    item.dd2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Day.ToString() : "";
                    item.mm2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Month.ToString() : "";
                    item.yyyy2 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Year.ToString() : "";
                    item.dd3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Day.ToString() : "";
                    item.mm3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Month.ToString() : "";
                    item.yyyy3 = r.DecitionDate.HasValue ? r.DecitionDate.Value.Year.ToString() : "";
                    item.bt = r.Period.Code.ToString();
                    item.id = r.CerfiticateCode + r.CerfiticateNumber;
                    item.position = "THỦ TƯỚNG";
                    item.signed = "Nguyễn Xuân Phúc";
                    listItem.Add(item);
                }
            }
            return ExportBangKhen(listItem,2);
        }

        public FileResult ExportBangKhen(List<PaperModel> listItem, int type)
        {
            

            string file = "";
            string path = "";
            Application app = new Application();
            Document doc = new Microsoft.Office.Interop.Word.Document();
            Document docMain = new Microsoft.Office.Interop.Word.Document();
            docMain.Paragraphs.SpaceAfter = 0;
            //Microsoft.Office.Interop.Word.Paragraph paragraph = docMain.Content.Paragraphs.Add(System.Reflection.Missing.Value);
            object break_type = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            Microsoft.Office.Interop.Word.Document docTemplate = new Microsoft.Office.Interop.Word.Document();

            object unknow = System.Type.Missing;
            object nothing = System.Reflection.Missing.Value;
            string folderPaper = Server.MapPath("~/Downloads/");
            path = "file" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".docx";
            file = folderPaper + path;

            if (!Directory.Exists(folderPaper))
            {
                Directory.CreateDirectory(folderPaper);
            }
            string templatePath = Server.MapPath("/Template/template_bang.docx");
            // copy teamlaple 
            docTemplate = app.Documents.Open(templatePath, System.Reflection.Missing.Value, true);
            //docMain.PageSetup = docTemplate.PageSetup;

            app.Selection.Find.ClearFormatting();
            app.Selection.Find.Replacement.ClearFormatting();

            //Thuc hien tao tung bang 
            Export cv = new Export();
            ConvertFont convertFont = new ConvertFont();
            foreach (var it in listItem)
            {
                try
                {
                    //xoa het
                    if (doc.Content.End > 1)
                    {
                        doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();
                    }
                    docTemplate.StoryRanges[WdStoryType.wdMainTextStory].Copy();
                    doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();

                    ////copy template
                    //doc = CopyToNewDocument(docTemplate);
                    //docMain.Paragraphs.PageBreakBefore = doc.Paragraphs.PageBreakBefore;
                    // ngày băt đầu nhập học và ngày kết thúc học tập 
                    //fill du lieu
                    if (doc.Bookmarks.Exists("Name"))
                    {
                        
                            string name = Data.Utils.Util.ConvertsExportFile(it.name);
                            convertFont.Convert(ref name, FontIndex.iUNI, FontIndex.iTCV);
                            doc.Bookmarks["Name"].Range.Text = name;
                       
                    }

                    if (doc.Bookmarks.Exists("Des1"))
                    {
                        string str = Data.Utils.Util.ConvertsExportFile(it.des1);
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des1"].Range.Text = str;
                    }

                    if (doc.Bookmarks.Exists("Des2"))
                    {
                        string str = Data.Utils.Util.ConvertsExportFile(it.des2);
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des2"].Range.Text = str + "";
                    }

                    if (doc.Bookmarks.Exists("Des3"))
                    {
                        string str = Data.Utils.Util.ConvertsExportFile(it.des3);
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des3"].Range.Text = str + "";
                    }

                    if (doc.Bookmarks.Exists("dd"))
                    {
                        doc.Bookmarks["dd"].Range.Text = it.dd + "";
                    }

                    if (doc.Bookmarks.Exists("mm"))
                    {
                        doc.Bookmarks["mm"].Range.Text = it.mm + "";
                    }

                    if (doc.Bookmarks.Exists("yyyy"))
                    {
                        doc.Bookmarks["yyyy"].Range.Text = it.yyyy + "";
                    }


                    if (doc.Bookmarks.Exists("num"))
                    {
                        doc.Bookmarks["num"].Range.Text = it.num + "";
                    }

                    if (doc.Bookmarks.Exists("symbol"))
                    {
                        doc.Bookmarks["symbol"].Range.Text = it.symbol + "";
                    }
                    if (doc.Bookmarks.Exists("dd3"))
                    {
                        doc.Bookmarks["dd3"].Range.Text = it.dd3 + "";
                    }
                    if (doc.Bookmarks.Exists("mm3"))
                    {
                        doc.Bookmarks["mm3"].Range.Text = it.mm3 + "";
                    }
                    if (doc.Bookmarks.Exists("yyyy3"))
                    {
                        doc.Bookmarks["yyyy3"].Range.Text = it.yyyy3 + "";
                    }
                    if (doc.Bookmarks.Exists("bt"))
                    {
                        doc.Bookmarks["bt"].Range.Text = it.bt;
                    }
                    if (doc.Bookmarks.Exists("cer"))
                    {
                        doc.Bookmarks["cer"].Range.Text = it.id + "";
                    }

                    if (doc.Bookmarks.Exists("dd2"))
                    {
                        doc.Bookmarks["dd2"].Range.Text = it.dd2 + "";
                    }
                    if (doc.Bookmarks.Exists("mm2"))
                    {
                        doc.Bookmarks["mm2"].Range.Text = it.mm2 + "";
                    }
                    if (doc.Bookmarks.Exists("yyyy2"))
                    {
                        doc.Bookmarks["yyyy2"].Range.Text = it.yyyy2 + "";
                    }
                    if (doc.Bookmarks.Exists("position"))
                    {
                        doc.Bookmarks["position"].Range.Text = it.position + "";
                    }
                    if (doc.Bookmarks.Exists("signed"))
                    {
                        doc.Bookmarks["signed"].Range.Text = it.signed + "";
                    }
                    if (doc.Bookmarks.Exists("cl"))
                    {
                        if(type == 1)
                        {
                            doc.Bookmarks["cl"].Range.Text = "";
                        }
                        else
                        {
                            doc.Bookmarks["cl"].Range.Text = "/CL";
                        }
                    }
                    if (doc.Bookmarks.Exists("css"))
                    {
                        doc.Bookmarks["css"].Range.Text = " ";
                    }

                    //copy main 

                    doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();
                    //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                    docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                    docMain.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                    //paragraph.Range.InsertParagraphAfter();
                    //paragraph.Range.InsertBreak(ref break_type);
                    //docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).InsertBreak();

                }
                catch (Exception ex)
                {
                    doc.Close(false, ref unknow, ref unknow);
                    docMain.Close(false, ref unknow, ref unknow);
                    docTemplate.Close(false, ref unknow, ref unknow);
                    app.Quit();

                    return null;
                }

            }

            try
            {
                docMain.PageSetup = docTemplate.PageSetup;
                docMain.SaveAs2(file);
                //docMain.ExportAsFixedFormat(file, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, false, Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 1, 1, Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, false, false, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, false, false, false, nothing);
                doc.Close(false, ref unknow, ref unknow);
                docMain.Close(false, ref unknow, ref unknow);
                docTemplate.Close(false, ref unknow, ref unknow);
                doc = null;
            }
            catch
            {
                doc.Close(false, ref unknow, ref unknow);
                docMain.Close(false, ref unknow, ref unknow);
                docTemplate.Close(false, ref unknow, ref unknow);
                doc = null;
                app.Quit();
                return null;
            }
            finally
            {
                app.Quit();
            }






            // docMain.Close(true, ref unknow, ref unknow);
            //app.Quit();
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return File(fs, "application/vnd.ms-word", "BangKhen.docx");

        }

        public FileResult Export_BangKhen()
        {
            //Nap du lieu mau

            List<PaperModel> listItem = new List<PaperModel>();

            PaperModel item = new PaperModel();
            item.bt = "bt";
            item.dd = "13";
            item.mm = "10";
            item.yyyy = "2020";
            item.name = "Nguyễn Cảnh Cường";
            item.des1 = "Thiếu tá Quân đội nhân dân Việt Nam";
            item.des2 = "Nguyên quán: xã Cao Sơn, huyện Anh Sơn, tỉnh Nghệ An";
            item.des3 = "Đã hy sinh vì sự nghiệp xây dựng và bảo vệ Tổ quốc";
            item.num = "1594";
            item.symbol = "QĐ-TTg";
            item.dd2 = "16";
            item.yyyy2 = "10";
            item.yyyy2 = "2020";
            item.id = "HPD171";
            item.dd3 = "16";
            item.mm3 = "10";
            item.yyyy3 = "2020";
            item.position = "THỦ TƯỚNG";
            item.signed = "Nguyễn Xuân Phúc";
            listItem.Add(item);

            PaperModel item2 = new PaperModel();
            item2.bt = "bt";
            item2.dd = "01";
            item2.mm = "01";
            item2.yyyy = "01";
            item2.name = "Ôn Như Bình";
            item2.des1 = "Tha ta quan a nha dan Via Nam";
            item2.des2 = "Nguyan qua: Xa Cao San Huya Anh San Ta Ngha An";
            listItem.Add(item2);

            string file = "";

            Application app = new Application();
            Document doc = new Microsoft.Office.Interop.Word.Document();
            Document docMain = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Document docTemplate = new Microsoft.Office.Interop.Word.Document();

            object unknow = System.Type.Missing;

            string folderPaper = Server.MapPath("~/Downloads/");
            file = folderPaper + "file" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".docx";

            if (!Directory.Exists(folderPaper))
            {
                Directory.CreateDirectory(folderPaper);
            }
            string templatePath = Server.MapPath("/Template/template_bang.docx");
            // copy teamlaple 
            docTemplate = app.Documents.Open(templatePath, System.Reflection.Missing.Value, true);
            //docMain.PageSetup = docTemplate.PageSetup;

            app.Selection.Find.ClearFormatting();
            app.Selection.Find.Replacement.ClearFormatting();

            //Thuc hien tao tung bang 
            Export cv = new Export();
            ConvertFont convertFont = new ConvertFont();
            foreach (var it in listItem)
            {
                try
                {
                    //xoa het
                    if (doc.Content.End > 1)
                    {
                        doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();

                    }

                    docTemplate.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                    doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();

                    ////copy template
                    //doc = CopyToNewDocument(docTemplate);

                    // ngày băt đầu nhập học và ngày kết thúc học tập 
                    //fill du lieu
                    if (doc.Bookmarks.Exists("Name"))
                    {
                        string name = it.name;
                        convertFont.Convert(ref name, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Name"].Range.Text = name;
                    }

                    if (doc.Bookmarks.Exists("Des1"))
                    {
                        string str = it.des1;
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des1"].Range.Text = str;
                    }

                    if (doc.Bookmarks.Exists("Des2"))
                    {
                        string str = it.des2;
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des2"].Range.Text = str + "";
                    }

                    if (doc.Bookmarks.Exists("Des3"))
                    {
                        string str = it.des3;
                        convertFont.Convert(ref str, FontIndex.iUNI, FontIndex.iTCV);
                        doc.Bookmarks["Des3"].Range.Text = str + "";
                    }

                    if (doc.Bookmarks.Exists("dd"))
                    {
                        doc.Bookmarks["dd"].Range.Text = it.dd + "";
                    }

                    if (doc.Bookmarks.Exists("mm"))
                    {
                        doc.Bookmarks["mm"].Range.Text = it.mm + "";
                    }

                    if (doc.Bookmarks.Exists("yyyy"))
                    {
                        doc.Bookmarks["yyyy"].Range.Text = it.yyyy + "";
                    }


                    if (doc.Bookmarks.Exists("num"))
                    {
                        doc.Bookmarks["num"].Range.Text = it.num + "";
                    }

                    if (doc.Bookmarks.Exists("symbol"))
                    {
                        doc.Bookmarks["symbol"].Range.Text = it.symbol + "";
                    }
                    if (doc.Bookmarks.Exists("dd3"))
                    {
                        doc.Bookmarks["dd3"].Range.Text = it.dd3 + "";
                    }
                    if (doc.Bookmarks.Exists("mm3"))
                    {
                        doc.Bookmarks["mm3"].Range.Text = it.mm3 + "";
                    }
                    if (doc.Bookmarks.Exists("yyyy3"))
                    {
                        doc.Bookmarks["yyyy3"].Range.Text = it.yyyy3 + "";
                    }
                    if (doc.Bookmarks.Exists("bt"))
                    {
                        doc.Bookmarks["bt"].Range.Text = it.bt + "";
                    }
                    if (doc.Bookmarks.Exists("id"))
                    {
                        doc.Bookmarks["id"].Range.Text = it.id + "";
                    }

                    if (doc.Bookmarks.Exists("dd2"))
                    {
                        doc.Bookmarks["dd2"].Range.Text = it.dd2 + "";
                    }
                    if (doc.Bookmarks.Exists("mm2"))
                    {
                        doc.Bookmarks["mm2"].Range.Text = it.mm2 + "";
                    }
                    if (doc.Bookmarks.Exists("yyyy2"))
                    {
                        doc.Bookmarks["yyyy2"].Range.Text = it.yyyy2 + "";
                    }
                    if (doc.Bookmarks.Exists("position"))
                    {
                        doc.Bookmarks["position"].Range.Text = it.position + "";
                    }
                    if (doc.Bookmarks.Exists("signed"))
                    {
                        doc.Bookmarks["signed"].Range.Text = it.signed + "";
                    }


                    //copy main 

                    doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                    //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                    docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                    //docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).InsertBreak();

                }
                catch (Exception ex)
                {
                    doc.Close(false, ref unknow, ref unknow);
                    docMain.Close(false, ref unknow, ref unknow);

                    docTemplate.Close(false, ref unknow, ref unknow);
                    app.Quit();

                    return null;
                }


            }

            try
            {
                docMain.PageSetup = docTemplate.PageSetup;

            }
            catch
            {
            }

            docMain.SaveAs2(file);


            doc.Close(false, ref unknow, ref unknow);
            docMain.Close(false, ref unknow, ref unknow);
            docTemplate.Close(false, ref unknow, ref unknow);
            doc = null;

            // docMain.Close(true, ref unknow, ref unknow);
            app.Quit();
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return File(fs, "application/vnd.ms-word", "BangKhen.docx");

        }

        public object ExportGroupNewprofile(string Str, int GroupId)
        {
            //xu li du lieu lay vao 
            string[] grID = Str.Split(',');

            List<int> it = new List<int>();
            for (int i = 0; i < grID.Length - 1; i++)
            {
                it.Add(Int32.Parse(grID[i]));
            }
            //lay danh sach tat ca cac ban ghi
            var data = (from v in Context.Records
                        join gr in Context.GroupRecords on v.ID equals gr.RecordID
                        where (v.Status.Equals(SystemParam.STATUS_ACCEPTED_RECORD) || v.Status.Equals(SystemParam.STATUS_PENDING_RECORD) || v.Status.Equals(SystemParam.STATUS_WAIT_PRESIDENT))
                         && (v.IsActive == SystemParam.ACTIVE)
                        orderby v.CerfiticateNumber ascending
                        select new NewProfileModel
                        {
                            ID = v.ID,
                            MartyrsName = v.MartyrsName,
                            DecitionCodeID = v.DecitionCodeID.Value,
                            codeDecisionCode = v.DecisionCode.Code,
                            CerfiticateCode = v.CerfiticateCode,
                            CerfiticateNumber = v.CerfiticateNumber.Value,
                            CreatedDate = v.CreatedDate,
                            DecitionNumber = v.DecitionNumber,
                            DistrictID = v.DistrictID.Value,
                            Number = v.Number,
                            NamePosition = v.Position.Name,
                            PositionID = v.PositionID,
                            IsArmy = v.Position.IsArmy,
                            IsGuerrilla = v.Position.IsGuerrilla,
                            ObjectCode = v.Object.Code,
                            NameProvince = v.Province.Name,
                            sacrifice_date = v.sacrifice_date != 0 ? v.sacrifice_date : null,
                            sacrifice_month = v.sacrifice_month != 0 ? v.sacrifice_month : null,
                            sacrifice_year = v.sacrifice_year != 0 ? v.sacrifice_year : null,
                            DecitionDate = v.DecitionDate.Value,
                            PeriodCode = v.Period.Code,
                            ProvinceID = v.ProvinceID.Value,
                            GroupRecordID = gr.ID,
                            ProvinceRequestID = v.ProvinceRequestID,
                            DisPlayOrder = v.Object.DisplayOrder,
                            DisPlayOrderPeriod = v.Period.DisplayOrder,
                            PrintContent = v.Period.PrintingContent,
                            Address = v.Address
                        }).ToList();
            List<NewProfileModel> list = new List<NewProfileModel>();
            for (int i = 0; i < it.Count(); i++)
            {
                var query = data.Where(q => q.GroupRecordID.Equals(it[i])).FirstOrDefault();
                if (query == null) continue;
                list.Add(query);
            }
            string file = "";

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Document doc = new Microsoft.Office.Interop.Word.Document();
            Document docMain = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Document docTemplate = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Paragraph paragraph = docMain.Content.Paragraphs.Add(System.Reflection.Missing.Value);
            object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            object originalFormat = Type.Missing;
            object routeDocument = Type.Missing;
           
            try
            {

                string folderPaper = Server.MapPath("~/Downloads/");
                file = folderPaper + "file" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".docx";

                if (!Directory.Exists(folderPaper))
                {
                    Directory.CreateDirectory(folderPaper);
                }
                string templatePath = Server.MapPath("/Template/template_create1.docx");
                // copy teamlaple 
                docTemplate = app.Documents.Open(templatePath, System.Reflection.Missing.Value, true);
                //docMain.PageSetup = docTemplate.PageSetup;

                app.Selection.Find.ClearFormatting();
                app.Selection.Find.Replacement.ClearFormatting();

                //Thuc hien tao tung bang 
                Export cv = new Export();
                ConvertFont convertFont = new ConvertFont();
                var listProvince = provinceBusiness.GetListProvince();
                //foreach (var it in listItem)
                //{
                //Lấy thông tin của nhóm
                var group = Context.Groups.Find(GroupId);
                int dem = 1;
                paragraph.Range.Text = "DANH SÁCH LIỆT SĨ CẤP ĐỔI, CẤP LẠI BẰNG “TỔ QUỐC GHI CÔNG”";
                paragraph.Range.Font.Size = 12;
                paragraph.Range.Font.Bold = 1;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.InsertParagraph();
                // kiểm tra trường hợp null của các trường dữ liệu cho số tờ trình, ngày quyết định
                string res = 
                paragraph.Range.Text = "(kèm theo Tờ trình số " + (group.ReportNumber.HasValue?group.ReportNumber.Value.ToString():"") + " /LĐTBXH-TTr " +(group.DecisionDate.HasValue?(" ngày " + group.DecisionDate.Value.Day.ToString() + " tháng " + group.DecisionDate.Value.Month.ToString() + " năm " + group.DecisionDate.Value.Year.ToString()):"");
                paragraph.Range.Font.Size = 12;
                paragraph.Range.Font.Bold = 0;
                paragraph.Range.Font.Italic = 1;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.Text = "của Bộ Lao động-Thương binh và Xã hội)";
                paragraph.Range.Font.Size = 12;
                paragraph.Range.Font.Italic = 1;
                paragraph.Range.Font.Bold = 0;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.InsertParagraph();
                foreach (var p in listProvince)
                {
                    var kt = list.Where(q => q.ProvinceRequestID.Value.Equals(p.Code)).ToList();

                    if (kt.Count() == 0) continue;
                    paragraph.Range.Text = p.ProvinceName.ToUpper();
                    paragraph.Range.Font.Size = 11;
                    paragraph.Range.Font.Italic = 0;
                    paragraph.Range.Font.Bold = 1;
                    paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                    paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    paragraph.Range.Font.Color = get_Word_Color_RGB(0, 112, 192);
                    paragraph.Range.InsertParagraphAfter();
                    paragraph.Range.InsertParagraph();
                    if(group.Type.Value == 1)
                    {
                        if(group.CerfiticateCode != null && group.CerfiticateCode != "")
                        {
                            kt = kt.OrderBy(x => x.CerfiticateNumber.Value).ToList();
                        }
                        else
                        {
                            kt.Sort((u1, u2) =>
                            {
                                if (u2.DisPlayOrder != null && u1.DisPlayOrder != null && compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value) != 0) return compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value);
                                if (u2.DisPlayOrderPeriod != null && u1.DisPlayOrderPeriod != null && compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value) != 0) return compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value);
                                if (String.IsNullOrEmpty(u1.MartyrsName)) return -1;
                                if (String.IsNullOrEmpty(u2.MartyrsName)) return 1;
                                string[] s1 = u1.MartyrsName.Split(' '); string[] s2 = u2.MartyrsName.Split(' ');
                                int result = s1[s1.Length - 1].CompareTo(s2[s2.Length - 1]);
                                if (result != 0) return result;
                                int i = 0, j = 0;
                                while ((i < s1.Length - 1) && (j < s2.Length - 1))
                                {
                                    if (s1[i].CompareTo(s2[j]) != 0) return s1[i].CompareTo(s2[j]);
                                    i++; j++;
                                }
                                if (i == s1.Length - 2) return s2.Length;
                                if (j == s2.Length - 2) return -s1.Length;
                                return 0;
                            });
                        }

                    }
                    else
                    {
                        kt.Sort((u1, u2) =>
                        {
                            if (u2.DisPlayOrder != null && u1.DisPlayOrder != null && compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value) != 0) return compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value);
                            if (u2.DisPlayOrderPeriod != null && u1.DisPlayOrderPeriod != null && compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value) != 0) return compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value);
                            if (String.IsNullOrEmpty(u1.MartyrsName)) return -1;
                            if (String.IsNullOrEmpty(u2.MartyrsName)) return 1; 
                            string[] s1 = u1.MartyrsName.Split(' '); string[] s2 = u2.MartyrsName.Split(' ');
                            int result = s1[s1.Length - 1].CompareTo(s2[s2.Length - 1]);
                            if (result != 0) return result;
                            int i = 0, j = 0;
                            while ((i < s1.Length - 1) && (j < s2.Length - 1))
                            {
                                if (s1[i].CompareTo(s2[j]) != 0) return s1[i].CompareTo(s2[j]);
                                i++; j++;
                            }
                            if (i == s1.Length - 2) return s2.Length;
                            if (j == s2.Length - 2) return -s1.Length;
                            return 0;
                        });
                    }
                    //kt = kt.OrderBy(x => x.CerfiticateNumber.Value).ToList();
                    //kt.Sort((u1, u2) =>
                    //{
                    //    if (u2.DisPlayOrder != null && u1.DisPlayOrder != null && compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value) != 0) return compare(u1.DisPlayOrder.Value, u2.DisPlayOrder.Value);
                    //    if (u2.DisPlayOrderPeriod != null && u1.DisPlayOrderPeriod != null && compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value) != 0) return compare(u1.DisPlayOrderPeriod.Value, u2.DisPlayOrderPeriod.Value);
                    //    string[] s1 = u1.MartyrsName.Split(' '); string[] s2 = u2.MartyrsName.Split(' ');
                    //    int result = s1[s1.Length - 1].CompareTo(s2[s2.Length - 1]);
                    //    if (result != 0) return result;
                    //    int i = 0, j = 0;
                    //    while ((i < s1.Length - 1) && (j < s2.Length - 1))
                    //    {
                    //        if (s1[i].CompareTo(s2[j]) != 0) return s1[i].CompareTo(s2[j]);
                    //        i++; j++;
                    //    }
                    //    if (i == s1.Length - 2) return s2.Length;
                    //    if (j == s2.Length - 2) return -s1.Length;
                    //    return 0;
                    //});
                    foreach (var d in kt)
                    {
                        try
                        {
                            //xoa het
                            if (doc.Content.End > 1)
                            {
                                doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();

                            }

                            docTemplate.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                            doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();

                            if (doc.Bookmarks.Exists("stt"))
                            {
                                doc.Bookmarks["stt"].Range.Text = dem.ToString();

                            }
                            if (doc.Bookmarks.Exists("ln1"))
                            {
                                doc.Bookmarks["ln1"].Range.Text = !String.IsNullOrEmpty(d.MartyrsName) ? d.MartyrsName.ToUpper() : "";
                            }
                            if (doc.Bookmarks.Exists("tt1"))
                            {
                                string str = "";
                                string text = "";
                                if (!String.IsNullOrEmpty(d.PrintContent)) str = d.PrintContent;
                                text = (d.PositionID.HasValue ? d.NamePosition : "") + ", Nguyên quán: " + d.Address + " , tỉnh " + d.NameProvince + ", " + str;
                                if (d.sacrifice_date != null) text += ", ngày " + d.sacrifice_date.Value.ToString();
                                if (d.sacrifice_month != null) text += " tháng " + d.sacrifice_month.Value.ToString();
                                if (d.sacrifice_year != null) text += " năm " + d.sacrifice_year.Value.ToString();
                                doc.Bookmarks["tt1"].Range.Text = text;
                            }
                            if (doc.Bookmarks.Exists("tt2"))
                            {
                                string str1 = d.CerfiticateCode + " " + (d.CerfiticateNumber.HasValue ? d.CerfiticateNumber.Value.ToString() : "")  +"  " + d.ObjectCode + d.PeriodCode;
                                if (String.IsNullOrEmpty(str1)) str1 = "";
                                doc.Bookmarks["tt2"].Range.Text = "Số bằng: " + str1 + " ";
                            }
                            if (doc.Bookmarks.Exists("tt3"))
                            {
                                string str = "";
                                if(d.DecitionNumber != null)
                                {
                                    str = d.DecitionNumber.Value.ToString()  + "/" + d.codeDecisionCode;
                                }
                                if (d.DecitionDate == null) doc.Bookmarks["tt3"].Range.Text = "Quyết định số: "+ str;
                                else doc.Bookmarks["tt3"].Range.Text = "Quyết định số: "+ str + (d.DecitionDate.HasValue ? ( " ngày " + d.DecitionDate.Value.Day.ToString() + " tháng " + d.DecitionDate.Value.Month.ToString() + " năm " + d.DecitionDate.Value.Year.ToString()):"");

                            }
                            //copy main 
                            dem++;
                            doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                            //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                            docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                            //docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).InsertBreak();

                        }
                        catch (Exception ex)
                        {
                            doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
                            return null;
                        }
                    }
                }

                docMain.PageSetup = docTemplate.PageSetup;
                docMain.SaveAs2(file);

            }
            catch
            {
                doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
                doc = null;
                return null;
            }


            doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
            doc = null;

            // docMain.Close(true, ref unknow, ref unknow);
            //app.Quit();
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return File(fs, "application/vnd.ms-word", "File.docx");
        }
        public object ExportFileSythetic(int GroupId)
        {
            //lay danh sach tat ca cac ban ghi
            var data = (from v in Context.Records
                        join gr in Context.GroupRecords on v.ID equals gr.RecordID
                        where gr.IsActive.Equals(1) && gr.GroupID.Equals(GroupId)
                         && (v.IsActive == SystemParam.ACTIVE) 
                         orderby v.CerfiticateNumber.Value
                        select v).ToList();
            //data.Sort((u1, u2) =>
            //{
            //    // sắp xếp theo tỉnh thành
            //    Province p1 = Context.Provinces.Find(u1.ProvinceRequestID.Value); Province p2 = Context.Provinces.Find(u2.ProvinceRequestID.Value);
            //    if (p1.Name.CompareTo(p2.Name) != 0) return p1.Name.CompareTo(p2.Name);
            //    // sắp xếp theo đối tượng
            //    var o1 = u1.ObjectID; var o2 = u2.ObjectID;
            //    if (o1 != null && o2 == null) return 1;
            //    if (o1 == null && o2 != null) return -1;
            //    if (o1 != null && o2 != null)
            //    {
            //        var displayOrderO1 = u1.Object.DisplayOrder; var displayOrderO2 = u2.Object.DisplayOrder;
            //        if (displayOrderO1 > displayOrderO2) return 1;
            //        else if (displayOrderO1 < displayOrderO2) return -1;
            //    }
            //    // sắp xếp theo thời kì
            //    var pe1 = u1.PeriodID; var pe2 = u2.PeriodID;
            //    if (pe1 != null && pe2 == null) return 1;
            //    if (pe1 == null && pe2 != null) return -1;
            //    if (pe1 != null && pe2 != null)
            //    {
            //        var displayOrderPe1 = u1.Period.DisplayOrder; var displayOrderPe2 = u2.Period.DisplayOrder;
            //        if (displayOrderPe1 > displayOrderPe2) return 1;
            //        else if (displayOrderPe1 < displayOrderPe2) return -1;
            //    }
            //    //sắp xếp theo tên 
            //    string[] s1 = u1.MartyrsName.Split(' '); string[] s2 = u2.MartyrsName.Split(' ');
            //    int result = s1[s1.Length - 1].CompareTo(s2[s2.Length - 1]);
            //    if (result != 0) return result;
            //    int i = 0, j = 0;
            //    while ((i < s1.Length - 1) && (j < s2.Length - 1))
            //    {
            //        if (s1[i].CompareTo(s2[j]) != 0) return s1[i].CompareTo(s2[j]);
            //        i++; j++;
            //    }
            //    if (i == s1.Length - 2) return s2.Length;
            //    if (j == s2.Length - 2) return -s1.Length;
            //    return 0;
            //});
            var listDecisionCode = (from d in Context.DecisionCodes
                                    where d.IsActive.Equals(SystemParam.ACTIVE)
                                    select d).ToList();
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Document doc = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Document docTemplate = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Document docTemplate2 = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Document docMain = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Paragraph paragraph = docMain.Content.Paragraphs.Add(System.Reflection.Missing.Value);
            object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            object originalFormat = Type.Missing;
            object routeDocument = Type.Missing;
            string file = "";
            try
            {

                string folderPaper = Server.MapPath("~/Downloads/");
                file = folderPaper + "file" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".docx";

                if (!Directory.Exists(folderPaper))
                {
                    Directory.CreateDirectory(folderPaper);
                }
                string templatePath = Server.MapPath("/Template/template_tonghop.docx");
                string templatePath2 = Server.MapPath("/Template/template_2.docx");
                // copy teamlaple 
                docTemplate = app.Documents.Open(templatePath, System.Reflection.Missing.Value, true);
                docTemplate2 = app.Documents.Open(templatePath2, System.Reflection.Missing.Value, true);
                //docMain.PageSetup = docTemplate.PageSetup;

                app.Selection.Find.ClearFormatting();
                app.Selection.Find.Replacement.ClearFormatting();
                //app.Selection.Find.ClearFormatting();
                //app.Selection.Find.Replacement.ClearFormatting();

                var listProvince = provinceBusiness.GetListProvince();
                var group = Context.Groups.Find(GroupId);
                int dem = 1;
                paragraph.Range.Text = "TỔNG HỢP QUẢN LÝ BẰNG";
                paragraph.Range.Font.Size = 16;
                paragraph.Range.Font.Bold = 1;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.Text = "“TỔ QUỐC GHI CÔNG”";
                paragraph.Range.Font.Size = 16;
                paragraph.Range.Font.Bold = 1;
                paragraph.Range.Font.Italic = 0;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.InsertParagraph();
                paragraph.Range.Text = "Tệp: "+group.Name;
                paragraph.Range.Font.Size = 16;
                paragraph.Range.Font.Bold = 0;
                paragraph.Range.Font.Italic = 0;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                string text = "Quyết định: "; string khd = "";
                text += group.DecitionNumber.HasValue ? group.DecitionNumber.Value.ToString() : "";
                foreach (DecisionCode d in listDecisionCode)
                {
                    if (group.DecisionDate.Value >= d.FromDate && group.DecisionDate.Value <= d.ToDate)
                    {
                        khd = d.Code; 
                        break;
                    }
                }
                text += "/" + khd +(group.DecisionDate.HasValue?( " Ngày " + group.DecisionDate.Value.Day + " tháng " + group.DecisionDate.Value.Month + " năm " + group.DecisionDate.Value.Year):"");
                paragraph.Range.Text = text ;
                paragraph.Range.Font.Size = 12;
                paragraph.Range.Font.Bold = 0;
                paragraph.Range.Font.Italic = 1;
                paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                paragraph.Range.InsertParagraphAfter();
                paragraph.Range.InsertParagraph();
                foreach (var p in listProvince)
                {
                    var kt = data.Where(q => q.ProvinceRequestID.Equals(p.Code)).ToList();

                    if (kt.Count() == 0) continue;
                    paragraph.Range.Text = "Tỉnh " + p.ProvinceName;
                    paragraph.Range.Font.Size = 11;
                    paragraph.Range.Font.Italic = 0;
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                    paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    //paragraph.Range.Font.Color = get_Word_Color_RGB(0, 112, 192);
                    paragraph.Range.InsertParagraphAfter();
                    paragraph.Range.InsertParagraph();
                    string str = (kt[0].ObjectID.HasValue ? kt[0].Object.Code : "") + (kt[0].PeriodID.HasValue ? kt[0].Period.Code : "");// lấy ra ObjectCode và PeriodCode
                    string tmp = "";
                    int start = 0; int end = 0;// bắt đầu từ phần tử thứ 2
                    int count = 0;
                    // duyệt cho đến khi nào mã OP khác sao với ban đầu thì fill dữ liệu
                    while ( end < kt.Count())
                    {
                        try
                        {
                            tmp = (kt[end].ObjectID.HasValue ? kt[end].Object.Code : "") + (kt[end].PeriodID.HasValue ? kt[end].Period.Code : "");//lấy mã OP của phần tử
                                                                                                                                                  // ktra xem có trùng hay không?
                            if (end == kt.Count() - 1)
                            {
                                if (doc.Content.End > 1)
                                {
                                    doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();

                                }

                                docTemplate.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                                doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();

                                if (doc.Bookmarks.Exists("code"))
                                {
                                    doc.Bookmarks["code"].Range.Text = group.CerfiticateCode + "_";

                                }
                                if (doc.Bookmarks.Exists("tt1"))
                                {
                                    doc.Bookmarks["tt1"].Range.Text = kt[start].CerfiticateNumber.Value.ToString();
                                }
                                if (doc.Bookmarks.Exists("tt2"))
                                {

                                    doc.Bookmarks["tt2"].Range.Text = str;
                                }
                                if (doc.Bookmarks.Exists("tt3"))
                                {
                                    doc.Bookmarks["tt3"].Range.Text = "-";
                                }
                                if (doc.Bookmarks.Exists("tt4"))
                                {

                                    doc.Bookmarks["tt4"].Range.Text = kt[end].CerfiticateNumber.Value.ToString();

                                }
                                if (doc.Bookmarks.Exists("tt5"))
                                {
                                    doc.Bookmarks["tt5"].Range.Text = str;
                                }
                                if (doc.Bookmarks.Exists("tt6"))
                                {
                                    doc.Bookmarks["tt6"].Range.Text = "=";
                                }
                                if (doc.Bookmarks.Exists("tt7"))
                                {
                                    doc.Bookmarks["tt7"].Range.Text = (count + 1).ToString();
                                }
                                //copy main 
                                doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                                //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                                docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                                //paragraph.Range.Text = group.CerfiticateCode + "_" + "          " + kt[start].CerfiticateNumber.Value.ToString() +"      " + str + "     -" + "     " + kt[end].CerfiticateNumber.Value.ToString() + "     " +str+ "        =" + "          " + (count+1).ToString();
                                //paragraph.Range.Font.Size = 10;
                                //paragraph.Range.Font.Italic = 0;
                                //paragraph.Range.Font.Bold = 0;
                                //paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                                //paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                //paragraph.Range.InsertParagraphAfter();
                                break;
                            }
                            if (tmp == str)// nếu trùng thực hiện tiếp 
                            {
                                count++;
                            }
                            // ngc lại nếu khác thêm 1 dòng dữ liệu
                            else
                            {
                                // thêm dòng
                                if (doc.Content.End > 1)
                                {
                                    doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();

                                }

                                docTemplate.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                                doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();

                                if (doc.Bookmarks.Exists("code"))
                                {
                                    doc.Bookmarks["code"].Range.Text = group.CerfiticateCode + "_";

                                }
                                if (doc.Bookmarks.Exists("tt1"))
                                {
                                    doc.Bookmarks["tt1"].Range.Text = kt[start].CerfiticateNumber.Value.ToString();
                                }
                                if (doc.Bookmarks.Exists("tt2"))
                                {

                                    doc.Bookmarks["tt2"].Range.Text = str;
                                }
                                if (doc.Bookmarks.Exists("tt3"))
                                {
                                    doc.Bookmarks["tt3"].Range.Text = "-";
                                }
                                if (doc.Bookmarks.Exists("tt4"))
                                {

                                    doc.Bookmarks["tt4"].Range.Text = kt[end - 1].CerfiticateNumber.Value.ToString();

                                }
                                if (doc.Bookmarks.Exists("tt5"))
                                {
                                    doc.Bookmarks["tt5"].Range.Text = str;
                                }
                                if (doc.Bookmarks.Exists("tt6"))
                                {
                                    doc.Bookmarks["tt6"].Range.Text = "=";
                                }
                                if (doc.Bookmarks.Exists("tt7"))
                                {
                                    doc.Bookmarks["tt7"].Range.Text = (count).ToString();
                                }
                                //copy main 
                                doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                                //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                                docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                                //paragraph.Range.Text = group.CerfiticateCode + "_" + "          " + kt[start].CerfiticateNumber.Value.ToString() + "      " + str + "     -" + "     " + kt[end].CerfiticateNumber.Value.ToString() + "     " + str + "        =" + "          " + (count).ToString();
                                //paragraph.Range.Font.Size = 10;
                                //paragraph.Range.Font.Italic = 0;
                                //paragraph.Range.Font.Bold = 0;
                                //paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                                //paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                //paragraph.Range.InsertParagraphAfter();
                                count = 1;
                                start = end;
                                str = tmp;
                            }
                        }
                        catch
                        {
                            doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            docTemplate2.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                            app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
                            return null;
                        }
                        end++;
                    }
                    if (doc.Content.End > 1)
                    {
                        doc.Range(doc.Content.Start, doc.Content.End - 1).Delete();

                    }

                    docTemplate2.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                    doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();
                    if (doc.Bookmarks.Exists("tt1"))
                    {
                        doc.Bookmarks["tt1"].Range.Text = "Cộng tỉnh:     "+kt.Count();
                    }
                    //copy main 
                    doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                    //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                    docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();

                    //paragraph.Range.Text = "            Cộng tỉnh:          " + kt.Count();
                    //paragraph.Range.Font.Size = 12;
                    //paragraph.Range.Font.Italic = 0;
                    //paragraph.Range.Font.Bold = 0;
                    //paragraph.Range.Font.Name = "Times New Roman (Headings CS)";
                    //paragraph.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    //paragraph.Range.InsertParagraphAfter();
                }
                docTemplate2.StoryRanges[WdStoryType.wdMainTextStory].Copy();


                doc.StoryRanges[WdStoryType.wdMainTextStory].Paste();
                if (doc.Bookmarks.Exists("tt1"))
                {
                    doc.Bookmarks["tt1"].Range.Text = "Cộng tệp:     " + data.Count();
                }
                //copy main 
                doc.StoryRanges[WdStoryType.wdMainTextStory].Copy();



                //doc.Content.PasteSpecial(DataType: Microsoft.Office.Interop.Word.WdPasteOptions.wdKeepSourceFormatting);
                docMain.Range(docMain.Content.End - 1, docMain.Content.End - 1).Paste();
                docMain.SaveAs2(file);

            }
            catch
            {
                doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                docTemplate2.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
                return null;
            }

            docMain.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            docTemplate.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            docTemplate2.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            // docMain.Close(true, ref unknow, ref unknow);
            app.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return File(fs, "application/vnd.ms-word", "File.docx");
        }

        private Microsoft.Office.Interop.Word.WdColor get_Word_Color_RGB(int red, int green, int blue)
        {
            //-------< get_Word_Color_RGB() >----------
            //*convert Red Green Blue to Word/Office Color
            //*using Microsoft.Office.Interop.Word.WdColor

            Color sysColor = Color.FromArgb(0, red, green, blue);
            return (Microsoft.Office.Interop.Word.WdColor)(sysColor.R + 0x100 * sysColor.G + 0x10000 * sysColor.B);
            //-------</ get_Word_Color_RGB() >----------
        }
        public int compare(int a, int b)
        {
            if (a > b) return 1;
            else if (a == b) return 0;
            else return -1;
        }

        static Document CopyToNewDocument(Document document)
        {
            document.StoryRanges[WdStoryType.wdMainTextStory].Copy();

            var newDocument = document.Application.Documents.Add();
            newDocument.StoryRanges[WdStoryType.wdMainTextStory].Paste();
            newDocument.Close();
            return newDocument;
        }
        public FileResult ExportFileSytheticExcel(int GroupId)
        {
            FileInfo file = new FileInfo(Server.MapPath(@"/Template/") + "template_tonghop_excel.xlsx");// tim den file
            ExcelPackage package = new ExcelPackage(file);//mo file ra

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets[0];
            var data = (from v in Context.Records
                        join gr in Context.GroupRecords on v.ID equals gr.RecordID
                        where gr.IsActive.Equals(1) && gr.GroupID.Equals(GroupId)
                         && (v.IsActive == SystemParam.ACTIVE)
                        orderby v.CerfiticateNumber ascending
                        select v).ToList();
            //data.Sort((u1, u2) =>
            //{
            //    // sắp xếp theo tỉnh thành
            //    Province p1 = Context.Provinces.Find(u1.ProvinceRequestID.Value); Province p2 = Context.Provinces.Find(u2.ProvinceRequestID.Value);
            //    if (p1.Name.CompareTo(p2.Name) != 0) return p1.Name.CompareTo(p2.Name);
            //    // sắp xếp theo đối tượng

            //    var o1 = u1.ObjectID; var o2 = u2.ObjectID;
            //    if (o1 != null && o2 == null) return 1;
            //    if (o1 == null && o2 != null) return -1;
            //    if (o1 != null && o2 != null)
            //    {
            //        var displayOrderO1 = u1.Object.DisplayOrder; var displayOrderO2 = u2.Object.DisplayOrder;
            //        if (displayOrderO1 > displayOrderO2) return 1;
            //        else if (displayOrderO1 < displayOrderO2) return -1;
            //    }
            //    // sắp xếp theo thời kì
            //    var pe1 = u1.PeriodID; var pe2 = u2.PeriodID;
            //    if (pe1 != null && pe2 == null) return 1;
            //    if (pe1 == null && pe2 != null) return -1;
            //    if (pe1 != null && pe2 != null)
            //    {
            //        var displayOrderPe1 = u1.Period.DisplayOrder; var displayOrderPe2 = u2.Period.DisplayOrder;
            //        if (displayOrderPe1 > displayOrderPe2) return 1;
            //        else if (displayOrderPe1 < displayOrderPe2) return -1;
            //    }
            //    //sắp xếp theo tên 
            //    string[] s1 = u1.MartyrsName.Split(' '); string[] s2 = u2.MartyrsName.Split(' ');
            //    int result = s1[s1.Length - 1].CompareTo(s2[s2.Length - 1]);
            //    if (result != 0) return result;
            //    int i = 0, j = 0;
            //    while ((i < s1.Length - 1) && (j < s2.Length - 1))
            //    {
            //        if (s1[i].CompareTo(s2[j]) != 0) return s1[i].CompareTo(s2[j]);
            //        i++; j++;
            //    }
            //    if (i == s1.Length - 2) return s2.Length;
            //    if (j == s2.Length - 2) return -s1.Length;
            //    return 0;
            //});
            var listProvince = provinceBusiness.GetListProvince();
            var group = Context.Groups.Find(GroupId);
            sheet.Cells["A2"].Value = group.Name;
            int startRow = 4;
            int stt = 1;
            foreach(var p in listProvince)
            {
                var kt = data.Where(q => q.ProvinceRequestID.Equals(p.Code)).ToList();

                if (kt.Count() == 0) continue;
                sheet.Cells[startRow, 1].Value = stt;
                sheet.Cells[startRow, 2].Value = p.ProvinceName;
                sheet.Cells[startRow, 3].Value = kt[0].MartyrsName;
                sheet.Cells[startRow, 4].Value = kt[0].CerfiticateNumber.Value.ToString() + "->" + kt[kt.Count() - 1].CerfiticateNumber.Value.ToString();
                sheet.Cells[startRow, 5].Value = kt.Count();
                startRow++; stt++;
            }
            sheet.Cells[startRow, 4].Value = "Tổng";
            sheet.Cells[startRow, 5].Value = data.Count();
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        //xuat file Excel
        public FileResult ExportExcelGNP(string Str)
        {

            FileInfo file = new FileInfo(Server.MapPath(@"/Template/") + "Book1.xlsx");// tim den file
            ExcelPackage package = new ExcelPackage(file);//mo file ra

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets[0];
            // tien hanh lay du lieu
            //xu li du lieu lay vao 
            string[] grID = Str.Split(',');
            List<int> it = new List<int>();
            for (int i = 0; i < grID.Length - 1; i++)
            {
                it.Add(Int32.Parse(grID[i]));
            }
            //lay danh sach tat ca cac ban ghi
            var data = (from v in Context.Records
                        join gr in Context.GroupRecords on v.ID equals gr.RecordID
                        where (v.Status.Equals(SystemParam.STATUS_ACCEPTED_RECORD) || v.Status.Equals(SystemParam.STATUS_PENDING_RECORD) || v.Status.Equals(SystemParam.STATUS_WAIT_PRESIDENT))
                         && (v.IsActive == SystemParam.ACTIVE)
                        orderby v.CreatedDate ascending
                        select new NewProfileModel
                        {
                            ID = v.ID,
                            MartyrsName = v.MartyrsName,
                            Address = v.Address,
                            DecitionCodeID = v.DecitionCodeID.Value,
                            CerfiticateCode = v.CerfiticateCode,
                            CerfiticateNumber = v.CerfiticateNumber.Value,
                            CreatedDate = v.CreatedDate,
                            DecitionNumber = v.DecitionNumber,
                            Number = v.Number,
                            NamePosition = v.Position.Name,
                            IsArmy = v.Position.IsArmy,
                            IsGuerrilla = v.Position.IsGuerrilla,
                            ObjectCode = v.Object.Code,
                            NameDistrict = v.District.Name,
                            NameProvince = v.Province.Name,
                            sacrifice_date = v.sacrifice_date != 0 ? v.sacrifice_date : null,
                            sacrifice_month = v.sacrifice_month != 0 ? v.sacrifice_month : null,
                            sacrifice_year = v.sacrifice_year != 0 ? v.sacrifice_year : null,
                            DecitionDate = v.DecitionDate.Value,
                            PeriodCode = v.Period.Code,
                            GroupRecordID = gr.ID,
                            DisPlayOrder = v.Object.DisplayOrder,
                            GroupID = gr.GroupID, 
                            PositionID = v.PositionID,
                            PeriodID = v.PeriodID,
                            ObjectID = v.ObjectID,
                            codeDecisionCode = v.DecisionCode.Code,
                        }).ToList();
            List<NewProfileModel> list = new List<NewProfileModel>();
            for (int i = 0; i < it.Count(); i++)
            {
                var query = data.Where(q => q.GroupRecordID.Equals(it[i])).FirstOrDefault();
                if (query == null) continue;
                list.Add(query);
            }
            //tien hanh fill 
            int dem = 0, startRow = 6;
            int gID = list[0].GroupID.Value;
            Group groups = Context.Groups.Find(gID);
            sheet.Cells["A3"].Value = "(Kèm theo công văn số " + (groups.DocumentNumber.HasValue ? groups.DocumentNumber.Value.ToString() : "")  + " / SLĐTBXH - NCC ngày " + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString()) + " tháng " + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + " năm " + DateTime.Now.Year.ToString() + ")";
            foreach (var item in list)
            {
                dem++;
                sheet.Cells[startRow, 1].Value = dem;
                sheet.Cells[startRow, 2].Value = !String.IsNullOrEmpty(item.MartyrsName) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.MartyrsName.ToLower()) : "";
                sheet.Cells[startRow, 3].Value = item.PositionID.HasValue ?item.NamePosition : "";
                sheet.Cells[startRow, 4].Value = item.ObjectID.HasValue?item.ObjectCode:"";
                sheet.Cells[startRow, 5].Value = item.Address;
                sheet.Cells[startRow, 6].Value = item.NameProvince;
                if(item.sacrifice_date != null)
                {
                    if(item.sacrifice_date.Value < 10)
                    sheet.Cells[startRow, 7].Value = "0" + item.sacrifice_date.Value.ToString();
                    else sheet.Cells[startRow, 7].Value =  item.sacrifice_date.Value.ToString();
                }
                else
                {
                    sheet.Cells[startRow, 7].Value = "0";
                }
                if(item.sacrifice_month != null)
                {
                    if (item.sacrifice_month.Value < 10)
                    sheet.Cells[startRow, 8].Value = "0" + item.sacrifice_month.ToString();
                    else sheet.Cells[startRow, 8].Value = item.sacrifice_month.ToString();
                }
                else
                {
                    sheet.Cells[startRow, 8].Value = "0";
                }
                if(item.sacrifice_year != null)
                {
                    sheet.Cells[startRow, 9].Value = item.sacrifice_year.Value;
                }
                else
                {
                    sheet.Cells[startRow, 9].Value = "0";
                }
                sheet.Cells[startRow, 10].Value =item.PeriodID.HasValue ? item.PeriodCode : "";
                sheet.Cells[startRow, 11].Value = item.CerfiticateCode;
                sheet.Cells[startRow, 12].Value = item.CerfiticateNumber.HasValue ? item.CerfiticateNumber.Value.ToString() : "";
                sheet.Cells[startRow, 13].Value = item.DecitionCodeID.HasValue?(item.DecitionNumber.Value.ToString()+"/"+item.codeDecisionCode):""; 
                if (item.DecitionDate != null)
                {
                    if(item.DecitionDate.Value.Day < 10)
                    {
                        sheet.Cells[startRow, 14].Value = "0" + item.DecitionDate.Value.Day.ToString();
                    }
                    else
                    {
                        sheet.Cells[startRow, 14].Value = item.DecitionDate.Value.Day;
                    }
                    if(item.DecitionDate.Value.Month < 3)
                    {
                        sheet.Cells[startRow, 15].Value = "0" + item.DecitionDate.Value.Month.ToString();
                    }
                    else
                    {
                        sheet.Cells[startRow, 15].Value = item.DecitionDate.Value.Month;
                    }
                    
                    sheet.Cells[startRow, 16].Value = item.DecitionDate.Value.Year;
                }
                else
                {
                    sheet.Cells[startRow, 14].Value = "0";
                    sheet.Cells[startRow, 15].Value = "0";
                    sheet.Cells[startRow, 16].Value = "0";
                }
                startRow++;

            }
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //xuat file Mau Excel
        public FileResult ExportExcelTemplate()
        {
            FileInfo file = new FileInfo(Server.MapPath(@"/Template/") + "Mau_import.xlsx");// tim den file
            ExcelPackage package = new ExcelPackage(file);//mo file ra

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets[1];
            // tien hanh lay du lieu
            //xu li du lieu lay vao 
            //lay danh sach tat ca cac ban ghi
            List<int?> positionParent = (from c in Context.Positions
                                        where c.IsActive == SystemParam.ACTIVE && c.ParentID != null
                                        select c.ParentID).ToList();
            var data = (from v in Context.Positions
                        where v.IsActive == SystemParam.ACTIVE && !positionParent.Contains(v.ID)
                        select new PositionOutputModel
                        {
                            Name = v.Name,
                            ID = v.ID
                        }).ToList();
            //tien hanh fill 
            int startRow = 1;
            foreach (var item in data)
            {
                sheet.Cells[startRow, 1].Value = item.Name;
                startRow++;
            }
            //fill data to sheet sở đề nghị
            ExcelWorksheet sheet2 = workbook.Worksheets[2];
            // tien hanh lay du lieu
            //xu li du lieu lay vao 
            //lay danh sach tat ca cac ban ghi
            var dataPro = (from v in Context.Provinces
                           where v.IsActive == SystemParam.ACTIVE
                           select new ProvinceOutputModel
                           {
                               Name = v.Name
                           }).ToList();
            //tien hanh fill 

            int startRow2 = 1;
            foreach (var item in dataPro)
            {
                sheet2.Cells[startRow2, 1].Value = item.Name;
                startRow2++;
            }
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //xuat file Mau Excel
        public FileResult ExportExcelTemplateRenew()
        {
            LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
            //return File(Server.MapPath(@"/Template/") + "Mau_import_renew.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            FileInfo file =userLogin.Role != 3? new FileInfo(Server.MapPath(@"/Template/") + "Mau_import_renew.xlsx"): new FileInfo(Server.MapPath(@"/Template/") + "Mau_import_renew1.xlsx");// tim den file
            ExcelPackage package = new ExcelPackage(file);//mo file ra

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet sheet = workbook.Worksheets[1];
            // tien hanh lay du lieu
            //xu li du lieu lay vao 
            //lay danh sach tat ca cac ban ghi
            List<int?> positionParent = (from c in Context.Positions
                                         where c.IsActive == SystemParam.ACTIVE && c.ParentID != null
                                         select c.ParentID).ToList();
            var data = (from v in Context.Positions
                        where v.IsActive == SystemParam.ACTIVE && !positionParent.Contains(v.ID)
                        select new PositionOutputModel
                        {
                            Name = v.Name,
                            ID = v.ID
                        }).ToList();
            //tien hanh fill 
            int startRow = 1;
            foreach (var item in data)
            {
                sheet.Cells[startRow, 1].Value = item.Name;
                startRow++;
            }
            //sở đề nghị
            if (userLogin.Role != 3)
            {
                ExcelWorksheet sheet2 = workbook.Worksheets[2];
                // tien hanh lay du lieu
                //xu li du lieu lay vao 
                //lay danh sach tat ca cac ban ghi
                var dataPro = (from v in Context.Provinces
                               where v.IsActive == SystemParam.ACTIVE
                               select new ProvinceOutputModel
                               {
                                   Name = v.Name
                               }).ToList();
                //tien hanh fill 

                int startRow2 = 1;
                foreach (var item in dataPro)
                {
                    sheet2.Cells[startRow2, 1].Value = item.Name;
                    startRow2++;
                }
            }
            return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //import excel
        public JsonResult UploadExcel()
        {
            int row = 3;
            var errorLog = new StringBuilder();
            try
            {
                HttpPostedFileBase ExcelFile = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    ExcelFile = file;                                //Use the following properties to get file's name, size and MIMEType
                    break;
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;

                }

                LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];//lấy sessions

                if (ExcelFile != null && (ExcelFile.FileName.EndsWith("xls") || ExcelFile.FileName.EndsWith("xlsx")))//nếu đúng là file excel 
                {
                    string path = Server.MapPath("~/Import/" + ExcelFile.FileName);// đường dẫn file trỏ vào
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    ExcelFile.SaveAs(path);// lưu file vào thư mục import trên server
                    FileInfo file = new FileInfo(path);
                    ExcelPackage pack = new ExcelPackage(file);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];


                    // lấy các chức vụ
                    List<Position> listPosition = (from p in Context.Positions
                                                   where p.IsActive.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();
                    // lấy ra đối tượng
                    List<Data.DB.Object> listObject = (from p in Context.Objects
                                                       where p.IsActive.Equals(SystemParam.ACTIVE)
                                                       select p).ToList();
                    // lấy ra quận huyện
                    List<District> listDistrict = (from p in Context.Districts
                                                   where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();
                    // lấy ra làng xã
                    List<Village> listVillage = (from p in Context.Villages
                                                 where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                 select p).ToList();
                    // lấy ra tỉnh thành
                    List<Province> listProvince = (from p in Context.Provinces
                                                   where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();
                    //lấy ra thời kì
                    List<Period> listPeriod = (from p in Context.Periods
                                               where p.IsActive.Equals(SystemParam.ACTIVE)
                                               select p).ToList();
                    //lấy quyết định 
                    List<DecisionCode> listDecisionCode = (from p in Context.DecisionCodes
                                                           where p.IsActive.Equals(SystemParam.ACTIVE)
                                                           select p).ToList();

                    object data = 0;// khởi tạo dữ liệu

                    List<NewProfileModel> listRecord = new List<NewProfileModel>();
                    while (data != null)
                    {
                        row++;
                        int col = 2;
                        object dataGroup = sheet.Cells[row, col].Value;// lấy dữ liệu
                        if (dataGroup == null)
                        {
                            if (row == 4)
                            {
                                errorLog.AppendLine("Không có dữ liệu import!");
                            }
                            break;

                        }

                        //check cot dau tien
                        NewProfileModel p = new NewProfileModel();
                        //Truong ten
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.MartyrsName = sheet.Cells[row, col].Value.ToString();
                        }
                        else
                        {
                            //errorLog.AppendLine("Dữ liệu tên KH dòng " + row.ToString() + " không hợp lệ!");
                            //continue;
                        }
                        col++;

                        // chức vụ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listPosition.Where(u => u.Name.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)// nếu tồn tại chức vụ thì lấy vào 
                            {
                                p.PositionID = col3.ID;
                            }
                            else// nếu không có thì thêm chức vụ mới
                            {
                                Position d = new Position();
                                d.Code = Data.Utils.Util.CreateMD5(DateTime.Now.ToString()).Substring(0, 5);
                                d.Name = sheet.Cells[row, col].Value.ToString();
                                d.ParentID = null;
                                d.IsArmy = 0;
                                d.IsGuerrilla = 0;
                                d.IsActive = SystemParam.ACTIVE;
                                d.CreatedDate = DateTime.Now;
                                d.Status = SystemParam.ACTIVE;
                                d.UserCreateID = userLogin.Id;
                                Context.Positions.Add(d);
                                Context.SaveChanges();
                                p.PositionID = d.ID;
                            }
                        }
                        else { p.PositionID = null; }
                        col++;
                        // đối tượng
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listObject.Where(u => u.Code.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.ObjectID = col3.ObjectID;
                                p.ObjectCode = col3.Code;
                            }
                            else
                            {
                                errorLog.AppendLine("Dữ liệu tên Đối tượng dòng " + row.ToString() + " không hợp lệ!");
                                continue;
                            }
                        }
                        else
                        {
                            p.ObjectID = null;
                        }
                        if (p.ObjectID == 0)
                        {
                            errorLog.AppendLine("Dữ liệu tên Đối tượng dòng " + row.ToString() + " không hợp lệ!");
                            continue;
                        }
                        col++;
                        
                        //địa chỉ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.Address = sheet.Cells[row, col].Value.ToString();
                        }
                        col++;

                        //nguyên quán tỉnh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listProvince.Where(u => u.Name.ToString().ToLower().Equals(sheet.Cells[row, col].Value.ToString().ToLower().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.ProvinceID = col3.Code;
                            }
                            else
                            {
                                var query = (from c in Context.Provinces select c.Code).Max();
                                Province pro = new Province();
                                pro.Name = sheet.Cells[row, col].Value.ToString();
                                pro.ProvinceCode = "otherProvince";
                                pro.Code = query + 1;
                                pro.IsActive = SystemParam.ACTIVE;
                                pro.CreatedDate = DateTime.Now;
                                pro.Type = "Tỉnh";
                                Context.Provinces.Add(pro);
                                Context.SaveChanges();
                                p.ProvinceID = query + 1;
                            }
                        }
                        else { p.ProvinceID = null; }
                        //if (p.VilageID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã Xã dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listDistrict.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.DistrictID = col3.Code;
                        //    }
                        //}
                        //if (p.DistrictID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã huyện dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        //col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listProvince.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.ProvinceID = col3.Code;
                        //    }
                        //}
                        //if (p.ProvinceID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã Tỉnh dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        //col++;

                        //ngày hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_date = col3;
                            }
                        }
                        col++;

                        //tháng hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_month = col3;
                            }
                        }
                        col++;

                        //năm hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_year = col3;
                            }
                        }
                        col++;

                        //nếu tất cả đều ngày, tháng, năm hi sinh đều null
                        if (p.sacrifice_month.HasValue && p.sacrifice_date.HasValue && p.sacrifice_year.HasValue)
                        {
                            try
                            {
                                p.SacrificeDate = new DateTime(p.sacrifice_year.Value, p.sacrifice_month.Value, p.sacrifice_date.Value);

                            }
                            catch
                            {

                            }
                        }
                        else { p.SacrificeDate = null; }

                        //thời kì
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listPeriod.Where(u => u.Code.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.PeriodCode = col3.Code.ToString();
                                p.PeriodID = col3.ID;
                            }
                            else
                            {
                                errorLog.AppendLine("Dữ liệu mã giai đoạn dòng" + row.ToString() + " không hợp lệ!");
                                continue;
                            }
                        }
                        else
                        {
                            p.PeriodID = null;
                        }
                        //if (p.PeriodID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã giai đoạn dòng" + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        col++;


                        //mã bằng 
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = sheet.Cells[row, col].Value.ToString();
                            if (col3 != null)
                            {
                                p.CerfiticateCode = col3;
                            }
                            else
                            {
                                p.CerfiticateCode = null;
                            }
                        }
                        col++;
                        //if (p.CerfiticateCode == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu ký hiệu bằng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //if (p.CerfiticateNumber == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số bằng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        // số bằng
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.CerfiticateNumber = col3;
                            }
                            else { p.CerfiticateNumber = null; }
                        }
                        col++;

                        //số quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.DecitionNumber = col3;
                            }
                            else { p.DecitionNumber = null; }
                        }
                        col++;
                        //if (p.DecitionNumber == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số quyết định " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //if (p.Number == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số HS " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        int decitiondate = 0;
                        int decitionmonth = 0;
                        int decitionyear = 0;

                        // ngày quyết địn
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitiondate = col3;
                            }
                        }
                        col++;

                        //số quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitionmonth = col3;
                            }
                        }
                        col++;

                        //năm quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitionyear = col3;
                            }
                        }
                        col++;

                        if (decitiondate != 0 && decitionmonth != 0 && decitionyear != 0)
                        {
                            try
                            {
                                p.DecitionDate = new DateTime(decitionyear, decitionmonth, decitiondate);
                            }
                            catch
                            {

                            }
                        }
                        else { p.DecitionDate = null; }
                        //if (p.DecitionDate == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu ngày quyết định dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //số hồ sơ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = sheet.Cells[row, col].Value.ToString();
                            if (col3 != null)
                            {
                                p.Number = col3;
                            }
                        }
                        col++;

                        //người đề nghị
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterName = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterName = null; }
                        col++;

                        //số điện thoại người đề nghị 
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterPhone = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterPhone = null; }
                        col++;

                        //địa chỉ người đề nghị
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterAddress = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterAddress = null; }
                        col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listDistrict.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.RequesterDistrict = col3.Code;
                        //    }
                        //}

                        //col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listProvince.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.RequesterProvince = col3.Code;
                        //    }
                        //}

                        //col++;


                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    p.RequesterAddress = sheet.Cells[row, col].Value.ToString();
                        //}
                        //col++;

                        //mối quan hệ với người có công
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterRelation = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterRelation = null; }
                        col++;

                        //lí do đề nghị 
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterResion = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterResion = null; }

                        //them thong tin nguoi tao
                        p.CreatedDate = DateTime.Now;

                        //them thong tin nguoi tao
                        p.CreatedDate = DateTime.Now;


                        p.CreateUserID = userLogin.Id;
                        if (userLogin.Role.Equals(SystemParam.ROLE_ADMIN) || userLogin.Role.Equals(SystemParam.ROLE_USER_POLICY_1))
                        {
                            p.Status = SystemParam.STATUS_ACCEPTED_RECORD;
                        }
                        else
                        {
                            p.Status = SystemParam.STATUS_PENDING_RECORD;
                        }
                        p.Type = SystemParam.TYPE_GROUP_NEW_PROFILE;
                        col++;
                        //thêm tỉnh nếu tỉnh đó không tồn tại
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listProvince.Where(u => u.Name.ToString().ToLower().Equals(sheet.Cells[row, col].Value.ToString().ToLower().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.ProvinceRequestID = col3.Code;
                            }
                            else
                            {
                                var query = (from c in Context.Provinces select c.Code).Max();
                                Province pro = new Province();
                                pro.Name = sheet.Cells[row, col].Value.ToString();
                                pro.ProvinceCode = "otherProvince";
                                pro.Code = query + 1;
                                pro.IsActive = SystemParam.ACTIVE;
                                pro.CreatedDate = DateTime.Now;
                                pro.Type = "Tỉnh";
                                Context.Provinces.Add(pro);
                                Context.SaveChanges();
                                p.ProvinceRequestID = query + 1;
                            }
                        }
                        else { p.ProvinceRequestID = null; }
                        errorLog.AppendLine("Thêm thành công dòng " + row);

                        //Them thanh cong 
                        errorLog.AppendLine("\n");

                        listRecord.Add(p);

                        //}
                    }
                    int record = 0;
                    //Thuc hien save
                    if (listRecord != null && listRecord.Count() > 0)
                        record = renewProfileBusiness.SaveImport(listRecord);
                    //Them thanh cong 
                    errorLog.AppendLine("\n");
                    errorLog.AppendLine("Số bản ghi lưu thành công:" + record.ToString());
                }
                return Json(new { status = 1, message = errorLog.ToString() }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = errorLog.ToString() }, JsonRequestBehavior.AllowGet); ;
            }
        }
        public JsonResult UploadExcelReNew()
        {
            int row = 3;
            var errorLog = new StringBuilder();
            try
            {
                HttpPostedFileBase ExcelFile = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    ExcelFile = file;                                //Use the following properties to get file's name, size and MIMEType
                    break;
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;

                }

                LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];

                if (ExcelFile != null && (ExcelFile.FileName.EndsWith("xls") || ExcelFile.FileName.EndsWith("xlsx")))
                {
                    string path = Server.MapPath("~/Import/" + ExcelFile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    ExcelFile.SaveAs(path);
                    FileInfo file = new FileInfo(path);
                    ExcelPackage pack = new ExcelPackage(file);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet sheet = pack.Workbook.Worksheets[0];


                    //bắt đầu lấy dữ liệu trong DB
                    List<Position> listPosition = (from p in Context.Positions
                                                   where p.IsActive.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();

                    List<Data.DB.Object> listObject = (from p in Context.Objects
                                                       where p.IsActive.Equals(SystemParam.ACTIVE)
                                                       select p).ToList();

                    List<District> listDistrict = (from p in Context.Districts
                                                   where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();

                    List<Village> listVillage = (from p in Context.Villages
                                                 where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                 select p).ToList();

                    List<Province> listProvince = (from p in Context.Provinces
                                                   where p.IsActive.Value.Equals(SystemParam.ACTIVE)
                                                   select p).ToList();

                    List<Period> listPeriod = (from p in Context.Periods
                                               where p.IsActive.Equals(SystemParam.ACTIVE)
                                               select p).ToList();

                    List<DecisionCode> listDecisionCode = (from p in Context.DecisionCodes
                                                           where p.IsActive.Equals(SystemParam.ACTIVE)
                                                           select p).ToList();//end

                    object data = 0;

                    List<NewProfileModel> listRecord = new List<NewProfileModel>();
                    while (data != null)
                    {
                        row++;
                        int col = 2;
                        object dataGroup = sheet.Cells[row, col].Value;
                        if (dataGroup == null)
                        {
                            if (row == 4)
                            {
                                errorLog.AppendLine("Không có dữ liệu import!");
                            }
                            break;

                        }

                        //check cot dau tien
                        NewProfileModel p = new NewProfileModel();
                        //Truong ten
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.MartyrsName = sheet.Cells[row, col].Value.ToString();
                        }
                        else
                        {
                            //errorLog.AppendLine("Dữ liệu tên KH dòng " + row.ToString() + " không hợp lệ!");
                            //continue;
                        }
                        col++;

                        //chức vụ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listPosition.Where(u => u.Name.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.PositionID = col3.ID;
                            }
                            else
                            {
                                Position d = new Position();
                                d.Code = Data.Utils.Util.CreateMD5(p.MartyrsName + DateTime.Now.ToString()).Substring(0, 5);
                                d.Name = sheet.Cells[row, col].Value.ToString();
                                d.ParentID = null;
                                d.IsArmy = 0;
                                d.IsGuerrilla = 0;
                                d.IsActive = SystemParam.ACTIVE;
                                d.CreatedDate = DateTime.Now;
                                d.Status = SystemParam.ACTIVE;
                                d.UserCreateID = userLogin.Id;
                                Context.Positions.Add(d);
                                Context.SaveChanges();
                                p.PositionID = d.ID;
                            }
                        }
                        else { p.PositionID = null; }
                        col++;

                        //đối tượng
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listObject.Where(u => u.Code.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.ObjectID = col3.ObjectID;
                                p.ObjectCode = col3.Code;
                            }
                            else
                            {
                                errorLog.AppendLine("Dữ liệu tên Đối tượng dòng " + row.ToString() + " không hợp lệ!");
                                continue;
                            }
                        }
                        else
                        {
                            p.ObjectID = null;
                        }
                        if (p.ObjectID == 0)
                        {
                            errorLog.AppendLine("Dữ liệu tên Đối tượng dòng " + row.ToString() + " không hợp lệ!");
                            continue;
                        }
                        col++;

                        //địa chỉ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.Address = sheet.Cells[row, col].Value.ToString();
                        }
                        col++;

                        //tỉnh thành phố
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listProvince.Where(u => u.Name.ToString().ToLower().Equals(sheet.Cells[row, col].Value.ToString().ToLower().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.ProvinceID = col3.Code;
                            }
                            else
                            {
                                var query = (from c in Context.Provinces select c.Code).Max();
                                Province pro = new Province();
                                pro.Name = sheet.Cells[row, col].Value.ToString();
                                pro.ProvinceCode = "otherProvince";
                                pro.Code = query + 1;
                                pro.IsActive = SystemParam.ACTIVE;
                                pro.CreatedDate = DateTime.Now;
                                pro.Type = "Tỉnh";
                                Context.Provinces.Add(pro);
                                Context.SaveChanges();
                                p.ProvinceID = query + 1;
                            }
                        }
                        else { p.ProvinceID = null; }
                        //if (p.VilageID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã Xã dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listDistrict.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.DistrictID = col3.Code;
                        //    }
                        //}
                        //if (p.DistrictID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã huyện dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        //col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listProvince.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.ProvinceID = col3.Code;
                        //    }
                        //}
                        //if (p.ProvinceID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã Tỉnh dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        //col++;

                        //ngày hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_date = col3;
                            }
                        }
                        col++;

                        //tháng hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_month = col3;
                            }
                        }
                        col++;

                        //năm hi sinh
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.sacrifice_year = col3;
                            }
                        }
                        col++;

                        //thêm vào db
                        if (p.sacrifice_month.HasValue && p.sacrifice_date.HasValue && p.sacrifice_year.HasValue)
                        {
                            try
                            {
                                p.SacrificeDate = new DateTime(p.sacrifice_year.Value, p.sacrifice_month.Value, p.sacrifice_date.Value);

                            }
                            catch
                            {

                            }
                        }
                        else {
                            if (p.sacrifice_year.HasValue)
                            {
                                p.SacrificeDate = (!p.sacrifice_date.HasValue && !p.sacrifice_month.HasValue) ? new DateTime(p.sacrifice_year.Value, 1, 1) : (!p.sacrifice_date.HasValue && p.sacrifice_month.HasValue ? new DateTime(p.sacrifice_year.Value, p.sacrifice_month.Value, 1) : new DateTime(p.sacrifice_year.Value, 1, p.sacrifice_date.Value));
                            }
                            else
                            {
                                p.SacrificeDate = null;
                            }
                        }

                        //thời kì
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = listPeriod.Where(u => u.Code.Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                            if (col3 != null)
                            {
                                p.PeriodCode = col3.Code.ToString();
                                p.PeriodID = col3.ID;
                            }
                            else
                            {
                                errorLog.AppendLine("Dữ liệu mã giai đoạn dòng" + row.ToString() + " không hợp lệ!");
                                continue;
                            }
                        }
                        else { p.PeriodID = null; }
                        //if (p.PeriodID == 0)
                        //{
                        //    errorLog.AppendLine("Dữ liệu mã giai đoạn dòng" + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}
                        col++;


                        //mã bằng
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = sheet.Cells[row, col].Value.ToString();
                            if (col3 != null)
                            {
                                p.CerfiticateCode = col3;
                            }
                            else
                            {
                                p.CerfiticateCode = null;
                            }
                        }
                        col++;
                        //if (p.CerfiticateCode == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu ký hiệu bằng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //số bằng 
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.CerfiticateNumber = col3;
                            }
                            else { p.CerfiticateNumber = null; }
                        }
                        col++;
                        //if (p.CerfiticateNumber == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số bằng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        // số quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                p.DecitionNumber = col3;
                            }
                            else { p.DecitionNumber = null; }
                        }
                        col++;
                        //if (p.DecitionNumber == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số quyết định " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //if (p.Number == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu số HS " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        int decitiondate = 0;
                        int decitionmonth = 0;
                        int decitionyear = 0;

                        //ngày quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitiondate = col3;
                            }
                        }
                        col++;

                        //tháng quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitionmonth = col3;
                            }
                        }
                        col++;

                        //năm quyết định
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            int col3 = 0;
                            if (int.TryParse(sheet.Cells[row, col].Value.ToString(), out col3))
                            {
                                decitionyear = col3;
                            }
                        }
                        col++;

                        //thêm vào DB quyết định
                        if (decitiondate != 0 && decitionmonth != 0 && decitionyear != 0)
                        {
                            try
                            {
                                p.DecitionDate = new DateTime(decitionyear, decitionmonth, decitiondate);
                            }
                            catch
                            {

                            }
                        }
                        else { p.DecitionDate = null; }
                        //if (p.DecitionDate == null)
                        //{
                        //    errorLog.AppendLine("Dữ liệu ngày quyết định dòng " + row.ToString() + " không hợp lệ!");
                        //    continue;
                        //}

                        //số hồ sơ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            var col3 = sheet.Cells[row, col].Value.ToString();
                            if (col3 != null)
                            {
                                p.Number = col3;
                            }
                        }
                        col++;

                        //người yêu cầu
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterName = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterName = null; }
                        col++;

                        //số điện thoại người yêu cầu
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterPhone = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterPhone = null; }
                        col++;

                        //địa chỉ người yêu cầu
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterAddress = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterAddress = null; }
                        col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listDistrict.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.RequesterDistrict = col3.Code;
                        //    }
                        //}

                        //col++;

                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    var col3 = listProvince.Where(u => u.Code.ToString().Equals(sheet.Cells[row, col].Value.ToString().Trim())).FirstOrDefault();
                        //    if (col3 != null)
                        //    {
                        //        p.RequesterProvince = col3.Code;
                        //    }
                        //}

                        //col++;


                        //if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        //{
                        //    p.RequesterAddress = sheet.Cells[row, col].Value.ToString();
                        //}
                        //col++;

                        //mối quan hệ với liệt sỹ
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterRelation = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterRelation = null; }
                        col++;

                        //lý do đề nghị
                        if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                        {
                            p.RequesterResion = sheet.Cells[row, col].Value.ToString();
                        }
                        else { p.RequesterResion = null; }
                        col++;

                        //kiểm tra Role
                        if(userLogin.Role != 3)
                        {
                            if (sheet.Cells[row, col].Value != null && sheet.Cells[row, col].Value.ToString().Count() > 0)
                            {
                                var col3 = listProvince.Where(u => u.Name.ToString().ToLower().Equals(sheet.Cells[row, col].Value.ToString().ToLower().Trim())).FirstOrDefault();
                                if (col3 != null)
                                {
                                    p.ProvinceRequestID = col3.Code;
                                }
                                else
                                {
                                    var query = (from c in Context.Provinces select c.Code).Max();
                                    Province pro = new Province();
                                    pro.Name = sheet.Cells[row, col].Value.ToString();
                                    pro.ProvinceCode = "otherProvince";
                                    pro.Code = query + 1;
                                    pro.IsActive = SystemParam.ACTIVE;
                                    pro.CreatedDate = DateTime.Now;
                                    pro.Type = "Tỉnh";
                                    Context.Provinces.Add(pro);
                                    Context.SaveChanges();
                                    p.ProvinceRequestID = query + 1;
                                }
                            }
                            else { p.ProvinceRequestID = null; }
                        }
                        else
                        {
                            p.ProvinceRequestID = Context.Users.Find(userLogin.Id).ProvinceID;
                        }
                        //them thong tin nguoi tao
                        p.CreatedDate = DateTime.Now;

                        //them thong tin nguoi tao
                        p.CreatedDate = DateTime.Now;


                        p.CreateUserID = userLogin.Id;
                        if (userLogin.Role.Equals(SystemParam.ROLE_ADMIN) || userLogin.Role.Equals(SystemParam.ROLE_USER_POLICY_1))
                        {
                            p.Status = SystemParam.STATUS_ACCEPTED_RECORD;
                        }
                        else
                        {
                            p.Status = SystemParam.STATUS_PENDING_RECORD;
                        }
                        p.Type = SystemParam.TYPE_GROUP_RENEW_PROFILE;
                        col++;

                        errorLog.AppendLine("Thêm thành công dòng " + row);

                        //Them thanh cong 
                        errorLog.AppendLine("\n");

                        listRecord.Add(p);

                        //}
                    }
                    int record = 0;
                    //Thuc hien save
                    if (listRecord != null && listRecord.Count() > 0)
                        record = renewProfileBusiness.SaveImport(listRecord);
                    //Them thanh cong 
                    errorLog.AppendLine("\n");
                    errorLog.AppendLine("Số bản ghi lưu thành công:" + record.ToString());
                }
                return Json(new { status = 1, message = errorLog.ToString() }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = errorLog.ToString() }, JsonRequestBehavior.AllowGet); ;
            }
        }
    }
}