# Contratos de API: MVP Leitor de Feed RSS

**Versão**: 1.0.0  
**Base URL (desenvolvimento)**: `http://localhost:5000`  
**Formato**: JSON  
**Autenticação**: Nenhuma (MVP local, usuário único)

---

## Endpoints

### POST /api/subscriptions

Adiciona uma nova assinatura de feed à lista em memória.

**Request**

```
POST /api/subscriptions
Content-Type: application/json
```

```json
{
  "url": "https://devblogs.microsoft.com/dotnet/feed/"
}
```

| Campo | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| `url` | string | Sim | URL do feed RSS/Atom a ser adicionado |

**Respostas**

| Status | Descrição | Body |
|---|---|---|
| `201 Created` | Assinatura adicionada com sucesso | Objeto `Subscription` criado |
| `400 Bad Request` | URL ausente, nula ou somente espaços em branco | Objeto de erro com `message` |

**Response 201 — Body**

```json
{
  "id": 1,
  "url": "https://devblogs.microsoft.com/dotnet/feed/"
}
```

**Response 400 — Body**

```json
{
  "message": "A URL não pode ser vazia."
}
```

---

### GET /api/subscriptions

Retorna a lista completa de assinaturas adicionadas na sessão atual.

**Request**

```
GET /api/subscriptions
```

Sem parâmetros, sem body.

**Respostas**

| Status | Descrição | Body |
|---|---|---|
| `200 OK` | Lista retornada com sucesso (pode ser vazia) | Array de objetos `Subscription` |

**Response 200 — Body**

```json
[
  {
    "id": 1,
    "url": "https://devblogs.microsoft.com/dotnet/feed/"
  },
  {
    "id": 2,
    "url": "https://feeds.feedburner.com/exemplo"
  }
]
```

Lista vazia (nenhuma assinatura adicionada ainda):

```json
[]
```

---

## Schema: Subscription

```json
{
  "id": "integer (gerado pelo servidor, começa em 1)",
  "url": "string (não vazia)"
}
```

---

## Regras de Validação na Fronteira da API

| Campo | Regra | Código HTTP em violação |
|---|---|---|
| `url` (POST) | Não pode ser nula, vazia ou somente espaços em branco | 400 |

> **Nota MVP**: Nenhuma outra validação (formato de URL, acessibilidade de rede, unicidade) é realizada no MVP. O backend aceita qualquer string não vazia.

---

## Configuração de CORS

O backend DEVE configurar CORS explicitamente para permitir a origem do frontend.

| Configuração | Valor |
|---|---|
| Origens permitidas | Configurado em `appsettings.json` → chave `AllowedOrigins` |
| Métodos permitidos | `GET`, `POST` |
| Headers permitidos | `Content-Type` |
| `AllowAnyOrigin()` | **PROIBIDO** — viola Princípio II da Constituição |

**Exemplo de `appsettings.json` (backend)**:

```json
{
  "AllowedOrigins": ["http://localhost:5001"]
}
```
