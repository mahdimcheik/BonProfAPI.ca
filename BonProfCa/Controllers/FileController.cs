using BonProfCa.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonProfCa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IFileService fileService, ILogger<FilesController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit] // Utile pour les gros fichiers
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromQuery] string folder = "uploads"
        )
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier n'a été fourni.");

            try
            {
                using var stream = file.OpenReadStream();
                var fileUrl = await _fileService.UploadFileAsync(stream, file.FileName, folder);

                return Ok(
                    new
                    {
                        Message = "Fichier uploadé avec succès",
                        Url = fileUrl,
                        FileName = file.FileName,
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erreur lors de l'upload du fichier {FileName}",
                    file.FileName
                );
                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string path)
        {
            if (string.IsNullOrEmpty(path))
                return BadRequest("Le chemin du fichier est requis.");

            try
            {
                var (content, contentType) = await _fileService.DownloadFileAsync(path);

                // On retourne le flux directement au navigateur/client Angular
                return File(content, contentType, Path.GetFileName(path));
            }
            catch (HttpRequestException ex)
                when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Le fichier n'existe pas sur le serveur de stockage.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du téléchargement de {Path}", path);
                return StatusCode(500, "Erreur lors de la récupération du fichier.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string path)
        {
            if (string.IsNullOrEmpty(path))
                return BadRequest("Le chemin du fichier est requis.");

            var success = await _fileService.DeleteFileAsync(path);

            if (success)
                return Ok(new { Message = "Fichier supprimé" });

            return NotFound("Le fichier n'a pas pu être trouvé ou supprimé.");
        }
    }
}
