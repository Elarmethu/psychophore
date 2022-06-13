using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public LetterData data;
    public Image image;
    public int position;

    public void InitializeItem()
    {
        Mood mood = data.mood;
        switch (mood)
        {
            case Mood.Good:
                image.sprite = Inventory.Instance.spritesOfMood[0];
                break;
            case Mood.Bad:
                image.sprite = Inventory.Instance.spritesOfMood[1];
                break;
            case Mood.Neutral:
                image.sprite = Inventory.Instance.spritesOfMood[2];
                break;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Interaction.Instance.isInteraction) return;

        var screenPoint = (Vector3)Input.mousePosition;
        screenPoint.z = 1.0f;
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Interaction.Instance.isInteraction)
            return;

        var prefab = Interaction.Instance.picturePrefab;
        var obj = Instantiate(prefab);
        var screenPoint = (Vector3)Input.mousePosition;
        screenPoint.z = 1.0f;


        obj.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        var picture = obj.GetComponent<Picture>();
        picture.InitializePicture(false, data.mood, 0);


        Database.Instance.WriteNewPictureDatabase(obj.transform.position, (int)data.mood, picture._numOfPicture); 
        AudioBehaviour.Instance.TranslateSound();
        Inventory.Instance.RemoveItem(position);
        Destroy(gameObject);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(120, 108, 140, 255);
        Inventory.Instance.ViewItemWords(this, true);       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Inventory.Instance.ViewItemWords(this, false);
    }
}
