# RSS Feed Reader

Projeto de treinamento desenvolvido com a metodologia **Spec Driven Development (SDD)** utilizando o **GitHub Spec Kit**.

---

## Metodologia: Spec Driven Development (SDD)

SDD é uma metodologia de desenvolvimento orientada por especificações formais, projetada para maximizar a qualidade e a previsibilidade ao trabalhar com agentes de IA (como GitHub Copilot). Em vez de prompts ad-hoc, toda a intenção do sistema é codificada em artefatos de design antes de qualquer linha de código ser escrita.

### Fluxo de trabalho utilizado

```
Constituição → Spec → Plano → Tarefas → Implementação
```

| Artefato | Ferramenta Spec Kit | Propósito |
|---|---|---|
| `constitution.md` | `/speckit.constitution` | "Lei suprema" do projeto — princípios inegociáveis |
| `spec.md` | `/speckit.specify` | O quê construir — user stories, requisitos, critérios de sucesso |
| `plan.md` + artifacts | `/speckit.plan` | Como construir — decisões técnicas, arquitetura, contratos de API |
| `tasks.md` | `/speckit.tasks` | Lista ordenada de tarefas prontas para execução |
| Implementação | `/speckit.implement` | Agente executa todas as tarefas com rastreamento de progresso |

---

## Objetivo de Aprendizado

Este projeto foi criado exclusivamente para aprender e praticar o fluxo completo do SDD com o GitHub Spec Kit, passando por todas as etapas da metodologia de ponta a ponta:

- Entender como uma **Constituição** estabelece princípios técnicos inegociáveis (ex.: TDD obrigatório, CORS sem wildcard, YAGNI)
- Aprender a transformar documentos de stakeholders em uma **especificação tecnicamente agnóstica** (`spec.md`)
- Praticar a geração de **artefatos de design** (data model, API contract, quickstart, research)
- Executar **TDD (Test Driven Development)** integrado ao fluxo SDD — testes escritos antes da implementação
- Delegar a implementação completa a um **agente de IA com contexto estruturado**, em vez de prompts soltos

---

## Feature: MVP Leitor de Feed RSS

**Branch:** `001-mvp-rss-reader` | **Spec:** `specs/001-mvp-rss-reader/`

### Funcionalidades implementadas

| User Story | Prioridade | Funcionalidade |
|---|---|---|
| US1 | P1 — MVP | Adicionar uma assinatura de feed RSS/Atom via URL |
| US2 | P2 | Visualizar a lista de assinaturas adicionadas na sessão |

### Stack técnica

| Camada | Tecnologia |
|---|---|
| Backend | ASP.NET Core 8 — Minimal APIs |
| Frontend | Blazor WebAssembly 8 |
| Testes | xUnit + `Microsoft.AspNetCore.Mvc.Testing` 8.x |
| Armazenamento | In-memory (`List<Subscription>` singleton) |
| Documentação | Swagger/OpenAPI |

### Executar localmente

```sh
# Backend — http://localhost:5000 (Swagger em /swagger)
cd backend/RSSFeedReader.API
dotnet run

# Frontend — http://localhost:5001
cd frontend/RSSFeedReader.UI
dotnet run

# Testes
dotnet test tests/RSSFeedReader.API.Tests/RSSFeedReader.API.Tests.csproj
```

### Resultados dos testes

```
Total: 11  |  Aprovados: 11  |  Falhas: 0
6 testes unitários (SubscriptionService)
5 testes de integração (POST /api/subscriptions + GET /api/subscriptions)
```

---

## Estrutura do Repositório

```
.specify/memory/constitution.md     # Constituição do projeto
specs/001-mvp-rss-reader/
  spec.md                           # Especificação da feature
  plan.md                           # Plano de implementação
  tasks.md                          # Tarefas (T001–T022, todas concluídas)
  research.md                       # Decisões técnicas documentadas
  data-model.md                     # Modelo de dados
  contracts/api.md                  # Contrato da API
  quickstart.md                     # Guia de execução local
backend/RSSFeedReader.API/          # ASP.NET Core Minimal API
frontend/RSSFeedReader.UI/          # Blazor WebAssembly
tests/RSSFeedReader.API.Tests/      # xUnit — unit + integration tests
```

# RSS Feed Reader

Projeto de treinamento desenvolvido com a metodologia **Spec Driven Development (SDD)** utilizando o **GitHub Spec Kit**.

---

## Metodologia: Spec Driven Development (SDD)

SDD é uma metodologia de desenvolvimento orientada por especificações formais, projetada para maximizar a qualidade e a previsibilidade ao trabalhar com agentes de IA (como GitHub Copilot). Em vez de prompts ad-hoc, toda a intenção do sistema é codificada em artefatos de design antes de qualquer linha de código ser escrita.

### Fluxo de trabalho utilizado

```
Constituição → Spec → Plano → Tarefas → Implementação
```

| Artefato | Ferramenta Spec Kit | Propósito |
|---|---|---|
| `constitution.md` | `/speckit.constitution` | "Lei suprema" do projeto — princípios inegociáveis |
| `spec.md` | `/speckit.specify` | O quê construir — user stories, requisitos, critérios de sucesso |
| `plan.md` + artifacts | `/speckit.plan` | Como construir — decisões técnicas, arquitetura, contratos de API |
| `tasks.md` | `/speckit.tasks` | Lista ordenada de tarefas prontas para execução |
| Implementação | `/speckit.implement` | Agente executa todas as tarefas com rastreamento de progresso |

---

## Objetivo de Aprendizado

Este projeto foi criado exclusivamente para aprender e praticar o fluxo completo do SDD com o GitHub Spec Kit, passando por todas as etapas da metodologia de ponta a ponta:

- Entender como uma **Constituição** estabelece princípios técnicos inegociáveis (ex.: TDD obrigatório, CORS sem wildcard, YAGNI)
- Aprender a transformar documentos de stakeholders em uma **especificação tecnicamente agnóstica** (`spec.md`)
- Praticar a geração de **artefatos de design** (data model, API contract, quickstart, research)
- Executar **TDD (Test Driven Development)** integrado ao fluxo SDD — testes escritos antes da implementação
- Delegar a implementação completa a um **agente de IA com contexto estruturado**, em vez de prompts soltos

---

## Feature: MVP Leitor de Feed RSS

**Branch:** `001-mvp-rss-reader` | **Spec:** `specs/001-mvp-rss-reader/`

### Funcionalidades implementadas

| User Story | Prioridade | Funcionalidade |
|---|---|---|
| US1 | P1 — MVP | Adicionar uma assinatura de feed RSS/Atom via URL |
| US2 | P2 | Visualizar a lista de assinaturas adicionadas na sessão |

### Stack técnica

| Camada | Tecnologia |
|---|---|
| Backend | ASP.NET Core 8 — Minimal APIs |
| Frontend | Blazor WebAssembly 8 |
| Testes | xUnit + `Microsoft.AspNetCore.Mvc.Testing` 8.x |
| Armazenamento | In-memory (`List<Subscription>` singleton) |
| Documentação | Swagger/OpenAPI |

### Executar localmente

```sh
# Backend — http://localhost:5000 (Swagger em /swagger)
cd backend/RSSFeedReader.API
dotnet run

# Frontend — http://localhost:5001
cd frontend/RSSFeedReader.UI
dotnet run

# Testes
dotnet test tests/RSSFeedReader.API.Tests/RSSFeedReader.API.Tests.csproj
```

### Resultados dos testes

```
Total: 11  |  Aprovados: 11  |  Falhas: 0
6 testes unitários (SubscriptionService)
5 testes de integração (POST /api/subscriptions + GET /api/subscriptions)
```

---

## Estrutura do Repositório

```
.specify/memory/constitution.md     # Constituição do projeto
specs/001-mvp-rss-reader/
  spec.md                           # Especificação da feature
  plan.md                           # Plano de implementação
  tasks.md                          # Tarefas (T001–T022, todas concluídas)
  research.md                       # Decisões técnicas documentadas
  data-model.md                     # Modelo de dados
  contracts/api.md                  # Contrato da API
  quickstart.md                     # Guia de execução local
backend/RSSFeedReader.API/          # ASP.NET Core Minimal API
frontend/RSSFeedReader.UI/          # Blazor WebAssembly
tests/RSSFeedReader.API.Tests/      # xUnit — unit + integration tests
```

