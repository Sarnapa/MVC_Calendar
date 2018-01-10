using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Calendar.ViewModels
{
    public class CalendarVM
    {
        public DateTime FirstDay { get; set; }
        public DateTime Today { get; set; }
        public List<Week> Weeks { get; set; }
        public String UserID { get; set; }
    }
}