using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ParticleSystem _partical;
    [SerializeField] private LetterBehaviour _letterBehaviour;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Inventory.Instance.isShowingWords)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Letter" && !Interaction.Instance.isFindObject)
            {
                var letter = hit.collider.gameObject.GetComponent<Letter>();
                _partical.gameObject.transform.position = hit.transform.position;
                _partical.Play();

                if (!Inventory.Instance.isFull && letter.stage >= 2)
                    StartCoroutine(Inventory.Instance.ReadLetter(letter.gameObject));
                else
                {
                    AudioBehaviour.Instance.ButtonClickSound();
                    StartCoroutine(_letterBehaviour.HelpTextAnimation());
                }
            }

            if(hit.collider.gameObject.tag == "Find" && Interaction.Instance.isFindObject)
            {
                var find = hit.collider.gameObject.GetComponent<Find>();

                if (!find.isActive)
                {
                    find.FindEnable();
                    AudioBehaviour.Instance.TranslateSound();
                }
            }

        } else if(Interaction.Instance.isSeedLetters)
        {
            GameObject obj = GameObject.Find("LetterCreate");
            if (obj == null) return;

            
            LetterBehaviour behaviour = GameObject.Find("LetterCreate").GetComponent<LetterBehaviour>();
            behaviour.QuestionToCreate(ray);
        }

     
    }
}
