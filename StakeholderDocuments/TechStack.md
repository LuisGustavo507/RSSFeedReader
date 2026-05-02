Stack Tecnológica para o Leitor de Feeds RSS

Nosso leitor de feeds RSS utilizará um backend em ASP.NET Core Web API e um frontend em Blazor WebAssembly. Essa combinação permite o desenvolvimento rápido do MVP, ao mesmo tempo em que suporta melhorias futuras prontas para produção.
Por que ASP.NET Core Web API + Blazor WebAssembly?

Construir um leitor de feeds RSS com um backend em ASP.NET Core Web API e um frontend em Blazor WebAssembly oferece diversas vantagens:

    Desenvolvimento rápido: Ambas as tecnologias funcionam bem juntas com configuração mínima, permitindo o desenvolvimento ágil da demonstração.
    Separação de responsabilidades: O backend gerencia os dados e (no MVP Estendido) as operações de feed, enquanto o frontend foca na interação com o usuário.
    Multiplataforma: Tanto o ASP.NET Core quanto o Blazor são multiplataforma, permitindo que a aplicação rode no Windows, macOS e Linux.
    Complexidade incremental: Comece com o gerenciamento simples de assinaturas (MVP), depois adicione a busca de feeds (MVP Estendido), e então adicione persistência e funcionalidades avançadas.
    Arquitetura preparada para o futuro: Embora o MVP seja mínimo (apenas gerenciamento da lista de assinaturas), essa arquitetura suporta a adição de:
        Busca e análise de feeds (System.ServiceModel.Syndication)
        Persistência em banco de dados (EF Core + SQLite)
        Processamento em segundo plano (BackgroundService para polling)
        Funcionalidades avançadas (lido/não lido, pastas, etc.)
    Código compartilhado: O Blazor WebAssembly usa C#, permitindo o compartilhamento de código entre frontend e backend quando necessário.

Responsabilidades

Para o MVP (apenas gerenciamento de assinaturas):

O backend é responsável por:

    Expor uma API para adicionar assinaturas
    Armazenar assinaturas em memória
    Retornar a lista de assinaturas

O frontend é responsável por:

    Interface de gerenciamento de assinaturas (campo de entrada + botão adicionar)
    Exibir a lista de assinaturas

Para o MVP Estendido (adicionar busca de feeds):

O backend adiciona:

    Busca e análise de feeds RSS/Atom quando solicitado
    Retorno dos itens do feed para a interface

O frontend adiciona:

    Botão de atualização manual
    Exibição de itens (título e link no mínimo)
    Mensagens básicas de erro

Abordagem de implementação com foco no MVP

Para entregar o MVP rapidamente:

MVP (apenas gerenciamento de assinaturas):

    Armazenamento: Use armazenamento em memória (List<string> ou modelo simples). As assinaturas são perdidas quando o app é encerrado.
    Sem operações de feed: Sem cliente HTTP, sem biblioteca de análise, sem busca de feeds
    Foco: Interface básica e comunicação com a API (adicionar assinatura, obter lista de assinaturas)

MVP Estendido (adicionar busca de feeds):

    Análise: Adicione System.ServiceModel.Syndication para análise básica de RSS/Atom
    Cliente HTTP: Adicione HttpClient para buscar feeds
    Atualização: Apenas manual — sem polling ou agendamento em segundo plano
    Tratamento de erros: Mensagens simples de "falha ao carregar", sem diagnósticos detalhados
    Exibição de conteúdo: Somente texto simples (título + link), sem necessidade de renderização HTML

Essa abordagem incremental torna o desenvolvimento extremamente rápido, mantendo a arquitetura limpa para melhorias futuras.
Desenvolvimento local
Inicialização do projeto Blazor

Ao criar um novo projeto Blazor WebAssembly a partir do template, o projeto inclui páginas de demonstração que devem ser removidas para evitar conflitos com as funcionalidades do MVP.

⚠️ CRÍTICO: Esta limpeza deve ser concluída na Fase 2 (Fundacional) e VERIFICADA antes de qualquer implementação de funcionalidades de interface. Erros em tempo de execução causados por limpeza incompleta desperdiçarão tempo de desenvolvimento.

Etapas de limpeza obrigatórias:

    Remova as páginas de demonstração do template em frontend/[NomeDoProjeto].UI/Pages/:
        Delete Home.razor (conflita com a rota raiz)
        Delete Counter.razor (página de demonstração)
        Delete Weather.razor (página de demonstração)
    Atualize o menu de navegação em frontend/[NomeDoProjeto].UI/Layout/NavMenu.razor:
        Remova os links de navegação para as páginas deletadas
        Atualize os itens do menu para refletir apenas as funcionalidades do MVP
        Altere o texto do link de navegação raiz para corresponder à sua página principal (ex.: "Assinaturas")
    Verifique o roteamento:
        Certifique-se de que apenas UMA página usa a diretiva @page "/" (sua página principal do MVP)
        Todas as outras páginas devem usar rotas únicas (ex.: @page "/configuracoes")
    Verifique a conclusão da limpeza antes de prosseguir com a implementação:

powershell

   # Lista todas as páginas Razor - deve exibir APENAS suas páginas do MVP (ex.: NotFound.razor, Subscriptions.razor)
   Get-ChildItem frontend/[NomeDoProjeto].UI/Pages/ -Filter *.razor | Select-Object Name

PARE: Não prossiga com a implementação de funcionalidades até que:

    ✗ Home.razor tenha sido REMOVIDO
    ✗ Counter.razor tenha sido REMOVIDO
    ✗ Weather.razor tenha sido REMOVIDO
    ✓ Apenas suas páginas do MVP permaneçam

    Teste imediatamente se há conflitos de roteamento após a limpeza:

powershell

   # Build limpo para remover assemblies em cache
   dotnet clean frontend/[NomeDoProjeto].UI/[NomeDoProjeto].UI.csproj
   dotnet build frontend/[NomeDoProjeto].UI/[NomeDoProjeto].UI.csproj
   
   # Inicie o frontend para verificar se não há erros de roteamento
   dotnet run --project frontend/[NomeDoProjeto].UI

Acesse a URL do frontend no navegador. Se você ver um erro de "rota ambígua" no console do navegador (Ferramentas do Desenvolvedor F12), a limpeza está incompleta. Corrija o problema antes de implementar qualquer funcionalidade.

Por que isso importa:

Os templates do Blazor incluem páginas de demonstração com rotas pré-configuradas. Se você criar novas páginas com as mesmas rotas (especialmente a rota raiz /), encontrará exceções de rota ambígua em tempo de execução. A mensagem de erro será similar a:

System.InvalidOperationException: The following routes are ambiguous:
'' in '[NomeDoProjeto].UI.Pages.Home'
'' in '[NomeDoProjeto].UI.Pages.SuaFuncionalidade'

Esses erros só aparecem em tempo de execução após você já ter implementado funcionalidades, tornando-os custosos para depurar. As etapas de verificação acima detectam esse problema imediatamente durante a limpeza da Fase 2, antes de qualquer trabalho em funcionalidades.

Limpar as páginas do template antes de implementar as funcionalidades do MVP evita esses conflitos e garante uma estrutura de projeto limpa e focada nos requisitos de negócio.
Configuração de portas

O backend da API e o frontend rodam em portas localhost separadas. A consistência das portas é crítica — elas devem ser coordenadas entre três locais:

    Porta do backend (definida em backend/RSSFeedReader.Api/Properties/launchSettings.json):
        Padrão: http://localhost:5151
        É onde a API escuta as requisições
    Porta do frontend (definida em frontend/RSSFeedReader.UI/Properties/launchSettings.json):
        Padrão: http://localhost:5213
        É onde o app Blazor é executado
    URL base da API (configurada em frontend/RSSFeedReader.UI/wwwroot/appsettings.json):
        Deve corresponder à porta do backend do passo 1
        Exemplo: {"ApiBaseUrl": "http://localhost:5151/api/"}
    Política de CORS (configurada em backend/RSSFeedReader.Api/Program.cs):
        Deve permitir a porta do frontend do passo 2
        Exemplo: .WithOrigins("http://localhost:5213", "https://localhost:7025")

Boas práticas de configuração

    Program.cs do frontend: Leia a URL da API a partir da configuração, não a deixe fixada no código:

csharp

  var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5151/api/";
  builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

    CORS do backend: Permita as portas reais do frontend definidas no launchSettings.json
    Configuração para testes: Antes de testar, verifique:
        O backend está em execução e acessível na porta configurada
        O appsettings.json do frontend aponta para a porta correta do backend
        O CORS permite a origem do frontend

Para o MVP: Teste adicionando URLs de assinatura e verificando se aparecem na lista.

Para o MVP Estendido: Teste com um feed reconhecidamente válido como https://devblogs.microsoft.com/dotnet/feed/
Melhorias futuras (pós-MVP)

Quando estiver pronto para expandir além da demonstração básica, esta arquitetura suporta:

    Persistência em banco de dados: Adicione EF Core + SQLite para armazenar assinaturas e itens entre sessões
    Polling em segundo plano: Implemente BackgroundService para atualizar feeds automaticamente em intervalos programados
    Sanitização de HTML: Adicione a biblioteca HtmlSanitizer para exibir com segurança conteúdo rico dos feeds
    Descoberta de feed a partir de site: Use HtmlAgilityPack para encontrar URLs de feed a partir de links de sites
    Tratamento de erros aprimorado: Implemente lógica de nova tentativa, timeouts e mensagens de erro detalhadas
    Testes: Adicione testes unitários e de integração usando xUnit
    Otimização: Implemente cache HTTP (ETag/Last-Modified), desduplicação e melhorias de desempenho

Resumo

O ASP.NET Core Web API com Blazor WebAssembly fornece um caminho direto para construir o leitor de feeds RSS de forma incremental:

    MVP: Apenas gerenciamento de assinaturas (adicionar + listar) — extremamente simples, sem operações de feed
    MVP Estendido: Adiciona busca de feeds e exibição de itens — ainda simples com armazenamento em memória e atualização manual
    Futuro: Adiciona persistência, processamento em segundo plano e funcionalidades avançadas

A arquitetura é intencionalmente mínima para permitir desenvolvimento rápido, enquanto as escolhas tecnológicas suportam a adição de funcionalidades prontas para produção futuramente, sem necessidade de reescrita completa.
