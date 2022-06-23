using UnityEngine;

public class SpriteRaycast : MonoBehaviour
{
    private SpriteRenderer _lastSpriteRenderer;

    private void Update()
    {
        RaycastSprite();
    }

    private void RaycastSprite()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject.tag == "Letter")
            {
                var spriteRenderer = hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.color = Color.white;
                _lastSpriteRenderer = spriteRenderer;
            } else if(hit.collider.gameObject.tag == "Picture")
            {
                var spriteRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
                _lastSpriteRenderer = spriteRenderer;
            } else
            {
                if (_lastSpriteRenderer == null) return;
                _lastSpriteRenderer.color = new Color32(106, 125, 227, 255);
                _lastSpriteRenderer = null;
            }
        } else
        {
            if (_lastSpriteRenderer == null) return;
            _lastSpriteRenderer.color = new Color32(106, 125, 227, 255);
            _lastSpriteRenderer = null;
        }
    } 
}
