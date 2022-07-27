namespace TextParser;

public class Punctuation : SentenceItem
{
    public string PunctuationString { get; }

    public Punctuation(string punctuationString)
    {
        Value = punctuationString;
        PunctuationString = punctuationString;
    }
}