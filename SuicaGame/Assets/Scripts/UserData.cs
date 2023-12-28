using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public static UserData Instance;            //Instance
    readonly string userData_key = "HighScore"; //PlayerPrefs��Key

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// �n�C�X�R�A���擾
    /// </summary>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(userData_key);
    }

    /// <summary>
    /// �n�C�X�R�A��ݒ�
    /// </summary>
    public void SetHighScore(int num)
    {
        PlayerPrefs.SetInt(userData_key, num);
    }

    /// <summary>
    /// �n�C�X�R�A���ǂ�������
    /// </summary>
    public bool isHighScore(int num)
    {
        if(GetHighScore() < num)
            return true;
        else
            return false;
    }

}
