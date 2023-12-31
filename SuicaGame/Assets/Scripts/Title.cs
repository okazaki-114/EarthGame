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
        //�t�F�[�h�C������
        StartCoroutine(GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeIn());
        Initialize();
    }

    private void Initialize()
    {
        DontDestroyOnLoad(BGM_obj);         //BGM�̃I�u�W�F�N�g�������Ȃ��I�u�W�F�N�g�֕ύX
        DontDestroyOnLoad(SE_obj);          //SE�̃I�u�W�F�N�g�������Ȃ��I�u�W�F�N�g�֕ύX
        DontDestroyOnLoad(BackGround_obj);  //�w�i�I�u�W�F�N�g�������Ȃ��I�u�W�F�N�g�֕ύX

        //�n�C�X�R�A�̃e�L�X�g��ݒ�
        highScoreText.text = UserData.Instance.GetHighScore().ToString();
    }

    /// <summary>
    /// ���C���Q�[���֑J��
    /// </summary>
    public void ChangeMainScene()
    {
        StartCoroutine(GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeOutLoadScene("Main"));
    }

    /// <summary>
    /// �Q�[���I������
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
