# UserDash (.NET 8)

RepositÃ³rio estruturado como uma soluÃ§Ã£o `.NET` (Visual Studio/JetBrains Rider) com mÃºltiplos projetos (`src.sln`), separando API, serviÃ§os em background (workers) e bibliotecas (camadas de domÃ­nio/infra).

> ObservaÃ§Ã£o: parte dos projetos (ex.: `Controllers`, `Dtos`, `Database`, `Repositorys`) ainda nÃ£o possui classes/arquivos `.cs` prÃ³prios â€” no estado atual, contÃªm principalmente o `.csproj` e artefatos de build (`bin/` e `obj/`).

## Estrutura do repositÃ³rio

```
.
â”œâ”€ src.sln
â”œâ”€ Api/
â”‚  â”œâ”€ Api.csproj
â”‚  â”œâ”€ Program.cs
â”‚  â”œâ”€ Api.http
â”‚  â”œâ”€ appsettings.json
â”‚  â”œâ”€ appsettings.Development.json
â”‚  â””â”€ Properties/
â”‚     â””â”€ launchSettings.json
â”œâ”€ OcrService/
â”‚  â”œâ”€ OcrService.csproj
â”‚  â”œâ”€ Program.cs
â”‚  â”œâ”€ Worker.cs
â”‚  â”œâ”€ appsettings.json
â”‚  â”œâ”€ appsettings.Development.json
â”‚  â””â”€ Properties/
â”‚     â””â”€ launchSettings.json
â”œâ”€ UploadService/
â”‚  â”œâ”€ UploadService.csproj
â”‚  â”œâ”€ Program.cs
â”‚  â”œâ”€ Worker.cs
â”‚  â”œâ”€ appsettings.json
â”‚  â”œâ”€ appsettings.Development.json
â”‚  â””â”€ Properties/
â”‚     â””â”€ launchSettings.json
â”œâ”€ Controllers/
â”‚  â””â”€ Controllers.csproj
â”œâ”€ Dtos/
â”‚  â””â”€ Dtos.csproj
â”œâ”€ Database/
â”‚  â””â”€ Database.csproj
â””â”€ Repositorys/
   â””â”€ Repositorys.csproj
```

TambÃ©m podem existir, em cada projeto, diretÃ³rios gerados automaticamente pelo build/restore:

- `bin/`: saÃ­das do build (assemblies, arquivos de runtime, etc.)
- `obj/`: intermediÃ¡rios do build e cache do restore (assets do NuGet, etc.)

## SoluÃ§Ã£o (`src.sln`)

O arquivo `src.sln` referencia os seguintes projetos:

- `Api` (`Api/Api.csproj`): aplicaÃ§Ã£o Web (ASP.NET Core) â€” host HTTP.
- `Controllers` (`Controllers/Controllers.csproj`): biblioteca para controllers/handlers (camada de apresentaÃ§Ã£o), quando implementada.
- `Dtos` (`Dtos/Dtos.csproj`): biblioteca para DTOs (contratos de entrada/saÃ­da).
- `Repositorys` (`Repositorys/Repositorys.csproj`): biblioteca para repositÃ³rios e acesso a dados.
- `Database` (`Database/Database.csproj`): biblioteca para camada de banco (ex.: contextos, migraÃ§Ãµes, helpers).
- `UploadService` (`UploadService/UploadService.csproj`): Worker Service (processamento assÃ­ncrono/background).
- `OcrService` (`OcrService/OcrService.csproj`): Worker Service (processamento assÃ­ncrono/background).

## Projetos em detalhe

### `Api/` (ASP.NET Core Web)

Arquivos principais:

- `Api/Program.cs`: ponto de entrada. No estado atual, Ã© o template padrÃ£o com Swagger e um endpoint `GET /weatherforecast`.
- `Api/Api.http`: arquivo para testes de requisiÃ§Ã£o (HTTP file).
- `Api/appsettings*.json`: configuraÃ§Ãµes de logging e hosts permitidos.
- `Api/Properties/launchSettings.json`: perfis de execuÃ§Ã£o locais (HTTP/HTTPS/IIS Express), incluindo URLs e ambiente.

DependÃªncias NuGet declaradas em `Api/Api.csproj`:

- `Microsoft.AspNetCore.Authentication.JwtBearer` e `System.IdentityModel.Tokens.Jwt`: autenticaÃ§Ã£o via JWT (infra/base).
- `Swashbuckle.AspNetCore` e `Microsoft.AspNetCore.OpenApi`: geraÃ§Ã£o de Swagger/OpenAPI.
- `MongoDB.Driver`: driver de acesso ao MongoDB.
- `Npgsql` e `Npgsql.EntityFrameworkCore.PostgreSQL`: acesso ao PostgreSQL (inclui provider EF Core).

### `OcrService/` (Worker Service)

Arquivos principais:

- `OcrService/Program.cs`: inicializa o host e registra o `HostedService`.
- `OcrService/Worker.cs`: loop de execuÃ§Ã£o do serviÃ§o. No estado atual, Ã© o template padrÃ£o logando a cada 1s.
- `OcrService/appsettings*.json`: configuraÃ§Ã£o de logging.
- `OcrService/Properties/launchSettings.json`: perfil de execuÃ§Ã£o local (ambiente `Development`).

ObservaÃ§Ã£o sobre segredos:

- `OcrService/OcrService.csproj` define `UserSecretsId`, habilitando `dotnet user-secrets` para armazenar segredos localmente (fora do repositÃ³rio).

### `UploadService/` (Worker Service)

Arquivos principais:

- `UploadService/Program.cs`: inicializa o host e registra o `HostedService`.
- `UploadService/Worker.cs`: loop de execuÃ§Ã£o do serviÃ§o. No estado atual, Ã© o template padrÃ£o logando a cada 1s.
- `UploadService/appsettings*.json`: configuraÃ§Ã£o de logging.
- `UploadService/Properties/launchSettings.json`: perfil de execuÃ§Ã£o local (ambiente `Development`).

ObservaÃ§Ã£o sobre segredos:

- `UploadService/UploadService.csproj` define `UserSecretsId`, habilitando `dotnet user-secrets` localmente.

### Bibliotecas (`Controllers/`, `Dtos/`, `Database/`, `Repositorys/`)

Estas pastas contÃªm projetos do tipo biblioteca (`Microsoft.NET.Sdk`) destinados a concentrar responsabilidades por camada.

No estado atual do repositÃ³rio, estes projetos:

- declaram `TargetFramework` `net8.0` e `Nullable/ImplicitUsings` habilitados;
- incluem referÃªncias NuGet relacionadas a JWT e acesso a dados (MongoDB/PostgreSQL);
- nÃ£o possuem classes `.cs` prÃ³prias (apenas artefatos gerados em `bin/` e `obj/` quando buildados).

## Como executar (local)

PrÃ©-requisitos:

- SDK do .NET 8 instalado.

Comandos Ãºteis (na raiz do repositÃ³rio):

```bash
# restaurar dependÃªncias
dotnet restore .\src.sln

# buildar a soluÃ§Ã£o inteira
dotnet build .\src.sln

# rodar a API
dotnet run --project .\Api\Api.csproj

# rodar o worker de OCR
dotnet run --project .\OcrService\OcrService.csproj

# rodar o worker de Upload
dotnet run --project .\UploadService\UploadService.csproj
```

No perfil `Development`, a API inicia com Swagger habilitado (ver `Api/Properties/launchSettings.json` para portas/URLs).

## ConvenÃ§Ãµes e notas

- Pastas `bin/` e `obj/` sÃ£o geradas automaticamente pelo build/restore; normalmente nÃ£o sÃ£o fonte de verdade do projeto.
- O nome do projeto/pasta `Repositorys` estÃ¡ mantido conforme o repositÃ³rio (mesmo nÃ£o sendo a grafia mais comum em inglÃªs).

