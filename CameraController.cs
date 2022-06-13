using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isDragging = false;
    [SerializeField] private float _startMouseX = 0.0f;
    [SerializeField] private float _startMouseY = 0.0f;

    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensetivityScroll;

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        if (Interaction.Instance.isInteraction)
            return;
        
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - (Input.GetAxis("Mouse ScrollWheel") * _sensetivityScroll), 90, 120);

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            isDragging = true;

            _startMouseX = Input.mousePosition.x;
            _startMouseY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
        }

    }


    private void LateUpdate()
    {
        if (Interaction.Instance.isInteraction)
            return;

        if (isDragging)
        {
            float endMouseX = Input.mousePosition.x;
            float endMouseY = Input.mousePosition.y;

            float diffX = endMouseX - _startMouseX;
            float diffY = endMouseY - _startMouseY;

            float newCenterX = Screen.width / 2 + diffX;
            float newCenterY = Screen.height / 2 + diffY;

            Vector3 LookHerePoint = _camera.ScreenToWorldPoint(new Vector3(newCenterX, newCenterY, _camera.nearClipPlane));
            transform.LookAt(LookHerePoint);

            _startMouseX = endMouseX;
            _startMouseY = endMouseY;
        }
    }

    public void Horizontal(bool isRight)
    {
        if(isRight)
            _camera.transform.position = new Vector3(_camera.transform.position.x + 0.5f, _camera.transform.position.y, _camera.transform.position.z);
        else
            _camera.transform.position = new Vector3(_camera.transform.position.x - 0.5f, _camera.transform.position.y, _camera.transform.position.z);

    }

    public void Vertical(bool isUp)
    {
        if (isUp)
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y + 0.5f, _camera.transform.position.z);
        else
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y - 0.5f, _camera.transform.position.z);
    }

}

