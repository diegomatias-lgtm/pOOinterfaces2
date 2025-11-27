# Fase 9 — Dublês avançados e testes assíncronos (async, streams e tempo controlado)

## Diagrama (leve)

```
IAsyncReader<T> (fake) -> PumpService -> IAsyncWriter<T> (fake)
                        ↑
                        IClock (fake)
```

## Contratos mínimos
```csharp
public interface IClock { DateTimeOffset Now { get; } }
public interface IIdGenerator { string NewId(); }
public interface IAsyncReader<T> { IAsyncEnumerable<T> ReadAsync(CancellationToken ct = default); }
public interface IAsyncWriter<T> { Task WriteAsync(T item, CancellationToken ct = default); }
```

## Serviço PumpService (leitura → escrita com retentativa)
```csharp
public sealed class PumpService<T>
{
    private readonly IAsyncReader<T> _reader;
    private readonly IAsyncWriter<T> _writer;
    private readonly IClock _clock;
    public PumpService(IAsyncReader<T> reader, IAsyncWriter<T> writer, IClock clock)
        => (_reader, _writer, _clock) = (reader, writer, clock);
    public async Task<int> RunAsync(CancellationToken ct)
    {
        var count = 0;
        await foreach (var item in _reader.ReadAsync(ct).WithCancellation(ct))
        {
            var attempt = 0;
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    await _writer.WriteAsync(item, ct);
                    count++;
                    break;
                }
                catch when (++attempt <= 3)
                {
                    // backoff simulated in tests via clock; no real delay here
                }
            }
        }
        return count;
    }
}
```

## Testes sugeridos
- Sucesso simples: reader emite 3 itens; writer escreve → `RunAsync` retorna 3.
- Retentativa: writer falha 2x e depois escreve → `RunAsync` retorna 3.
- Cancelamento: cancelar após 1 item → `RunAsync` cancela e lança `OperationCanceledException`.
- Stream vazio → retorno 0.
- Erro no meio do stream: reader lança no 2º item → exceção propagada.

## Observações
- Testes usam dublês (fakes/stubs) sem I/O real.
- Não usar `Thread.Sleep` nos testes; use clock fake para simular tempo.
