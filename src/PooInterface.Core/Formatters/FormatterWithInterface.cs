using PooInterface.Core.Interfaces;

namespace PooInterface.Core.Formatters;

public sealed class InterfacePlainFormatter : IFormatter
{
    public string Format(string input) => input ?? string.Empty;
}

public sealed class InterfaceTitleCaseFormatter : IFormatter
{
    public string Format(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var parts = input.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            var p = parts[i];
            if (p.Length == 0) continue;
            parts[i] = char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1).ToLowerInvariant() : string.Empty);
        }
        return string.Join(' ', parts);
    }
}