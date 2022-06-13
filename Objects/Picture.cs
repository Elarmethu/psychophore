using UnityEngine;

public class Picture : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    public int _numOfPicture;

    public void InitializePicture(bool isLoaded, Mood mood, int pictureNum)
    {
        _image.color = new Color32(106, 125, 227, 255);

        if (!isLoaded)
        {
            switch (mood)
            {
                case Mood.Good:
                    _numOfPicture = Random.Range(0, Interaction.Instance.picturesGood.Length);
                    _image.sprite = Interaction.Instance.picturesGood[_numOfPicture];
                    break;
                case Mood.Bad:
                    _numOfPicture = Random.Range(0, Interaction.Instance.picturesBad.Length);
                    _image.sprite = Interaction.Instance.picturesBad[_numOfPicture];
                    break;
                case Mood.Neutral:
                    _numOfPicture = Random.Range(0, Interaction.Instance.picturesNeutral.Length);
                    _image.sprite = Interaction.Instance.picturesNeutral[_numOfPicture];
                    break;
            }
        } else
        {
            switch (mood)
            {
                case Mood.Good:
                    _image.sprite = Interaction.Instance.picturesGood[pictureNum];
                    break;
                case Mood.Bad:
                    _image.sprite = Interaction.Instance.picturesBad[pictureNum];
                    break;
                case Mood.Neutral:
                    _image.sprite = Interaction.Instance.picturesNeutral[pictureNum];
                    break;
            }
        }

        gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(-180, 180));

        Interaction.Instance.picturesInitialized.Add(this.gameObject);
    }
}
