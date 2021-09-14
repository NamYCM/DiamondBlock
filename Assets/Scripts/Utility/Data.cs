using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Data
{
    //Singleton
    private Data () {}
    static readonly Data instance = new Data();
    public static Data Instance => instance;

    public int HighScore
    {
        get {
            return PlayerPrefs.GetInt(keyHighScore, 0);
        }
    }

    private const string keyHighScore = "score";

    public void SaveHighScore (int highScore)
    {
        PlayerPrefs.SetInt (keyHighScore, highScore);
    }
}
