namespace Schedoo.Server.Services.Models;
public class Semester
{
    public int Id { get; set; }
    public string Description { get; set; }
    
    public DateTime StartDay { get; set; }
    public DateTime EndDay { get; set; }

    public bool CurrentSemester { get; set; }
}