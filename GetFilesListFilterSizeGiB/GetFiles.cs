List<string> GetFiles(string searchPath, int thresholdGiB)
{
    var files = new List<string>();
    GetFilesRecursive(thresholdGiB, searchPath, files);
    return files;
}

void GetFilesRecursive(int thresholdGiB, string currentPath, List<string> files)
{
    string[] filesPath = Directory.GetFiles(currentPath);

    foreach (string file in filesPath)
    {
        var info = new FileInfo(file);
        long size = info.Length;
        long gib = (long)Math.Truncate(size / (1024M * 1024M * 1024M));
        if (gib >= thresholdGiB)
            files.Add(file);
    }

    string[] subFolders = Directory.GetDirectories(currentPath);

    foreach (string subFolder in subFolders)
        GetFilesRecursive(thresholdGiB, subFolder, files);
}
