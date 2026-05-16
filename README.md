# UserDash (.NET 8)

Este repositório contém uma solução estruturada em **.NET 8** (compatível com Visual Studio e JetBrains Rider) utilizando uma arquitetura desacoplada com múltiplos projetos (`src.sln`). A solução separa a API principal, serviços de processamento em segundo plano (Workers) e bibliotecas de suporte para as camadas de domínio e infraestrutura.

> **Observação:** Atualmente, alguns projetos (como `Controllers`, `Dtos`, `Database` e `Repositorys`) funcionam como placeholders estruturais. Eles contêm os arquivos `.csproj` e referências necessárias, mas as classes de negócio e lógica ainda serão implementadas.

---

## 🛠️ Tecnologias e Dependências

A solução utiliza tecnologias modernas para garantir escalabilidade e performance:

*   **Runtime:** .NET 8.0
*   **Banco de Dados:** 
    *   **MongoDB:** Utilizado via `MongoDB.Driver`.
    *   **PostgreSQL:** Utilizado via `Npgsql` e `EntityFrameworkCore.PostgreSQL`.
*   **Autenticação:** JWT (JSON Web Token) com `Microsoft.AspNetCore.Authentication.JwtBearer`.
*   **Documentação:** Swagger/OpenAPI via `Swashbuckle.AspNetCore`.
*   **Processamento:** Worker Services para tarefas assíncronas.

---

## 📂 Estrutura da Solução

A organização das pastas segue o padrão de separação de responsabilidades:

```text
.
├── src.sln                 # Arquivo de solução global
├── Api/                    # Host da aplicação Web (ASP.NET Core)
├── OcrService/             # Worker Service para processamento de OCR
├── UploadService/          # Worker Service para gestão de uploads
├── Controllers/            # Camada de apresentação (Controllers/Handlers)
├── Dtos/                   # Contratos de entrada e saída (Data Transfer Objects)
├── Database/               # Infraestrutura de banco (Contextos e Migrações)
├── Repositorys/            # Camada de acesso a dados (Repositórios)
└── FileService/            # Worker Service para gestão de arquivos.
```

*Nota: Diretórios como `bin/` e `obj/` são gerados automaticamente durante o build e não devem ser versionados.*

---

## 🚀 Detalhes dos Projetos

### 🌐 Api (Web API)
É o ponto de entrada principal via HTTP.
*   **Configurações:** Gerenciadas via `appsettings.json` e perfis no `launchSettings.json`.
*   **Funcionalidades:** Inclui suporte nativo a Swagger para testes de endpoints e integração com JWT.

### ⚙️ Worker Services (`OcrService` , `UploadService` & `FileService`)
Serviços focados em processamento em background.
*   **Execução:** Utilizam o `BackgroundService` do .NET para loops de processamento.
*   **Segurança:** Configurados para usar `UserSecretsId`, permitindo o armazenamento seguro de credenciais localmente durante o desenvolvimento.

### 📚 Bibliotecas de Suporte
*   **Controllers:** Centraliza a lógica de roteamento para manter a API limpa.
*   **Dtos:** Define os contratos de comunicação entre as camadas.
*   **Database & Repositorys:** Camadas dedicadas à persistência, garantindo que a lógica de acesso a dados esteja isolada.

---

## ⚙️ Como Executar Localmente

### Pré-requisitos
*   [SDK do .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.

### Passo a Passo

1.  **Restaurar Dependências:**
    ```bash
    dotnet restore src.sln
    ```

2.  **Compilar a Solução:**
    ```bash
    dotnet build src.sln
    ```

3.  **Executar os Projetos:**
    Você pode rodar cada componente individualmente conforme a necessidade:

    *   **API:** `dotnet run --project Api/Api.csproj`
    *   **OCR Service:** `dotnet run --project OcrService/OcrService.csproj`
    *   **Upload Service:** `dotnet run --project UploadService/UploadService.csproj`

A API iniciará por padrão com o Swagger disponível no navegador (verifique a porta no console ou em `launchSettings.json`).

---

## 📝 Convenções do Projeto

*   **Nomenclatura:** O projeto mantém o nome da pasta `Repositorys` conforme a estrutura original do repositório.
*   **Segredos:** Utilize `dotnet user-secrets` para configurar strings de conexão e chaves de API locais, evitando expor dados sensíveis no código-fonte.
