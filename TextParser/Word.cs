namespace TextParser;

public class Word : SentenceItem
{
    public string WordString { get; }
    public Word(string word)
    {
        Value =word;
        WordString = word;
    }
}