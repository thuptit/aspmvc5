using System;
using Data.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using APIProject.App_Start;
using Data.Model.APIWeb;

namespace APIProject.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            LoginOutputModel userLogin = (LoginOutputModel)Session[Sessions.LOGIN];
            ViewBag.Role = userLogin.Role;
            ViewBag.Name = userLogin.Name;
            ViewBag.Email = userLogin.Email;
            ViewBag.UserDetail = Context.Users.Find(userLogin.Id);
            
            //lay du lieu ds tinh thanh
            var pro = from p in Context.Provinces
                      where p.IsActive.Value.Equals(1)
                      orderby p.Name
                      select p ;
            ViewBag.Province = pro.ToList();
            var ss = (LoginOutputModel)HttpContext.Session[Sessions.LOGIN];
           
            return View();
        }

       public PartialViewResult Search( int Page, int? Status, int? Type, int? Address , string Name="")
        {

            ViewBag.page = Page;
            ViewBag.name = Name;
            ViewBag.address = Address;
            ViewBag.status = Status;
            ViewBag.type = Type;
            return PartialView("_TableUser", userBusiness.Search(Page, Status, Type, Address, Name));
        }
        //[HttpPost]
        //public int AddUser(string phone, string username)
        //{
        //    return userBusiness.AddUser(phone, username);
        //}
        [UserAuthenticationFilter]
        public int DeleteUser(string str)
        {
            return userBusiness.DeleteUser(str);
        }
        [UserAuthenticationFilter]
        public PartialViewResult GetUserDetail(int ID)
        {
            //lay du lieu ds tinh thanh
            var pro = from p in Context.Provinces
                      where p.IsActive.Value.Equals(1)
                      orderby p.Name
                      select p;
            ViewBag.Province = pro.ToList();
            var ss = (LoginOutputModel)HttpContext.Session[Sessions.LOGIN];
            return PartialView("_UserDetail", userBusiness.GetUserDetail(ID));
        }
        [UserAuthenticationFilter]
        public int EditUser(int? Id, string Name, string Email, string Phone, int Address, int UserType, int UserStatus, string Account)
        {
            return userBusiness.EditUser(Id, Name, Email, Phone, Address, UserType, UserStatus, Account);
        }
        [HttpPost]
        public int ChangePass(string OldPass,string NewPass)
        {
            return userBusiness.ChangePass(OldPass, NewPass);
        }
        
        // Reset User
        [UserAuthenticationFilter]
        public int ResetPass(int UserId)
        {
            return userBusiness.ResetPass(UserId);
        }
        // thêm User
        [UserAuthenticationFilter]
        public int CreateUser(string Name, string Account, string Email, int Address, string Phone, int UserType, string Password)
        {
            try
            {
                UserDetailOutputModel userLogin = UserLogins;
                //if (userLogin.Role != SystemParam.ROLE_ADMIN)
                //{
                //    Session[Sessions.LOGIN] = null;
                //    return SystemParam.ROLE_NOT_USER_ADMIN;
                //}
                //else
                    return userBusiness.CreateUser(Name, Account, Email, Address, Phone, UserType, Password);
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }


        public ActionResult IndexE()
        {
            //lay du lieu ds tinh thanh
            var pro = from p in Context.Provinces
                      where p.IsActive.Value.Equals(1)
                      select p;
            ViewBag.Province = pro.ToList();
            var ss = (LoginOutputModel)HttpContext.Session[Sessions.LOGIN];
            if (ss.Role != SystemParam.ROLE_USER_SUPER_ADMIN)
            { Response.Redirect("/Home/Index"); }
            return View();
        }

    }
}