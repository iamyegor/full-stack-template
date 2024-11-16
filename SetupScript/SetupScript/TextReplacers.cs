namespace SetupScript;

public class TextReplacers
{
    public static void ReplaceInFile(string filePath, string searchText, string replaceText)
    {
        try
        {
            string content = File.ReadAllText(filePath);
            content = content.Replace(searchText, replaceText);
            File.WriteAllText(filePath, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating file {filePath}: {ex.Message}");
            throw;
        }
    }
}
