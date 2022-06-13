using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehaviour : MonoBehaviour
{
    public static AudioBehaviour Instance;

    private  AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _translateSound;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void ButtonClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
    }

    public void TranslateSound()
    {
        _audioSource.PlayOneShot(_translateSound);
    }

}
