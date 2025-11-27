using System;

namespace Fase09.DublesAsync
{
    public sealed class FakeClock : IClock
    {
        private DateTimeOffset _now;
        public FakeClock(DateTimeOffset initial) => _now = initial;
        public DateTimeOffset Now => _now;
        public void Advance(TimeSpan ts) => _now = _now.Add(ts);
    }
}
