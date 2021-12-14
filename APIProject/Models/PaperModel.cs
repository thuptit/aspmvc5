using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIProject.Models
{
    public class PaperModel
    {
        public string name { get; set; }
        public string des1 { get; set; }
        public string des2 { get; set; }
        public string des3 { get; set; }
        public string dd { get; set; }
        public string mm { get; set; }
        public string yyyy { get; set; }
        public string num { get; set; }
        public string symbol { get; set; }
        public string dd2 { get; set; }
        public string mm2 { get; set; }
        public string yyyy2 { get; set;}
        public string dd3 { get; set; }
        public string mm3 { get; set; }
        public string yyyy3 { get; set; }
        public string bt { get; set; }
        public string id { get; set; }
        public string position { get; set; }
        public string signed { get; set; }

    }
    public class ExportNewProfileModel
    {
        public string pp1 { get; set; }
        public string stt1 { get; set; }
        public string ln1 { get; set; }
        public string tt1 { get; set; }
        public string tt2 { get; set; }
        public string tt3 { get; set; }
    }
}