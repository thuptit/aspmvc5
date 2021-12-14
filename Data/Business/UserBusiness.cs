using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DB;
using Data.Model.APIWeb;
using Data.Utils;
using System.Web;
using System.Data.Entity.Validation;
using PagedList;
using static Data.Utils.SystemParam;

namespace Data.Business
{
    public class UserBusiness : GenericBusiness
    {

        public UserBusiness(TO_QUOC_GHI_CONGEntities context = null) : base()
        {

        }
        public IPagedList<UserDetailOutputModel> Search(int Page, int? Status, int? Type, int? Address, string Name = "")
        {
            try
            {
                var query = (from u in cnn.Users
                             where u.IsActive.Equals(SystemParam.ACTIVE)
                            && (!String.IsNullOrEmpty(Name) ? (u.Phone.Contains(Name) || u.UserName.Contains(Name)) : true)
                            && (Status.HasValue ? u.Status.Value.Equals(Status.Value) : true)
                            && (Address.HasValue ? u.ProvinceID.Value.Equals(Address.Value) : true)
                            && (Type.HasValue ? u.Role.Equals(Type.Value) : true)
                             orderby u.CreatedDate descending
                             select new UserDetailOutputModel
                             {
                                 Id = u.UserID,
                                 Account = u.Account,
                                 Status = u.Status.Value,
                                 Phone = u.Phone,
                                 Address = u.ProvinceID,
                                 NameA = u.ProvinceID.HasValue ? u.Province.Name : "",
                                 Role = u.Role,
                                 Email = u.Email,
                                 CreatedDate = u.CreatedDate

                             }).ToPagedList(Page, SystemParam.MAX_ROW_IN_LIST);
                var a = query.ToList();
                return query;
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<UserDetailOutputModel>().ToPagedList(1, 1); ;
            }
        }

        //public dynamic GetUserDetail()
        //{
        //    throw new NotImplementedException();
        //}



        //public int AddUser(string phone, string username)
        //{
        //    try
        //    {
        //        LoginOutputModel session = (LoginOutputModel)HttpContext.Current.Session[Sessions.LOGIN];

        //        if (cnn.Users.Where(u => u.UserName == phone && u.IsActive == ACTIVE).Count() > 0)
        //        {
        //            return USER_EXIST;
        //        }
        //        else if (session.Role != ROLE_USER_SUPER_ADMIN)
        //        {
        //            return NO_ROLE_SUPER_ADMIN;
        //        }
        //        else
        //        {
        //            User us = new User();
        //            us.IsActive = ACTIVE;
        //            us.CreatedDate = DateTime.Now;
        //            us.Role = ROLE_USER_ADMIN;
        //            us.Password = Util.CreateMD5(DEFAULT_PASSWORD);
        //            cnn.Users.Add(us);
        //            cnn.SaveChanges();
        //            return SUCCESS;
        //        }
        //    }
        //    catch
        //    {
        //        return ERROR;
        //    }
        //}


        public int DeleteUser(string str)
        {
            try
            {
                string[] s = str.Split(',');
                for (int i = 0; i < s.Length - 1; i++)
                {
                    User u = cnn.Users.Find(Int32.Parse(s[i]));
                    u.IsActive = SystemParam.NO_ACTIVE;
                    cnn.SaveChanges();
                }
                return SystemParam.SUCCESS;
            }
            catch (Exception ex)
            {
                return SystemParam.ERROR;
            }
        }

        // lấy TT user lên Modal
        public UserDetailOutputModel GetUserDetail(int ID)
        {
            try
            {
                UserDetailOutputModel ud = new UserDetailOutputModel();
                User c = cnn.Users.Find(ID);
                ud.Id = c.UserID;
                ud.Name = c.UserName;
                ud.IsActive = c.IsActive;
                ud.Email = c.Email;
                ud.Role = c.Role;
                ud.Account = c.Account;
                ud.Status = c.Status.Value;
                ud.Phone = c.Phone;
                ud.NameA = c.ProvinceID.HasValue ? c.Province.Name : "";
                ud.Address = c.ProvinceID;
                return ud;

            }
            catch
            {
                return new UserDetailOutputModel();
            }
        }

        // sửa User
        public int EditUser(int? Id, string Name, string Email, string Phone, int Address, int UserType, int UserStatus, string Account)
        {
            try
            {
                var ListPro = (from u in cnn.Users
                               where u.IsActive.Equals(SystemParam.ACTIVE) && u.ProvinceID == Address
                               select u.ProvinceID).ToList();
                //if (UserType == 3)
                //{
                //    if (ListPro.Contains(Address))
                //    {
                //        return 2000;
                //    }
                //}
                if (UserType == SystemParam.ROLE_USER_DEPARTMENT)
                {
                    var checkData = cnn.Users.Where(m => m.ProvinceID.Value.Equals(Address) && m.IsActive.Equals(SystemParam.ACTIVE)).Count();
                    if (checkData == 1)
                    {
                        return 2000;
                    }
                }
                User user = cnn.Users.Find(Id);
                user.UserName = Name;
                user.Email = Email;
                user.Status = UserStatus;
                user.Role = UserType;
                user.Phone = Phone;
                user.ProvinceID = Address;
                user.Account = Account;
                cnn.SaveChanges();

                return SUCCESS;

            }
            catch
            {
                return ERROR;
            }
        }

        public int ChangePass(string OldPass, string NewPass)
        {
            try
            {
                LoginOutputModel session = (LoginOutputModel)HttpContext.Current.Session[Sessions.LOGIN];
                var user = cnn.Users.Find(session.Id);
                if (user.Password == (Util.CreateMD5(OldPass)))
                {
                    NewPass = Util.CreateMD5(NewPass);
                    user.Password = NewPass;
                    cnn.SaveChanges();
                    HttpContext.Current.Session[Sessions.LOGIN] = null;
                    return SUCCESS;
                }
                else
                {
                    return ERROR_PASSWORD;
                }
            }
            catch
            {
                return ERROR;

            }
        }

        // Reset Mật khẩu
        public int ResetPass(int UserId)
        {
            try
            {
                var user = cnn.Users.Find(UserId);
                user.Password = Util.GenPass(user.Phone);
                cnn.SaveChanges();
                return SystemParam.SUCCESS;
                //if(user.IsActive==SystemParam.ACTIVE)
                //{
                //    user.Password = Util.CreateMD5(SystemParam.DEFAULT_PASSWORD);
                //    cnn.SaveChanges();
                //    return SystemParam.SUCCESS;

                //}    
                //else
                //{
                //  return  SystemParam.ERROR;
                //}    
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }

        //check trùng tên User
        public Boolean CheckDuplicateNameUser(string Account)
        {
            try
            {
                var user = cnn.Users.Where(u => u.Account.Equals(Account) && u.IsActive.Equals(SystemParam.ACTIVE)).ToList();
                if (user != null && user.Count() > 0)
                {
                    return SystemParam.BOOLEAN_TRUE;
                }
                return SystemParam.BOOLEAN_FALSE;
            }
            catch
            {
                return SystemParam.BOOLEAN_FALSE;
            }
        }

        // thêm User
        public int CreateUser(string Name, string Account, string Email, int Address, string Phone, int UserType, string Password)
        {
            try
            {
                //var create = cnn.Users.Where(u => u.IsActive.Equals(SystemParam.ACTIVE));
                //if (create != null && create.Count() > 0)
                //{
                //    return ;
                //}
                if (UserType == SystemParam.ROLE_USER_DEPARTMENT)
                {
                    var checkData = cnn.Users.Where(m => m.ProvinceID.Value.Equals(Address) && m.IsActive.Equals(SystemParam.ACTIVE)).Count();
                    if (checkData != 0)
                    {
                        return SystemParam.CHECK_COINCIDENT;
                    }
                }
                if (CheckDuplicateNameUser(Account))
                {
                    return SystemParam.DUPLICATE_NAME;
                }
                User us = new User();
                //var ListPro = (from u in cnn.Users
                //               where u.IsActive.Equals(SystemParam.ACTIVE) && u.ProvinceID == Address
                //               select u.ProvinceID).ToList();
                var checkList = (from u in cnn.Users
                                 where u.IsActive.Equals(1) && u.Status.Value.Equals(3) && u.ProvinceID.Value.Equals(Address) && u.Province.IsActive.Value.Equals(1)
                                 select u).Count();

                if (checkList >= 1) return 3;
                us.Role = UserType;
                us.UserName = Name;
                us.Account = Account;
                us.Email = Email;
                us.ProvinceID = Address;
                us.Phone = Phone;
                us.Password = Util.GenPass(Password);
                us.CreatedDate = DateTime.Now;
                us.Token = "";
                us.CountLogin = 0;
                us.IsActive = SystemParam.ACTIVE;
                us.Status = 1;
                cnn.Users.Add(us);
                cnn.SaveChanges();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return ERROR;
            }
        }

        private bool CheckDuplicateBatchCode(object batchCode)
        {
            throw new NotImplementedException();
        }
    }
}
