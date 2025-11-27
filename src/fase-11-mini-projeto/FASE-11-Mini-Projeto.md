# Fase 11 — Mini‑projeto de consolidação (1 sprint curto)

## Objetivo
Construir um mini‑projeto consolidando: interfaces, Repository (InMemory + persistência), ISP, serviços, dublês avançados e testes.

## Estrutura sugerida
```
src/fase-11-mini-projeto/
  ├── Domain/
  ├── Persistence/
  │    ├── InMemory/
  │    └── Json/
  ├── ConsoleApp/
  └── Tests/
```

## Requisitos mínimos (resumo)
- Contratos `IReadRepository` / `IWriteRepository`.
- InMemory + JSON persistence.
- Serviços que orquestram casos de uso (register/list/find/update/remove).
- Testes unitários com dublês sem I/O + integração com ficheiro temporário.
- CLI simples demonstrando 3–5 cenários.

## Como executar os testes desta fase
```powershell
dotnet test src/fase-11-mini-projeto/Tests
```

## Observações
O mini‑projeto será criado como scaffold; depois implementaremos as partes mais importantes (domain, repository, tests) conforme build incremental.
