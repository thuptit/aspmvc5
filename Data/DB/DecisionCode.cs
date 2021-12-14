//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class DecisionCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DecisionCode()
        {
            this.Records = new HashSet<Record>();
            this.RecordUpdateHistories = new HashSet<RecordUpdateHistory>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> fDate { get; set; }
        public Nullable<int> fMonth { get; set; }
        public Nullable<int> fYear { get; set; }
        public Nullable<int> tDate { get; set; }
        public Nullable<int> tMonth { get; set; }
        public Nullable<int> tYear { get; set; }
        public int UserCreateID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Record> Records { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecordUpdateHistory> RecordUpdateHistories { get; set; }
    }
}