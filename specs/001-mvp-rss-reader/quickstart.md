# Guia de Início Rápido: MVP Leitor de Feed RSS

**Branch**: `001-mvp-rss-reader`  
**Data**: 2026-05-02

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado
- Terminal (PowerShell, bash ou zsh)
- Navegador moderno (Chrome, Edge, Firefox)

Verificar instalação:

```bash
dotnet --version
# Deve retornar 8.x.x
```

---

## Estrutura de Diretórios

```text
RSSFeedReader/
├── backend/
│   └── RSSFeedReader.API/          # ASP.NET Core Minimal API
└── frontend/
    └── RSSFeedReader.UI/            # Blazor WebAssembly
```

---

## 1. Executar o Backend

```bash
cd backend/RSSFeedReader.API
dotnet run
```

O backend iniciará em `http://localhost:5000` (HTTP). Confirme no terminal a mensagem:

```
Now listening on: http://localhost:5000
```

---

## 2. Configurar a URL do Backend no Frontend

Antes de iniciar o frontend, verifique o arquivo de configuração:

```
frontend/RSSFeedReader.UI/wwwroot/appsettings.json
```

```json
{
  "BackendBaseUrl": "http://localhost:5000"
}
```

Ajuste a URL se necessário (ex.: porta diferente).

---

## 3. Executar o Frontend

Em um **novo terminal**:

```bash
cd frontend/RSSFeedReader.UI
dotnet run
```

O frontend iniciará em `http://localhost:5001`. Abra no navegador.

---

## 4. Verificar Funcionamento

Antes de testar, confirme o **Checklist de Prontidão Local** (Constituição, Fluxo de Desenvolvimento):

- [ ] Backend iniciou sem erros e escuta em `http://localhost:5000`
- [ ] Frontend iniciou sem erros e carregou em `http://localhost:5001`
- [ ] `wwwroot/appsettings.json` aponta para `http://localhost:5000`
- [ ] DevTools do navegador não exibe erros de CORS ou de conexão

---

## 5. Testar o MVP Manualmente

1. Abrir `http://localhost:5001` no navegador
2. Colar uma URL no campo de entrada, ex.: `https://devblogs.microsoft.com/dotnet/feed/`
3. Clicar em **Adicionar**
4. Verificar que a URL aparece na lista abaixo do campo
5. Adicionar mais 2 URLs e verificar que todas aparecem na ordem de adição
6. Verificar que o campo é esvaziado após cada adição bem-sucedida

---

## 6. Executar os Testes

```bash
cd backend/RSSFeedReader.API.Tests
dotnet test
```

Resultado esperado: todos os testes passando, zero falhas, zero pulos.

---

## Endpoints da API (referência rápida)

| Método | Endpoint | Descrição |
|---|---|---|
| `POST` | `/api/subscriptions` | Adiciona uma assinatura |
| `GET` | `/api/subscriptions` | Lista todas as assinaturas |

Detalhes completos: [contracts/api.md](contracts/api.md)

---

## Comportamento Esperado no MVP

| Ação | Resultado esperado |
|---|---|
| Adicionar URL | URL aparece no final da lista |
| Fechar e reabrir o app | Lista esvaziada (armazenamento em memória) |
| Tentar adicionar campo vazio | Nenhuma entrada adicionada |
| Adicionar URL duplicada | Duplicata adicionada (sem validação de unicidade no MVP) |
