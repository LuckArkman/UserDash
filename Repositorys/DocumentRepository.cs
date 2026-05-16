using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Repositorys;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<OcrDocument> _collection;

    public DocumentRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("UserDashDb");
        _collection = database.GetCollection<OcrDocument>("ProcessedDocuments");
    }

    public async Task CreateAsync(OcrDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public async Task<OcrDocument?> GetByIdAsync(string id)
    {
        return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<OcrDocument>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task UpdateStatusAsync(string id, string status, string? data, string relevance)
    {
        var update = Builders<OcrDocument>.Update
            .Set(d => d.Status, status)
            .Set(d => d.ExtractedData, data)
            .Set(d => d.Relevance, relevance)
            .Set(d => d.ProcessedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(d => d.Id == id, update);
    }
}
