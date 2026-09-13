using System.Text.Json;

Console.WriteLine("Введи путь к конфиг-файлу (или Enter для config.json по умолчанию):");
string configPathInput = Console.ReadLine();

string configPath = string.IsNullOrWhiteSpace(configPathInput)
    ? "config.json"
    : configPathInput;

if (!File.Exists(configPath))
{
    Console.WriteLine($"Файл конфига не найден: {configPath}");
    return;
}

string configJson = File.ReadAllText(configPath);
Config config = JsonSerializer.Deserialize<Config>(configJson)!;

Console.WriteLine("Введи имя сессии (или просто нажми Enter для случайного):");
string userInput = Console.ReadLine();

string sessionName;
if (string.IsNullOrWhiteSpace(userInput))
{
    sessionName = Guid.NewGuid().ToString().Substring(0, 8);
}
else
{
    sessionName = userInput;
}

var splitter = new AudioSplitter(
    inputFile: config.InputFile,
    chunkSeconds: config.ChunkSeconds,
    sessionName: sessionName
);

splitter.Split();