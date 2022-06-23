using UnityEngine;
using UnityEngine.EventSystems;

public class CameraButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isHorizontal;
    public bool isReverse;

    private bool _isPressed;

    private void Update()
    {
        if(!isHorizontal && !isReverse && _isPressed)
        {
            Vector3 vertical = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.5f, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, vertical, Time.deltaTime * 0.5f);
        } else if(!isHorizontal && isReverse && _isPressed)
        {
            Vector3 vertical = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 0.5f, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, vertical, Time.deltaTime * 0.5f);
        } else if(isHorizontal && !isReverse && _isPressed)
        {
            Vector3 horizontal = new Vector3(Camera.main.transform.position.x + 0.5f, Camera.main.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, horizontal, Time.deltaTime * 0.5f);
        } else if(isHorizontal && isReverse && _isPressed)
        {
            Vector3 horizontal = new Vector3(Camera.main.transform.position.x - 0.5f, Camera.main.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, horizontal, Time.deltaTime * 0.5f);
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }
}
