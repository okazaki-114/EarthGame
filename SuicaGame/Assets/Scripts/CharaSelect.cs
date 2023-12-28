using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CHARA_ID
{
    GOD,    //�_�A�p�b�V�u�Ȃ�
    KAIROS, //�J�C���X�A�X�e�[�W���X������
    THEMIS, //�e�[�~�X�A���������o�Ȃ��Ȃ�A�ΐ����o��悤�ɂȂ�
    COUNT   //��ސ�
}

public class CharaSelect : MonoBehaviour
{
    public static int CharaID = 0; //�L�����N�^�[ID

    [SerializeField] Text charaDescriptionText;
    [SerializeField] Image charaSelectImage;
    [SerializeField] GameObject charaSelectWindow;

    [SerializeField] Button[] charaButtons;
    [SerializeField] Sprite[] charaSprite;

    Color32 charaButtonColor = new Color32(200, 200, 200, 255);
    Color32 charaSelectColor = new Color32(71, 255, 78, 255);

    bool charaSelectActive = false;

    readonly string[] CharacterDescriptions = new string[(int)CHARA_ID.COUNT]
    {
        "���ʂȂ��B���ʂ̃Q�[�����y���߂�B",
        "�v���C���AQ�ō��ցAE�ŉE�ցA�X�e�[�W���X���邱�Ƃ��o����B",
        "�v���C���A���������o�Ȃ��Ȃ�A�ΐ����o��悤�ɂȂ�B"
    };

    private void Awake()
    {
        SetSelectCharaImg();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCharaDescription();
        SetCharacterButtonColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �L�����N�^�[�I����ʂ�\��
    /// </summary>
    public void SetActiveTrue()
    {
        charaSelectWindow.transform.localScale = Vector3.zero;

        charaSelectActive = true;
        SetActive();
        iTween.ScaleTo(charaSelectWindow, iTween.Hash(
              "x", 1,
              "y", 1,
              "time", 0.3f,
              "oncompletetarget", gameObject,
              "oncomplete", "SetActive",
              "easetype", iTween.EaseType.linear
              ));
    }

    /// <summary>
    /// �L�����N�^�[�I����ʂ��\��
    /// </summary>
    public void SetActiveFalse()
    {
        charaSelectActive = false;
        iTween.ScaleTo(charaSelectWindow,iTween.Hash(
              "x", 0,
              "y", 0,
              "time", 0.3f,
              "oncompletetarget", gameObject,
              "oncomplete", "SetActive",
              "easetype", iTween.EaseType.linear
              ));
    }

    private void SetActive()
    {
        charaSelectWindow.SetActive(charaSelectActive);
    }

    /// <summary>
    /// �L�����N�^�[�ݒ�{�^��
    /// </summary>
    /// <param name="id"></param>
    public void OnSetCharaID(int id)
    {
        SetCharaID(id); //�L�����N�^�[ID�ݒ�
        SetSelectCharaImg(); //�L�����N�^�[�摜�ݒ�
        SetCharaDescription(); //�L�����N�^�[�������ݒ�
        SetCharacterButtonColor(); //�ݒ肳��Ă���L�����N�^�[�̃{�^���̐F��ύX
    }

    /// <summary>
    /// �L�����N�^�[�ݒ�
    /// </summary>
    private bool SetCharaID(int id)
    {
        if(CharaID != id)
        {
            CharaID = id; //�L�����N�^�[ID�ݒ�
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetSelectCharaImg()
    {
        charaSelectImage.sprite = charaSprite[CharaID];
    }

    /// <summary>
    /// �ݒ肵�Ă���L�����N�^�[�̐��������Z�b�g
    /// </summary>
    private void SetCharaDescription()
    {
        charaDescriptionText.text = CharacterDescriptions[CharaID];
    }

    /// <summary>
    /// �ݒ肵�Ă���L�����N�^�[�̃{�^���̐F��ύX����
    /// </summary>
    private void SetCharacterButtonColor()
    {
        foreach(var b in charaButtons) 
        {
            ButtonStateColorChange(b, charaButtonColor, 0);
        }

        ButtonStateColorChange(charaButtons[CharaID], charaSelectColor, 0);
    }

    /// <summary>
    /// �{�^����Ԃɂ��F�ύX
    /// </summary>
    private void ButtonStateColorChange(Button button, Color32 color, int changeState)
    {
        ColorBlock colorblock = button.colors;
        switch (changeState)
        {
            case 0://normalColor
                colorblock.normalColor = color;
                break;
            case 1://highlightedColor
                colorblock.highlightedColor = color;
                break;
            case 2://pressedColor
                colorblock.pressedColor = color;
                break;
            case 3://selectedColor
                colorblock.selectedColor = color;
                break;
            case 4://disabledColor
                colorblock.disabledColor = color;
                break;
        }
        button.colors = colorblock;
    }
}
