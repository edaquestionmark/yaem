using System.IO;

public static class FileUtils
{
    /// <summary>
    /// Create a new file with givven path or open existing
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static FileStream CreateFileWtihFolders(string path)
    {
        string directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(path))
        {
            return File.Create(path);
        }

        return File.OpenRead(path);
    }
}
