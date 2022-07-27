using System.Collections.Concurrent;

namespace TextParser;

public class TextParser
{
    public List<Sentence> GetSentences(string content)
    {
        content = PrepareText(content);

        var stringSentences = content.Split("__", StringSplitOptions.RemoveEmptyEntries);

        var sentences = new ConcurrentBag<Sentence>();

        var result = Parallel.For(0, stringSentences.Length,
            i =>
            {
                var sentence = GetSentenceFromString(stringSentences[i], i);
                sentences.Add(sentence);
            });

        while (!result.IsCompleted)
        {

        }

        return sentences.ToList();

    }

    private Sentence GetSentenceFromString(string sentence, int i)
    {
        var sentenceParts = sentence
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .SelectMany(GetWordAndPunctuation)
            .ToList();

        var sentenceObj = new Sentence(sentence, sentenceParts, i);

        return sentenceObj;
    }

    private string PrepareText(string content)
    {
        content = content.Replace("\n", "\n__");
        content = content.Replace(". ", ". __");
        content = content.Replace("! ", "! __");
        content = content.Replace("? ", "? __");
        content = content.Replace(".\n", ".\n__");
        content = content.Replace("!\n", "!\n__");
        content = content.Replace("?\n", "?\n__");
        content = content.Replace("...", "...__");
        content = content.Replace("?!", "?!__");
        content = content.Replace("?..", "?..__");
        content = content.Replace("!..", "!..__");

        return content;
    }

    private List<SentenceItem> GetWordAndPunctuation(string rawWord)
    {
        var prePunctuationString = "";
        var postPunctuationString = "";
        var wordString = "";

        var items = new List<SentenceItem>();

        foreach (var t in rawWord)
        {
            if (char.IsPunctuation(t)
                && string.IsNullOrEmpty(wordString))
            {
                prePunctuationString += t;
            }
            else if (char.IsLetterOrDigit(t))
            {
                wordString += t;
            }
            else
            {
                postPunctuationString += t;
            }
        }

        if (!string.IsNullOrEmpty(prePunctuationString))
        {
            var prePunct = new Punctuation(prePunctuationString);
            items.Add(prePunct);
        }
        if (!string.IsNullOrEmpty(wordString))
        {
            var word = new Word(wordString);
            items.Add(word);
        }
        if (!string.IsNullOrEmpty(postPunctuationString))
        {
            var postPunct = new Punctuation(postPunctuationString);
            items.Add(postPunct);
        }

        if (items.Any())
        {
            return items;
        }
        else
        {
            return null;
        }
    }

    public Dictionary<string, int> GetAlphabeticalWordsDictionary
        (List<Sentence> sentences)
    {
        var alphabeticalWordsDictionary =
            new Dictionary<string, int>();

        var words = sentences
                      .SelectMany(s => s.SentenceItems)
                      .OfType<Word>()
                      .Select(word => word.WordString.ToUpperInvariant())
                      .ToList();

        foreach (var word in words)
        {

            if (alphabeticalWordsDictionary.ContainsKey(word))
            {
                alphabeticalWordsDictionary[word]++;
            }
            else
            {
                alphabeticalWordsDictionary.Add(word, 1);
            }
        }

        return alphabeticalWordsDictionary;
    }

    public string GetMostOftenSymbol
        (List<Sentence> sentences)
    {
        var charDictionary = 
            new Dictionary<char, int>();

        var symbols = sentences
            .SelectMany(s => s.SentenceItems)
            .OfType<Word>()
            .Select(word => word.WordString.ToUpperInvariant())
            .SelectMany(s => s.ToCharArray())
            .ToList();

        foreach (var symbol in symbols)
        {

            if (charDictionary.ContainsKey(symbol))
            {
                charDictionary[symbol]++;
            }
            else
            {
                charDictionary.Add(symbol, 1);
            }
        }

        var element = charDictionary.MaxBy(pair => pair.Value).Key.ToString();

        return element;
    }
}
