using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WorkshopGrantSystem; 


namespace WorkshopGrantSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ExcelImporter _excelImporter;

        public UploadController(ExcelImporter excelImporter)
        {
            _excelImporter = excelImporter;
        }

        [HttpPost("excel")]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("⚠️ No file uploaded.");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    _excelImporter.ImportStudentsAndWorkshops(stream);
                }

                return Ok("✅ Excel file uploaded and processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error processing file: {ex.Message}");
            }
        }
    }
}
