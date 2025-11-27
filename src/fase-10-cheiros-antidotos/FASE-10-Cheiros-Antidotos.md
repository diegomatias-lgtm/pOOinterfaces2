# Fase 10 — Cheiros e Antídotos (refatorações com diffs pequenos)

## Objetivo
Identificar 5–7 cheiros no código do projeto e aplicar antídotos com refatorações pequenas e testadas.

## Cheiros escolhidos (exemplos)
1. Interface gorda → aplicar ISP (segregação).
2. Downcast no cliente → aplicar DIP + polimorfismo.
3. Decisão espalhada → criar catálogo/Factory.
4. Testes com I/O direto → trocar por dublês/InMemory.
5. Long parameter list → Policy Object / Value Object.

## Entregáveis
- Snippets antes/depois (diffs pequenos).
- Testes que provem que o comportamento se manteve.
- Justificativa curta para cada antídoto (princípio associado).

## Como executar os testes desta fase
```powershell
dotnet test src/fase-08-isp/CatalogQueryTests.cs
# (outros testes já existentes serão executados com dotnet test)
```

> Observação: os exemplos concretos de diffs serão adicionados como arquivos no diretório quando aplicarmos as refatorações.
