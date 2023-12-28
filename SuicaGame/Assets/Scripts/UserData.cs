using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public static UserData Instance;            //Instance
    readonly string userData_key = "HighScore"; //PlayerPrefsのKey

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ハイスコアを取得
    /// </summary>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(userData_key);
    }

    /// <summary>
    /// ハイスコアを設定
    /// </summary>
    public void SetHighScore(int num)
    {
        PlayerPrefs.SetInt(userData_key, num);
    }

    /// <summary>
    /// ハイスコアかどうか判定
    /// </summary>
    public bool isHighScore(int num)
    {
        if(GetHighScore() < num)
            return true;
        else
            return false;
    }

}
