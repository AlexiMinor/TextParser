using System.Text;

namespace TextParser;

public class FileHandler
{
    public async Task<string> ReadFileAsync(string path)
    {
        try
        {
            using (var sr = new StreamReader(File.OpenRead(path)))
            {
                var fileContent = await sr.ReadToEndAsync();
                return fileContent;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception handled:");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            return "";
        }
        
    }

    public async Task WriteContentToFileAsync(string path, IEnumerable<Sentence> sentences)
    {
        var orderedSentences = sentences
            .OrderBy(sentence => sentence.Number)
            .ToList();

        using (var sw = new StreamWriter(File.OpenWrite(path)))
        {
            foreach (var sentence in orderedSentences)
            {
                var stringCollection =
                    sentence.SentenceItems
                        .Where(sentence => sentence != null)
                        .Select(item => item.Value)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();
                var result = string.Join(" ", stringCollection);
                await sw.WriteLineAsync(result);
            }
        }
    }

    public async Task WriteContentToFileAsync(string path, Tuple<string, int>[] wordsCount)
    {
        using (var sw = new StreamWriter(
                   File.Open(path, FileMode.Create, FileAccess.Write),
                   Encoding.UTF8))
        {
            foreach (var word in wordsCount)
            {
                await sw.WriteLineAsync($"{word.Item1} - {word.Item2}");
            }
        }
    }

    public async Task WriteContentToFileAsync(string path, IEnumerable<string> resultStrings)
    {
        using (var sw = new StreamWriter(
                   File.Open(path, FileMode.Create, FileAccess.Write),
                   Encoding.UTF8))
        {
            foreach (var word in resultStrings)
            {
                await sw.WriteLineAsync(word);
            }
        }
    }
}