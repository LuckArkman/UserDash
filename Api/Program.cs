using Database.Postgres;
using Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<Interfaces.IOcrService, Api.Services.OcrService>();
builder.Services.AddScoped<Interfaces.IDocumentRepository, Repositorys.DocumentRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

// Ensure PostgreSQL Database is created and migrations applied on startup
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}
catch (Exception ex)
{
    Console.WriteLine($"[STARTUP ERROR] Falha ao criar/verificar o banco Postgres: {ex.Message}");
}

app.Run();