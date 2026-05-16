using Microsoft.AspNetCore.Http;

namespace Interfaces;

public interface IOcrService
{
    Task<OcrResponse> UploadAndProcessAsync(IFormFile file);
    Task<OcrResultDto?> GetOcrResultAsync(string id);
    Task<IEnumerable<OcrResultDto>> GetAllResultsAsync();
}

public record OcrResponse(bool Success, string Message, string? DocumentId);

public record OcrResultDto(
    string Id, 
    string FileName, 
    string Status, 
    string? ExtractedData, 
    DateTime ProcessedAt,
    string Relevance
);
