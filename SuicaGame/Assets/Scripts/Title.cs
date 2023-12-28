using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{

    [SerializeField] GameObject BGM_obj;
    [SerializeField] GameObject SE_obj;
    [SerializeField] GameObject BackGround_obj;
    [SerializeField] TextMeshProUGUI highScoreText;

    private void Awake()
    {
    }

    void Start()
    {
        StartCoroutine(Fade.Instance.FadeIn());
        Initialize();
    }

    private void Initialize()
    {
        DontDestroyOnLoad(BGM_obj);         //BGMのオブジェクトを消さないオブジェクトへ変更
        DontDestroyOnLoad(SE_obj);          //SEのオブジェクトを消さないオブジェクトへ変更
        DontDestroyOnLoad(BackGround_obj);  //背景オブジェクトを消さないオブジェクトへ変更

        //ハイスコアのテキストを設定
        highScoreText.text = UserData.Instance.GetHighScore().ToString();
    }

    /// <summary>
    /// メインゲームへ遷移
    /// </summary>
    public void ChangeMainScene()
    {
        StartCoroutine(Fade.Instance.FadeOutLoadScene("Main"));
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
