using PooInterface.Core.Tools.Export;
using Xunit;

namespace PooInterface.Tests
{
    public class ExporterTests
    {
        [Fact]
        public void Export_OldAndNew_AreEquivalent()
        {
            var path = "out/file";
            var old = Exporter.ExportOld(path, zip: true, level: 3, async: false, mode: "upper", locale: "pt-BR");
            var policy = new ExportPolicy(zip: true, level: 3, async: false, mode: "upper", locale: "pt-BR");
            var neu = Exporter.Export(path, policy);
            Assert.Equal(old, neu);
        }
    }
}
