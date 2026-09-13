using System.Diagnostics;

public class AudioSplitter
{
    private readonly string _inputFile;
    private readonly double _chunkSeconds;
    private readonly string _outputDir;

    public AudioSplitter(string inputFile, double chunkSeconds, string sessionName)
    {
        _inputFile = inputFile;
        _chunkSeconds = chunkSeconds;
        _outputDir = Path.Combine($"artifacts-{sessionName}", "wav");
    }

    public void Split()
    {
        Directory.CreateDirectory(_outputDir);

        double totalSeconds = 7;
        int chunkCount = (int)Math.Ceiling(totalSeconds / _chunkSeconds);

        for (int i = 1; i <= chunkCount; i++)
        {
            string fileName = $"artifact-{i:D2}.wav";
            string fullPath = Path.Combine(_outputDir, fileName);

            double startSeconds = (i - 1) * _chunkSeconds;
            string startTime = TimeSpan.FromSeconds(startSeconds).ToString(@"hh\:mm\:ss");

            string ffmpegArgs = $"-i {_inputFile} -ss {startTime} -t 00:00:{_chunkSeconds:00} -ar 16000 -ac 1 -c:a pcm_s16le {fullPath}";
            RunFfmpeg(ffmpegArgs);

            Console.WriteLine($"Готово: {fullPath}");
        }
    }

    private void RunFfmpeg(string ffmpegArgs)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = ffmpegArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi)!;
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Console.WriteLine(process.StandardError.ReadToEnd());
        }
    }
}