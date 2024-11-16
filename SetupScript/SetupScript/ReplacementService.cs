namespace SetupScript;

public class ReplacementService
{
    private readonly string _pascalName;
    private readonly string _kebabName;

    public ReplacementService(string pascalName, string kebabName)
    {
        _pascalName = pascalName;
        _kebabName = kebabName;
    }

    public void RenameDirectories()
    {
        Console.WriteLine("Renaming directories...");
        Console.WriteLine("Renaming template-server to {_kebabName}-server");
        Directory.Move("template-server", $"{_kebabName}-server");
        
        Console.WriteLine("Renaming template-client to {_kebabName}-client");
        Directory.Move("template-client", $"{_kebabName}-client");

        Console.WriteLine($"Renaming Template folder to {_pascalName}");
        Directory.Move(
            Path.Combine($"{_kebabName}-server", "Template"),
            Path.Combine($"{_kebabName}-server", _pascalName)
        );
    }

    public void UpdateMainProjectFiles()
    {
        Console.WriteLine("Updating main project files...");

        string mainSettings = Path.Combine(
            $"{_kebabName}-server",
            _pascalName,
            "Api",
            "appsettings.json"
        );
        Console.WriteLine($"Processing {mainSettings}");
        TextReplacers.ReplaceInFile(mainSettings, "template.server", $"{_kebabName}.server");
        TextReplacers.ReplaceInFile(mainSettings, "template.client", $"{_kebabName}.client");

        string mainDevSettings = Path.Combine(
            $"{_kebabName}-server",
            _pascalName,
            "Api",
            "appsettings.Development.json"
        );
        Console.WriteLine($"Processing {mainDevSettings}");
        TextReplacers.ReplaceInFile(mainDevSettings, "template_db", $"{_kebabName}_db");

        string mainDi = Path.Combine(
            $"{_kebabName}-server",
            _pascalName,
            "Api",
            "DependencyInjection.cs"
        );
        Console.WriteLine($"Processing {mainDi}");
        TextReplacers.ReplaceInFile(mainDi, "Template Error", $"{_pascalName} Error");
    }

    public void UpdateAuthFiles()
    {
        Console.WriteLine("Updating auth files...");

        string authSettings = Path.Combine($"{_kebabName}-server", "Auth", "Api", "appsettings.json");
        Console.WriteLine($"Processing {authSettings}");
        TextReplacers.ReplaceInFile(authSettings, "template.server", $"{_kebabName}.server");
        TextReplacers.ReplaceInFile(authSettings, "template.client", $"{_kebabName}.client");

        string authDevSettings = Path.Combine(
            $"{_kebabName}-server",
            "Auth",
            "Api",
            "appsettings.Development.json"
        );
        Console.WriteLine($"Processing {authDevSettings}");
        TextReplacers.ReplaceInFile(authDevSettings, "template_auth_db", $"{_kebabName}_auth_db");

        string authDi = Path.Combine($"{_kebabName}-server", "Auth", "Api", "DependencyInjection.cs");
        Console.WriteLine($"Processing {authDi}");
        TextReplacers.ReplaceInFile(authDi, "Template (Auth) Error", $"{_pascalName} (Auth) Error");
    }

    public void UpdateDockerfiles()
    {
        UpdateMainDockerfile();
        UpdateMigrationsDockerfile();
    }

    private void UpdateMainDockerfile()
    {
        string dockerfile = Path.Combine($"{_kebabName}-server", _pascalName, "Dockerfile");
        Console.WriteLine($"Updating {dockerfile}...");
        
        TextReplacers.ReplaceInFile(
            dockerfile,
            "COPY [\"Template/Api/Api.csproj\"",
            $"COPY [\"{_pascalName}/Api/Api.csproj\""
        );
        TextReplacers.ReplaceInFile(
            dockerfile,
            "COPY [\"Template/Application/Application.csproj\"",
            $"COPY [\"{_pascalName}/Application/Application.csproj\""
        );
        TextReplacers.ReplaceInFile(
            dockerfile,
            "COPY [\"Template/Domain/Domain.csproj\"",
            $"COPY [\"{_pascalName}/Domain/Domain.csproj\""
        );
        TextReplacers.ReplaceInFile(
            dockerfile,
            "COPY [\"Template/Infrastructure/Infrastructure.csproj\"",
            $"COPY [\"{_pascalName}/Infrastructure/Infrastructure.csproj\""
        );
        TextReplacers.ReplaceInFile(dockerfile, "COPY Template/", $"COPY {_pascalName}/");
    }

    private void UpdateMigrationsDockerfile()
    {
        string migrationsDockerfile = Path.Combine(
            $"{_kebabName}-server",
            "migrators",
            _kebabName,
            "Dockerfile"
        );
        Console.WriteLine($"Updating migrations {migrationsDockerfile}...");
        
        TextReplacers.ReplaceInFile(
            migrationsDockerfile, 
            "COPY migrators/template/", 
            $"COPY migrators/{_kebabName}/"
        );
        TextReplacers.ReplaceInFile(
            migrationsDockerfile,
            "COPY [\"migrators/template/",
            $"COPY [\"migrators/{_kebabName}/"
        );
        TextReplacers.ReplaceInFile(
            migrationsDockerfile, 
            "COPY [\"Template/", 
            $"COPY [\"{_pascalName}/"
        );
    }

    public void RenameMigratorsFolder()
    {
        Directory.Move(
            Path.Combine($"{_kebabName}-server", "migrators", "template"),
            Path.Combine($"{_kebabName}-server", "migrators", _kebabName)
        );
    }

    public void ExecuteAllReplacements()
    {
        try
        {
            RenameDirectories();
            UpdateMainProjectFiles();
            UpdateAuthFiles();
            UpdateDockerfiles();
            RenameMigratorsFolder();

            Console.WriteLine("Project setup complete!");
            Console.WriteLine($"Project {_pascalName} ({_kebabName}) has been configured successfully.");
            Console.WriteLine("Please review the changes and commit them to the new repository.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during project setup: {ex.Message}");
            Console.WriteLine("Please check the error and try again.");
        }
    }
}
