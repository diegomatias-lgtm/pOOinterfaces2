namespace PooInterface.Core.Tools.Export;

public sealed record ExportPolicy(bool Zip, int Level, bool Async, string Mode, string Locale);

public static class Exporter
{
    // Versão "antes" (long parameter list)
    public static string ExportOld(string path, bool zip, int level, bool async, string mode, string locale)
    {
        // For the exercise we don't perform I/O; return a deterministic summary string
        return $"{path}|zip={zip}|level={level}|async={async}|mode={mode}|locale={locale}";
    }

    // Versão "depois" (policy object)
    public static string Export(string path, ExportPolicy policy)
    {
        return ExportOld(path, policy.Zip, policy.Level, policy.Async, policy.Mode, policy.Locale);
    }
}
