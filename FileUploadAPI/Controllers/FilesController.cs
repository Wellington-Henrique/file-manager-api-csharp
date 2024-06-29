using FileUploadAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class FilesController : Controller
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IFileService _fileService;

        public FilesController(ILogger<FilesController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var filename = await _fileService.UploadFileAsync(file);
                return Ok(new { Filename = filename });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("upload-multiple")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                var filesName = await _fileService.UploadFilesAsync(files);
                return Ok(new { Filenames = filesName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("download/{filename}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile(string filename)
        {
            try
            {
                var file = await _fileService.GetFileAsync(filename);
                return File(file, "application/octet-stream", filename);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("delete/{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFile(string filename)
        {
            try
            {
                var result = await _fileService.DeleteFileAsync(filename);
                if (result)
                    return Ok(new { Message = "File deleted successfully." });
                else
                    return BadRequest(new { Message = "File could not be deleted." });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
