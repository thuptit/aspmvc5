using Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Business
{
    public class GenericBusiness
    {
        //public TO_QUOC_GHI_CONGEntities context;
        //public TO_QUOC_GHI_CONGEntities cnn;
        //public ResponseBusiness rpBus = new ResponseBusiness();
        //public GenericBusiness(TO_QUOC_GHI_CONGEntities context = null)
        //{
        //    if (context == null)
        //    {
        //        this.context = new TO_QUOC_GHI_CONGEntities();
        //    }
        //    cnn = this.context;
        //    //this.context.Configuration.AutoDetectChangesEnabled = false;
        //    //this.context.Configuration.ValidateOnSaveEnabled = false;
        //    //this.context.Configuration.LazyLoadingEnabled = false;
        //}
        public TO_QUOC_GHI_CONGEntities context;
        public TO_QUOC_GHI_CONGEntities cnn;
        public ResponseBusiness rpBus = new ResponseBusiness();

        public GenericBusiness(TO_QUOC_GHI_CONGEntities context = null)
        {
            this.context = context == null ? new TO_QUOC_GHI_CONGEntities() : context;
            cnn = this.context;
            //this.context.Configuration.AutoDetectChangesEnabled = false;
            //this.context.Configuration.ValidateOnSaveEnabled = false;
            //this.context.Configuration.LazyLoadingEnabled = false;
        }
    }
}
