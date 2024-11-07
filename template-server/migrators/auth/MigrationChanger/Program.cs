using System.Text;
using CleanAuthMigrationChanger;

SolutionDirectoryFinder solutionDirectoryFinder = new();

string solutionDirPath = solutionDirectoryFinder.GetSolutionDirectoryPath();
string migrationsDirPath = Path.Combine(solutionDirPath, "Migrator", "Migrations");

if (string.IsNullOrEmpty(migrationsDirPath) || !Directory.Exists(migrationsDirPath))
{
    Console.WriteLine("Invalid directory path.");
    return;
}

ProcessDirectory(migrationsDirPath);
Console.WriteLine("Processing complete.");

static void ProcessDirectory(string directoryPath)
{
    string[] files = Directory.GetFiles(directoryPath);

    foreach (string filePath in files)
    {
        ProcessFile(filePath);
    }
}

static void ProcessFile(string filePath)
{
    StringBuilder fileText = new(File.ReadAllText(filePath));

    fileText.Replace("using Infrastructure.Data;", "using Migrator;");
    // fileText.Replace("ApplicationContext", "ApplicationDbContext");

    File.WriteAllText(filePath, fileText.ToString());
    Console.WriteLine($"Updated file: {filePath}");
}
