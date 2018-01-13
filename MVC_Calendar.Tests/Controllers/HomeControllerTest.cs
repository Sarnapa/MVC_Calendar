using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC_Calendar.Controllers;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using System.Web;
using MVC_Calendar.Models;
using MVC_Calendar.Controllers.Helpers;

namespace MVC_Calendar.Tests.Controllers
{

    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            controller.HttpContext.Session["userID"] = "janusz";
            ViewResult result = controller.Index(null) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            Appointment foundAppointment = _storage.GetAppointment(appointment.AppointmentID);
            Assert.IsNotNull(foundAppointment);
        }

        [TestMethod]
        public void EditAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            appointment.Title = "wyd2";
            controller.EditAppointment(appointment);
            Appointment foundAppointment = _storage.GetAppointment(appointment.AppointmentID);
            Assert.AreEqual(appointment.Title, foundAppointment.Title);
        }

        [TestMethod]
        public void DeleteAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            controller.HttpContext.Session["appointment"] = appointment;
            controller.DeleteAppointment();
            Appointment foundAppointment = _storage.GetAppointment(appointment.AppointmentID);
            Assert.IsNull(foundAppointment);
        }

        [TestMethod]
        public void ModifyModifiedAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            appointment.Title = "wyd2";
            controller.EditAppointment(appointment);
            appointment.Title = "wyd3";
            controller.EditAppointment(appointment);
            Assert.IsNotNull(controller.HttpContext.Session["errorText"]);
        }

        [TestMethod]
        public void ModifyDeletedAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            controller.HttpContext.Session["appointment"] = appointment;
            controller.DeleteAppointment();
            controller.EditAppointment(appointment);
            Assert.IsNotNull(controller.HttpContext.Session["errorText"]);
        }

        [TestMethod]
        public void DeleteModifiedAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            appointment.Title = "wyd2";
            controller.EditAppointment(appointment);
            controller.HttpContext.Session["appointment"] = appointment;
            controller.DeleteAppointment();
            Assert.IsNotNull(controller.HttpContext.Session["errorText"]);
        }

        [TestMethod]
        public void DeleteDeletedAppointmentTest()
        {
            Storage _storage = new Storage();
            TestControllerBuilder builder = new TestControllerBuilder();
            HomeController controller = new HomeController();
            builder.InitializeController(controller);
            var today = DateTime.Today;
            controller.HttpContext.Session["userID"] = "janusz";
            controller.HttpContext.Session["firstDate"] = CalendarService.GetFirstDateOfWeek(today);
            var appointment = CreateAppointment(today);
            controller.AddAppointment(appointment, today);
            controller.HttpContext.Session["appointment"] = appointment;
            controller.DeleteAppointment();
            controller.DeleteAppointment();
            Assert.IsNotNull(controller.HttpContext.Session["errorText"]);
        }

        [TestMethod]
        public void PrevWeekTest()
        {
            HomeController controller = new HomeController();
            var today = DateTime.Today;
            var actionResult = (RedirectToRouteResult)controller.PrevWeek(today);
            Assert.AreEqual(actionResult.RouteValues["date"], today.AddDays(-7));
        }

        [TestMethod]
        public void NextWeekTest()
        {
            HomeController controller = new HomeController();
            var today = DateTime.Today;
            var actionResult = (RedirectToRouteResult)controller.NextWeek(today);
            Assert.AreEqual(actionResult.RouteValues["date"], today.AddDays(7));
        }

        private Appointment CreateAppointment(DateTime day)
        {
            Appointment appointment = new Appointment();
            appointment.Title = "wyd";
            appointment.Description = "opis";
            appointment.StartTime = day.TimeOfDay;
            appointment.EndTime = day.AddMinutes(10).TimeOfDay;
            return appointment;
        }
    }
}
