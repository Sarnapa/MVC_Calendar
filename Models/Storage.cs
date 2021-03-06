﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;

namespace MVC_Calendar.Models
{
    public class Storage
    {

        public List<Appointment> GetDayAppointments(String userID, DateTime day)
        {
            using (var db = new StorageContext())
            {
                var dayAllAppointments = db.Appointments.Where(e => e.AppointmentDate == day).ToList();
                var appointments = dayAllAppointments.Where(e => e.Attendances.Where(a => a.Person.UserID == userID).ToList().Count != 0).ToList();
                return appointments.OrderBy(a => a.StartTime).ToList();
            }
        }

        public Appointment GetAppointment(Guid appointmentID)
        {
            using (var db = new StorageContext())
            {
                return db.Appointments.Find(appointmentID);
            }     
        }

        public Guid GetPersonID(String userID)
        {
            using (var db = new StorageContext())
            {
                return db.People.Where(p => p.UserID == userID).ToList().FirstOrDefault().PersonID;
            }
        }

        public void CreateAppointment(String userID, Appointment appointment)
        {
            using (var db = new StorageContext())
            {
                db.Appointments.Add(appointment);
                Guid personID = GetPersonID(userID);
                Attendance attendance = new Attendance();
                attendance.PersonID = personID;
                attendance.AppointmentID = appointment.AppointmentID;
                db.Attendances.Add(attendance);
                db.SaveChanges();
                Logger.Log.Info("Saved new appointment to database.");
            }
        }

        public void UpdateAppointment(Appointment appointment)
        {
            using (var db = new StorageContext())
            {
                var original = db.Appointments.Find(appointment.AppointmentID);
                if (original != null)
                {
                    original.AppointmentDate = appointment.AppointmentDate;
                    original.StartTime = appointment.StartTime;
                    original.EndTime = appointment.EndTime;
                    original.Title = appointment.Title;
                    original.Description = appointment.Description;
                    db.Entry(original).OriginalValues["timestamp"] = appointment.timestamp;
                    try
                    {
                        db.SaveChanges();
                        Logger.Log.Info("Saved modified appointment to database.");
                    }
                    // ktos wczesniej nadpisal dane, a my ich nie pobralismy - nie akceptujemy naszej zmiany
                    catch (DbUpdateConcurrencyException)
                    {
                        Logger.Log.Error("Modifying appointment failure due to overwriting data by another user.");
                        throw new Exception("Modifying appointment failure due to overwriting data by another user.");
                    }
                }
                else
                {
                    Logger.Log.Error("Modifying appointment failure due to removing appointment by another user.");
                    throw new Exception("Modifying appointment failure due to removing appointment by another user.");
                }

            }
        }

        public void DeleteAppointment(Appointment appointment, String userID)
        {
            using (var db = new StorageContext())
            {
                var original = db.Appointments.Find(appointment.AppointmentID);
                if (original != null)
                {
                    var allAttendances = db.Attendances.Where(a => a.AppointmentID.Equals(appointment.AppointmentID));
                    var originalAttendance = allAttendances.FirstOrDefault(a => a.Person.UserID.Equals(userID));
                    if(originalAttendance != null)
                        db.Attendances.Remove(originalAttendance);
                    if(allAttendances.Count() <= 1)
                        db.Appointments.Remove(original);
                    db.Entry(original).OriginalValues["timestamp"] = appointment.timestamp;
                    try
                    {
                        db.SaveChanges();
                        Logger.Log.Info("Remove appointment from database.");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Logger.Log.Error("Removing appointment failure due to overwriting data by another user.");
                        throw new Exception("Removing appointment failure due to overwriting data by another user.");
                    }
                }
                else
                {
                    Logger.Log.Error("Removing appointment failure due to removing appointment by another user.");
                    throw new Exception("Removing appointment failure due to removing appointment by another user.");
                }

            }
        }
    }
}