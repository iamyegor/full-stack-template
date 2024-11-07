namespace CleanAuthMigrationChanger;

public class SolutionDirectoryFinder
{
    public string GetSolutionDirectoryPath()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        DirectoryInfo dirInfo = new DirectoryInfo(baseDirectory);
        while (!ContainsSolutionFile(dirInfo))
        {
            dirInfo = dirInfo.Parent!;
        }

        return dirInfo.FullName;
    }

    private bool ContainsSolutionFile(DirectoryInfo dirInfo)
    {
        return dirInfo.GetFiles().Select(x => x.Name).Contains("Migrator.sln");
    }
}
