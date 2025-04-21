using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopGrantSystem.Data;
using WorkshopGrantSystem.Models;
using WorkshopGrantSystem;
using OfficeOpenXml;




var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


// Add database connection
builder.Services.AddDbContext<WorkshopContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services
builder.Services.AddScoped<ExcelImporter>();             // ✅ Excel importer service
builder.Services.AddControllers();                       // ✅ Enable controller-based APIs

// Enable CORS for frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500")   // Adjust this as needed
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine("🚀 Workshop Grant System API is Starting...");

// Apply database migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkshopContext>();
    context.Database.EnsureCreated();
    Console.WriteLine("✅ Database initialized.");
}

// Optional: Initial static Excel import at startup
try
{
    Console.WriteLine("🔍 Starting Excel Import...");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<WorkshopContext>();
        var importer = new ExcelImporter(context);
        importer.ImportStudentsAndWorkshops("data.xlsx");
    }
    Console.WriteLine("✅ Excel Import Completed.");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Excel Import Failed: {ex.Message}");
}

// Configure the HTTP request pipeline
app.UseCors("AllowFrontend"); // Apply CORS policy
app.UseStaticFiles();         // Serve static frontend if any

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workshop Grant System API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// ✅ Enable attribute-based controller routes
app.MapControllers();

// ✅ Minimal API Endpoints (Students, Workshops, Eligibility)
app.MapGet("/", () => "Workshop Grant System API is running!");

// 🚀 STUDENTS
app.MapGet("/students", async (WorkshopContext db) =>
    await db.Students.ToListAsync());

app.MapGet("/students/{id}", async (int id, WorkshopContext db) =>
    await db.Students.FindAsync(id) is Student student
        ? Results.Ok(student)
        : Results.NotFound());

app.MapPost("/students", async (Student student, WorkshopContext db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{student.StudentId}", student);
});

app.MapPut("/students/{id}", async (int id, Student updatedStudent, WorkshopContext db) =>
{
    var student = await db.Students.FindAsync(id);
    if (student == null) return Results.NotFound();

    student.Name = updatedStudent.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/students/{id}", async (int id, WorkshopContext db) =>
{
    var student = await db.Students.FindAsync(id);
    if (student == null) return Results.NotFound();

    db.Students.Remove(student);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 🚀 WORKSHOPS
app.MapGet("/workshops", async (WorkshopContext db) =>
    await db.Workshops.ToListAsync());

app.MapGet("/workshops/{id}", async (int id, WorkshopContext db) =>
    await db.Workshops.FindAsync(id) is Workshop workshop
        ? Results.Ok(workshop)
        : Results.NotFound());

app.MapPost("/workshops", async (Workshop workshop, WorkshopContext db) =>
{
    db.Workshops.Add(workshop);
    await db.SaveChangesAsync();
    return Results.Created($"/workshops/{workshop.WorkshopId}", workshop);
});

app.MapPut("/workshops/{id}", async (int id, Workshop updatedWorkshop, WorkshopContext db) =>
{
    var workshop = await db.Workshops.FindAsync(id);
    if (workshop == null) return Results.NotFound();

    workshop.Title = updatedWorkshop.Title;
    workshop.Date = updatedWorkshop.Date;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/workshops/{id}", async (int id, WorkshopContext db) =>
{
    var workshop = await db.Workshops.FindAsync(id);
    if (workshop == null) return Results.NotFound();

    db.Workshops.Remove(workshop);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 🚀 GRANT ELIGIBILITY
app.MapGet("/eligibility/student/{id}", async (int id, WorkshopContext db) =>
{
    var student = await db.Students.FindAsync(id);
    if (student == null) return Results.NotFound("Student not found.");

    var workshops = await db.Attendances
        .Where(a => a.StudentId == id)
        .Select(a => new { a.Workshop.WorkshopId, a.Workshop.Title, a.Workshop.Date })
        .ToListAsync();

    int workshopCount = workshops.Count;
    bool isEligible = workshopCount >= 3;

    return Results.Ok(new
    {
        StudentId = id,
        Name = student.Name,
        WorkshopCount = workshopCount,
        IsEligible = isEligible,
        WorkshopsAttended = workshops
    });
});

// Optional: Keep Printing API is alive
Task.Run(async () =>
{
    while (true)
    {
        Console.WriteLine("🟢 API is still running...");
        await Task.Delay(5000);
    }
});

app.Run();
