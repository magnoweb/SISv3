# Selecta — Nova arquitetura (.NET 10 + Blazor WebAssembly)

Reescrita da solução **Selecta** original (ASP.NET MVC 5 / .NET Framework, EF6,
9 projetos) para **.NET 10**, simplificada em **4 camadas**, **ligada à mesma
base de dados SQL Server já existente**.

## Convenção de nomes: código em inglês, base de dados em português

Classes, propriedades, métodos e ficheiros usam nomes em **inglês** (`City`,
`User`, `CreateCityDto`, `GetByIdAsync`, ...). A base de dados existente
continua com os nomes originais em **português** (tabela `Cidades`, colunas
`Codigo`/`Nome`/`Uf`, tabela `Usuarios`, colunas `Nome`/`Senha`/`Ativo`/...).

Todo esse mapeamento fica isolado e explícito em
`Selecta.Infra/Data/Configurations/*.cs`, usando `IEntityTypeConfiguration<T>`
do EF Core — cada `.Property(x => x.Algo).HasColumnName("NomeReal")` liga o
nome em inglês da propriedade à coluna real na BD. As entidades em
`Selecta.Core` não sabem nada sobre nomes de tabelas/colunas; só a
`Configuration` sabe. Isto é o mesmo espírito do antigo `CidadeConfig`/
`UsuarioConfig` (EF6), só que agora com o mapeamento de nomes explícito em
vez de depender de coincidência entre nome de propriedade e nome de coluna.

Exemplo (`CityConfiguration.cs`):

```csharp
builder.ToTable("Cidades");                                   // tabela real
builder.Property(c => c.Id).HasColumnName("CidadeId");        // PK real
builder.Property(c => c.Code).HasColumnName("Codigo");
builder.Property(c => c.Name).HasColumnName("Nome");
builder.Property(c => c.State).HasColumnName("Uf");
```

Para migrar um módulo novo: define a entidade em inglês em `Selecta.Core`,
depois vai ao `*Config.cs` (EF6) equivalente na solução antiga para veres os
nomes reais de tabela/coluna, e repete esse padrão de `HasColumnName(...)` —
sem isso, o EF Core assume (por convenção) que o nome da coluna é igual ao
nome da propriedade, o que já não é verdade a partir de agora.

## Arquitetura

```
Selecta.sln
├── Selecta.Core     (class library) — entidades, DTOs, interfaces, regras de negócio
├── Selecta.Infra    (class library) — EF Core 10, DbContext, mapeamentos, repositórios, DI, Dapper (dashboard)
├── Selecta.Api      (ASP.NET Core Web API) — controllers, JWT, Swagger
└── Selecta.Web       (Blazor WebAssembly + MudBlazor + Radzen.Blazor) — UI, consome a Api via HTTP
```

Mapeamento em relação à solução antiga:

| Antigo (9 projetos)                                             | Novo          |
|-------------------------------------------------------------------|---------------|
| `Selecta.Domain` + `Selecta.Application`                        | `Selecta.Core` |
| `Selecta.Infra.Data` + `Selecta.Infra.Ioc` + `Selecta.Infra.Shared` | `Selecta.Infra` |
| `Selecta.Web` (MVC5, controllers + views Razor)                 | `Selecta.Api` (só API) |
| Views/Razor/jQuery/DataTables                                    | `Selecta.Web` (Blazor WASM + MudBlazor) |
| `Selecta.Infra.Worker`                                           | Pode voltar como `BackgroundService` dentro da `Selecta.Api`, quando migrado |
| `Selecta.Infra.Data.Sync` / `.Scripts`                            | Fora de âmbito nesta 1ª fase — scripts SQL e sync ficam como estão |

As três interfaces quase idênticas da solução antiga (`IRepositoryBase`,
`IServiceBase`, `IAppServiceBase` — todas com `Adicionar/ObterPorId/ObterTodos/
Atualizar/Remover`) foram colapsadas: `Selecta.Infra` implementa
`IRepositoryBase<T>` (agora assíncrono, `AddAsync/GetByIdAsync/GetAllAsync/
Update/Remove/SaveChangesAsync`) e `Selecta.Core` tem um serviço por entidade
(`CityService`, `UserService`, ...) que já fala em DTOs — sem a camada extra
de "AppService" que só passava tudo para a frente.

## UI: MudBlazor + Radzen.Blazor lado a lado

As telas usam as duas bibliotecas de propósito: formulários, diálogos e o
layout continuam em **MudBlazor** (`MudDialog`, `MudTextField`,
`MudDatePicker`, `MudCheckBox`, ...); a listagem de cada módulo usa
**`RadzenDataGrid`**, com filtro e ordenação por coluna ligados diretamente à
paginação server-side (não é um filtro só sobre a página já carregada). Os
**dropdowns ligados a listas de entidades** (Company, JobTitle, User, etc. —
qualquer lista que possa crescer muito) usam **`RadzenDropDown`** com
`AllowVirtualization="true"` (só renderiza os itens visíveis no popup, em
vez de todos de uma vez) e `AllowFiltering="true"` (o utilizador digita para
filtrar) — evita o travamento da UI que o `MudSelect` tinha ao carregar
listas grandes, já que o `MudSelect` renderiza um `<MudSelectItem>` por
opção de uma só vez. Os dropdowns de **enum** (Gender, CompanyType, Status,
etc. — conjunto fixo e pequeno de opções) continuam em `MudSelect` — não há
nada a "virtualizar" numa lista de 3-6 valores fixos, converter esses só
adicionaria risco sem benefício.

Para os casos em que o texto exibido no dropdown é composto (ex.: `"Nome
(Empresa)"`), `RadzenDropDown` não aceita uma expressão — `TextProperty`
só aceita o nome de uma propriedade simples — por isso esses usam o
parâmetro `Template` (`Context="item"`, tipo `dynamic`) para desenhar o
texto à mão; `TextProperty` continua a apontar para o campo principal, que
é o que o filtro efetivamente pesquisa.

**Bug corrigido nesta conversão**: em `CompetencyProfileLines`, a lista de
perfis combinados (Job Title + Professional Group) era um
`List<(Guid Id, string Name)>` — um tuplo. Tuplos só expõem `Item1`/`Item2`
em tempo de execução (os nomes amigáveis "Id"/"Name" são só açúcar de
compilação, não sobrevivem à reflection que `RadzenDropDown` usa para
`TextProperty`/`ValueProperty`) — o mesmo tipo de bug já visto com o tipo
anónimo do `RadzenChart` no dashboard. Corrigido com um `record
ProfileOption(Guid Id, string Name)` concreto.

Isso é possível porque `IRepositoryBase<T>.GetPagedAsync` aceita `filter` e
`orderBy` como expressões [Dynamic LINQ](https://dynamic-linq.net/)
(`System.Linq.Dynamic.Core`) — exatamente o formato que o `RadzenDataGrid`
já gera sozinho em `LoadDataArgs.Filter`/`LoadDataArgs.OrderBy` quando o
utilizador filtra ou clica num cabeçalho de coluna. O fluxo, por módulo:

```
RadzenDataGrid.LoadData(args)
  → ApiClient.GetPagedAsync(page, pageSize, args.Filter, args.OrderBy)
    → GET /api/{recurso}/paged?filter=...&orderBy=...
      → Service.GetPagedAsync(...)
        → Repository.GetPagedAsync(...)
          → DbSet.Where(filter).OrderBy(orderBy).Skip(...).Take(...)   // EF Core + Dynamic LINQ
```

Cada `RepositoryBase<T>` tem um `DefaultOrderBy` (ex.: `"Name"`, `"Order"`,
`"CreatedAt desc"`) usado enquanto o utilizador não clicou em nenhuma
coluna; um filtro que a Dynamic LINQ não consiga interpretar é ignorado
(não derruba o pedido com 500).

**Colunas marcadas `Sortable="false" Filterable="false"`**: campos do DTO
que vêm de navegação/join ou são calculados (não existem como propriedade
direta na entidade) — `JobOpeningDto.ManagerName/ContactName/JobTitleName/
RecruitmentStageName/OpenDuration`, `ProposalDto.ServiceOfferingName/
ProspectCompanyName/TotalDays/TotalWorkingDays`,
`ScheduleBlockDto.UserName`, e as colunas de "Company"/"Base result" em
Contacts/JobTitles/CompanyEvaluationResults (resolvidas no cliente a partir
de uma lista já carregada, para mostrar o nome em vez do Guid).

## Identidade visual e navegação: alinhadas com o SIS v2

Foram partilhadas capturas de tela da v2 (login, dashboard, Agenda de
Seleção, Lista de Parecer, Laudos). Usei-as para alinhar a v3 desde já —
é a mudança mais barata de fazer cedo (toca em todas as páginas) e mais
cara de adiar:

- **Nome e versão**: "SIS . Sistema Integrado Selecta", `v3.0` no rodapé do
  login (a v2 mostrava `v2.0`)
- **Paleta** (`Selecta.Web/SelectaTheme.cs`): laranja/vermelho quente
  (`#E8590C`/`#D84315`), aproximando a cor do botão "Entrar" e da barra
  lateral da v2. **Aproximação visual, não extração exata da marca** — se
  surgirem os valores hex oficiais, é só ajustar esse ficheiro
- **Login** (`Pages/Login.razor`): título e rodapé "Selecta Instituto de
  Psicologia LTDA." iguais à v2; campos e mensagens continuam em inglês,
  consistente com o resto das telas já construídas — não fiz uma tradução
  geral da UI, que seria uma mudança maior e separada desta
- **Navegação** (`Layout/NavMenu.razor`): reagrupada em `MudNavGroup`
  espelhando as categorias da v2 — Dashboard, Agendas, Avaliações,
  Recrutamento, Administrativo, Tabelas Gerais, Admin — em vez da lista
  plana de 29 links que existia antes. **O agrupamento é a minha melhor
  interpretação** a partir das capturas (a v2 mostra só 3 itens dentro de
  "Avaliações" — Lista de Parecer, Laudos, Diretórios —, então assumi que
  os catálogos mais granulares — competências, templates, etc. — vivem em
  "Tabelas Gerais"); ajusta se não bater com a categorização real.

### Dashboard: expandido com os widgets reais

O `DashboardRepository` (Dapper) ganhou mais consultas, e o `Home.razor`
mais secções, para aproximar os painéis da v2:

- **Aniversariantes de hoje** — corrigido: são **Contacts** (pessoas nas
  empresas clientes), não candidatos, e só os de **hoje**, não do mês —
  confirmado a partir do código original: `Contatos.Where(c =>
  c.DiaAniversario == hoje.Day && c.MesAniversario == hoje.Month &&
  c.Ativo && c.ReceberNotificacoes)`. `Contact.BirthDay`/`BirthMonth`
  (`DiaAniversario`/`MesAniversario`) já estavam mapeados desde que o
  módulo foi portado — só não tinham sido usados aqui
- **Avaliações Dia/Mês/Ano** — contagens simples sobre `EventosAvaliacao.Data`
- **Gráfico "Avaliações no último ano"** — `RadzenChart` com os últimos 12
  meses (meses sem avaliação aparecem como zero, preenchido em C# depois de
  trazer só os meses com dados via SQL)
- **Distribuição de resultado** — Aconselhável/Aconselhável-Restrição/
  Desaconselhável, com barras de percentagem (`Components/ResultBar.razor`)

**Fora desta fase**: os painéis "Avisos", "Entrevistas" e as prévias
"Agenda Seleção"/"Agenda Recrutamento" da v2 dependem de módulos ainda não
portados (`Avisos`, e o próprio módulo de Agenda de entrevistas) — não
adicionei versões com dados falsos só para parecer completo.

### O que as capturas esclareceram para os próximos módulos

- **Lista de Parecer** (`ListasParecer`/`ListaParecerEventos`, antes um dos
  "não investigados"): agora está claro — agrupa vários candidatos de uma
  mesma empresa, cada um com um Parecer (mesmos valores de
  `AssessmentResult` que já portei), ligado a um Contact/Responsible, com
  opção de notificação por email.
- **Agenda** (`AgendaSelecao`/`AgendaRecrutamento`): a tela real mostra
  exatamente os campos que eu esperava a partir do schema — Status,
  Empresa, Cargo, Solicitante (Contact), Nome, CPF, Origem, Data, Horário,
  Observações, Observações do Cliente — mais um histórico de agendamentos
  anteriores do mesmo candidato. Boa base para desenhar a entidade com
  confiança.
- **Laudo**: confirma que "Analítico"/"Sintético"/"Relatório"/
  "Analítico/L.A.B.E.L"/"Sintético (Competências)" são os `ReportTemplate`
  que já portei — a UI de "Adicionar Laudo" escolhe entre eles. As abas
  Descritivo/Competências/Produtividade/Testes/Anexos dentro de um Laudo
  confirmam que `Report.Descriptive` e `ReportCompetency` (já portados)
  estão corretos, e que Produtividade/Testes/Anexos são do
  `AssessmentEvent`, não do `Report`.
- **Produtividade** (`Produtividades`, sub-coleção pendente): confirmado
  como um log simples de `Activity` + `User` (Responsável) + Data, ligado a
  um `AssessmentEvent` — agora facilmente portável.

### Productivity Entry — portado

Primeiro módulo entregue a partir do entendimento ganho com as capturas:
**Productivity Entry** (`ProductivityEntry` → tabela `Produtividades`).
Confirmado contra o schema real: `EventoAvaliacaoId` + `AtividadeId` +
`UsuarioId` (todas obrigatórias, `DeleteBehavior.Restrict`) + `Data` +
`Tempo` (minutos) + `DataInclusao`. `GET /api/productivityentries?
assessmentEventId=...` permite listar as entradas de um Assessment Event
específico — mesmo padrão já usado em `ReportCompetency`/
`CompetencyProfileLine`.

**Ainda por portar** (mesma aba "Detalhamento" da v2): `EventoAvaliacaoAnexo`
(aba "Anexos") — ver nota abaixo sobre porquê ficou de fora. `Lista de
Parecer` e `Agenda` continuam como os blocos maiores em aberto, já bem
mais claros a partir das capturas (ver acima).

### Psychological Test, Assessment Event Test e Report Template Component — portados

Fecha a aba "Testes" do `AssessmentEvent` e a última sub-coleção do
subsistema de Laudos:

- **Psychological Test** (`PsychologicalTest` → tabela
  `TestesPsicologico`): catálogo simples, mesmo padrão de `Activity`/
  `EvaluationResult`.
- **Assessment Event Test** (`AssessmentEventTest` → tabela
  `EventoAvaliacaoTestes`): liga um `AssessmentEvent` a um
  `PsychologicalTest` aplicado, com percentual opcional. `GET /api/
  assessmenteventtests?assessmentEventId=...` para listar os testes de
  um evento específico — mesmo padrão de `ProductivityEntry`.
- **Report Template Component** (`ReportTemplateComponent` → tabela
  `TipoLaudoComponentes`): liga um `ReportTemplate` a um
  `ReportComponent` extra, além do cabeçalho/rodapé já existentes em
  `ReportTemplate.Header`/`Footer`. **Fecha por completo o subsistema de
  Laudos** — não resta nenhuma sub-coleção pendente.

**`EventoAvaliacaoAnexo` fica de fora, e não é um adiamento arbitrário**:
ao contrário dos módulos acima, esta tabela representa **anexos reais**
(`NomeReal`, `Extensao`, `Tamanho`, mais uma flag `Bloqueado` — o cadeado
verde nas capturas). Portar só a *metadata* da linha sem um destino real
para o ficheiro (disco, blob storage) criaria uma tela que deixa "criar"
um anexo sem nunca ter um ficheiro por trás — pior do que não ter a tela.
Fica pendente de uma decisão de armazenamento antes de fazer sentido
implementar.

### Opinion List e Opinion List Entry — portados (Lista de Parecer)

Um dos dois grandes blocos que as capturas de tela deixaram claro:

- **Opinion List** (`OpinionList` → tabela `ListasParecer`): agrupa
  avaliações de uma mesma empresa (via `Contact`) para um parecer
  consolidado, com opção de notificar por email (campo `Enviar
  Notificação` na v2 — ainda não implementado aqui, já que depende de
  envio de email, fora do escopo desta fase). Duas FKs para `User`
  (`Responsible`/`CreatedBy`, mesmo critério de FKs distintas já usado em
  `Report`/`JobOpening`).

  **Detalhe encontrado nas capturas e confirmado no schema**: o campo
  `Nome` da tabela original (`Code` aqui) tem exatamente 20 caracteres —
  o mesmo tamanho do formato `yyyyMMdd_HHmmss` (`20260813_063704`) visto
  nas listagens da v2. Não é um nome digitado pelo utilizador; é gerado
  no servidor em `OpinionListService.CreateAsync`, e por isso não faz
  parte de `CreateOpinionListDto`.
- **Opinion List Entry** (`OpinionListEntry` → tabela
  `ListaParecerEventos`): uma linha de uma `OpinionList` — liga um
  `AssessmentEvent` a um resultado. Reaproveita o enum `AssessmentResult`
  que já existia (mesmos valores de `AssessmentEvent.Result` — a v2
  mostra "Sem Resultado" como opção por omissão, que já mapeava para
  `AssessmentResult.NoResult`), mais um rótulo `EvaluationResult`
  opcional, mesmo padrão usado em `AssessmentEvent.EvaluationResultId`.
  `GET /api/opinionlistentries?opinionListId=...` para listar as linhas
  de uma lista específica.

**Simplificação consciente**: a v2 tem um fluxo de "adicionar lista"
onde se escolhe a empresa e depois se marca o parecer de vários
candidatos numa única tela (ver captura "Adicionar Lista de Parecer").
Aqui, `OpinionList` e `OpinionListEntry` são duas telas de CRUD
separadas — cria-se a lista primeiro, depois adicionam-se as entradas
uma a uma. Funcionalmente equivalente, mas sem esse fluxo combinado de
um único formulário.

### Agenda — portada (Recruitment/Selection Schedule, Schedule Note)

O bloco que mais tempo ficou bloqueado, agora resolvido com o schema real
e as capturas de tela. **Descoberta importante**: apesar da hierarquia C#
original ter uma classe abstrata `Agenda` da qual `AgendaRecrutamento` e
`AgendaSelecao` herdavam, **o schema real confirma que são duas tabelas
totalmente planas e independentes** — sem discriminador, sem herança ao
nível da BD (cada tabela tem todas as colunas "herdadas" duplicadas). Isso
elimina de vez a preocupação original com TPH/TPT que tinha bloqueado este
módulo — não há truque de mapeamento nenhum a fazer, só duas entidades EF
Core normais.

- **Recruitment Schedule** (`RecruitmentSchedule` → tabela
  `AgendaRecrutamento`): entrevistas, provas, dinâmicas de grupo e
  entrevistas com gestor para uma `JobOpening`. Três enums novos, valores
  preservados do original — `ScheduleStatus` (`StatusAgendamento`:
  Pending/Present/Absent/Cancelled), `RecruitmentScheduleType`
  (`TipoAgendamentoRecrutamento`) e `InterviewResult`
  (`ResuultadoEntrevista` — sic, erro de digitação no nome da classe
  original; **os valores não batem com `AssessmentResult`** —
  `InterviewResult` usa 0/1/2/3, `AssessmentResult` usa 0/1/2/100 —, teria
  sido um erro real reaproveitar um enum pelo outro). Coluna
  `ResponavelId` (sic, falta o "v") reproduzida tal e qual no
  `HasColumnName`.
- **Selection Schedule** (`SelectionSchedule` → tabela `AgendaSelecao`):
  avaliações agendadas para um `JobTitle`, com `Contact`
  ("Solicitante" nas capturas) e `AssessmentEvent` opcionais, e
  `Origin` (`ServiceOrigin`, reaproveitado). Bate campo a campo com a
  tela "Agenda Seleção" das capturas partilhadas: Status, Cargo,
  Solicitante, Nome, CPF, Origem, Data, Horário, Observações
  (`InternalNotes`) e Observações do Cliente (`ClientNotes`).
- **Schedule Note** (`ScheduleNote` → tabela `AgendaObservacoes`):
  corresponde exatamente ao modal "Observações na agenda" das capturas —
  uma nota geral sobre um dia/horário, não ligada a um agendamento
  específico.

**`HasHistory` calculado no servidor**: a v2 mostra automaticamente um
ícone de histórico quando já existem agendamentos anteriores para o mesmo
CPF — não é um campo que o utilizador marca à mão. Implementado em
`RecruitmentScheduleService.CreateAsync`/`SelectionScheduleService.CreateAsync`
via `IRecruitmentScheduleRepository.HasPriorEntriesAsync`/equivalente,
mesma lógica do `Code` auto-gerado em `OpinionList`.

**Simplificações conscientes desta fase**:
- As telas são CRUD padrão (grid + formulário), **não** a vista de
  calendário com abas de horário do dia (tabs "08:00"/"13:00", navegação
  dia-a-dia) mostrada nas capturas — isso é uma UI bespoke bem maior que
  um CRUD normal, fica como possível evolução futura da interface.
- `Nome`/`Cpf` em ambas as entidades são texto livre (não FK para
  `Candidate`), exatamente como no original — reflete que nem todo
  agendamento tem necessariamente um `Candidate` já cadastrado no
  sistema no momento do agendamento.
- `AgendaCandidatoAnexos` fica de fora — mesmo motivo de
  `EventoAvaliacaoAnexo` (anexos reais, sem destino de armazenamento
  definido).

## Dados: EF Core para tudo, Dapper só onde compensa

O CRUD e as listagens (com filtro/ordenação dinâmicos, ver acima) continuam
inteiramente no **EF Core** — é onde o tracking de mudanças e a tradução de
LINQ/Dynamic LINQ pagam pelo overhead. A única exceção proposital é o
**dashboard** (contagens-resumo na Home): seis `COUNT(*)` simples, sem
necessidade de tracking, sem filtro dinâmico, sempre com a mesma forma —
exatamente o cenário onde vale a pena trocar o EF Core por SQL direto.

`DashboardRepository` (`Selecta.Infra/Repositories/DashboardRepository.cs`)
usa **Dapper** com `QueryMultipleAsync` para trazer as seis contagens numa
única ida à base de dados, através de uma `ISqlConnectionFactory` própria
(`Selecta.Infra/Data/SqlConnectionFactory.cs`) que abre uma ligação ADO.NET
crua com a mesma connection string do EF Core — os dois convivem lado a
lado sem conflito, cada um usado onde faz mais sentido.

Se mais consultas de leitura pura/agregada surgirem no futuro (relatórios,
outros dashboards), o padrão está pronto para reaproveitar: implementa
`I<Algo>Repository` com Dapper em vez de EF Core, sem tocar no resto —
`IDashboardService` (`Selecta.Core`) nem sabe que a implementação usa SQL
direto, só depende da interface.

## O que já está implementado (ponta a ponta, a funcionar contra a BD existente)

- **Autenticação** (`User` → tabela `Usuarios`): login valida a password com o
  **mesmo algoritmo MD5** da solução original (`Helpers.HashPassword`), para não
  invalidar nenhuma conta existente. No primeiro login bem-sucedido, o hash é
  automaticamente atualizado para um formato mais forte (PBKDF2-SHA256, com
  salt), respeitando o limite de 50 caracteres da coluna `Senha` já existente.
  Ver `Selecta.Core/Security/PasswordHasher.cs` para o detalhe e para como
  alargar os parâmetros se um dia alargares essa coluna.
- **Cities** (`City` → tabela `Cidades`): CRUD completo — Entity → Configuration
  (EF Core) → Repository → Service → DTO → Controller → página Blazor com
  `MudTable`. Serve de **modelo a copiar** para migrar os restantes módulos.
- **Candidates** (`Candidate` → tabela `Candidatos`): CRUD completo + busca
  única por nome/CPF (`GET /api/candidates/search?term=...`, réplica do antigo
  `ObterPorNomeCpf`). Inclui a validação de CPF (dígitos verificadores, porta
  fiel de `Helpers.ValidaCpf` — ver `Selecta.Core/Validation/CpfValidator.cs`)
  e a checagem de CPF duplicado, ambas lançando `DomainException` (→ `400 Bad
  Request` na Api, com a mensagem mostrada no formulário do Blazor). O enum
  `Gender` mantém os mesmos valores numéricos do antigo `Genero`
  (`Female=1`/`Male=2`) — é assim que já está gravado na coluna.
  Colunas do formulário antigo (endereço, escolaridade, telefone, histórico
  via `AgendaRecrutamento`/`AgendaSelecao`) ficaram fora desta 1ª fase.
- **Job Openings** (`JobOpening` → tabela `Vagas`): CRUD + endpoint dedicado
  `PATCH /api/jobopenings/{id}/status` com a máquina de estados original
  (`Vaga.ValidarAtualizacao` → `Selecta.Core/Validation/JobOpeningStatusRules.cs`).
  O enum `JobOpeningStatus` mantém os valores do antigo `StatusVaga`
  (`New=0/InProgress=1/InReplacement=2/Finished=3/Cancelled=4`). Os campos
  calculados `TempoVaga`/`DiasTrabalhados` viraram `OpenDuration`/
  `WorkingDaysOpen` no DTO (calculados no serviço, não guardados na
  entidade) — a conversão de fuso horário do original usava um id de fuso
  só do Windows (`"E. South America Standard Time"`, quebraria num
  container Linux), por isso aqui o cálculo é feito em UTC.

  As FKs (`Manager`/`CreatedBy` → `User`, `Contact`, `JobTitle`,
  `RecruitmentStage`) já são relações reais com navegação (`DeleteBehavior.Restrict`
  em todas, para evitar múltiplos caminhos de cascade no SQL Server —
  `Manager` e `CreatedBy` apontam para a mesma tabela `Usuarios` por FKs
  diferentes). O DTO de leitura já vem com `ManagerName`/`ContactName`/
  `JobTitleName`/`RecruitmentStageName` pré-carregados (`Include` no
  repositório), e o formulário do Blazor usa dropdowns reais para os
  quatro, com o `CreatedBy` inferido automaticamente do utilizador
  autenticado (claim `sub` do JWT) — já não pede nenhum Guid solto.
  `TicketId` continua manual (o módulo de tickets está fora do escopo).

  **Fora do escopo desta 1ª fase**: Tags, Histórico, Observações, Anexos,
  Entrevistas com gestor, e o envio de notificação por e-mail ao trocar de
  etapa (dependia de Mensagem/`Notification.Send` na solução original).
- **Company** (`Company` → tabela `Empresas`), **Contact** (`Contact` →
  tabela `Contatos`), **JobTitle** (`JobTitle` → tabela `Cargos`) e
  **RecruitmentStage** (`RecruitmentStage` → tabela `EtapasRecrutamento`):
  CRUD completo nos quatro, com relações reais entre eles (`Contact.Company`,
  `JobTitle.Company`, `Company.City` opcional) usando `DeleteBehavior.Restrict`
  para evitar o erro clássico do SQL Server de múltiplos caminhos de cascade
  quando há mais de uma FK apontando (direta ou indiretamente) para a mesma
  tabela. `Company.Document` é validado como único (`DomainException`, igual
  ao padrão do CPF em Candidate). O enum `CompanyType` mantém os valores do
  antigo `TipoEmpresa`. `JobTitle.ProfessionalGroupId` agora é uma relação
  real com `ProfessionalGroup` (ver mais abaixo) — deixou de ser Guid solto.

  Nota sobre `Company`: a tabela original guarda a cidade de duas formas ao
  mesmo tempo — um texto livre (`Cidade`) e, opcionalmente, uma FK
  normalizada (`CidadeId`). Mantive as duas no DTO/entidade para não perder
  dados; o formulário do Blazor já usa a FK normalizada (dropdown de
  `City`), deixando o texto livre como resquício a descontinuar aos poucos.
- **Users** (somente leitura, `GET /api/users`): endpoint simples para
  popular o dropdown de "Manager" em Job Openings. Não é um CRUD completo —
  a gestão de utilizadores (perfis, permissões, alteração de senha) fica
  fora do escopo por agora; o login já cobre a autenticação.
- **Evaluation Results** (`EvaluationResult` → tabela `AvaliacaoResultados`)
  e **Company Evaluation Results** (`CompanyEvaluationResult` → tabela
  `AvaliacaoResultadosCustom`, com relação real para `EvaluationResult` e
  `Company`): catálogo de resultados de avaliação (ex.: "Aprovado") e a
  possibilidade de cada empresa usar um nome customizado para o mesmo
  resultado base.
- **Service Offerings** (`ServiceOffering` → tabela `Servicos`): catálogo
  dos tipos de serviço que a Selecta oferece (Recrutamento/Seleção/Proposta
  comercial) — usado por Proposals.
- **Prospect Companies** (`ProspectCompany` → tabela `EmpresasTemp`,
  ligação opcional para `Company`): empresas ainda não efetivadas como
  cliente, usadas para o envio de propostas comerciais. `Document` é
  validado como único (mesmo padrão do CPF em Candidate).
- **Proposals** (`Proposal` → tabela `Propostas`): CRUD + endpoint dedicado
  `PATCH /api/proposals/{id}/status`. Diferente de Job Openings, aqui não
  havia uma máquina de estados explícita no código original — a única regra
  de negócio real é: **motivo de recusa é obrigatório ao mudar para
  Declined** (`DomainException` se faltar). O enum `ProposalStatus` mantém
  os valores do antigo `StatusProposta`, e `DeclineReason` os do antigo
  `MotivoRecusa`. O campo calculado `Dias` (dias corridos/úteis desde a
  criação) virou `TotalDays`/`TotalWorkingDays` no DTO, calculado no
  serviço em UTC — mesmo tratamento dado a `TempoVaga`/`DiasTrabalhados` em
  Job Openings.

  **Fora do escopo desta 1ª fase**: Contatos/Observações/Anexos da proposta
  (existiam na solução original como `PropostaContato`/`PropostaObservacao`/
  `PropostaAnexo`) — mesmo critério aplicado aos sub-módulos de Job Opening.
- **Schedule Blocks** (`ScheduleBlock` → tabela `AgendaBloqueios`): marca uma
  data (e opcionalmente hora) como indisponível para um utilizador, por tipo
  de serviço (`ServiceOrigin`, valores do antigo `ServicosBase`). Só
  Create/List/Delete — bloqueios não se editam no original, removem-se e
  recriam-se.

  **Fora do escopo desta 1ª fase (resto do módulo Agenda)**: as entradas de
  agendamento propriamente ditas — entrevistas de recrutamento/seleção
  (antigos `AgendaRecrutamento`/`AgendaSelecao`) — usam herança EF6 (TPT, a
  partir de uma classe abstrata `Agenda`) e dependem de stored procedures
  (`Agenda_Propagate` para recorrência, `Agenda_Relatorio` para relatórios)
  que precisam de uma análise própria antes de portar. `ScheduleBlock` foi
  a parte autocontida e imediatamente útil desse módulo.
- **Professional Groups** (`ProfessionalGroup` → tabela `GruposProfissional`):
  catálogo simples, mesmo padrão de Recruitment Stages/Service Offerings.
  Fechou uma dívida técnica: `JobTitle.ProfessionalGroupId` era Guid solto
  desde que Job Title foi portado — agora é uma relação real
  (`DeleteBehavior.Restrict`, igual às outras FKs), e o formulário de Job
  Titles passou a ter um dropdown em vez de pedir o Guid colado à mão.

Todas as outras ~18 tabelas/entidades da solução original (histórico de
avaliação — `EventoAvaliacaoAnexo` (depende de armazenamento de ficheiros,
ver nota acima) —, `AgendaCandidatoAnexos` (mesmo motivo) —, Propostas —
Contatos/Observações/Anexos —, Job Openings — Histórico/Observações/
Anexos/Entrevistas com Gestor —, Usuários/Segurança completos, etc.)
**ainda não foram portadas** — não fazia sentido gerar dezenas de CRUDs às
cegas sem saber a tua prioridade. O padrão está pronto; é replicar por
módulo.

- **Activity** (`Activity` → tabela `Atividades`) e **Report Component**
  (`ReportComponent` → tabela `LaudoComponentes`): **Fase 1 do subsistema de
  Laudos** — os dois catálogos simples de que `TipoLaudo` depende
  (atividades de produção/leitura, e os blocos de cabeçalho/rodapé de um
  modelo). `Activity` é a primeira entidade desta solução com chave `int`
  identity em vez de `Guid` — obrigou a generalizar `IEntity`/
  `IRepositoryBase`/`RepositoryBase` para aceitar um `TKey` genérico
  (`IEntity<TKey>`, `IRepositoryBase<TEntity, TKey>`), mantendo
  `IEntity`/`IRepositoryBase<TEntity>` (sem o segundo parâmetro) como atalho
  para o caso comum — **os 16 módulos anteriores continuam exatamente
  iguais**, não precisaram de nenhuma alteração. O enum `ComponentType`
  mantém os valores do antigo `TipoComponente`; `Activity.Origin` reaproveita
  o `ServiceOrigin` que já existia (mesmo enum do `ScheduleBlock`).

- **Report Template** (`ReportTemplate` → tabela `TipoLaudos`): **Fase 2 do
  subsistema de Laudos** — corresponde ao antigo "Tipo de Laudo". Duas FKs
  obrigatórias para `Activity` (produção/leitura) e duas opcionais para
  `ReportComponent` (cabeçalho/rodapé), todas com `DeleteBehavior.Restrict`
  (mesmo critério do `JobOpening`, já que há mais de uma FK para a mesma
  tabela `Atividades`). Chave `int` identity, igual a `Activity`. O DTO já
  vem com os nomes das quatro relações resolvidos (`ProductionActivityName`,
  `ReadingActivityName`, `HeaderName`, `FooterName`), com `Include` no
  repositório — mesmo padrão do `JobOpeningDto`.

  **Fora do escopo desta fase**: a coleção de Laudos gerados a partir de um
  modelo (isso é o próprio `Laudo`) e `TipoLaudoComponente` (lista adicional
  de blocos do corpo do laudo, além de cabeçalho/rodapé).
- **Assessment Event** (`AssessmentEvent` → tabela `EventosAvaliacao`):
  **Fase 3 do subsistema de Laudos** — o "hub" de uma avaliação (o maior
  bloco que restava). Cinco relações reais: `Candidate` e `JobTitle`
  obrigatórios, `Contact`/`City`/`EvaluationResult` opcionais, todas com
  `DeleteBehavior.Restrict`. Seis enums novos, todos com os valores
  numéricos do original preservados — `EducationLevel` (`Escolaridades`),
  `MaritalStatus` (`EstadoCivil`), `DriverLicenseCategory`
  (`CategoriaHabilitacao`), `AssessmentResult` (`ResultadoAvaliacao` — note
  o salto para 99/100, já assim no original), `AssessmentStatus`
  (`StatusAvaliacao`) e `AssessmentPurpose` (`FinalidadeAvaliacao`). Mesmo
  padrão dual de `Company.CityName` para o texto livre legado de cidade.

  **Fora do escopo desta fase**: a relação com `Laudo` (ver correção na
  entrada de `Report`, abaixo) e as sub-coleções `Produtividades`,
  `Testes` (`EventoAvaliacaoTeste`) e `Anexos` (`EventoAvaliacaoAnexo`).
- **Report** (`Report` → tabela `Laudos`): **Fase 4 e última do subsistema
  de Laudos** — corresponde ao "Laudo" propriamente dito. FK real para
  `AssessmentEvent` via coluna própria `EventoAvaliacaoId` (**corrigido**
  depois de partilhares o script de criação da BD — a suposição inicial de
  1:1 com chave partilhada estava errada: a tabela `Laudos` tem
  `EventoAvaliacaoId` como coluna própria, separada de `LaudoId`; o
  `Report.Id` original passou a `Report.AssessmentEventId`, com FK normal
  `HasOne().WithMany()` em vez de `HasForeignKey<Report>(r => r.Id)`). Não
  há índice único em `EventoAvaliacaoId` na BD real, então "no máximo um
  Report por AssessmentEvent" é só regra de aplicação — validado em
  `ReportService.CreateAsync` (`DomainException` → `400 Bad Request` se já
  existir). Mais 5 FKs para `User` (Responsible, Supervisor,
  ResponsibleSignature, SupervisorSignature, UpdatedBy — a mais rica em
  relações de todos os módulos), todas com `DeleteBehavior.Restrict`.
  `UpdatedById` é inferido do utilizador autenticado ao editar (mesmo
  padrão de `JobOpening.CreatedBy`), `UpdatedAt` é definido no servidor.

  **Fora do escopo desta fase**: `LaudoCompetencia` (depende de
  `Competencia`/`CompetenciaDescritivo`/`ScoreCompetencia`, nenhum ainda
  portado).

Com isto, **o subsistema de Laudos está completo**: `Activity` →
`ReportComponent` → `ReportTemplate` → `AssessmentEvent` → `Report`, cada
um com relações reais para o anterior.

- **Collaborator** (`Collaborator` → tabela `Colaboradores`) e **Access
  Profile** (`AccessProfile` → tabela `PerfisAcesso`): dois catálogos
  simples e autocontidos do lado de segurança/RH. `Collaborator` é um
  registo de colaborador interno, distinto de `User` (não implica acesso
  ao sistema). `AccessProfile` é o catálogo de perfis/papéis (ex.: "Admin",
  "Recrutador") — **sem ligação normalizada a `User` nesta fase**:
  `User.Roles` continua a ser o texto livre já existente (coluna `Perfis`
  da tabela `Usuarios`), exatamente como estava.

  **Fora do escopo**: `Perfil`/`PerfilCargo`/`PerfilGrupoProfissional` — um
  conceito totalmente diferente apesar do nome parecido (é "perfil de
  competências esperadas" para uma avaliação, não "perfil de acesso").
  Usa herança EF6 (uma classe abstrata `Perfil` com duas subclasses, sem
  Configuration própria — sugere discriminador implícito por convenção) e
  agora depende de `Competency`, que já existe (ver abaixo) — mas replicar
  o discriminador da hierarquia sem conseguir confirmar contra a BD real
  qual o nome/valores exatos da coluna tem risco genuíno de mapear errado,
  então continua de fora por essa razão específica, não por falta da
  dependência.
- **Competency** (`Competency` → tabela `Competencias`) e **Competency
  Descriptor** (`CompetencyDescriptor` → tabela `CompetenciaDescritivos`):
  **Fase 1 do subsistema de Competências**. `Competency` é um catálogo
  simples (comportamental/habilidade, enum `CompetencyGroup` com os
  valores do antigo `GrupoCompetencia`). `CompetencyDescriptor` associa um
  descritivo a uma competência e ao utilizador que o redigiu — duas
  relações reais (`Competency`/`User`, ambas obrigatórias,
  `DeleteBehavior.Restrict`).

  **Fora do escopo desta fase**: `ScoreCompetenciaDescritivo` (sub-coleção
  de `CompetencyDescriptor`, depende de `ScoreCompetencia`, ainda não
  portada) — e, como já referido acima, a hierarquia `Perfil`.
- **Competency Score** (`CompetencyScore` → tabela `ScoreCompetencias`) e
  **Competency Score Descriptor** (`CompetencyScoreDescriptor` → tabela
  `ScoreCompetenciaDescritivos`): **Fase 2 e última do subsistema de
  Competências** — fecha a sub-coleção que tinha ficado pendente na Fase 1.
  `CompetencyScore` é um catálogo simples (nome, sigla de 2 caracteres,
  cor em hex, valor numérico). `CompetencyScoreDescriptor` liga um
  `CompetencyDescriptor` a um `CompetencyScore` com texto próprio — duas
  relações reais, ambas obrigatórias, `DeleteBehavior.Restrict`.

Com isto, **o subsistema de Competências está completo**: `Competency` →
`CompetencyDescriptor` → `CompetencyScoreDescriptor` ← `CompetencyScore`.

- **Report Competency** (`ReportCompetency` → tabela `LaudoCompetencias`):
  fecha a sub-coleção de `Report` que tinha ficado de fora na Fase 4 de
  Laudos (na altura, `Competency`/`CompetencyScore` ainda não existiam).
  Liga um `Report` a uma `Competency`, com `CompetencyDescriptor` opcional,
  e **duas FKs opcionais para `CompetencyScore`** — `ProfileScore`
  (pontuação esperada) e `Score` (pontuação obtida) — mesmo critério de FKs
  distintas para a mesma tabela usado em `Report` (Responsible/Supervisor/
  ...) e `JobOpening` (Manager/CreatedBy). `GET /api/reportcompetencies?
  reportId=...` permite listar as linhas de um Report específico.

## Hierarquia `Perfil`: desbloqueada com o schema real da BD

Este módulo estava documentado como bloqueado por falta de acesso à base
de dados para confirmar o discriminador da herança TPH (Table Per
Hierarchy) do EF6. Com o script de criação da BD partilhado, confirmei
exatamente:

```sql
CREATE TABLE [dbo].[Perfis](
	[PerfilId] [uniqueidentifier] NOT NULL,
	[Nome] [varchar](150) NOT NULL,
	...
	[CargoId] [uniqueidentifier] NULL,
	[GrupoProfissionalId] [uniqueidentifier] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
```

Uma tabela só, coluna `Discriminator` (convenção padrão do EF6 — valor
igual ao nome exato da classe CLR), e `CargoId`/`GrupoProfissionalId`
nullable (cada subtipo só preenche a sua). Implementado com TPH real do
EF Core (`HasDiscriminator<string>("Discriminator")`):

- **Competency Profile** (`CompetencyProfile`, abstrata, base da
  hierarquia → tabela `Perfis`)
- **Job Title Competency Profile** (`JobTitleCompetencyProfile` →
  discriminador `"PerfilCargo"`, FK opcional para `JobTitle` — opcional
  porque a coluna partilhada é nullable, embora sempre preenchida na
  prática para este subtipo)
- **Professional Group Competency Profile**
  (`ProfessionalGroupCompetencyProfile` → discriminador
  `"PerfilGrupoProfisisonal"`, com o erro de digitação **reproduzido de
  propósito** — é literalmente o nome da classe CLR original, e é isso que
  já está gravado na coluna; corrigir a grafia quebraria a leitura dos
  perfis já existentes)
- **Competency Profile Line** (`CompetencyProfileLine` → tabela
  `PerfilCompetencias`) — liga um perfil (de qualquer um dos dois
  subtipos, via FK para a base abstrata `CompetencyProfile`) a uma
  `Competency`, com uma `CompetencyScore` esperada opcional

Como os dois subtipos partilham uma tabela mas são conceitos distintos na
UI (assim como no original, com DTOs/telas separadas), mantive duas
páginas Blazor separadas em vez de uma só — a tela de Competency Profile
Lines combina as duas listas num único dropdown para selecionar "a que
perfil esta linha pertence".

## ⚠️ Segurança — antes de fazeres seja o que for com isto

No `App.config` da solução original vinham **credenciais reais de produção**
em texto simples (servidor, utilizador e password da BD de produção). Não as
copiei para lado nenhum deste novo projeto — os `appsettings*.json` aqui têm
apenas placeholders. Recomendo:

1. Rodar (trocar) essa password de produção assim que possível;
2. Nunca voltar a commitar connection strings/segredos em `appsettings.json`
   — usar `dotnet user-secrets` em desenvolvimento e variáveis de ambiente /
   Key Vault / equivalente em produção;
3. Definir `Jwt:Key` (mínimo 32 caracteres) da mesma forma, nunca em texto
   simples no repositório.

## Como correr

```bash
# 1. Configurar a connection string e a chave JWT (não commitar isto)
cd Selecta.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Selecta" "Server=...;Database=selecta_SIS;User Id=...;Password=...;TrustServerCertificate=True;"
dotnet user-secrets set "Jwt:Key" "uma-chave-aleatoria-com-pelo-menos-32-caracteres"

# 2. Correr a Api (por omissão em https://localhost:7080, ajusta em launchSettings.json)
dotnet run --project Selecta.Api

# 3. Noutro terminal, correr o Web (ajusta Selecta.Web/wwwroot/appsettings.json
#    se a Api não ficar em https://localhost:7080)
dotnet run --project Selecta.Web
```

> Este código foi escrito sem acesso a um SDK .NET/NuGet neste ambiente, por
> isso não foi compilado nem testado localmente — revê com `dotnet build`
> antes de correr contra a BD de produção, e experimenta primeiro contra uma
> cópia/ambiente de dev.

## "TypeError: Failed to fetch" no browser

Este erro é lançado pelo próprio browser (Fetch API), antes de o C# ver
qualquer resposta — não é falha de injeção de dependência (`HttpClient` é
injetado normalmente via `Program.cs` do Web). Checklist, por ordem de
probabilidade:

1. **As duas portas não batem certo.** `Selecta.Web/Properties/launchSettings.json`
   fixa o Web em `https://localhost:7001`; `Selecta.Api/Properties/launchSettings.json`
   fixa a Api em `https://localhost:7080` — e é exatamente isso que
   `Selecta.Web/wwwroot/appsettings.json` (`ApiBaseAddress`) e
   `Selecta.Api/appsettings.json` (`Cors:OrigemPermitida`) já assumem. Se
   alteraste alguma porta num dos lados, atualiza o outro também.
2. **A Api não está a correr** (ou caiu por falta de connection string —
   confere a consola onde correste `dotnet run --project Selecta.Api`).
3. **Certificado de desenvolvimento HTTPS não confiável** — primeira vez na
   máquina, corre `dotnet dev-certs https --trust`. Sem isto, o Chrome/Edge
   bloqueia a ligação (aparece como `ERR_CERT_AUTHORITY_INVALID` no separador
   Network do DevTools, mas chega ao JS só como "Failed to fetch").
4. **Confirma no separador Network do DevTools** (F12) o que realmente
   aconteceu com o pedido a `/api/auth/login` — "CORS error", "connection
   refused" e "certificate error" têm causas (e correções) diferentes; o
   `TypeError: Failed to fetch` sozinho não distingue qual foi.

## Roadmap sugerido para os restantes módulos

Por cada módulo (ex.: Candidatos, Vagas, Avaliações, Laudos, Agenda, Propostas):

1. **Core**: entidade em inglês (só as colunas de que precisas já, dá para
   acrescentar mais depois), DTOs, interface de repositório, interface +
   implementação do serviço de negócio (portar as regras do antigo
   `*AppService`/`*Service`).
2. **Infra**: `IEntityTypeConfiguration<T>` com `ToTable(...)` e
   `HasColumnName(...)` a espelhar exatamente a `*Config.cs` (EF6)
   equivalente — mesma tabela, mesmas colunas/tamanhos, mesmos índices — e o
   repositório concreto.
3. **Api**: controller fino, só a chamar o serviço.
4. **Web**: página Blazor/MudBlazor (copia `Pages/Cities/Index.razor` como
   ponto de partida) ou um componente de formulário mais elaborado, consoante
   a complexidade do módulo.
5. Registar tudo em `Selecta.Infra/DependencyInjection.cs`.

Diz-me qual módulo queres a seguir — o **subsistema de Laudos** (`Activity`
→ `ReportComponent` → `ReportTemplate` → `AssessmentEvent` → `Report` →
`ReportCompetency`/`ReportTemplateComponent`, mais `ProductivityEntry` e
`AssessmentEventTest` das abas de detalhamento) está **completo por
inteiro** — não resta nenhuma sub-coleção pendente. O **subsistema de
Competências** (`Competency` → `CompetencyDescriptor` →
`CompetencyScoreDescriptor` ← `CompetencyScore`), a **hierarquia `Perfil`**
(`CompetencyProfile` → `JobTitleCompetencyProfile`/
`ProfessionalGroupCompetencyProfile` → `CompetencyProfileLine`), a
**Lista de Parecer** (`OpinionList` → `OpinionListEntry`) e a **Agenda**
(`RecruitmentSchedule`/`SelectionSchedule`/`ScheduleNote`) também estão
completas no essencial, e **Collaborator**/**Access Profile** fecharam os
catálogos simples do lado de segurança/RH. O que resta agora é sobretudo
**anexos** (`EventoAvaliacaoAnexo`, `AgendaCandidatoAnexos` — ambos
pendentes de uma decisão de armazenamento de ficheiros) e **histórico**
(Propostas — Contatos/Observações/Anexos —, Job Openings —
Histórico/Observações/Anexos/Entrevistas com Gestor —), mais o lado de
**Usuários/Segurança completo** que ainda não foi abordado. Continuo pela
ordem que fizer mais sentido se preferires que eu decida.
