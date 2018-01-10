//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Calendar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Person
    {
        public Person()
        {
            this.Attendances = new HashSet<Attendance>();
        }

        [Key]
        public System.Guid PersonID { get; set; }
        [MaxLength(16), Required]
        public string FirstName { get; set; }
        [MaxLength(16), Required]
        public string LastName { get; set; }
        [MaxLength(10), Required]
        public string UserID { get; set; }
        [Timestamp]
        public byte[] timestamp { get; set; }
    
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}