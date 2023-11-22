using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.DTO;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        const string FILES_DIRECTORY = "filesStorage";

        [HttpGet("download")]
        [Authorize(Roles = "admin, manager")]
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                if (!Directory.Exists(path))
                {
                    return BadRequest("File not found");
                }
                var fileBytes = System.IO.File.ReadAllBytes(Path.Combine(path, fileName));
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("upload")]
        [Authorize(Roles = "admin, manager")]
        public IActionResult UploadFile([FromBody] FileRequestDto fileDto)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileBytes = Convert.FromBase64String(fileDto.FileString);
                System.IO.File.WriteAllBytes(Path.Combine(path, fileDto.FileName), fileBytes);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                if (!Directory.Exists(path))
                {
                    return BadRequest("File not found");
                }
                System.IO.File.Delete(Path.Combine(path, fileName));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
