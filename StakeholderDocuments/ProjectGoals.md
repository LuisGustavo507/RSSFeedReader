Objetivos do Projeto
Construir um leitor simples de feeds RSS/Atom. O objetivo é demonstrar a capacidade mais básica (gerenciar uma lista de assinaturas) sem a complexidade de buscar e exibir o conteúdo dos feeds.
Propósito
O app existe para demonstrar como um usuário pode construir uma lista de assinaturas de feeds RSS. Trata-se de uma prova de conceito focada na interface de gerenciamento de assinaturas.
Escopo alvo (apenas MVP)
Esta é uma aplicação POC mínima para um único usuário, executada localmente. Foi projetada para ser desenvolvida e testada no Windows, macOS ou Linux.
O MVP inclui apenas:

Adicionar uma assinatura de feed por URL
Exibir a lista de assinaturas na interface

Todas as demais funcionalidades (busca de feeds, exibição de itens, persistência, remoção de assinaturas, etc.) são adiadas para o MVP Estendido ou Pós-MVP.
Abordagem de entrega
O foco está no desenvolvimento rápido da funcionalidade do MVP. Construa a funcionalidade mínima primeiro:

Adicionar uma assinatura por URL
Exibir a lista de assinaturas

Para manter o desenvolvimento ágil:

Sem necessidade de busca ou análise de feeds para o MVP
Sem validação de URLs de feed (pressuponha que o usuário fornece URLs válidas)
Armazene assinaturas apenas em memória (abordagem mais simples)
Mantenha a interface simples e funcional em vez de polida

O que significa "MVP funcionando"
O MVP está completo quando:

Um usuário consegue adicionar uma assinatura de feed colando uma URL
A interface exibe a lista atualizada de assinaturas

Nenhuma busca, análise ou exibição de itens de feed é necessária para o MVP.
MVP Estendido (próxima fase)
Após o MVP básico estar funcionando, o MVP Estendido adiciona capacidades de busca e exibição de feeds:

Um usuário pode clicar em um botão para atualizar o feed manualmente
Os itens do feed são exibidos (título e link no mínimo)

Teste com um feed RSS reconhecidamente válido, como https://devblogs.microsoft.com/dotnet/feed/.
Checklist de desenvolvimento local
Antes de testar o MVP, verifique:

 O backend executa sem erros e escuta na porta configurada
 O frontend executa sem erros e carrega no navegador
 A configuração do frontend (wwwroot/appsettings.json) aponta para a URL correta do backend
 O CORS do backend permite a origem do frontend
 O console do DevTools do navegador não exibe erros de conexão ao carregar a página

Melhorias futuras (pós-MVP)
Assim que o MVP Estendido estiver funcionando (gerenciamento de assinaturas + busca de feeds + exibição de itens), estas funcionalidades poderão ser adicionadas:

Persistência: Salvar assinaturas e itens entre sessões (requer implementação de banco de dados)
Remoção de assinaturas: Permitir que usuários excluam feeds que não desejam mais
Polling em segundo plano: Atualizar feeds automaticamente em intervalos programados
Tratamento de erros aprimorado: Exibir mensagens de erro detalhadas para diferentes cenários de falha
Renderização de conteúdo: Exibir o conteúdo completo dos itens, não apenas título e link
Controle de lido/não lido: Marcar itens como lidos e filtrar adequadamente
Organização: Agrupar feeds em pastas ou categorias

Nota sobre seleção de tecnologia
Embora este MVP seja intencionalmente simples, as escolhas tecnológicas (ASP.NET Core + Blazor) devem suportar futuras funcionalidades prontas para produção sem exigir uma reescrita completa. A arquitetura permite adicionar persistência, operações em segundo plano e capacidades aprimoradas de interface conforme necessário.
Como este documento se relaciona com os demais

AppFeatures.md descreve as funcionalidades específicas voltadas ao usuário para o MVP
TechStack.md explica as escolhas tecnológicas e como elas suportam os objetivos do MVP