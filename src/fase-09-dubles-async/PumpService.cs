using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Fase09.DublesAsync
{
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
                        // Retentativa: sem espera real aqui. Tests can inspect clock.
                    }
                }
            }
            return count;
        }
    }
}
