using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CHARA_ID
{
    GOD,    //神、パッシブなし
    KAIROS, //カイロス、ステージを傾けられる
    THEMIS, //テーミス、冥王星が出なくなり、火星が出るようになる
    COUNT   //種類数
}

public class CharaSelect : MonoBehaviour
{
    public static int CharaID = 0; //キャラクターID

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
        "効果なし。普通のゲームを楽しめる。",
        "プレイ中、Qで左へ、Eで右へ、ステージを傾けることが出来る。",
        "プレイ中、冥王星が出なくなり、火星が出るようになる。"
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
    /// キャラクター選択画面を表示
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
    /// キャラクター選択画面を非表示
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
    /// キャラクター設定ボタン
    /// </summary>
    /// <param name="id"></param>
    public void OnSetCharaID(int id)
    {
        SetCharaID(id); //キャラクターID設定
        SetSelectCharaImg(); //キャラクター画像設定
        SetCharaDescription(); //キャラクター説明文設定
        SetCharacterButtonColor(); //設定されているキャラクターのボタンの色を変更
    }

    /// <summary>
    /// キャラクター設定
    /// </summary>
    private bool SetCharaID(int id)
    {
        if(CharaID != id)
        {
            CharaID = id; //キャラクターID設定
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
    /// 設定しているキャラクターの説明文をセット
    /// </summary>
    private void SetCharaDescription()
    {
        charaDescriptionText.text = CharacterDescriptions[CharaID];
    }

    /// <summary>
    /// 設定しているキャラクターのボタンの色を変更する
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
    /// ボタン状態による色変更
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
