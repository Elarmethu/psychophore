using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class LetterJSON
{
    public Vector3Int position;
    public Vector3Int rotation;
    public string[] words;
    public int numOfMood;
    public string createTime;

    public LetterJSON(Vector3 _position, Vector3 _rotation, string[] _words, int _numOfMood, DateTime _time)
    {
        position = new Vector3Int((int)_position.x, (int)_position.y, (int)_position.z);
        rotation = new Vector3Int((int)_rotation.x, (int)_rotation.y, (int)_rotation.z);
        numOfMood = (int)_numOfMood;
        words = _words;
        words = _words;
        createTime = _time.ToString("g");
    }
}

[System.Serializable]
public class AllLetterJSON
{
    public List<LetterJSON> letters = new List<LetterJSON>();
}