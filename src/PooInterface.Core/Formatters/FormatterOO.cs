namespace PooInterface.Core.Formatters;

// Fase 3 — OO sem interface: base abstrata com implementações concretas
public abstract class FormatterBase
{
    public abstract string Format(string input);
}

public sealed class PlainFormatter : FormatterBase
{
    public override string Format(string input) => input ?? string.Empty;
}

public sealed class TitleCaseFormatter : FormatterBase
{
    public override string Format(string input)
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

public sealed class UpperFormatter : FormatterBase
{
    public override string Format(string input) => (input ?? string.Empty).ToUpperInvariant();
}