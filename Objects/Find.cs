using UnityEngine;

public class Find : MonoBehaviour
{
    [SerializeField] private GameObject _findSprite;

    public bool isActive { get { return _findSprite.activeSelf; } }

    public void FindEnable()
    {
        _findSprite.SetActive(true);
    }
}
