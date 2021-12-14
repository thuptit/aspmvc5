using Data.DB;
using Data.Model.APIWeb;
using Data.Utils;
using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Business
{
    public class NotificationBussiness : GenericBusiness
    {
        RavenClient ravenClient = new RavenClient(SystemParam.DSN_SENTRY);
        public NotificationBussiness(TO_QUOC_GHI_CONGEntities context = null) : base()
        {

        }
        public void PushNoti(int ID, int Status, int UserID)
        {
            try
            {
                //DateTime? fdt = Util.ConvertDate(fromdate);
                //DateTime? tdt = Util.ConvertDate(todate);
                Notification n = new Notification();
                n.UserID = UserID;
                n.IsActive = SystemParam.ACTIVE;
                n.RecordID = ID;
                var r = cnn.Records.Find(ID);
                string contextstatus = "";
                if (Status == SystemParam.STATUS_ACCEPTED_RECORD)
                    contextstatus = "đã xác nhận";
                else if (Status == SystemParam.STATUS_REJECT_RECORD)
                    contextstatus = "đã từ chối";
                else if (Status == SystemParam.STATUS_GROUP_GAVE_PRIMEMINISTER)
                    contextstatus = "đã trình thử tướng";
                n.Content = "Hồ sơ " + r.Number + " " + contextstatus;
                n.IsReaded = SystemParam.SEND;
                n.CreadedDate = DateTime.Now;
                return;
            }
            catch (Exception ex)
            {
                ravenClient.Capture(new SentryEvent(ex));
                return;
            }
        }
        public CountNotify GetListNoify(int Role, int userId)
        {
            try
            {
                CountNotify cn = new CountNotify();
                var query = (from c in cnn.Notifications
                             join g in cnn.Groups on c.GroupID equals g.ID
                             where c.IsActive == SystemParam.ACTIVE
                             orderby c.CreadedDate descending
                             select new NotifyModel()
                             {
                                 ID = c.ID,
                                 Content = c.Content,
                                 IsReaded = c.IsReaded,
                                 CreadedDate = c.CreadedDate,
                                 statusRecord = (c.RecordID != null ? c.Record.Status : 0),
                                 RecordID = c.RecordID,
                                 GroupID = c.GroupID,
                                 userCreateGroup = g.UserCreateID
                             }).ToList();
                if (Role == SystemParam.ROLE_ADMIN)
                {
                    List<NotifyModel> lst = query.Where(n => n.RecordID == null).ToList();
                    cn.countNoti = lst.Where(n => n.IsReaded == 0).Count();
                    cn.listNotifyModel = lst;
                    return cn;
                }
                if (Role == SystemParam.ROLE_USER_DEPARTMENT)
                {
                    List<NotifyModel> lst = query.Where(n => n.RecordID != null && n.userCreateGroup == userId).ToList();
                    cn.countNoti = lst.Where(n => n.IsReaded == 0).Count();
                    cn.listNotifyModel = lst;
                    return cn;
                }
                return new CountNotify();
            }

            catch (Exception ex)
            {
                ex.ToString();
                return new CountNotify();
            }
        }
        public int? ReadNotify(int Id)
        {
            var noti = cnn.Notifications.Find(Id);
            noti.IsReaded = 1;
            var recordId = noti.RecordID != null? noti.RecordID: 0;
            cnn.SaveChanges();
            return recordId;
        }
        public void ReadAllNotify(LoginOutputModel userLogin)
        {
            if(userLogin.Role == SystemParam.ROLE_ADMIN)
            {
                var lstNoti = cnn.Notifications.Where(p => p.IsActive == SystemParam.ACTIVE&&p.RecordID==null).ToList();
                lstNoti.ForEach(a => a.IsReaded = 1);
                cnn.SaveChanges();
            }
            if (userLogin.Role == SystemParam.ROLE_USER_DEPARTMENT)
            {
                var lstNoti = cnn.Notifications.Where(p => p.IsActive == SystemParam.ACTIVE && p.Record.UserCreateID == userLogin.Id).ToList();
                lstNoti.ForEach(a => a.IsReaded = 1);
                cnn.SaveChanges();
            }
        }
    }
}