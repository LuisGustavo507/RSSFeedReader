---
description: "Lista de tarefas para implementação do MVP Leitor de Feed RSS"
---

# Tarefas: MVP Leitor de Feed RSS

**Entrada**: Documentos de design em `specs/001-mvp-rss-reader/`
**Pré-requisitos**: plan.md ✅ | spec.md ✅ | research.md ✅ | data-model.md ✅ | contracts/api.md ✅ | quickstart.md ✅

**Testes**: Incluídos — a Constituição (Princípio III) exige TDD obrigatório em toda lógica de negócio.

**Organização**: Tarefas agrupadas por User Story para permitir implementação e teste independente de cada história.

## Formato: `[ID] [P?] [Story?] Descrição`

- **[P]**: Pode executar em paralelo (arquivos diferentes, sem dependências de tarefas incompletas)
- **[Story]**: A qual User Story esta tarefa pertence (US1, US2)
- Caminhos de arquivo exatos incluídos em todas as descrições

---

## Fase 1: Setup (Infraestrutura Compartilhada)

**Propósito**: Inicialização dos projetos e estrutura básica de diretórios

- [ ] T001 Criar solution e estrutura de diretórios: `dotnet new sln -n RSSFeedReader` na raiz; criar pastas `backend/`, `frontend/`, `tests/`
- [ ] T002 [P] Criar projeto backend: `dotnet new webapi -minimal -n RSSFeedReader.API -o backend/RSSFeedReader.API`; adicionar à solution
- [ ] T003 [P] Criar projeto frontend: `dotnet new blazorwasm -n RSSFeedReader.UI -o frontend/RSSFeedReader.UI`; adicionar à solution
- [ ] T004 [P] Criar projeto de testes: `dotnet new xunit -n RSSFeedReader.API.Tests -o tests/RSSFeedReader.API.Tests`; adicionar referência ao projeto backend; adicionar `Microsoft.AspNetCore.Mvc.Testing` ao projeto de testes; adicionar à solution

---

## Fase 2: Fundacional (Pré-requisitos Bloqueantes)

**Propósito**: Infraestrutura central que DEVE estar completa antes de qualquer User Story

**⚠️ CRÍTICO**: Nenhum trabalho de User Story pode começar até que esta fase esteja concluída

- [ ] T005 Limpar páginas de demonstração do template Blazor: deletar `frontend/RSSFeedReader.UI/Pages/Home.razor`, `Counter.razor`, `Weather.razor`; atualizar `frontend/RSSFeedReader.UI/Layout/NavMenu.razor` removendo os links de navegação das páginas deletadas
- [ ] T006 Configurar CORS no backend: em `backend/RSSFeedReader.API/Program.cs`, adicionar política de CORS que lê origens permitidas de `AllowedOrigins` no `appsettings.json`; em `backend/RSSFeedReader.API/appsettings.json`, adicionar chave `"AllowedOrigins": ["http://localhost:5001"]`; proibido uso de `AllowAnyOrigin()`
- [ ] T007 Configurar HttpClient no frontend: em `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`, adicionar chave `"BackendBaseUrl": "http://localhost:5000"`; em `frontend/RSSFeedReader.UI/Program.cs`, registrar `HttpClient` com `BaseAddress` lida de `appsettings.json`
- [ ] T008 Configurar Swagger/OpenAPI no backend: em `backend/RSSFeedReader.API/Program.cs`, adicionar `AddOpenApi()` e `MapOpenApi()` para documentar os endpoints

**Checkpoint**: Fundação pronta — implementação das User Stories pode começar

---

## Fase 3: User Story 1 — Adicionar Assinatura de Feed (Prioridade: P1) 🎯 MVP

**Objetivo**: O usuário cola uma URL no campo de entrada, clica em "Adicionar" e a URL aparece na lista imediatamente. O campo é esvaziado após a adição.

**Teste Independente**: Abrir o app, digitar qualquer string no campo de URL, clicar em "Adicionar" e verificar que a string aparece na lista abaixo. Testar que campo vazio não adiciona entrada.

### Testes para User Story 1 (escrever PRIMEIRO — devem FALHAR antes da implementação)

- [ ] T009 [P] [US1] Escrever testes unitários falhando para `SubscriptionService`: cobrir `AddSubscription` com URL válida (retorna `Subscription` com Id e Url), `AddSubscription` com URL vazia (lança exceção ou retorna erro), e `GetAll` retornando lista vazia e lista com entradas — em `tests/RSSFeedReader.API.Tests/Unit/SubscriptionServiceTests.cs`
- [ ] T010 [P] [US1] Escrever testes de integração falhando para `POST /api/subscriptions`: cobrir 201 com URL válida (verifica body de resposta), 400 com URL vazia (verifica mensagem de erro) — em `tests/RSSFeedReader.API.Tests/Integration/SubscriptionsEndpointTests.cs`

### Implementação da User Story 1

- [ ] T011 [P] [US1] Criar model `Subscription` com propriedades `Id` (int) e `Url` (string) em `backend/RSSFeedReader.API/Models/Subscription.cs`
- [ ] T012 [US1] Implementar `SubscriptionService` como singleton: lista em memória `List<Subscription>`, método `AddSubscription(string url)` com validação de URL não vazia, método `GetAll()` retornando a lista — em `backend/RSSFeedReader.API/Services/SubscriptionService.cs`; registrar como singleton em `backend/RSSFeedReader.API/Program.cs`
- [ ] T013 [US1] Implementar endpoint `POST /api/subscriptions` em `backend/RSSFeedReader.API/Program.cs`: aceita `{ "url": string }`, valida que url não é vazia (retorna 400 com `{ "message": "A URL não pode ser vazia." }`), delega ao `SubscriptionService`, retorna 201 com objeto `Subscription` criado
- [ ] T014 [US1] Implementar `SubscriptionApiClient` com método `AddSubscriptionAsync(string url)`: faz `POST /api/subscriptions`, retorna `Subscription` em caso de sucesso — em `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs`; registrar no `frontend/RSSFeedReader.UI/Program.cs`
- [ ] T015 [US1] Criar página `Subscriptions.razor` com campo de entrada para URL, botão "Adicionar", injeção de `SubscriptionApiClient`, handler que chama `AddSubscriptionAsync`, esvazia o campo após sucesso e exibe lista como placeholder (a ser preenchida na US2) — em `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` com rota `@page "/"`
- [ ] T016 [US1] Atualizar `frontend/RSSFeedReader.UI/Layout/NavMenu.razor` adicionando link de navegação para a página de Assinaturas (`/`)

**Checkpoint**: User Story 1 totalmente funcional e testável de forma independente. Executar `dotnet test` — todos os testes de US1 devem passar.

---

## Fase 4: User Story 2 — Visualizar Lista de Assinaturas (Prioridade: P2)

**Objetivo**: O usuário vê todas as URLs adicionadas na sessão atual exibidas em uma lista, na ordem em que foram adicionadas. Lista vazia é exibida sem erro ao abrir o app.

**Teste Independente**: Adicionar 3 URLs distintas em sequência e verificar que todas as 3 aparecem na lista na ordem de adição. Verificar que lista vazia não exibe mensagem de erro.

### Testes para User Story 2 (escrever PRIMEIRO — devem FALHAR antes da implementação)

- [ ] T017 [P] [US2] Escrever testes de integração falhando para `GET /api/subscriptions`: cobrir 200 com array vazio (lista vazia inicial), 200 com array contendo entradas após POSTs anteriores (verifica ordem de inserção) — em `tests/RSSFeedReader.API.Tests/Integration/SubscriptionsEndpointTests.cs`

### Implementação da User Story 2

- [ ] T018 [US2] Implementar endpoint `GET /api/subscriptions` em `backend/RSSFeedReader.API/Program.cs`: delega ao `SubscriptionService.GetAll()`, retorna 200 com array de `Subscription` (array vazio `[]` se não houver entradas)
- [ ] T019 [US2] Implementar método `GetSubscriptionsAsync()` em `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs`: faz `GET /api/subscriptions`, retorna `List<Subscription>`
- [ ] T020 [US2] Atualizar `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor`: carregar a lista via `GetSubscriptionsAsync()` no `OnInitializedAsync` e após cada adição bem-sucedida; renderizar a lista com `@foreach` exibindo cada URL; estado de lista vazia exibido sem mensagem de erro

**Checkpoint**: User Stories 1 e 2 totalmente funcionais. Executar `dotnet test` — todos os testes devem passar.

---

## Fase Final: Polimento e Verificações Transversais

**Propósito**: Validações que afetam todas as User Stories e garantia de conformidade com a Constituição

- [ ] T021 [P] Verificar Checklist de Prontidão Local da Constituição: (1) backend inicia sem erros em `http://localhost:5000`; (2) frontend inicia sem erros em `http://localhost:5001`; (3) `wwwroot/appsettings.json` aponta para URL correta do backend; (4) CORS permite origem do frontend sem wildcard; (5) DevTools do navegador sem erros de CORS ou conexão
- [ ] T022 Executar suite completa de testes e confirmar gate de qualidade: `dotnet test` com zero falhas e zero pulos; verificar ausência de novos warnings do compilador; confirmar que Swagger documenta `POST /api/subscriptions` e `GET /api/subscriptions`

---

## Dependências e Ordem de Execução

### Dependências entre Fases

- **Setup (Fase 1)**: Sem dependências — pode iniciar imediatamente
- **Fundacional (Fase 2)**: Depende da conclusão da Fase 1 — **BLOQUEIA todas as User Stories**
- **User Story 1 (Fase 3)**: Depende da Fase 2; sem dependência de outras stories
- **User Story 2 (Fase 4)**: Depende da Fase 2; **integra com US1** (usa o mesmo `SubscriptionService` e a mesma página Razor)
- **Polish (Fase Final)**: Depende da conclusão de US1 e US2

### Dependências entre User Stories

- **US1 (P1)**: Independente — pode iniciar após Fase 2 ✅
- **US2 (P2)**: Depende de US1 estar completa (reusa `SubscriptionService.GetAll()` e complementa `Subscriptions.razor`)

### Dentro de cada User Story

1. Testes DEVEM ser escritos e confirmados como falhando antes da implementação (Princípio III — INEGOCIÁVEL)
2. Model antes do serviço (T011 antes de T012)
3. Serviço antes do endpoint (T012 antes de T013)
4. Endpoint backend antes do cliente frontend (T013 antes de T014)
5. Cliente frontend antes do componente UI (T014 antes de T015)

---

## Exemplos de Execução em Paralelo

### User Story 1 — Paralelos disponíveis

```
# Testes e model podem ser escritos simultaneamente (arquivos diferentes):
T009 — SubscriptionServiceTests.cs       (testes unitários)
T010 — SubscriptionsEndpointTests.cs     (testes de integração POST)
T011 — Models/Subscription.cs            (model)

# Após T011 e T009: implementar T012 (SubscriptionService)
# Após T012 e T010: implementar T013 (endpoint POST)
# T014 e T015 são sequenciais (cliente antes do componente)
```

### Setup — Paralelos disponíveis

```
# T002, T003, T004 podem ser criados simultaneamente (projetos independentes):
T002 — backend/RSSFeedReader.API
T003 — frontend/RSSFeedReader.UI
T004 — tests/RSSFeedReader.API.Tests
```

---

## Estratégia de Implementação

### MVP First (apenas User Story 1)

1. Concluir Fase 1: Setup
2. Concluir Fase 2: Fundacional (**CRÍTICO** — bloqueia tudo)
3. Concluir Fase 3: User Story 1 (T009 → T016)
4. **PARAR E VALIDAR**: testar US1 de forma independente com `dotnet test` e verificação manual no browser
5. Demonstrar o MVP se estiver pronto

### Entrega Incremental

1. Setup + Fundacional → Fundação pronta
2. User Story 1 → Testar independentemente → **Demo do MVP!**
3. User Story 2 → Testar independentemente → **Demo do MVP Completo!**
4. Cada story adiciona valor sem quebrar as anteriores
