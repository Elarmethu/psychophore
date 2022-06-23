using UnityEngine;
using Proyecto26;
using System;
using System.Collections.Generic;

public class Database : MonoBehaviour
{
    public static Database Instance { get; private set; }

    public List<LetterJSON> lettersInitialized;
    public List<PictureJSON> picturesInitialized;


    #region Singleton
    private void InitSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
   
    #endregion

    private void Awake()
    {
        InitSingleton();
        LoadLetterFromDatabase();
        LoadPicturesFromDatabase();
    }

    public void WriteNewLetterDatabase(Vector3 _position, Vector3 _rotation, string[] _words, int _numOfMood, DateTime _time)
    {
        LetterJSON letterJSON = new LetterJSON(_position, _rotation, _words, _numOfMood, _time);
        AllLetterJSON letters = new AllLetterJSON();

        RestClient.GetArray<LetterJSON>("https://letters-unity-default-rtdb.firebaseio.com/Letters/letters.json").Then(task =>
        {
            foreach (LetterJSON letter in task)
                letters.letters.Add(letter);

            letters.letters.Add(letterJSON);
            RestClient.Put("https://letters-unity-default-rtdb.firebaseio.com/Letters/.json", letters);        

        });

    }

    public void LoadLetterFromDatabase()
    {
        RestClient.GetArray<LetterJSON>("https://letters-unity-default-rtdb.firebaseio.com/Letters/letters.json").Then(task =>
       {
           lettersInitialized.Clear();
           foreach(LetterJSON letter in task)
               lettersInitialized.Add(letter);
       });
     
    }

    public void DeleteLetterFromDatabase(int pos)
    {
        AllLetterJSON allLetterJSON = new AllLetterJSON();

        lettersInitialized.RemoveAt(pos);
       
        foreach(LetterJSON letter in lettersInitialized)
            allLetterJSON.letters.Add(letter);

        RestClient.Put("https://letters-unity-default-rtdb.firebaseio.com/Letters/.json", allLetterJSON);
    }

    public void WriteNewPictureDatabase(Vector2 _pos, int _numOfMood, int _numOfPicture)
    {
        PictureJSON pictureJSON = new PictureJSON(_pos, _numOfMood, _numOfPicture);
        AllPictureJSON pictures = new AllPictureJSON();

        RestClient.GetArray<PictureJSON>("https://letters-unity-default-rtdb.firebaseio.com/Pictures/pictures.json").Then(task =>
       {
           foreach (PictureJSON picture in task)
               pictures.pictures.Add(picture);

           pictures.pictures.Add(pictureJSON);
           RestClient.Put("https://letters-unity-default-rtdb.firebaseio.com/Pictures/.json", pictures);
       });
    }

    public void LoadPicturesFromDatabase()
    {
        RestClient.GetArray<PictureJSON>("https://letters-unity-default-rtdb.firebaseio.com/Pictures/pictures.json").Then(task =>
        {
            picturesInitialized.Clear();
            foreach (PictureJSON picture in task)
                picturesInitialized.Add(picture);
        });
    }

    public void DeletePictureFromDatabase(int pos)
    {
        AllPictureJSON allPictureJSON = new AllPictureJSON();
        picturesInitialized.RemoveAt(pos);

        foreach (PictureJSON pictures in picturesInitialized)
            allPictureJSON.pictures.Add(pictures);

        RestClient.Put("https://letters-unity-default-rtdb.firebaseio.com/Pictures/.json", allPictureJSON);
    }
}
