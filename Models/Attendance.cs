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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Attendance
    {
        public Attendance()
        {
            AttendanceID = Guid.NewGuid();
        }

        [Key]
        public System.Guid AttendanceID { get; set; }
        [ForeignKey("Appointment")]
        public System.Guid AppointmentID { get; set; }
        [ForeignKey("Person")]
        public System.Guid PersonID { get; set; }
        public bool Accepted { get; set; }
        [Timestamp]
        public byte[] timestamp { get; set; }

        public virtual Appointment Appointment { get; set; }
        public virtual Person Person { get; set; }
    }
}
