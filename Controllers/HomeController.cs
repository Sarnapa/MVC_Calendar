using MVC_Calendar.Controllers.Helpers;
using MVC_Calendar.Models;
using MVC_Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;

namespace MVC_Calendar.Controllers
{
    public class HomeController : Controller
    {
        private Storage _storage = new Storage();

        public ActionResult Index(DateTime? date)
        {
            // to disable migration
            Database.SetInitializer<StorageContext>(null);
            // so far
            String userID = "adama";
            Session["userID"] = userID;
            var today = DateTime.Today;
            var firstDate = date ?? CalendarService.GetFirstDateOfWeek(today);
            Session["firstDate"] = firstDate;
            var weeks = GetWeeks(firstDate);
            var model = new CalendarVM
            {
                FirstDay = firstDate,
                Today = today,
                Weeks = weeks,
                UserID = userID
            };
            if(Session["errorText"] != null)
                ViewBag.errorText = Session["errorText"].ToString();
            Session["errorText"] = "";
            return View(model);
        }

        public ActionResult PrevWeek(DateTime day)
        {
            Logger.Log.Info("Get previous week.");
            return RedirectToAction("Index", new { date = day.AddDays(-7) });
        }

        public ActionResult NextWeek(DateTime day)
        {
            Logger.Log.Info("Get next week.");
            return RedirectToAction("Index", new { date = day.AddDays(7) });
        }

        public ActionResult AddAppointment(DateTime? day)
        {
            if (day == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.day = day.Value;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment([Bind(Include = "Title, Description, StartTime, EndTime")]Appointment appointment, DateTime day)
        {
            if (ModelState.IsValid)
            {
                appointment.AppointmentDate = day;
                _storage.CreateAppointment(Session["userID"].ToString(), appointment);
                return RedirectToAction("Index", (DateTime)Session["firstDate"]);
            }
            ViewBag.day = day;
            return View();
        }

        public ActionResult EditAppointment(Guid? appointmentID)
        {
            if(appointmentID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _storage.GetAppointment(appointmentID.Value);
            Session["appointment"] = appointment;
            if(appointment == null)
            {
                return RedirectToAction("Index", (DateTime)Session["firstDate"]);
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointment([Bind(Include = "AppointmentID, AppointmentDate, Title, Description, StartTime, EndTime, timestamp")]Appointment newAppointment)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _storage.UpdateAppointment(newAppointment);
                }
                catch(Exception e)
                {
                    Session["errorText"] = e.Message;
                    return RedirectToAction("Index", (DateTime)Session["firstDate"]);
                }
                return RedirectToAction("Index", (DateTime)Session["firstDate"]);
            }
            return View((Appointment)Session["appointment"]);
        }

        public ActionResult DeleteAppointment()
        {
            try
            {
                _storage.DeleteAppointment((Appointment)Session["appointment"], Session["userID"].ToString());
            }
            catch(Exception e)
            {
                Session["errorText"] = e.Message;
                return RedirectToAction("Index", (DateTime)Session["firstDate"]);
            }
            return RedirectToAction("Index", (DateTime)Session["firstDate"]);
        }

        private List<Week> GetWeeks(DateTime day)
        {
            List<Week> weeks = new List<Week>();
            for(int i = 0; i < 4; ++i)
            {
                var week = new Week
                {
                    Number = CalendarService.GetWeekOfYear(day),
                    Year = CalendarService.GetYear(day),
                    Days = GetDays(day)
                };
                weeks.Add(week);
                day = day.AddDays(7);
            }
            return weeks;
        }

        private List<Day> GetDays(DateTime day)
        {
            List<Day> days = new List<Day>();
            for(int i = 0; i < 7; ++i)
            {
                var newDay = new Day
                {
                    Date = day.Date,
                    Appointments = GetAppointments(day)
                };
                days.Add(newDay);
                day = day.AddDays(1);
            }
            return days;
        }

        private List<Appointment> GetAppointments(DateTime day)
        {
            return _storage.GetDayAppointments(Session["userID"].ToString(), day);
        }

    }
}