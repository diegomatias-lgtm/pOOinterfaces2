using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Fase09.DublesAsync;

public class PumpServiceTests
{
    [Fact]
    public async Task RunAsync_SuccessThreeItems_Returns3()
    {
        var reader = new FakeReader<int>(new[] { 1, 2, 3 });
        var writer = new RecordingWriter<int>();
        var clock = new FakeClock(DateTimeOffset.UtcNow);
        var svc = new PumpService<int>(reader, writer, clock);
        var ct = CancellationToken.None;
        var result = await svc.RunAsync(ct);
        Assert.Equal(3, result);
        Assert.Equal(3, writer.Written.Count);
    }

    [Fact]
    public async Task RunAsync_FlakyWriter_RetriesAndSucceeds()
    {
        var reader = new FakeReader<string>(new[] { "a", "b", "c" });
        var writer = new FlakyWriter<string>(failTimes: 2);
        var clock = new FakeClock(DateTimeOffset.UtcNow);
        var svc = new PumpService<string>(reader, writer, clock);
        var result = await svc.RunAsync(CancellationToken.None);
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task RunAsync_CancelAfterFirst_ThrowsOperationCanceledException()
    {
        var tcsFirstWritten = new TaskCompletionSource<bool>();
        var reader = new FakeReader<int>(new[] { 1, 2, 3 });
        var writer = new RecordingWriter<int>(async item =>
        {
            if (item == 1) tcsFirstWritten.TrySetResult(true);
            await Task.CompletedTask;
        });
        var clock = new FakeClock(DateTimeOffset.UtcNow);
        var svc = new PumpService<int>(reader, writer, clock);

        using var cts = new CancellationTokenSource();
        var runTask = svc.RunAsync(cts.Token);
        // wait until first written
        await tcsFirstWritten.Task;
        // cancel before second item
        cts.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await runTask);
    }

    [Fact]
    public async Task RunAsync_EmptyStream_Returns0()
    {
        var reader = new FakeReader<int>(Array.Empty<int>());
        var writer = new RecordingWriter<int>();
        var clock = new FakeClock(DateTimeOffset.UtcNow);
        var svc = new PumpService<int>(reader, writer, clock);
        var result = await svc.RunAsync(CancellationToken.None);
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task RunAsync_ReaderThrowsInSecond_Propagates()
    {
        var reader = new FakeReader<int>(new[] { 1, 2, 3 }, throwOnSecond: new InvalidOperationException("boom"));
        var writer = new RecordingWriter<int>();
        var clock = new FakeClock(DateTimeOffset.UtcNow);
        var svc = new PumpService<int>(reader, writer, clock);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await svc.RunAsync(CancellationToken.None));
    }
}
