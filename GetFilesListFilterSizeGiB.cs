TextWriter @out = Console.Out;
string searchPath = @"\\nas2020\films";
int thresholdGiB = 50;

List<string> files = GetFiles(searchPath, thresholdGiB);

foreach (string file in files)
{
    string fileName = Path.GetFileName(file);
    Console.WriteLine($"ffmpeg.exe -i \"{file}\" -c:a copy -c:s copy -c:v hevc_nvenc -crf 10 -b:v 30M -preset medium \".\\{fileName}\"");
}

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
