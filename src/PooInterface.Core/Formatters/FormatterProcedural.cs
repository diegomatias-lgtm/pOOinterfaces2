namespace PooInterface.Core.Formatters;

// Fase 2 — Procedural mínimo: a função recebe um modo e usa switch/if
public static class FormatterProcedural
{
    public enum Mode { Plain, TitleCase, Upper }

    public static string Format(string input, Mode mode)
    {
        if (input is null) return string.Empty;

        switch (mode)
        {
            case Mode.Plain:
                return input;
            case Mode.TitleCase:
                return ToTitleCase(input);
            case Mode.Upper:
                return input.ToUpperInvariant();
            default:
                return input;
        }
    }

    private static string ToTitleCase(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return s;
        var parts = s.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            var p = parts[i];
            if (p.Length == 0) continue;
            parts[i] = char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1).ToLowerInvariant() : string.Empty);
        }
        return string.Join(' ', parts);
    }
}