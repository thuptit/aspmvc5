using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DB;
using Data.Model.APIWeb;
using Data.Utils;
using System.Web;
using SharpRaven;
using SharpRaven.Data;
using static Data.Utils.SystemParam;

namespace Data.Business
{
    public class SettingBusiness : GenericBusiness
    {
        RavenClient ravenClient = new RavenClient(SystemParam.DSN_SENTRY);
        public SettingBusiness(TO_QUOC_GHI_CONGEntities context = null) : base()
        {

        }
        public SettingDetailOutputModel ListSetting()
        {
            try
            {
                var q = (from c in cnn.Configs
                         select new SettingDetailOutputModel
                         {
                             Code = /*Int32.Parse(c.Code),*/ c.Code ,
                             Position = c.Position,
                             WrongLoginTime = /*Int32.Parse(c.WrongLoginTime),*/ c.WrongLoginTime,
                             UserDecition = c.UserDecition,
                             Locktime = c.LockTime,
                         }).FirstOrDefault();
                q.Locktime = q.Locktime.Trim();
                return q;
            }
            catch (Exception ex)
            {
                ravenClient.Capture(new SentryEvent(ex));
                return new SettingDetailOutputModel();
            }
        }

        // sửa số lần đăng nhập sai
        public int checkLogin(string ID, string Service, string LockTime)
        {
            try
            {
                var c = cnn.Configs.Where( a => a.Code == ID).FirstOrDefault();
                c.WrongLoginTime = Service;
                c.LockTime = LockTime;
                cnn.SaveChanges();
                return SystemParam.SUCCESS;
            }
            catch(Exception ex)
            {
                ex.ToString();
                return SystemParam.ERROR;
            }
        }

        // sửa tên ng ký
        public int checkPrint(string ID, string People, string Sign)
        {
            try
            {
                var c = cnn.Configs.Where(a => a.Code == ID).FirstOrDefault();
                c.UserDecition= People;
                c.Position = Sign;
                cnn.SaveChanges();
                return SystemParam.SUCCESS;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.ERROR;
            }
        }

    }
}
