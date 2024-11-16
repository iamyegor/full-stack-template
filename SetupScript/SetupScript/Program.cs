using System.Diagnostics;
using SetupScript;

if (Directory.Exists(".git"))
{
    Console.WriteLine("Removing existing .git directory...");
    Directory.Delete(".git", true);
}

Process.Start("git", "init").WaitForExit();

Console.Write("Enter project name in PascalCase (e.g., NetIQ): ");
string? pascalName = Console.ReadLine();

Console.Write("Enter project name in kebab-case (e.g., netiq): ");
string? kebabName = Console.ReadLine();

if (!Validators.PascalCaseValid(pascalName) || !Validators.KebabCaseValid(kebabName))
    return;

ReplacementService replacementService = new(pascalName!, kebabName!);
replacementService.ExecuteAllReplacements();
