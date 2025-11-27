using System;

namespace Fase09.DublesAsync
{
    public sealed class FakeIdGenerator : IIdGenerator
    {
        private int _next = 1;
        public string NewId() => (_next++).ToString();
    }
}
