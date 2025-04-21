using System.ComponentModel.DataAnnotations.Schema;
using WorkshopGrantSystem.Models;

namespace WorkshopGrantSystem.Models;

public class Attendance
{
    public int StudentId { get; set; }  // Foreign Key to Student
    public Student Student { get; set; }

    public int WorkshopId { get; set; }  // Foreign Key to Workshop
    public Workshop Workshop { get; set; }
}
