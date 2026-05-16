using Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Services;

public class OcrService : IOcrService
{
    private readonly IDocumentRepository _repository;
    private readonly ILogger<OcrService> _logger;

    public OcrService(IDocumentRepository repository, ILogger<OcrService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OcrResponse> UploadAndProcessAsync(IFormFile file)
    {
        try
        {
            // 1. Criar registro inicial no MongoDB
            var document = new OcrDocument
            {
                FileName = file.FileName,
                Status = "Pending"
            };

            await _repository.CreateAsync(document);

            // 2. Simular o trigger para os Workers (UploadService -> FileService -> OcrService)
            // Em uma implementação real, aqui enviaríamos uma mensagem para uma fila (RabbitMQ/Service Bus)
            // ou salvaríamos o arquivo em um local monitorado pelos workers.
            _logger.LogInformation($"Documento {document.Id} recebido e pronto para processamento.");

            return new OcrResponse(true, "Documento recebido com sucesso. O processamento iniciará em breve.", document.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar upload de documento.");
            return new OcrResponse(false, "Erro interno ao processar o documento.", null);
        }
    }

    public async Task<OcrResultDto?> GetOcrResultAsync(string id)
    {
        var doc = await _repository.GetByIdAsync(id);
        if (doc == null) return null;

        return MapToDto(doc);
    }

    public async Task<IEnumerable<OcrResultDto>> GetAllResultsAsync()
    {
        var docs = await _repository.GetAllAsync();
        return docs.Select(MapToDto);
    }

    private OcrResultDto MapToDto(OcrDocument doc)
    {
        return new OcrResultDto(
            doc.Id,
            doc.FileName,
            doc.Status,
            doc.ExtractedData,
            doc.ProcessedAt ?? doc.CreatedAt,
            doc.Relevance
        );
    }
}
