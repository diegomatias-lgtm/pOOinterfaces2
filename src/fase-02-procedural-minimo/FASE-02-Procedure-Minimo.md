# Fase 2 — Procedural mínimo (ex.: formatar texto)

## Objetivo (1–2 linhas)
Implementar uma função simples de formatação de texto que ofereça múltiplos modos (ex.: upper, lower, title, reverse) e um modo padrão (sem alteração). O artefato descreve o fluxo procedural, cenários de teste/fronteira e limitações.

## Modos (mín. 3 + padrão)
- `upper`: converte todo o texto para maiúsculas.
- `lower`: converte todo o texto para minúsculas.
- `title`: capitaliza a primeira letra de cada palavra (Title Case).
- `reverse`: inverte a ordem dos caracteres na string.
- `default` (padrão): retorna a string inalterada.

Observação: o modo padrão é aplicado sempre que o modo informado for invalido/nulo/vazio.

## Fluxo procedural (3–6 linhas)
1. Recebe entrada: `text` (string) e `mode` (string).
2. Normaliza `mode` (trim + lower) e valida entrada básica (null → empty string).
3. Usa um `switch`/`if-else` para escolher qual operação aplicar com base em `mode`.
4. Executa a transformação correspondente (ou retorna a entrada se `default`).

Decisão (ponto de acoplamento): o `switch` central concentra toda a lógica de seleção de comportamento; cada novo modo exige alteração deste ponto.

## 5 cenários de teste / fronteira (apenas texto)
1) Valor/entrada mínima: string vazia (`""`) e modo `upper` → espera-se `""` (não falhar, tratar como válido).
2) Valor/entrada máxima/limite: string muito longa (ex.: 100k caracteres) e modo `lower` → verificar performance e uso de memória (espera-se conversão completa, sem exceções de memória).
3) Modo inválido: modo `unknown` ou string vazia como modo → espera-se cair no modo `default` e retornar a entrada sem alteração.
4) Combinação que revela ambiguidade: entrada com espaços e diferentes capitalizações (`"  hello-world  "`) e modo `title` vs `upper`. Explicação: `title` deve capitalizar por palavra, mas definição de "palavra" pode variar (hífen, pontuação); decidimos que `title` considera separators por espaços, então `"hello-world"` vira `"Hello-world"`. A ambiguidade evidencia que modos com objetivos próximos exigem regras adicionais (tokenização) que o procedural central tende a misturar.
5) Caso comum representativo: entrada `"Olá Mundo"` e modo `lower` → espera-se `"olá mundo"` (verificar preservação de acentos/Unicode).

## Por que não escala (4–6 linhas)
- Crescimento de `if/switch`: cada novo modo adiciona um novo ramo no `switch`, concentrando decisões em um só ponto e aumentando o acoplamento.
- Duplicação e lógica espalhada: operações que compartilham etapas (ex.: trim + case) tendem a ser reimplementadas em ramos diferentes ou exigem extra boilerplate para centralizar pré-processamento.
- Testes e combinações: o número de cenários cresce com modos e com input variantes (p.ex. tokenização diferente para `title`), tornando a matriz de testes maior e mais cara.
- Extensão arriscada: adicionar modos especializados (p.ex.: `title` com regras de idioma) força alteração do código existente; mudanças podem introduzir regressões em outros modos.

## Observações/Entrega C# (referência didática)
Como referência, incluímos um pequeno `Program.cs` com uma implementação puramente procedural (switch/if) para estudo. O objetivo é pedagógico: mostrar por que o design cresce com complexidade e como a alternativa orientada a objetos com estratégias pode melhorar extensibilidade e testabilidade.

---

Arquivo: `src/fase-02-procedural-minimo/FASE-02-Procedure-Minimo.md`
