using System;
using System.Globalization;

// Pequena implementação procedimental para demonstrar modos de formatação
static string FormatText(string text, string mode)
{
    if (text == null) text = string.Empty;
    mode = (mode ?? string.Empty).Trim().ToLowerInvariant();

    switch (mode)
    {
        case "upper":
            return text.ToUpperInvariant();
        case "lower":
            return text.ToLowerInvariant();
        case "title":
            // Title case using CultureInfo invariant (exemplo simples)
            TextInfo ti = CultureInfo.InvariantCulture.TextInfo;
            return ti.ToTitleCase(text.ToLowerInvariant());
        case "reverse":
            char[] arr = text.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        default:
            // modo padrão: retorna sem alterações
            return text;
    }
}

// Demonstração
var examples = new[] {
    ("Olá Mundo", "upper"),
    ("Olá Mundo", "lower"),
    ("Olá Mundo", "title"),
    ("Olá Mundo", "reverse"),
    ("  hello-world  ", "title"),
    ("Teste", "unknown")
};

Console.WriteLine("Demo: Procedural text formatter (switch/if)");
foreach (var (text, mode) in examples)
{
    var result = FormatText(text, mode);
    Console.WriteLine($"mode='{mode}' input='{text}' => '{result}'");
}

// Nota: arquivo é apenas para referência didática. Em projetos reais, preferir
// composição/estratégia para reduzir if/switch e melhorar testabilidade.
