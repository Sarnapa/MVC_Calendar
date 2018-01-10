using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Calendar.ViewModels
{
    public class Week
    {
        public int Year { get; set; }
        public int Number { get; set; }
        public List<Day> Days { get; set; }
    }
}