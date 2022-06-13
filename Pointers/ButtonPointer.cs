using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _description?.SetActive(false);
    }
}
