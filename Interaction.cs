using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Interaction : MonoBehaviour
{
    public static Interaction Instance { get; private set; }
    
    [Header("Main References")]
    [SerializeField] private GameObject _letterCreateObject;
    [SerializeField] private GameObject _interactionObject;
    [SerializeField] private Transform[] _transforms;
    [SerializeField] private Material _letterSkybox;
    [SerializeField] private Material _interactionSkybox;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameObject[] _buttonsInteraction;
    [SerializeField] private Button _sceneButton;
    [SerializeField] private Hint _hint;

    [Header("Sprites")]
    public List<GameObject> picturesInitialized;
    public Sprite[] stageSprites;
    public Sprite[] picturesGood;
    public Sprite[] picturesBad;
    public Sprite[] picturesNeutral;

    [Header("Prefabs")]
    public GameObject picturePrefab;
    public Transform picturesCanvas;

    [Header("Buttons")]
    [SerializeField] private Button _viewButton;
    [SerializeField] private Button _findButton;
    [SerializeField] private Button _seedButton;


    public bool isInteraction { get; private set; }
    public bool isSeedLetters { get; private set; }

    public bool isFindObject { get; private set; }

    private void Awake()
    {
        Instance = this;

        string question = PlayerPrefs.GetString("IsFirstGame");

        if (!string.IsNullOrEmpty(question))
            View();
        else
        {
            _letterCreateObject.SetActive(true);
            isSeedLetters = false;
            _cameraController.enabled = false;
            _hint.enableObject = true;
        }

        _sceneButton.onClick.AddListener(() => ActivationIntrection());       
    }

    private void Start()
    {
        _letterCreateObject.GetComponent<LetterBehaviour>().UpdateLettersFromDatabase();
    }

    private void ActivationLetterCreate()
    {    
        _letterCreateObject.SetActive(true);
        _interactionObject.SetActive(false);

        _letterCreateObject.GetComponent<LetterBehaviour>().UpdateLettersFromDatabase();
        Database.Instance.LoadLetterFromDatabase();

        isInteraction = false;
        RenderSettings.skybox = _letterSkybox;
        Camera.main.transform.position = _transforms[0].position;
        AudioBehaviour.Instance.ButtonClickSound();

        _sceneButton.onClick.RemoveAllListeners();
        _sceneButton.onClick.AddListener(() => ActivationIntrection());
        View();

        foreach (GameObject button in _buttonsInteraction)
            button.SetActive(true);
    }

    private void ActivationIntrection()
    {
         _letterCreateObject.SetActive(false);
        _interactionObject.SetActive(true);

        isInteraction = true;
        RenderSettings.skybox = _interactionSkybox;
        Camera.main.transform.position = _transforms[1].position;
        Camera.main.transform.eulerAngles = Vector3.zero;
        AudioBehaviour.Instance.ButtonClickSound();

        _sceneButton.onClick.RemoveAllListeners();
        _sceneButton.onClick.AddListener(() => ActivationLetterCreate());
        isSeedLetters = false;
        _cameraController.enabled = true;

        foreach (GameObject button in _buttonsInteraction)
            button.SetActive(false);

        UpdatePicturesFromDatabase();
    }

    public void UpdatePicturesFromDatabase()
    {
        foreach (var picture in picturesInitialized)
            Destroy(picture.gameObject);

        picturesInitialized.Clear();
        Database.Instance.LoadPicturesFromDatabase();

        for (int i = 0; i < Database.Instance.picturesInitialized.Count; i++)
        {
            var init = Database.Instance.picturesInitialized[i];
            var obj = Instantiate(picturePrefab);

            obj.transform.position = new Vector3(init.screenPosition.x, init.screenPosition.y, 1);
            obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);


            var picture = obj.GetComponent<Picture>();
            picture.InitializePicture(true, (Mood)init.numOfMood, init.numOfSprite);

            picturesInitialized.Add(obj);
        }
    }

    #region InteractiveButtons
    
    public void Seed()
    {
        _letterCreateObject.SetActive(true);
        isSeedLetters = true;
        _cameraController.enabled = false;

        AudioBehaviour.Instance.ButtonClickSound();

        _findButton.interactable = true;
        _seedButton.interactable = false;
        _viewButton.interactable = true;
        isFindObject = false;

    }

    public void View()
    {
        _letterCreateObject.SetActive(false);
        isSeedLetters = false;
        _cameraController.enabled = true;
        _cameraController.isDragging = false;

        AudioBehaviour.Instance.ButtonClickSound();

        _findButton.interactable = true;
        _seedButton.interactable = true;
        _viewButton.interactable = false;
        isFindObject = false;

    }

    public void Find()
    {
        _letterCreateObject.SetActive(false);
        AudioBehaviour.Instance.ButtonClickSound();
        _cameraController.enabled = true;
        _cameraController.isDragging = false;

        isFindObject = true;

        _findButton.interactable = false;
        _seedButton.interactable = true;
        _viewButton.interactable = true;
    }

    public void DisableAll()
    {
        isFindObject = false;

        _letterCreateObject.SetActive(false);
        isSeedLetters = false;
        _cameraController.enabled = false;
    }

    #endregion
}
