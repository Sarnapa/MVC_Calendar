using MVC_Calendar.Controllers.Helpers;
using MVC_Calendar.Models;
using MVC_Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Web.Mvc;

namespace MVC_Calendar.Controllers
{
    public class HomeController : Controller
    {
        private Storage _storage = new Storage();

        public ActionResult Index(DateTime? date)
        {
            Database.SetInitializer<StorageContext>(null);
            // so far
            String userID = "baby"; 
            var today = DateTime.Today;
            var firstDate = date ?? CalendarService.GetFirstDateOfWeek(today);
            var weeks = GetWeeks(firstDate, userID);
            var model = new CalendarVM
            {
                FirstDay = firstDate,
                Today = today,
                Weeks = weeks,
                UserID = userID
            };
            return View(model);
        }

        public ActionResult PrevWeek(DateTime day)
        {
            return RedirectToAction("Index", new { date = day.AddDays(-7) });
        }

        public ActionResult NextWeek(DateTime day)
        {
            return RedirectToAction("Index", new { date = day.AddDays(7) });
        }

        private List<Week> GetWeeks(DateTime day, String userID)
        {
            List<Week> weeks = new List<Week>();
            for(int i = 0; i < 4; ++i)
            {
                var week = new Week
                {
                    Number = CalendarService.GetWeekOfYear(day),
                    Year = CalendarService.GetYear(day),
                    Days = GetDays(day, userID)
                };
                weeks.Add(week);
                day = day.AddDays(7);
            }
            return weeks;
        }

        private List<Day> GetDays(DateTime day, String userID)
        {
            List<Day> days = new List<Day>();
            for(int i = 0; i < 7; ++i)
            {
                var newDay = new Day
                {
                    Date = day.Date,
                    Appointments = GetAppointments(day, userID)
                };
                days.Add(newDay);
                day = day.AddDays(1);
            }
            return days;
        }

        private List<Appointment> GetAppointments(DateTime day, String userID)
        {
            return _storage.GetDayAppointments(userID, day);
        }

    }
}