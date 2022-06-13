using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Main References")]
    [SerializeField] private List<InventoryCell> _inventoryCells;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private TMP_Text _wordsText;
    [SerializeField] private Sprite[] _spritesOfMood;

    [Header("Words")]
    [SerializeField] private GameObject _wordsInventory;
    [SerializeField] private TMP_Text[] _wordsTextInventory = new TMP_Text[3];
    [SerializeField] private LetterBehaviour _letterBehaviour;
    public Sprite[] spritesOfMood { get { return _spritesOfMood; } }
    public bool isShowingWords { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void AddItem(LetterData data)
    {
        int position = 0;

        for(int i = 0; i < _inventoryCells.Count; i++)
        {
            if (_inventoryCells[i].item == null)
            {
                position = i;
                break;
            }
        }

        GameObject cell = Instantiate(_itemPrefab);
        cell.transform.SetParent(_inventoryCells[position].place);
        cell.GetComponent<RectTransform>().localPosition = Vector3.zero;
        cell.GetComponent<RectTransform>().localScale = Vector3.one;
        cell.GetComponent<RectTransform>().eulerAngles = Camera.main.transform.eulerAngles;

        Item item = cell.GetComponent<Item>();
        item.data = data;
        item.position = position;
        item.InitializeItem();

        _inventoryCells[position].item = item;
        
    }

    public void RemoveItem(int pos)
    {
        _inventoryCells[pos].item = null;
    }

    public IEnumerator ReadLetter(GameObject letter)
    {
        isShowingWords = true;
        LetterData data = letter.GetComponent<Letter>().data;

        _wordsText.gameObject.SetActive(true);
        _wordsText.text = data.words[0];
        AudioBehaviour.Instance.TranslateSound();
        yield return new WaitForSecondsRealtime(2.0f);

        _wordsText.text = data.words[1];
        AudioBehaviour.Instance.TranslateSound();
        yield return new WaitForSecondsRealtime(2.0f);

        _wordsText.text = data.words[2];
        AudioBehaviour.Instance.TranslateSound();
        yield return new WaitForSecondsRealtime(2.0f);

        _wordsText.text = string.Empty;
        _wordsText.gameObject.SetActive(false);
        isShowingWords = false;

        Database.Instance.DeleteLetterFromDatabase(letter.GetComponent<Letter>().position);

        AddItem(data);
        Destroy(letter);      
    }

    public void ViewItemWords(Item item, bool isEnable)
    {
        if (!isEnable)
        {
            _wordsInventory.SetActive(false);
            return;
        }

        for(int i = 0; i < 3; i++)
            _wordsTextInventory[i].text = item.data.words[i];

        _wordsInventory.SetActive(true);
    }

    public bool isFull
    {
        get
        {
            foreach (var item in _inventoryCells)
            {
                if (item.item == null)
                    return false;
            }

            return true;
        }
    }
}

[System.Serializable]
public class InventoryCell
{
    public Transform place;
    public Item item;
}
