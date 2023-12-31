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
        //フェードイン処理
        StartCoroutine(GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeIn());
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
        StartCoroutine(GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeOutLoadScene("Main"));
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
