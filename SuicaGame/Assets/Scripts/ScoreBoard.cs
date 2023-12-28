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
    /// �^�C�g���V�[���ɑJ��
    /// </summary>
    public void OnTitleScene()
    {
        StartCoroutine(Fade.Instance.FadeOutLoadScene("Title"));
    }

    /// <summary>
    /// �X�R�A�{�[�h�\��
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
    /// �X�R�A�e�L�X�g���Z�b�g
    /// </summary>
    private void SetScoreText()
    {
        scoreText.text = gameMain.TotalScore.ToString();
    }

    /// <summary>
    /// �n�C�X�R�A�̃e�L�X�g���Z�b�g
    /// </summary>
    private void SetHighScoreText()
    {
        highscoreText.text = UserData.Instance.GetHighScore().ToString();
    }

    /// <summary>
    /// �n�C�X�R�A�Ȃ�n�C�X�R�A��ۑ�
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
    /// �n�C�X�R�A���̃A�j���[�V����
    /// </summary>
    private void HighScoreAnim()
    {
        //�n�C�X�R�A���̃A�j���[�V����
    }

    /// <summary>
    /// �X�R�A�{�[�h�̏o���A�j���[�V����
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
