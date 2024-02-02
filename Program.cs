using System;
using System.Diagnostics;
using System.Drawing;

static class Program
{
    static readonly string currentDirectory = Directory.GetCurrentDirectory();
    static readonly string programPath = Path.Combine(" C:\\Users\\PavelPetrovich\\ozon\\bin\\Debug\\net8.0\\ozon.exe");
    static readonly string testsPath = Path.Combine(currentDirectory, "tests");

    struct TestData
    {
        public string input;
        public string awnser;
    }
    static void Main()
    {
        int testNumber = 1;
        while (File.Exists(Path.Combine(testsPath, testNumber.ToString())))
        {
            TestData data = ReadTestData(testNumber);
            if (MakeTest(data))
            {
                Console.WriteLine($"Тест{testNumber} пройден");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Тест{testNumber} НЕ пройден");
                Console.ResetColor();
                return;
            };
            testNumber++;
        }
    }
    static TestData ReadTestData(int path)
    {
        TestData testData = new()
        {
            input = File.ReadAllText(Path.Combine(testsPath, path.ToString())),
            awnser = File.ReadAllText(Path.Combine(testsPath, path.ToString() + ".a")),
        };
        return testData;
    }

    static bool MakeTest(TestData testData)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = programPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        string actualOutput;
        using (var process = new Process { StartInfo = processInfo })
        {
            process.Start();

            process.StandardInput.WriteLine(testData.input);
            process.StandardInput.Close();

            actualOutput = process.StandardOutput.ReadToEnd();
        }
        if (testData.awnser.Trim().Equals(actualOutput.Trim())) return true;

        string awnserPath = Path.Combine(currentDirectory, "ErrorOut/awnser.txt");
        string actualOutputPath = Path.Combine(currentDirectory, "ErrorOut/output.txt");
        string inputPath = Path.Combine(currentDirectory, "ErrorOut/input.txt");

        using (StreamWriter awnserFile = new StreamWriter(awnserPath))
        using (StreamWriter inputFile = new StreamWriter(inputPath))
        using (StreamWriter outputFile = new StreamWriter(actualOutputPath))
        {
            awnserFile.WriteLine(testData.awnser);
            inputFile.WriteLine(testData.input);
            outputFile.WriteLine(actualOutput);
        }

        File.WriteAllText(actualOutputPath, actualOutput);
        return false;

    }
}
