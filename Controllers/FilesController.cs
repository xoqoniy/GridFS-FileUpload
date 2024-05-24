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
                var uploadedFile = await _fileService.UploadFileAsync(stream, file.FileName, file.ContentType);
                return Ok(new { FileId = uploadedFile.FileId, FileName = uploadedFile.Name });
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
                var file = await _fileService.DownloadFileAsync(id);
                if (file == null || file.Content == null)
                {
                    return NotFound("File not found.");
                }

                return File(file.Content, file.ContentType, file.Name);
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
