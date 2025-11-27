# Fase 10 — Cheiros e Antídotos (refatorações com diffs pequenos)

Nesta fase identificamos cheiros no código existente e aplicamos antídotos pequenos, com provas por teste.

Cheiros escolhidos (5):

1) Interface gorda
- Descrição: `IRepository<T>` originalmente expunha todas operações; consumidores que só leem recebiam métodos de escrita.
- Antes: monolítica `IRepository<T>`.
- Depois: segregação em `IReadRepository<T>` e `IWriteRepository<T>`; `IRepository<T>` passa a herdar ambas (compatibilidade).
- Antídoto: ISP (Interface Segregation Principle).
- Teste: `RepositoryIspTests.ReadOnlyConsumer_ShouldDependOnlyOnIReadRepository` demonstra que um consumidor lê usando apenas `IReadRepository<T>`.

2) Uso de reflection para ajustar `Id` em `CsvRepository`
- Descrição: ao desserializar CSV, o código usava `typeof(ToDo).GetProperty("Id")!.SetValue(...)`.
- Antes: reflexão (frágil, lento e arriscado).
- Depois: usar inicializador/constructors que aceitam `Id` (ou propriedade `init`) para atribuir `Id` sem reflexão.
- Antídoto: Remover reflection; tratar campos explicitamente.
- Teste: `RepositoryIspTests.CsvRepository_ReadsWrittenItem_PreservesId` garante o Id é preservado corretamente.

3) Decisão espalhada (switch/if por toda parte)
- Descrição: seleção de formatter podia ser espalhada em vários clientes.
- Antes: cada cliente fazia if/else no modo.
- Depois: `FormatterCatalog.Resolve(mode)` centraliza a decisão (ponto único de composição).
- Antídoto: Catalog/Factory centralizada (Composição centralizada).
- Prova: `Phase4InterfaceFormatterTests` já usa `FormatterCatalog.Resolve` mostrando política centralizada.

4) Testes lentos com I/O
- Descrição: testes que acessam disco ficam lentos e frágil.
- Antídoto: mover os testes de unidade para usar `InMemoryRepository` e usar repositórios com arquivo só em testes de integração.
- Exemplo: para validar lógica de serviços, use `InMemoryRepository` em vez de `CsvRepository`.
- Prova: adicionamos testes na suite que usam `InMemoryRepository` para cenários unitários.

5) Long Parameter List
- Descrição: métodos com muitos parâmetros são difíceis de manter e testar.
- Antídoto: Introduzir `ExportPolicy` value object para agrupar parâmetros.
- Antes: `string Export(string path, bool zip, int level, bool async, string mode, string locale)`
- Depois: `string Export(string path, ExportPolicy policy)` onde `ExportPolicy` é um `record` com as opções.
- Prova: `ExporterTests` compara saída entre a versão antiga e a nova e garante paridade.

---

Todos os diffs aplicados foram pequenos e com foco em um ponto. Os arquivos de teste que provam a segurança das mudanças foram adicionados em `tests`.

Veja os arquivos relevantes:
- `src/fase-09-cheiros-antidotos/FASE-09-Cheiros-Antidotos.md` (documentação desta fase)
- `tests/PooInterface.Tests/RepositoryIspTests.cs` (teste ISP + CSV Id preservation)
- `src/PooInterface.Core/Repositories/IRepository.cs` (segregação ISP aplicada)
- `src/PooInterface.Core/Repositories/CsvRepository.cs` (remoção de reflection)
- `src/fase-09-cheiros-antidotos/Export/Exporter.cs` (exemplo antes/depois)
- `tests/PooInterface.Tests/ExporterTests.cs` (teste de paridade de Export API)

