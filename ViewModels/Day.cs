using MVC_Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVC_Calendar.ViewModels
{
    public class Day
    {
        public DateTime Date { get; set; }
        public String DayText
        {
            get
            {
                return Date.ToString("dd MMMM", CultureInfo.CreateSpecificCulture("en-GB"));
            }
        }
        public List<Appointment> Appointments { get; set; }

        public Day() 
        {
            Appointments = new List<Appointment>();
        }
    }
}