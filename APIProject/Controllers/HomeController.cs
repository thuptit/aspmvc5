using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Business;
using Data.Utils;
using APIProject.App_Start;
using Newtonsoft.Json;
using SharpRaven;
using SharpRaven.Data;

namespace APIProject.Controllers
{
    public class HomeController : BaseController
    {
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
            ViewBag.Title = "Trang chủ";
            //Role admin
            ViewBag.countRecord = newProfileBusiness.countRecord();
            ViewBag.countPending = newProfileBusiness.countPendingRecord();
            ViewBag.countReject = newProfileBusiness.countRejectRecord();
            ViewBag.countUser = newProfileBusiness.countUser();
            ViewBag.countProfile = newProfileBusiness.countProfile();
            ViewBag.Role = userLogin.Role;
            //Role nhân viên sở
            ViewBag.countGroupRecord = newProfileBusiness.countGroupRecord(userLogin);
            ViewBag.countRenewRecord = newProfileBusiness.countRenewRecord(userLogin);
            ViewBag.countRecordUserPending = newProfileBusiness.countRecordUserPending(userLogin);
            ViewBag.countRecordUserReject = newProfileBusiness.countRecordUserReject(userLogin);

            //Role phòng chính sách
            ViewBag.countRenew1 = newProfileBusiness.countRenew1(userLogin);
            ViewBag.countRenew2 = newProfileBusiness.countRenew2(userLogin);
            ViewBag.countNew1 = newProfileBusiness.countNew1(userLogin);
            ViewBag.countNew2 = newProfileBusiness.countNew2(userLogin);
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Title = "Đăng nhập";
            return View();
        }


        public int UserLogin(string account, string password)
        {
            return loginBusiness.LoginWeb(account, password);
        }

        ///đăng xuất
        public int Logout()
        {
            try
            {
                Session[Sessions.LOGIN] = null;
                return SystemParam.SUCCESS;
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }

        //lưu lại thông tin đối tượng vừa đăng nhập
        [UserAuthenticationFilter]
        public JsonResult GetUserLogin()
        {
            try
            {
                if (Session[Sessions.LOGIN] != null)
                {
                    LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
                    //int? userID = loginBusiness.CheckTokenAdmin(userLogin.Token);
                    //if (String.IsNullOrEmpty(userLogin.Token) || userID == 0)
                    //{
                    //    Session[Sessions.LOGIN] = null;
                    //    userLogin.Role = -1;
                    //}
                    return Json(userLogin, JsonRequestBehavior.AllowGet);
                }
                return Json(new LoginOutputModel(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new LoginOutputModel(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListNotify()
        {
            try
            {
                LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
                return Json(notificationBussiness.GetListNoify(userLogin.Role,userLogin.Id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ex.ToString();
                return Json(new LoginOutputModel(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ReadNotify(int Id)
        {
            LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
            var recordID = notificationBussiness.ReadNotify(Id);
            return Json(new{ Role= userLogin.Role,recordID = recordID}, JsonRequestBehavior.AllowGet);
        }
        public void ReadAllNotify()
        {
            LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
            notificationBussiness.ReadAllNotify(userLogin);
        }
    }
}
