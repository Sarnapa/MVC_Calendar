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
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Appointment
    {
        public Appointment()
        {
            this.Attendances = new HashSet<Attendance>();
            AppointmentID = Guid.NewGuid();
        }

        [Key]
        public System.Guid AppointmentID { get; set; }
        [StringLength(16, ErrorMessage = "Title can be no longer than 16 characters."), 
        Required(ErrorMessage = "Please enter appointment title.")]
        public string Title { get; set; }
        [StringLength(50, ErrorMessage = "Title can be no longer than 50 characters."), 
        Required(ErrorMessage = "Please enter appointment description.")]
        public string Description { get; set; }
        [Column(TypeName = "date"), Required]
        public System.DateTime AppointmentDate { get; set; }
        [Column(TypeName = "time"), Required(ErrorMessage = "Please enter start time field.")]
        public System.TimeSpan StartTime { get; set; }
        [Column(TypeName = "time"), Required(ErrorMessage = "Please enter end time field.")]
        public System.TimeSpan EndTime { get; set; }
        [Timestamp]
        public byte[] timestamp { get; set; }
    
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
