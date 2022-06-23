using UnityEngine;
using System;

public class Letter : MonoBehaviour
{
    public LetterData data;
    public int position;

    public LetterTime letterTime;
    private int _timeToChangePicture = 300; // Меняете сколько должно пройти время.
    public int stage = 0;

    public void InitializeLetter(bool isLoaded, string date, int pos)
    {        
        letterTime = isLoaded ? new LetterTime(date) : new LetterTime();
        position = pos;
        float rotation = UnityEngine.Random.Range(-43.0f, 43.0f);
        
        if(!isLoaded)
            GetComponent<Transform>().eulerAngles = new Vector3(90.0f, 180.0f,  rotation);

        GetComponent<Transform>().localScale = new Vector3(1f, 1, 0.5f);

        var behaviour = GameObject.Find("LetterCreate").GetComponent<LetterBehaviour>();
        behaviour.lettersInitialized.Add(this.gameObject);

    }

    private void FixedUpdate()
    {
        if (letterTime.GetMinutesLiftetime() >= _timeToChangePicture && stage < 2)
        {
            _timeToChangePicture += _timeToChangePicture;
            stage += 1;

            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Interaction.Instance.stageSprites[stage];

        }
    }

}

[Serializable]
public class LetterTime
{
    public DateTime createData;
    public string date;
    private TimeSpan _timeSpan;
    public LetterTime()
    {
        createData = DateTime.Now;
        date = DateTime.Now.ToString("g");
    }

    public LetterTime(string _date)
    {
        createData = DateTime.Parse(_date);
        date = _date;
    }

    public int GetHoursLifetime()
    {
        _timeSpan = DateTime.Now - createData;
        return _timeSpan.Hours;
    }

    public int GetMinutesLiftetime()
    {
        _timeSpan = DateTime.Now - createData;
        return _timeSpan.Minutes;
    }

    public int GetSecondsLifetime()
    {
        _timeSpan = DateTime.Now - createData;
        return _timeSpan.Seconds;
    }
    
}