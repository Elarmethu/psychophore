using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    public string word;
    [SerializeField] private Button _button;
    [SerializeField] private TMPro.TMP_Text text;
    private LetterBehaviour letter;
    private bool isChoosed;

    public void InitializeWord()
    {
        text.text = word;
        _button.onClick.AddListener(() => ChooseWord());
        letter = GameObject.Find("LetterCreate").GetComponent<LetterBehaviour>();
    }

    private void ChooseWord()
    {
        if (letter.wordsInLetter.Count > 3 && !isChoosed)
            return;
        else if (!isChoosed && letter.wordsInLetter.Count < 3)
        {
            gameObject.GetComponent<Image>().color = new Color32(175, 175, 175, 255);
            letter.wordsInLetter.Add(word);
            letter.OnWordsCountChange.Invoke(letter.wordsInLetter.Count);
            isChoosed = true;
        }
        else if (isChoosed)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            letter.wordsInLetter.Remove(word);
            letter.OnWordsCountChange.Invoke(letter.wordsInLetter.Count);
            isChoosed = false;
        }

        letter.ViewChoosedWord(word);
        AudioBehaviour.Instance.ButtonClickSound();

    }
}
