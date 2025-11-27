using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fase09.DublesAsync
{
    public sealed class FakeReader<T> : IAsyncReader<T>
    {
        private readonly IEnumerable<T> _items;
        private readonly Exception? _throwOnSecond;
        public FakeReader(IEnumerable<T> items, Exception? throwOnSecond = null)
        {
            _items = items;
            _throwOnSecond = throwOnSecond;
        }
        public async IAsyncEnumerable<T> ReadAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
        {
            var index = 0;
            foreach (var item in _items)
            {
                ct.ThrowIfCancellationRequested();
                index++;
                if (index == 2 && _throwOnSecond is not null)
                {
                    throw _throwOnSecond;
                }
                yield return item;
                await Task.Yield();
            }
            await Task.CompletedTask;
        }
    }
}
