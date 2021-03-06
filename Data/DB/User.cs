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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.DecisionCodes = new HashSet<DecisionCode>();
            this.Notifications = new HashSet<Notification>();
            this.Objects = new HashSet<Object>();
            this.Periods = new HashSet<Period>();
            this.Positions = new HashSet<Position>();
            this.Records = new HashSet<Record>();
        }
    
        public int UserID { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int IsActive { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<int> WrongLoginAmount { get; set; }
        public string FullName { get; set; }
        public string ResetPassword { get; set; }
        public Nullable<System.DateTime> ResetPasswordExprice { get; set; }
        public string Account { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CountLogin { get; set; }
        public Nullable<System.DateTime> DayLogin { get; set; }
        public Nullable<System.DateTime> TimeOutLogin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DecisionCode> DecisionCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Object> Objects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Period> Periods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Position> Positions { get; set; }
        public virtual Province Province { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Record> Records { get; set; }
    }
}
