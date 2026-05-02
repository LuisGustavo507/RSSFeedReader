<!--
Relatório de Impacto de Sincronização
======================================
Alteração de versão: N/A (ratificação inicial) → 1.0.0
Princípios modificados: N/A
Seções adicionadas:
  - Princípios Fundamentais (I–V)
  - Restrições de Stack Tecnológica
  - Fluxo de Desenvolvimento
  - Governança
Seções removidas: N/A
Templates verificados:
  - .specify/templates/plan-template.md  ✅ nenhuma alteração necessária (gate Verificação de Constituição é genérico)
  - .specify/templates/spec-template.md  ✅ nenhuma alteração necessária
  - .specify/templates/tasks-template.md ✅ nenhuma alteração necessária
TODOs adiados: nenhum
-->

# Constituição do RSS Feed Reader

## Princípios Fundamentais

### I. MVP em Primeiro Lugar (INEGOCIÁVEL)
O desenvolvimento DEVE sempre começar pelo escopo mínimo viável definido em `ProjectGoals.md`.
Funcionalidades pós-MVP NÃO DEVEM ser implementadas até que o milestone de MVP atual esteja
estável e verificado.

- "MVP completo" = o usuário consegue adicionar uma URL de assinatura de feed e a lista
  atualizada é exibida imediatamente
- Armazenamento em memória é a única estratégia de persistência aceita para o MVP inicial
- Nenhum parsing de feed, cliente HTTP ou operação de rede pertence ao escopo do MVP
- Qualquer funcionalidade das listas MVP Estendido ou Pós-MVP introduzida antes da verificação
  do MVP viola este princípio

**Justificativa**: Previne expansão de escopo, garante entrega antecipada de valor verificável
e mantém os ciclos de feedback curtos.

### II. Segurança por Padrão
Segurança é uma preocupação de primeira classe e DEVE ser tratada no momento do design,
não adicionada após o fato.

- Todo endpoint HTTP DEVE validar e higienizar todas as entradas fornecidas pelo usuário
  na fronteira da API
- A política de CORS DEVE ser configurada explicitamente; origens curinga (`*`) são proibidas
  em produção
- URLs fornecidas pelo usuário DEVEM ser validadas (formato + esquemas permitidos: `http`/`https`)
  antes de qualquer operação HTTP ser executada (MVP Estendido+)
- Todas as dependências de terceiros DEVEM ser verificadas quanto a CVEs conhecidos
  (`dotnet list package --vulnerable`) antes de serem adicionadas ou atualizadas
- Nenhum segredo, chave de API ou credencial DEVE aparecer no código-fonte ou em arquivos
  `appsettings.json` versionados
- Respostas HTTP NÃO DEVEM vazar stack traces internos ou detalhes de implementação

**Justificativa**: Defeitos de segurança são desproporcionalmente caros de corrigir após a
implantação. Os controles do OWASP Top 10 (validação de entrada, CORS, higiene de dependências)
DEVEM ser aplicados desde o primeiro commit.

### III. Testes em Primeiro Lugar (INEGOCIÁVEL)
TDD é obrigatório em toda lógica de negócio. Os testes DEVEM ser escritos e confirmados como
falhos antes de qualquer código de implementação ser escrito.

- Ciclo obrigatório: Vermelho (teste falhando) → Verde (implementação mínima) → Refatoração
- Testes unitários DEVEM cobrir toda a lógica de camada de serviço e domínio em isolamento
  de I/O e frameworks
- Testes de integração DEVEM ser adicionados para cada novo endpoint de API e para qualquer
  alteração de contrato
- Uma funcionalidade NÃO é considerada completa até que todos os seus testes passem sem
  supressões ou pulos
- A cobertura de testes NÃO DEVE regredir entre commits

**Justificativa**: A disciplina de testes em primeiro lugar detecta falhas de design cedo,
documenta a intenção e previne regressões. Pular este princípio nunca é uma troca válida
por velocidade de entrega.

### IV. Separação Clara de Responsabilidades
O sistema DEVE ser composto por duas camadas distintas e não sobrepostas.

- **Backend (ASP.NET Core Web API)**: responsável por todo gerenciamento de dados,
  operações de feed e contratos de API
- **Frontend (Blazor WebAssembly)**: responsável por todo estado de UI, renderização de
  componentes e interação com o usuário
- Lógica de negócio NÃO DEVE ser colocada dentro de componentes Razor; componentes DEVEM
  delegar para serviços
- Contratos de API DEVEM ser documentados via Swagger/OpenAPI para todos os endpoints
- Preocupações transversais (logging, tratamento de erros, validação) DEVEM ser tratadas
  via middleware ou abstrações de serviço compartilhadas — não inline em controllers
  ou componentes

**Justificativa**: Garante testabilidade independente de cada camada, permite a evolução
de frontend/backend sem alterações acopladas e previne o anti-padrão de "componente gordo".

### V. Manutenibilidade e Simplicidade
O código DEVE ser escrito para ser compreendido e alterado com segurança por qualquer membro
da equipe, incluindo futuros colaboradores não familiarizados com a implementação original.

- Os princípios SOLID DEVEM ser aplicados a todas as classes e interfaces não triviais
- Strings mágicas e números mágicos são proibidos; use constantes, enums ou modelos
  fortemente tipados
- YAGNI se aplica estritamente: complexidade introduzida para necessidades futuras hipotéticas
  DEVE ser rejeitada
- Mudanças incompatíveis em APIs ou contratos de dados compartilhados DEVEM ser comunicadas
  via versionamento semântico
- Revisões de código DEVEM rejeitar alterações que aumentem a complexidade ciclomática sem
  justificativa documentada

**Justificativa**: Manutenibilidade é um multiplicador para todo desenvolvimento futuro.
Simplicidade hoje reduz o custo de integração, a superfície de bugs e a dívida técnica acumulada.

## Restrições de Stack Tecnológica

A stack a seguir é fixada para este projeto. Desvios requerem uma emenda formal à constituição
(mínimo bump de versão MINOR).

| Camada | Tecnologia | Observações |
|---|---|---|
| Backend | ASP.NET Core Web API (.NET / C#) | Fixo para todos os milestones |
| Frontend | Blazor WebAssembly (C#) | Fixo para todos os milestones |
| Parsing de Feed | `System.ServiceModel.Syndication` | Somente MVP Estendido+ |
| Cliente HTTP | `HttpClient` via ASP.NET Core DI | Somente MVP Estendido+ |
| Persistência | Em memória (`List<T>`) | Somente MVP |
| Persistência | EF Core + SQLite | Somente milestone Pós-MVP |
| Plataforma Alvo | Multiplataforma (Windows / macOS / Linux) | Obrigatório em todos os builds |

Nenhuma nova dependência de runtime pode ser introduzida sem: (a) uma justificativa documentada,
(b) um resultado de verificação de CVE, e (c) reconhecimento explícito da equipe.

## Fluxo de Desenvolvimento

### Checklist de Prontidão Local
Antes de qualquer funcionalidade ser considerada testável, TODOS os itens a seguir DEVEM
ser confirmados:

- [ ] Backend inicia sem erros de runtime e escuta na porta configurada
- [ ] Frontend inicia sem erros de runtime e carrega no navegador
- [ ] `wwwroot/appsettings.json` aponta para a URL de origem correta do backend
- [ ] A política de CORS do backend permite explicitamente a origem do frontend (sem curinga)
- [ ] O console do DevTools do navegador não exibe erros de conexão ou CORS ao carregar a página

### Limpeza Obrigatória do Template (Única Vez)
O template padrão do projeto Blazor inclui páginas de demonstração que DEVEM ser removidas
antes que qualquer trabalho de funcionalidade do MVP comece. Este é um gate da Fase 2
(Fundacional) — nenhuma implementação de funcionalidade pode prosseguir até que a limpeza
seja verificada como completa.

Arquivos a deletar: `Pages/Home.razor`, `Pages/Counter.razor`, `Pages/Weather.razor`
Arquivo a atualizar: `Layout/NavMenu.razor` (remover links de navegação para as páginas deletadas)

### Gate de Qualidade por Funcionalidade
Uma branch de funcionalidade DEVE satisfazer todos os itens a seguir antes do merge:

1. Todos os testes unitários e de integração passam (`dotnet test` — zero falhas, zero pulos)
2. Nenhum novo aviso do compilador introduzido
3. Checklist de segurança (Princípio II) verificado para qualquer endpoint novo ou alterado
4. Documentação Swagger/OpenAPI atualizada para qualquer contrato de API novo ou alterado

## Governança

Esta constituição substitui todas as outras convenções, padrões e decisões ad-hoc dentro
deste projeto. Qualquer conflito entre este documento e uma decisão local DEVE ser resolvido
em favor desta constituição, a menos que uma emenda formal seja ratificada.

**Procedimento de emenda**:
1. Propor a alteração por escrito, citando o(s) princípio(s) afetado(s) e a justificativa
2. Classificar o bump de versão antes de redigir:
   - MAJOR — remoção de princípio, redefinição ou alteração de governança incompatível com versões anteriores
   - MINOR — novo princípio ou seção adicionado, ou orientação material substancialmente expandida
   - PATCH — esclarecimentos, correções de redação ou refinamentos não semânticos
3. Após qualquer emenda MAJOR, todas as especificações em andamento e tarefas abertas DEVEM
   ser revisadas quanto à conformidade antes que o trabalho seja retomado
4. Todo PR DEVE verificar a conformidade com todos os princípios ativos antes da aprovação do merge
5. `LAST_AMENDED_DATE` DEVE ser atualizado a cada alteração ratificada

**Versão**: 1.0.0 | **Ratificada em**: 2026-05-02 | **Última Emenda**: 2026-05-02
