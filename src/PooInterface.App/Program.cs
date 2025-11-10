using PooInterface.Core.Formatters;
using PooInterface.Core.Models;
using PooInterface.Core.Repositories;

Console.WriteLine("PooInterface demo\n");

// Demo formatter usage (Fase 2/3/4)
var raw = "hello world from poointerface";
Console.WriteLine("Procedural TitleCase: " + FormatterProcedural.Format(raw, FormatterProcedural.Mode.TitleCase));
var fmt = new TitleCaseFormatter();
Console.WriteLine("OO TitleCase: " + fmt.Format(raw));
var ifmt = new InterfaceTitleCaseFormatter();
Console.WriteLine("Interface TitleCase: " + ifmt.Format(raw));

// Demo repository (in-memory)
var repo = new InMemoryRepository();
var t1 = new ToDo("Write demo");
repo.Add(t1);
Console.WriteLine($"Added todo: {t1.Id} - {t1.Title}");

foreach (var t in repo.List())
    Console.WriteLine($"Stored: {t.Id} {t.Title} (done:{t.Done})");

Console.WriteLine("\nDemo finished.");