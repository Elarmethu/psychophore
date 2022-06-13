using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class LetterBehaviour : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private string[] words;
    [SerializeField] private GameObject wordPrefab;
    [SerializeField] private Transform wordsTransform;
    private List<GameObject> wordsInitialized = new List<GameObject>();
    
    [Header("Tabs")]
    [SerializeField] private GameObject _buttonCreate;
    [SerializeField] private GameObject _createQuestion;
    [SerializeField] private GameObject _moodQuestion;
    [SerializeField] private GameObject _wordsQuestion;

    [Header("Letter")]
    public List<string> wordsInLetter;
    [SerializeField] private Mood mood;
    [SerializeField] private UnityEngine.UI.Button _createButton;
    [HideInInspector] public UnityEvent<int> OnWordsCountChange;

    [Header("Spawner")]
    [SerializeField] private GameObject _letterText;
    [SerializeField] private GameObject _helpText;
    [SerializeField] private GameObject _letterPrefab;
    [SerializeField] private Place[] _places;
    [SerializeField] private Material[] _moodMaterials;

    [Header("Words")]
    [SerializeField] private List<TMP_Text> _wordsChoosedTexts;

    public List<GameObject> lettersInitialized;
    private Ray rayLetterCreate;

    public int getCountPlaces
    {
        get { return _places.Length; }
    }

    private void Awake()
    {
        _createButton.onClick.AddListener(() => CreateNewLetter());
        OnWordsCountChange.AddListener((count) =>
        {
            _createButton.interactable = count >= 3 ? true : false;
        });
    }

    private void Start()
    {
        UpdateLettersFromDatabase();
    }

    public void QuestionToCreate(Ray ray)
    {
        _buttonCreate.SetActive(false);
        _createQuestion.SetActive(true);
        rayLetterCreate = ray;
    }

    public void QuestionChoose(bool agree)
    {
        if (agree)
        {
            _createQuestion.SetActive(false);
            _moodQuestion.SetActive(true);
        }
        else
        {
            _buttonCreate.SetActive(true);
            _createQuestion.SetActive(false);
        }
        AudioBehaviour.Instance.ButtonClickSound();
    }

    public void QuestionChooseMood(int _moodNum)
    {
        switch (_moodNum)
        {
            case 0:
                mood = Mood.Good;
                break;
            case 1:
                mood = Mood.Bad;
                break;
            case 2:
                mood = Mood.Neutral;
                break;
        }
        AudioBehaviour.Instance.ButtonClickSound();
        _moodQuestion.SetActive(false);
        _wordsQuestion.SetActive(true);

        InitializeWords();
    }

    private void InitializeWords()
    {
        foreach (string word in words)
        {
            GameObject obj = Instantiate(wordPrefab, wordsTransform);
            Word w = obj.GetComponent<Word>();
            wordsInitialized.Add(obj);

            w.word = word;
            w.InitializeWord();
        }
    }

    private void CreateNewLetter()
    {
        LetterData letterData = new LetterData(wordsInLetter.ToArray(), mood);
        GameObject obj = Instantiate(_letterPrefab);
        Letter letter = obj.GetComponent<Letter>();
        lettersInitialized.Add(obj);

        Vector3 letterPosition = rayLetterCreate.direction * Random.Range(2.0f, 10.0f);
        obj.transform.position = letterPosition;

        letter.data = letterData;

        letter.InitializeLetter(false, null, lettersInitialized.Count - 1);
        mood = Mood.Neutral;
        wordsInLetter.Clear();

        _wordsQuestion.SetActive(false);
        _buttonCreate.SetActive(true);

        foreach (GameObject word in wordsInitialized)
            Destroy(word);

        wordsInitialized.Clear();
        foreach (var text in _wordsChoosedTexts)
            text.text = string.Empty;

        StartCoroutine(LetterTextAnimation());
        AudioBehaviour.Instance.ButtonClickSound();
        Interaction.Instance.View();

        Database.Instance.WriteNewLetterDatabase(letterPosition, obj.transform.eulerAngles, letterData.words, (int)letterData.mood, System.DateTime.Now);
    }

    public void UpdateLettersFromDatabase()
    {
        foreach (var letter in lettersInitialized)
            Destroy(letter.gameObject);

        lettersInitialized.Clear();
        Database.Instance.LoadLetterFromDatabase();

        for(int i = 0; i < Database.Instance.lettersInitialized.Count; i++)
        {
            var init = Database.Instance.lettersInitialized[i];
            LetterData letterData = new LetterData(Database.Instance.lettersInitialized[i].words, (Mood)init.numOfMood);
            GameObject obj = Instantiate(_letterPrefab);
            Letter letter = obj.GetComponent<Letter>();

            obj.transform.position = init.position;
            obj.transform.eulerAngles = init.rotation;

            letter.data = letterData;
            letter.InitializeLetter(true, init.createTime, i);

            lettersInitialized.Add(obj);
        }
    }

    private int ChooseLetterPosition()
    {
        var rnd = Random.Range(0, _places.Length);
        if (_places[rnd].letter != null)
            ChooseLetterPosition();

        return rnd;
    }

    private IEnumerator LetterTextAnimation()
    {
        _letterText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _letterText.SetActive(false);

    }

    public IEnumerator HelpTextAnimation()
    {
        _helpText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _helpText.SetActive(false);
    }

    public void DestroyAllLetters()
    {
        while (lettersInitialized.Count > 0)
        {
            Destroy(lettersInitialized[0]);
            lettersInitialized.RemoveAt(0);
        }
    }

    public void ViewChoosedWord(string word)
    {
        List<string> words = new List<string>();
        foreach (var s in _wordsChoosedTexts)
            words.Add(s.text);

        if (words.Contains(word))
        {
            for(int i = 0; i < _wordsChoosedTexts.Count; i++) 
            {
                if (_wordsChoosedTexts[i].text == word)
                {
                    _wordsChoosedTexts[i].text = string.Empty;
                    break;
                }
            }

            return;
        }

        for(int i = 0; i < _wordsChoosedTexts.Count; i++)
        {
            if (string.IsNullOrEmpty(_wordsChoosedTexts[i].text))
            {
                _wordsChoosedTexts[i].text = word;
                break;
            }
        }
    }
}

[System.Serializable]
public class Place
{
    public Transform place;
    public Letter letter;
}