using APIProject.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIProject.Controllers
{
    public class VillageController : BaseController
    {
        // GET: Village
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            ViewBag.ListProvince = provinceBusiness.GetListProvince();
            return View();
        }
        [UserAuthenticationFilter]
        public PartialViewResult SearchVillage(int page, int provinceID, int districtID, string name = "", string fromDate = "", string toDate="")
        {
            ViewBag.name = name;
            ViewBag.fromdDate = fromDate;
            ViewBag.provinceID = provinceID;
            ViewBag.districtID = districtID;
            ViewBag.toDate = toDate;
            return PartialView("_ListVillage", villageBusiness.SearchVillage(page, provinceID, districtID, name, fromDate,toDate));
        }
        [UserAuthenticationFilter]
        public PartialViewResult GetListDistrict(int ProvinceID)
        {
            ViewBag.ListDistrict = villageBusiness.GetListDistrict(ProvinceID);
            return PartialView("_ListDistrict");
        }
        [UserAuthenticationFilter]
        public PartialViewResult GetListDistrictCreate(int ProvinceID)
        {
            ViewBag.ListDistrict = villageBusiness.GetListDistrict(ProvinceID);
            return PartialView("_ListDistrictCreate");
        }
        [UserAuthenticationFilter]
        public PartialViewResult GetListDistrictEdit(int ProvinceID, int DistrictID)
        {
            ViewBag.Id = DistrictID;
            ViewBag.ListDistrict = villageBusiness.GetListDistrict(ProvinceID);
            return PartialView("_ListDistrictEdit");
        }
        [UserAuthenticationFilter]
        public int CreateVillage(int proID, int disID, string Name)
        {
            return villageBusiness.CreateVillage(proID, disID, Name);
        }
        [UserAuthenticationFilter]
        public int DeleteVillage(string str)
        {
            return villageBusiness.DeleteVillage(str);
        }
        public PartialViewResult GetDetailVillage(int ID, int Code)
        {
            ViewBag.ListProvince = provinceBusiness.GetListProvince();
            ViewBag.ListDistrict = villageBusiness.GetListDistrict(Code);
            return PartialView("_VillageDetail", villageBusiness.GetDetailVillage(ID));
        }
        public void ChangeListDistrict(int Code)
        {
            ViewBag.ListDistrict = villageBusiness.GetListDistrict(Code);
        }
        [UserAuthenticationFilter]
        public int SaveVillage(string name,int id, int proId, int disId)
        {
            return villageBusiness.SaveVillage(name, id, proId, disId);
        }
    }
}