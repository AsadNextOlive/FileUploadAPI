using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TestRegisterAPI.Data;
using TestRegisterAPI.Model;
using Microsoft.AspNetCore.StaticFiles;

namespace TestRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetFile/{id}")]
        public IActionResult GetFile(int id)
        {
            var uploadContent = _context.UploadContent.Find(id);

            if (uploadContent == null)
            {
                return NotFound("File not found");
            }

            var filePath = uploadContent.FileUpload;

            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/png"); // Set the appropriate content type
            }
            else
            {
                return NotFound("File not found on disk");
            }
        }

        //[HttpGet]
        //[Route("GetFile/{id}")]
        //public IActionResult GetFile(int id)
        //{
        //    var uploadContent = _context.UploadContent.Find(id);

        //    if (uploadContent == null)
        //    {
        //        return NotFound("File not found");
        //    }

        //    var filePath = uploadContent.FileUpload;

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        //    }
        //    else
        //    {
        //        return NotFound("File not found on disk");
        //    }
        //}

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.').LastOrDefault();
                filename = $"{DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)}{extension}";

                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\Files");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, filename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save the full file path to the database
                SaveFilePathToDatabase(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Not Stored in the Database. Error: {ex.Message}");
            }
            return filename;
        }

        private void SaveFilePathToDatabase(string filePath)
        {
            try
            {
                var uploadContent = new UploadContent
                {
                    FileUpload = filePath
                };

                _context.UploadContent.Add(uploadContent);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not Stored into the Datbase");
            }
        }
    }
}
