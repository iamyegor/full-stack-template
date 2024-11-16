using System.Text.RegularExpressions;

namespace SetupScript;

public class Validators
{
    public static bool PascalCaseValid(string? name)
    {
        Regex regex = new("^[A-Z][a-zA-Z0-9]+$");
        if (name != null && !regex.IsMatch(name))
        {
            Console.WriteLine(
                "Error: Project name in PascalCase must start with uppercase and contain only letters and numbers"
            );
            return false;
        }
        return true;
    }

    public static bool KebabCaseValid(string? name)
    {
        Regex regex = new("^[a-z][a-z0-9-]+$");
        if (name != null && !regex.IsMatch(name))
        {
            Console.WriteLine(
                "Error: Project name in kebab-case must be lowercase, start with a letter, and contain only letters, numbers, and hyphens"
            );
            return false;
        }
        return true;
    }
}
