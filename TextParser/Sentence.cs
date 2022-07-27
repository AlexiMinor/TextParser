namespace TextParser;

public class Sentence
{
    public List<SentenceItem> SentenceItems;

    public string SentenceString { get; }
    
    public int Number { get; }

    public Sentence(string sentenceString, 
        List<SentenceItem> sentenceItems, 
        int number)
    {
        SentenceString = sentenceString;
        SentenceItems = sentenceItems;
        Number = number;
    }

    public int WordsNumber => SentenceItems.OfType<Word>().Count();
    public int LengthInSymbols => SentenceString.Length;

}