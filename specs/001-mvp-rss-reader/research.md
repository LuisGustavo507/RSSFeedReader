# Pesquisa: MVP Leitor de Feed RSS

**Fase**: 0 — Pesquisa e resolução de incógnitas  
**Branch**: `001-mvp-rss-reader`  
**Data**: 2026-05-02

---

## Decisão 1: Versão do .NET

**Decisão**: .NET 8 (LTS)  
**Justificativa**: Versão LTS atual com suporte garantido até novembro de 2026. Compatível com ASP.NET Core e Blazor WebAssembly. Disponível em Windows, macOS e Linux — satisfaz o requisito de multiplataforma da Constituição.  
**Alternativas consideradas**:  
- .NET 9 (atual não-LTS): descartado — sem suporte de longo prazo, risco para projeto de aprendizado  
- .NET 6 (LTS anterior): descartado — suporte encerrado em novembro de 2024

---

## Decisão 2: Estilo da API Backend

**Decisão**: ASP.NET Core Minimal APIs (sem Controllers)  
**Justificativa**: O MVP requer apenas 2 endpoints (`POST /api/subscriptions` e `GET /api/subscriptions`). Minimal APIs eliminam o boilerplate de Controllers e atributos de roteamento, mantendo o código mais próximo do princípio V (Manutenibilidade e Simplicidade). A lógica de negócio permanece em um `SubscriptionService` separado, satisfazendo o princípio IV.  
**Alternativas consideradas**:  
- Controllers (`[ApiController]`): descartado para o MVP — verbosidade desnecessária para 2 endpoints; pode ser adotado no MVP Estendido ou Pós-MVP se a complexidade justificar

---

## Decisão 3: Framework de Testes

**Decisão**: xUnit + `Microsoft.AspNetCore.Mvc.Testing`  
**Justificativa**: xUnit é o framework de testes padrão para o ecossistema .NET moderno. `Microsoft.AspNetCore.Mvc.Testing` permite testes de integração da API usando um servidor em memória (`WebApplicationFactory<T>`), sem necessidade de subir infraestrutura externa — ideal para o MVP.  
**Alternativas consideradas**:  
- NUnit: descartado — xUnit tem melhor integração com as ferramentas do .NET SDK e é o padrão dos templates oficiais  
- Testes manuais apenas: descartado — viola o Princípio III (Testes em Primeiro Lugar)

---

## Decisão 4: Nomenclatura dos Projetos

**Decisão**:  
- Backend: `RSSFeedReader.API` (projeto ASP.NET Core Minimal API)  
- Frontend: `RSSFeedReader.UI` (projeto Blazor WebAssembly Standalone)  
- Testes backend: `RSSFeedReader.API.Tests` (projeto xUnit)  

**Justificativa**: Nomenclatura descritiva e convencional para o ecossistema .NET. O sufixo `.API` e `.UI` deixa clara a responsabilidade de cada projeto à primeira vista.  
**Alternativas consideradas**: Nomenclatura genérica (`Backend`, `Frontend`) — descartada por ser menos informativa em contextos multi-projeto

---

## Decisão 5: Configuração de CORS

**Decisão**: Política de CORS explícita com origem específica do frontend (`http://localhost:5001` por padrão em desenvolvimento), sem uso de `AllowAnyOrigin()`.  
**Justificativa**: O Princípio II (Segurança por Padrão) proíbe origens curinga (`*`) mesmo em desenvolvimento. A origem do frontend é conhecida desde o início — não há razão para usar wildcard.  
**Implementação**: A URL de origem do frontend DEVE ser configurável via `appsettings.json` (chave `AllowedOrigins`), não hardcoded no código.  
**Alternativas consideradas**:  
- `AllowAnyOrigin()`: descartado — viola explicitamente o Princípio II  
- Sem CORS (mesma origem): descartado — backend e frontend são projetos separados em portas diferentes

---

## Decisão 6: Comunicação Frontend → Backend

**Decisão**: `HttpClient` pré-configurado via injeção de dependência no Blazor WASM, com `BaseAddress` configurada em `wwwroot/appsettings.json`.  
**Justificativa**: Padrão oficial do Blazor WebAssembly. A URL base do backend no arquivo de configuração (não hardcoded) satisfaz o Princípio II e o Checklist de Prontidão Local da Constituição.  
**Alternativas consideradas**:  
- URL hardcoded no componente: descartado — viola Princípio V (sem strings mágicas) e Princípio II

---

## Decisão 7: Estrutura de Diretórios

**Decisão**: Dois diretórios de primeiro nível — `backend/` e `frontend/` — com projetos .NET dentro de cada um.  
**Justificativa**: Separação física dos projetos reforça o Princípio IV (Separação de Responsabilidades). Permite que backend e frontend sejam executados e testados de forma independente.

---

## Incógnitas Resolvidas

Todas as incógnitas identificadas no contexto técnico foram resolvidas acima. Nenhum item `NEEDS CLARIFICATION` permanece pendente.
