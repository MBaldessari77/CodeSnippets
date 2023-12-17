TextWriter @out = Console.Out;
string path = @"\\nas2020\download";
long thresholdGB = 50;

foreach (FileInfo file in EnumerateFilesInfoRecursively(path))
    if (file.Length > (1000 * 1000 * 1000 * thresholdGB))
        @out.WriteLine($"ffmpeg.exe -i \"{file.FullName}\" -c:a copy -c:s copy -c:v hevc_nvenc -crf 10 -b:v 30M -preset medium \".\\{file.Name}\"");

IEnumerable<FileInfo> EnumerateFilesInfoRecursively(string path)
{
    string[] files = Directory.GetFiles(path);

    foreach (string file in files)
    {
        FileInfo info = new FileInfo(file);
        yield return info;
    }

    string[] subDirectories = Directory.GetDirectories(path);

    foreach (string subDirectory in subDirectories)
        foreach (var info in EnumerateFilesInfoRecursively(subDirectory))
            yield return info;
}
