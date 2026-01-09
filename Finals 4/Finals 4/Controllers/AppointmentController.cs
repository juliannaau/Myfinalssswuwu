using Finals_4.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Finals_4.Controllers
{
    [ApiController]
    [Route("/api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly string connStr = "server=localhost;database=attendance;user=root;password=Finals;";

        [HttpGet("clients")]
        public IActionResult GetClients()
        {
            var list = new List<Client>();
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Clients", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Client
                {
                    ClientID = reader.GetInt32("ClientID"),
                    Name = reader.GetString("Name"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                    Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone")
                });
            }
            return Ok(list);
        }

        [HttpGet("services")]
        public IActionResult GetServices()
        {
            var list = new List<Service>();
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Services", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Service
                {
                    ServiceID = reader.GetInt32("ServiceID"),
                    Name = reader.GetString("Name"),
                    DurationMinutes = reader.GetInt32("DurationMinutes")
                });
            }
            return Ok(list);
        }

        [HttpGet("all")]
        public IActionResult GetAppointments()
        {
            var list = new List<Appointment>();
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var sql = @"SELECT a.AppointmentID, a.ClientID, a.ServiceID, a.Date, a.Time, a.Notes,
                               c.Name as ClientName, s.Name as ServiceName, s.DurationMinutes
                        FROM Appointments a
                        JOIN Clients c ON a.ClientID = c.ClientID
                        JOIN Services s ON a.ServiceID = s.ServiceID
                        ORDER BY a.Date DESC, a.Time DESC";
            var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Appointment
                {
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    ClientID = reader.GetInt32("ClientID"),
                    ServiceID = reader.GetInt32("ServiceID"),
                    Date = reader.GetDateTime("Date"),
                    Time = reader.GetTimeSpan("Time").ToString(@"hh\:mm"),
                    Notes = reader.IsDBNull("Notes") ? null : reader.GetString("Notes"),
                    Client = new Client
                    {
                        ClientID = reader.GetInt32("ClientID"),
                        Name = reader.GetString("ClientName")
                    },
                    Service = new Service
                    {
                        ServiceID = reader.GetInt32("ServiceID"),
                        Name = reader.GetString("ServiceName"),
                        DurationMinutes = reader.GetInt32("DurationMinutes")
                    }
                });
            }
            return Ok(list);
        }

        [HttpPost]
        public IActionResult AddAppointments([FromBody] List<Appointment> appointments)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            foreach (var a in appointments)
            {
                var cmd = new MySqlCommand("INSERT INTO Appointments (ClientID, ServiceID, Date, Time, Notes) VALUES (@c, @s, @d, @t, @n)", conn);
                cmd.Parameters.AddWithValue("@c", a.ClientID);
                cmd.Parameters.AddWithValue("@s", a.ServiceID);
                cmd.Parameters.AddWithValue("@d", a.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@t", a.Time);
                cmd.Parameters.AddWithValue("@n", a.Notes ?? "");
                cmd.ExecuteNonQuery();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] Appointment a)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand("UPDATE Appointments SET ClientID=@c, ServiceID=@s, Date=@d, Time=@t, Notes=@n WHERE AppointmentID=@id", conn);
            cmd.Parameters.AddWithValue("@c", a.ClientID);
            cmd.Parameters.AddWithValue("@s", a.ServiceID);
            cmd.Parameters.AddWithValue("@d", a.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@t", a.Time);
            cmd.Parameters.AddWithValue("@n", a.Notes ?? "");
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Appointments WHERE AppointmentID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return Ok();
        }
    }
}
