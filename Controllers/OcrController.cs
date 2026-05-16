using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class OcrController : ControllerBase
{
    private readonly IOcrService _ocrService;

    public OcrController(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    /// <summary>
    /// Envia um documento para processamento de OCR.
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OcrResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        var result = await _ocrService.UploadAndProcessAsync(file);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Obtém o resultado do OCR de um documento específico.
    /// </summary>
    [HttpGet("results/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OcrResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResult(string id)
    {
        var result = await _ocrService.GetOcrResultAsync(id);
        
        if (result == null)
        {
            return NotFound(new { message = "Resultado não encontrado." });
        }

        return Ok(result);
    }

    /// <summary>
    /// Lista todos os resultados de OCR processados.
    /// </summary>
    [HttpGet("results")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OcrResultDto>))]
    public async Task<IActionResult> GetAllResults()
    {
        var results = await _ocrService.GetAllResultsAsync();
        return Ok(results);
    }
}
