using Data.DB;
using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Business;

namespace APIProject.Controllers
{
    public class BaseController : Controller
    {
        protected TO_QUOC_GHI_CONGEntities Context;
        public LoginBusiness loginBusiness;
        public UserBusiness userBusiness;
        public DecisionCodeBusiness codeBusiness;
        public DistrictBusiness districtBusiness;
        public GroupNewProfileBusiness groupNewProfileBusiness;
        public GroupRenewProfileBusiness groupRenewProfileBusiness;
        public NewProfileBusiness newProfileBusiness;
        public ObjectBusiness objectBusiness;
        public PeriodBusiness periodBusiness;
        public PositionBusiness positionBusiness;
        public ProfilePendingBusiness profilePendingBusiness;
        public ProvinceBusiness provinceBusiness;
        public RenewProfileBusiness renewProfileBusiness;
        public SettingBusiness settingBusiness;
        public VillageBusiness villageBusiness;
        public NotificationBussiness notificationBussiness;
        public ProfileDeclinedBussiness profileDeclinedBussiness;
        public BaseController() : base()
        {
            loginBusiness = new LoginBusiness(this.GetContext());
            userBusiness = new UserBusiness(this.GetContext());
            codeBusiness = new DecisionCodeBusiness(this.GetContext());
            districtBusiness = new DistrictBusiness(this.GetContext());
            groupNewProfileBusiness = new GroupNewProfileBusiness(this.GetContext());
            groupRenewProfileBusiness = new GroupRenewProfileBusiness(this.GetContext());
            newProfileBusiness = new NewProfileBusiness(this.GetContext());
            objectBusiness = new ObjectBusiness(this.GetContext());
            periodBusiness = new PeriodBusiness(this.GetContext());
            positionBusiness = new PositionBusiness(this.GetContext());
            profilePendingBusiness = new ProfilePendingBusiness(this.GetContext());
            provinceBusiness = new ProvinceBusiness(this.GetContext());
            renewProfileBusiness = new RenewProfileBusiness(this.GetContext());
            settingBusiness = new SettingBusiness(this.GetContext());
            villageBusiness = new VillageBusiness(this.GetContext());
            notificationBussiness = new NotificationBussiness(this.GetContext());
            profileDeclinedBussiness = new ProfileDeclinedBussiness(this.GetContext());
        }
        /// <summary>
        /// Create new context if null
        /// </summary>
        public TO_QUOC_GHI_CONGEntities GetContext()
        {
            if (Context == null)
            {
                Context = new TO_QUOC_GHI_CONGEntities();
            }
            return Context;
        }
        public UserDetailOutputModel UserLogins
        {
            get
            {
                return Session[Data.Utils.Sessions.LOGIN] as UserDetailOutputModel; 
            }
        }
    }
}