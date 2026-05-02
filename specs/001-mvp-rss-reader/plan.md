# Plano de Implementação: MVP Leitor de Feed RSS

**Branch**: `001-mvp-rss-reader` | **Data**: 2026-05-02 | **Spec**: [spec.md](spec.md)
**Entrada**: Especificação de funcionalidade em `specs/001-mvp-rss-reader/spec.md`

## Resumo

Implementar o MVP mínimo de um leitor de feeds RSS/Atom: o usuário pode adicionar URLs de
feeds a uma lista e visualizar essa lista. Nenhuma operação de rede, parsing ou persistência
faz parte deste milestone. Backend em ASP.NET Core Minimal API (.NET 8) com armazenamento
em memória; frontend em Blazor WebAssembly com chamadas HTTP simples ao backend.

## Contexto Técnico

**Linguagem/Versão**: C# / .NET 8 (LTS)  
**Dependências Primárias**: ASP.NET Core Minimal APIs (backend), Blazor WebAssembly (frontend), xUnit + Microsoft.AspNetCore.Mvc.Testing (testes)  
**Armazenamento**: Em memória — `List<Subscription>` singleton no `SubscriptionService`  
**Testes**: xUnit (unitários) + WebApplicationFactory (integração)  
**Plataforma Alvo**: Multiplataforma — Windows / macOS / Linux (local, usuário único)  
**Tipo de Projeto**: Aplicação web (backend API + frontend WASM)  
**Metas de Performance**: Resposta imediata da UI (< 1s para adicionar e exibir); nenhuma meta de throughput — aplicação local de usuário único  
**Restrições**: Sem operações de rede no MVP; sem cliente HTTP; sem bibliotecas de parsing de feed; sem persistência além da sessão  
**Escopo/Escala**: 1 usuário, execução local, 2 endpoints de API, 1 tela de UI

## Verificação da Constituição

*GATE: Verificar antes da Fase 0. Reverificar após o design da Fase 1.*

| Princípio | Pré-Design | Pós-Design | Observação |
|---|---|---|---|
| I. MVP em Primeiro Lugar | ✅ PASSA | ✅ PASSA | Escopo restrito a adicionar + listar; nenhuma feature pós-MVP incluída |
| II. Segurança por Padrão | ✅ PASSA | ✅ PASSA | CORS explícito configurado; validação de entrada vazia no endpoint POST |
| III. Testes em Primeiro Lugar | ✅ PASSA | ✅ PASSA | xUnit unitário para `SubscriptionService`; integração para ambos os endpoints |
| IV. Separação de Responsabilidades | ✅ PASSA | ✅ PASSA | Backend API isolado do frontend WASM; lógica no serviço, não no controller/componente |
| V. Manutenibilidade e Simplicidade | ✅ PASSA | ✅ PASSA | `List<T>` sem abstração desnecessária; Minimal APIs sem boilerplate; YAGNI aplicado |

## Project Structure

### Documentation (this feature)

## Estrutura do Projeto

### Documentação (esta funcionalidade)

```text
specs/001-mvp-rss-reader/
├── plan.md              # Este arquivo
├── research.md          # Saída da Fase 0
├── data-model.md        # Saída da Fase 1
├── quickstart.md        # Saída da Fase 1
├── contracts/
│   └── api.md           # Saída da Fase 1
└── tasks.md             # Saída do /speckit.tasks (NÃO criado pelo /speckit.plan)
```

### Código-Fonte (raiz do repositório)

```text
backend/
└── RSSFeedReader.API/              # ASP.NET Core Minimal API (.NET 8)
    ├── Models/
    │   └── Subscription.cs         # Entidade de domínio
    ├── Services/
    │   └── SubscriptionService.cs  # Lógica de negócio (singleton em memória)
    └── Program.cs                  # Configuração da app, endpoints, CORS

frontend/
└── RSSFeedReader.UI/               # Blazor WebAssembly (.NET 8)
    ├── Pages/
    │   └── Subscriptions.razor     # Página principal do MVP
    ├── Services/
    │   └── SubscriptionApiClient.cs # Cliente HTTP para o backend
    ├── Layout/
    │   └── NavMenu.razor           # Menu de navegação (limpo do template)
    └── wwwroot/
        └── appsettings.json        # BackendBaseUrl

tests/
└── RSSFeedReader.API.Tests/        # xUnit + WebApplicationFactory
    ├── Unit/
    │   └── SubscriptionServiceTests.cs
    └── Integration/
        └── SubscriptionsEndpointTests.cs
```

**Decisão de estrutura**: Aplicação web com dois projetos separados (`backend/` e `frontend/`), satisfazendo o Princípio IV da Constituição. Os testes ficam em `tests/` na raiz, independentes do projeto de produção.

## Rastreamento de Complexidade

Nenhuma violação da Constituição identificada. Tabela não aplicável.
