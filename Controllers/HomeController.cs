using MVC_Calendar.Controllers.Helpers;
using MVC_Calendar.Models;
using MVC_Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Calendar.Controllers
{
    public class HomeController : Controller
    {
        private Storage _storage = new Storage();

        public ActionResult Index()
        {
            Database.SetInitializer<StorageContext>(null);
            // so far
            String userID = "janusz"; 
            var today = DateTime.Today;
            var firstDate = CalendarService.GetFirstDateOfWeek(today);
            var weeks = GetWeeks(firstDate, userID);
            var model = new MVC_Calendar.ViewModels.CalendarVM
            {
                FirstDay = firstDate,
                Today = today,
                Weeks = weeks,
                UserID = userID
            };
            return View(model);
        }

        private List<Week> GetWeeks(DateTime day, String userID)
        {
            List<Week> weeks = new List<Week>();
            for(int i = 0; i < 4; ++i)
            {
                var week = new Week
                {
                    Year = day.Year,
                    Number = CalendarService.GetWeekOfYear(day),
                    Days = GetDays(day, userID)
                };
                weeks.Add(week);
                day.AddDays(7);
            }
            return weeks;
        }

        private List<Day> GetDays(DateTime day, String userID)
        {
            List<Day> days = new List<Day>();
            for(int i = 0; i < 7; ++i)
            {
                day.AddDays(i);
                var newDay = new Day
                {
                    Date = day.Date,
                    Appointments = GetAppointments(day, userID)
                };
                Debug.WriteLine("xD: " + newDay.Appointments.Count);
            }
            return days;
        }

        private List<Appointment> GetAppointments(DateTime day, string userID)
        {
            return _storage.GetDayAppointments(userID, day);
        }

    }
}