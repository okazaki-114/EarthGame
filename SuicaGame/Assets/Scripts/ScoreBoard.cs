using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject ScoreBoardObj;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] GameMain gameMain;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SetScoreText();
    }

    /// <summary>
    /// タイトルシーンに遷移
    /// </summary>
    public void OnTitleScene()
    {
        StartCoroutine(Fade.Instance.FadeOutLoadScene("Title"));
    }

    /// <summary>
    /// スコアボード表示
    /// </summary>
    public void SetActiveTrue()
    {
        gameObject.SetActive(true);
        OpenWindowAnim();
        SetHighScore();
        SetScoreText();
        SetHighScoreText();
    }

    /// <summary>
    /// スコアテキストをセット
    /// </summary>
    private void SetScoreText()
    {
        scoreText.text = gameMain.TotalScore.ToString();
    }

    /// <summary>
    /// ハイスコアのテキストをセット
    /// </summary>
    private void SetHighScoreText()
    {
        highscoreText.text = UserData.Instance.GetHighScore().ToString();
    }

    /// <summary>
    /// ハイスコアならハイスコアを保存
    /// </summary>
    private bool SetHighScore()
    {
        if (UserData.Instance.isHighScore(gameMain.TotalScore))
        {
            UserData.Instance.SetHighScore(gameMain.TotalScore);
            HighScoreAnim();
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// ハイスコア時のアニメーション
    /// </summary>
    private void HighScoreAnim()
    {
        //ハイスコア時のアニメーション
    }

    /// <summary>
    /// スコアボードの出現アニメーション
    /// </summary>
    private void OpenWindowAnim() 
    {
        iTween.PunchScale(ScoreBoardObj,
            iTween.Hash(
                "x", 2f,
                "y", 2f,
                "time", 1f
                )) ;
    }
}
