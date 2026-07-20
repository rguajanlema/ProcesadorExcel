using Microsoft.AspNetCore.Mvc;
using ProcesadorExcel.Application.Exceptions;
using ProcesadorExcel.Application.Services;

namespace ProcesadorExcel.Controllers;

[ApiController]
[Route("api/files")]
public sealed class FilesController : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".xls", ".xlsx" };

    private readonly ExcelProcessorService _processor;
    private readonly ILogger<FilesController> _logger;

    public FilesController(
        ExcelProcessorService processor,
        ILogger<FilesController> logger)
    {
        _processor = processor;
        _logger = logger;
    }

    [HttpPost("process")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProcessAsync(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "Debe enviar un archivo que no esté vacío." });

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
            return BadRequest(new { message = "Solo se permiten archivos .xls o .xlsx." });

        var temporaryPath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}");

        try
        {
            await using (var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            await _processor.ProcessFileAsync(temporaryPath);

            return Ok(new { message = "Archivo procesado correctamente." });
        }
        catch (ExcelValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        finally
        {
            try
            {
                System.IO.File.Delete(temporaryPath);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(
                    exception,
                    "No se pudo eliminar el archivo temporal {TemporaryPath}.",
                    temporaryPath);
            }
        }
    }
}
