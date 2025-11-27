using System.Collections.Generic;
using System.Threading;

namespace Fase09.DublesAsync
{
    public interface IAsyncReader<T>
    {
        IAsyncEnumerable<T> ReadAsync(CancellationToken ct = default);
    }
}
