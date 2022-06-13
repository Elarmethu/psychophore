using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PictureJSON
{
    public Vector2 screenPosition;
    public int numOfMood;
    public int numOfSprite;

    public PictureJSON(Vector2 _pos, int _mood, int _sprite)
    {
        screenPosition = _pos;
        numOfMood = _mood;
        numOfSprite = _sprite;
    }
}

[System.Serializable]
public class AllPictureJSON
{
    public List<PictureJSON> pictures = new List<PictureJSON>();
}