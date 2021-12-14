using Data.DB;
using Data.Utils;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Data.Model.APIWeb;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Data.Utils.SystemParam;
using System.Web.Http;
using SharpRaven;
using SharpRaven.Data;

namespace Data.Business
{
    public class LoginBusiness : GenericBusiness
    {
        RavenClient ravenClient = new RavenClient(SystemParam.DSN_SENTRY);
        public LoginBusiness(TO_QUOC_GHI_CONGEntities context = null) : base()
        {

        }

        public int LoginWeb(string account, string password)
        {
            try
            {

                 var user = cnn.Users.Where(u => /*u.Phone.Equals(account) ||*/ u.Account.Equals(account) && u.IsActive != 0);
                if (user.Count() > 0)
                {
                    User us = user.FirstOrDefault();
                    Config c = cnn.Configs.Where(d => d.Code == "1").FirstOrDefault();
                    int inum = int.Parse(c.LockTime);
                    if (us.Status == 0)
                    {
                        return SystemParam.ERROR;
                    }
                    else
                    {
                        if (Util.CheckPass(password, us.Password))
                        {
                            if (us.CountLogin.Value >= Convert.ToInt32(c.WrongLoginTime))
                            {
                                TimeSpan span = DateTime.Now.Subtract(us.DayLogin.Value);
                                if (us.DayLogin.Value.Date != DateTime.Now.Date)
                                {
                                    us.CountLogin = 0;
                                }
                                else if (us.DayLogin.Value.Date == DateTime.Now.Date)
                                {
                                    if (span.Hours * 60 + span.Minutes >= Convert.ToInt32(c.LockTime))
                                        us.CountLogin = 0;
                                    else
                                        return 1000;
                                }
                            }
                            string token = Util.CreateMD5(DateTime.Now.ToString());
                            LoginOutputModel data = new LoginOutputModel();
                            data.Account = us.UserName;
                            data.Name = us.UserName;
                            data.Role = us.Role;
                            data.Id = us.UserID;
                            data.Token = token;
                            us.Token = token;
                            data.Status = (int)us.Status;
                            cnn.SaveChanges();
                            HttpContext.Current.Session[Sessions.LOGIN] = data;
                            return SUCCESS;
                        }
                        else
                        {
                            if (us.CountLogin >= Convert.ToInt32(c.WrongLoginTime))
                            {
                                return 1000;
                            }


                            us.CountLogin += 1;
                            if (us.CountLogin == Convert.ToInt32(c.WrongLoginTime))
                                us.DayLogin = DateTime.Now;
                            cnn.SaveChanges();
                            return FAIL_LOGIN;

                        }
                    }

                }
                else
                {
                    return FAIL_LOGIN;
                }
            }
            catch (Exception ex)
            {
                ravenClient.Capture(new SentryEvent(ex));
                return FAIL;
            }
        }



        /// <summary>
        /// Lấy thông tin khách hàng đăng nhập app
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>



        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="item"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
    }
}
