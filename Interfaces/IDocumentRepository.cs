namespace Interfaces;

public interface IDocumentRepository
{
    Task CreateAsync(OcrDocument document);
    Task<OcrDocument?> GetByIdAsync(string id);
    Task<IEnumerable<OcrDocument>> GetAllAsync();
    Task UpdateStatusAsync(string id, string status, string? data, string relevance);
}

public class OcrDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FileName { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public string? ExtractedData { get; set; }
    public string Relevance { get; set; } = "Low";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}
