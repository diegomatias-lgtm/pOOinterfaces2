using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Fase09.DublesAsync
{
    // Fails N times per distinct item (by string representation) then succeeds
    public sealed class FlakyWriter<T> : IAsyncWriter<T>
    {
        private readonly int _failTimes;
        private readonly ConcurrentDictionary<string, int> _attempts = new();
        private readonly Func<T, Task>? _onWrite;

        public FlakyWriter(int failTimes, Func<T, Task>? onWrite = null)
        {
            _failTimes = failTimes;
            _onWrite = onWrite;
        }

        public async Task WriteAsync(T item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var key = item?.ToString() ?? "<null>";
            var attempts = _attempts.AddOrUpdate(key, 1, (_, v) => v + 1);
            if (attempts <= _failTimes)
            {
                throw new InvalidOperationException($"flaky failure attempt {attempts} for {key}");
            }
            if (_onWrite is not null) await _onWrite(item);
            await Task.CompletedTask;
        }
    }
}
