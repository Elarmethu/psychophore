using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Spine.Unity;

public class StartController : MonoBehaviour
{
    [Header("Logo References")]
    [SerializeField] private SkeletonGraphic _logo;
    [SerializeField] private GameObject _logoButtonObject;

    [Header("Description References")]
    [SerializeField] private GameObject _descriptionObject;
    [SerializeField] private TMP_Text _descriptionText;
    private void Start()
    {
        LogoFade();
        _logoButtonObject.GetComponent<Button>().onClick.AddListener(() => DescriptionEnable());
    }

    private void LogoFade()
    {        
        Image buttonImage = _logoButtonObject.GetComponent<Image>();
        StartCoroutine(UnfadeImage(buttonImage));
    }


    private IEnumerator DescriptionFade()
    {
        AudioBehaviour.Instance.ButtonClickSound();
        StartCoroutine(WaitSceneLoad());
        while (_descriptionText.color.a <= 255.0f)
        {
            _descriptionText.color = new Color(_descriptionText.color.r, _descriptionText.color.g, _descriptionText.color.b, _descriptionText.color.a + 0.001f);
            yield return null;
        }

    }

    private IEnumerator UnfadeImage(Image image)
    {
        while (image.color.a < 255.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.001f);
            yield return null;
        }
    }

    private IEnumerator WaitSceneLoad()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(1);
    }

    private void DescriptionEnable()
    {
        _descriptionObject.SetActive(true);
        _logo.gameObject.SetActive(false);

        StartCoroutine(DescriptionFade());
    }

}
