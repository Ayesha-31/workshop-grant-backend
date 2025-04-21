using Microsoft.EntityFrameworkCore;
using WorkshopGrantSystem.Models;

namespace WorkshopGrantSystem.Data;

public class WorkshopContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Workshop> Workshops { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    public WorkshopContext(DbContextOptions<WorkshopContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define composite primary key for Attendance
        modelBuilder.Entity<Attendance>()
            .HasKey(a => new { a.StudentId, a.WorkshopId });

        base.OnModelCreating(modelBuilder);
    }
}
