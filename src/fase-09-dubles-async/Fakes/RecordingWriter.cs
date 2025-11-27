using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fase09.DublesAsync
{
    public sealed class RecordingWriter<T> : IAsyncWriter<T>
    {
        private readonly ConcurrentBag<T> _written = new();
        private readonly Func<T, Task>? _onWrite;
        public RecordingWriter(Func<T, Task>? onWrite = null) => _onWrite = onWrite;
        public Task WriteAsync(T item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _written.Add(item);
            if (_onWrite is not null) return _onWrite(item);
            return Task.CompletedTask;
        }
        public IReadOnlyList<T> Written => _written.ToArray();
    }
}
