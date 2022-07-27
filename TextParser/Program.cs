namespace TextParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string TARGET_FILE = @"C:\Users\AlexiMinor\Desktop\sample.txt";

            var fileHandler = new FileHandler();

            var fileContent = await fileHandler.ReadFileAsync(TARGET_FILE);

            var parser = new TextParser();

            var sentences = parser.GetSentences(fileContent);

            await fileHandler.WriteContentToFileAsync(@"C:\Users\AlexiMinor\Desktop\sample-restored.txt", sentences);

            var alphabeticalWordsCount
                = parser.GetAlphabeticalWordsDictionary(sentences)
                    .Select(pair => new Tuple<string, int>(pair.Key, pair.Value))
                    .OrderBy(tuple => tuple.Item1)
                    .ToArray();

            await fileHandler.WriteContentToFileAsync(@"C:\Users\AlexiMinor\Desktop\alph-dict.txt", 
                alphabeticalWordsCount);


            var resultStrings = new List<string>
            {
                sentences.MaxBy(sentence => sentence.LengthInSymbols)!.SentenceString,
                sentences.Where(sentence => sentence.WordsNumber>=1).MinBy(sentence => sentence.WordsNumber)!.SentenceString,
                parser.GetMostOftenSymbol(sentences)
            };


            await fileHandler.WriteContentToFileAsync(@"C:\Users\AlexiMinor\Desktop\file-result.txt", resultStrings);

        }
    }
}