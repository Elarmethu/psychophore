using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject _hintObject;
    public bool enableObject { set { _hintObject.SetActive(value); } }

    public void EnableObject()
    {
        Interaction.Instance.DisableAll();
        _hintObject.SetActive(true);

        AudioBehaviour.Instance.ButtonClickSound();
    }

    public void DisableObject()
    {
        Interaction.Instance.View();
        _hintObject.SetActive(false);

        AudioBehaviour.Instance.ButtonClickSound();
    }
}
