using OfficeOpenXml;
using WorkshopGrantSystem.Data;
using WorkshopGrantSystem.Models;
using System;
using System.IO;
using System.Linq;

public class ExcelImporter
{
    private readonly WorkshopContext _context;

    public ExcelImporter(WorkshopContext context)
    {
        _context = context;
    }

    // ✅ Old method (keep it if you're still loading from file path)
    public void ImportStudentsAndWorkshops(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            ProcessWorkbook(package);
        }
    }

    // ✅ New method for real-time upload from frontend (using stream)
    public void ImportStudentsAndWorkshops(Stream fileStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(fileStream))
        {
            ProcessWorkbook(package);
        }
    }

    // 🔁 Common logic extracted here
    private void ProcessWorkbook(ExcelPackage package)
    {
        Console.WriteLine("=== Debug: Excel File Processing ===");
        Console.WriteLine($"Total Sheets Found: {package.Workbook.Worksheets.Count}");

        foreach (var sheet in package.Workbook.Worksheets)
        {
            Console.WriteLine($"Checking Sheet: {sheet.Name}");

            if (sheet.Dimension == null)
            {
                Console.WriteLine($"Skipping empty sheet: {sheet.Name}");
                continue;
            }

            string workshopTitle = sheet.Name.Trim();
            Console.WriteLine($"Processing Workshop: {workshopTitle}");

            var workshop = _context.Workshops.FirstOrDefault(w => w.Title == workshopTitle);
            if (workshop == null)
            {
                workshop = new Workshop { Title = workshopTitle, Date = DateTime.Now };
                _context.Workshops.Add(workshop);
                _context.SaveChanges();
                Console.WriteLine($"Added Workshop: {workshop.WorkshopId} - {workshop.Title}");
            }

            int rowCount = sheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                string studentName = sheet.Cells[row, 1].Text.Trim();
                string studentIdText = sheet.Cells[row, 2].Text.Trim();

                if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(studentIdText))
                {
                    Console.WriteLine($"Skipping empty row {row} in {sheet.Name}");
                    continue;
                }

                if (!int.TryParse(studentIdText, out int studentId))
                {
                    Console.WriteLine($"Skipping invalid Student ID {studentIdText} in {sheet.Name}");
                    continue;
                }

                var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);
                if (student == null)
                {
                    student = new Student { StudentId = studentId, Name = studentName };
                    _context.Students.Add(student);
                    _context.SaveChanges();
                    Console.WriteLine($"Added Student: {student.StudentId} - {student.Name}");
                }

                bool attendanceExists = _context.Attendances
                    .Any(a => a.StudentId == studentId && a.WorkshopId == workshop.WorkshopId);

                if (!attendanceExists)
                {
                    _context.Attendances.Add(new Attendance
                    {
                        StudentId = studentId,
                        WorkshopId = workshop.WorkshopId
                    });
                    _context.SaveChanges();
                    Console.WriteLine($"Recorded Attendance: Student {student.StudentId} -> Workshop {workshop.WorkshopId}");
                }
            }
        }
    }
}
