[System.Serializable]
public class LetterData
{
    public string[] words;
    public Mood mood;

    public LetterData(string[] _words, Mood _mood)
    {
        words = _words;
        mood = _mood;
    }
}

[System.Serializable]
public enum Mood
{
    Good,
    Bad,
    Neutral
}
