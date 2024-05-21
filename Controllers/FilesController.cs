using FilesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is null or empty.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                var fileId = await _fileService.UploadFileAsync(stream, file.FileName);
                return Ok(new { FileId = fileId, FileName = file.FileName });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("File ID is null or empty.");
            }

            try
            {
                var stream = await _fileService.DownloadFileAsync(id);
                if (stream == null)
                {
                    return NotFound("File not found.");
                }

                // Assuming you want to return the file with its original name
                var contentType = "application/octet-stream"; // Adjust according to your needs
                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error downloading file: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("File ID is null or empty.");
            }

            try
            {
                await _fileService.DeleteFileAsync(id);
                return Ok(new { Message = "File deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting file: {ex.Message}");
            }
        }
    }
}
