# Modelo de Dados: MVP Leitor de Feed RSS

**Fase**: 1 — Design  
**Branch**: `001-mvp-rss-reader`  
**Data**: 2026-05-02

---

## Entidades

### Subscription (Assinatura)

Representa um feed RSS/Atom que o usuário deseja acompanhar.

| Campo | Tipo | Obrigatório | Regras de Validação |
|---|---|---|---|
| `Id` | `int` | Sim | Gerado internamente; sequencial; começa em 1 |
| `Url` | `string` | Sim | Não pode ser nula, vazia ou somente espaços em branco |

**Notas de design**:
- No MVP, `Url` é tratada como texto simples — sem validação de formato de URL ou verificação de acessibilidade de rede
- `Id` é incluído para facilitar a identificação de entradas na lista e para suportar a remoção de assinaturas no MVP Estendido sem quebrar contratos existentes
- Nenhum outro atributo (título, categoria, data de adição) é necessário no MVP — YAGNI

---

## Armazenamento em Memória

O estado do MVP é mantido em uma lista singleton gerenciada pelo `SubscriptionService` no backend.

```
SubscriptionService (Singleton)
└── List<Subscription>  →  cresce ao longo da sessão; resetado ao reiniciar o processo
```

**Características**:
- Registro: adição ao final da lista
- Leitura: retorno da lista completa em ordem de inserção
- Persistência: nenhuma — comportamento esperado e documentado na spec

---

## Transições de Estado

O MVP não possui transições de estado complexas. O ciclo de vida de uma assinatura é:

```
[criada]  →  [na lista em memória]  →  [perdida ao encerrar o processo]
```

---

## Escopo Fora do MVP

Os seguintes atributos e entidades NÃO fazem parte do modelo do MVP e NÃO devem ser implementados:

- `Title`, `Description`, `LastFetched` — atributos do MVP Estendido
- `FeedItem` — entidade do MVP Estendido (itens individuais de um feed)
- Persistência em banco de dados — Pós-MVP (EF Core + SQLite)
- Unicidade de URL — sem validação de duplicatas no MVP
