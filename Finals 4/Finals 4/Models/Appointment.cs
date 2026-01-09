namespace Finals_4.Models;

public class Client
{
    public int ClientID { get; set; }
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class Service
{
    public int ServiceID { get; set; }
    public string Name { get; set; } = "";
    public int DurationMinutes { get; set; }
}

public class Appointment
{
    public int AppointmentID { get; set; }
    public int ClientID { get; set; }
    public int ServiceID { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string? Notes { get; set; }

    public Client? Client { get; set; }
    public Service? Service { get; set; }
}
