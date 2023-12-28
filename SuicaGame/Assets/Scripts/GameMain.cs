using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance { get; private set; }   //GameMain のインスタンス
    public bool isDrop { get; private set; }                //種を落とせるかフラグ
    public bool isNext { get; set; }                        //次の種を生成するかどうかのフラグ
    public int MaxSeedNo { get; private set; }              //種の種類上限
    public int seedNum { get; private set; }                //生成する種のindex
    public float limitMouse_X { get; private set; }         //種落とすときの可動域 X
    public float limitMouse_Y { get; private set; }         //種落とすときの可動域 Y

    //生成される種の種類 上限下限
    int Create_Max = 3;
    int Create_Min = 0;

    //生成される種の種類 上限下限
    int CharaID;

    //スコアの累乗
    readonly int SCORE_POW = 3;
    //次の種の数値
    readonly int NEXT_SEED = 1;

    //マウスの制限距離 指標
    readonly float LIMIT_MOUSE_X_POS = 3f;
    readonly float LIMIT_MOUSE_Y_POS = 4f;

    //生成ディレイ
    readonly float DELAY_CREATE_TIME = 1.5f;

    [SerializeField] private Transform seedPosition;
    [SerializeField] private Transform charaPosition;
    [SerializeField] private Transform stageObject;
    [SerializeField] private Text txtScore;
    [SerializeField] private AudioClip[] SE_clips;
    [SerializeField] private Seed[] seedPrefab;
    [SerializeField] private Sprite[] seedSprites;
    [SerializeField] private Sprite[] charaSprites;

    public enum SEED_TYPE
    {
        SEED_0, //冥王星
        SEED_1, //月
        SEED_2, //水星
        SEED_3, //火星
        SEED_4, //金星
        SEED_5, //地球
        SEED_6, //海王星
        SEED_7, //天王星
        SEED_8, //土星
        SEED_9, //木星
        SEED_10 //太陽
    }

    //スコア
    private int totalscore;
    public int TotalScore { get { return totalscore; } }
    //ハイスコア
    private int highscore;

    private int seedDropCount;
    List<int> seedNumList = new List<int>();

    //マウスの制限距離
    float[] mouseLimitX = new float[] { 0.60f, 0.5f, 0.25f, 0f };

    public enum SE_LIST
    {
        SEED_CREATE = 0,
        SEED_MERGE,
        GAME_OVER
    }

    private void Awake()
    {
        //初期化
        Initialized();

    }
    void Start()
    {
        //SeedNumを追加
        AddSeedNumList();
        //Next分を追加
        AddSeedNumList();                   

        //フェードイン処理
        StartCoroutine(Fade.Instance.FadeIn());

        //作成する種をセット
        CreateSeed();
    }

    void Update()
    {
        CreateNextSeed();

        if(CharaID == (int)CHARA_ID.KAIROS)
            OnRotateStage();

        LimitMoveSeed();
    }



    //************************************************************************************
    //
    //  初期化等の処理
    //
    //************************************************************************************

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialized()
    {
        Instance = this;                    //GameMain インスタンス

        CharaID = CharaSelect.CharaID;      //CharaID設定
        highscore = UserData.Instance.GetHighScore(); //ハイスコアセット


        seedNumList.Clear();                //List初期化
        seedDropCount = 0;                  //種の落としているカウントを初期化
        SetCreateMaxMin();                  //種類数設定
        isDrop = true;                      //種を落とせるかフラグ
        isNext = false;                     //ネクストの落とすフラグをFalse
        MaxSeedNo = seedPrefab.Length;      //種の種類の最大を設定
        SetScore(0);                        //スコアを0に設定
        SetScoreText(totalscore);           //スコアのテキストを更新
        SetLimitMouse(LIMIT_MOUSE_X_POS, LIMIT_MOUSE_Y_POS);    //マウスの可動域設定
        SetCharaSprite();                   //キャラクターの画像を設定

    }

    private void SetCharaSprite()
    {
        charaPosition.GetComponent<SpriteRenderer>().sprite = charaSprites[CharaID];
    }

    /// <summary>
    /// 作成する種の種類数を設定
    /// </summary>
    void SetCreateMaxMin()
    {
        switch(CharaID) 
        {
            case (int)CHARA_ID.GOD:
            case (int)CHARA_ID.KAIROS:
                Create_Max = (int)SEED_TYPE.SEED_3; //作成する種の最大種類
                Create_Min = (int)SEED_TYPE.SEED_0; //作成する種の最小種類
                break;
            case (int)CHARA_ID.THEMIS:
                Create_Max = (int)SEED_TYPE.SEED_4; //作成する種の最大種類
                Create_Min = (int)SEED_TYPE.SEED_1; //作成する種の最小種類
                break;
        }
    }

    /// <summary>
    /// 作成する種のリスト
    /// </summary>
    void AddSeedNumList()
    {
        seedNumList.Add(SetNextSeedRandomNum());
    }

    /// <summary>
    /// マウスの可動域を設定
    /// </summary>
    void SetLimitMouse(float x, float y)
    {
        limitMouse_X = x;
        limitMouse_Y = y;
    }

    /// <summary>
    /// 種の可動域
    /// </summary>
    private void LimitMoveSeed()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -limitMouse_X, limitMouse_X);
        mousePos.y = limitMouse_Y;
        charaPosition.position = new Vector3(mousePos.x + 0.3f, mousePos.y - 0.1f, 10);
    }

    /// <summary>
    /// SeedNumを取得
    /// </summary>
    /// <returns></returns>
    public int GetSeedNum()
    {
        return seedNumList[seedDropCount];
    }

    /// <summary>
    /// 種を落とせるかフラグを設定
    /// </summary>
    public void SetIsDrop(bool b)
    {
        isDrop = b;
    }

    //************************************************************************************
    //
    //  メイン処理
    //
    //************************************************************************************

    /// <summary>
    /// 種作成処理
    /// </summary>
    private void CreateSeed()
    {
        //生成時の効果音再生
        PlaySE(GameMain.SE_LIST.SEED_CREATE);

        //種を作成
        Seed seedIns = Instantiate(seedPrefab[GetSeedNum()], seedPosition);
        seedIns.seedNo = GetSeedNum();
        seedIns.gameObject.SetActive(true);

        //マウスの可動域設定
        SetLimitMouse(LIMIT_MOUSE_X_POS + mouseLimitX[GetSeedNum()], LIMIT_MOUSE_Y_POS);

        //ネクストのイメージを設定
        SetNextImage(seedNumList[seedDropCount + 1]);
    }

    /// <summary>
    /// 種が次のに変わる処理
    /// </summary>
    public void MergeNext(Vector3 target, int seedNo)
    {
        //次の種を生成する
        Seed seedIns = Instantiate(seedPrefab[seedNo + NEXT_SEED], target, Quaternion.identity, seedPosition);
        
        //種のステータスをセット
        seedIns.seedNo = seedNo + NEXT_SEED;
        seedIns.isDrop = true;
        seedIns.GetComponent<Rigidbody2D>().simulated = true;
        seedIns.gameObject.SetActive(true);

        //スコアを増やす
        AddScore((int)Mathf.Pow(SCORE_POW, seedNo));
        SetScoreText(totalscore);
    }

    public void MergeMax(int seedNo) 
    {
        //スコアを増やす
        AddScore((int)Mathf.Pow(SCORE_POW, seedNo));
        SetScoreText(totalscore);
    }

    /// <summary>
    /// キーを押した場合ステージを回転させる
    /// </summary>
    private void OnRotateStage()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            RotateStage(stageObject, false);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            RotateStage(stageObject, true);
        }
    }


    /// <summary>
    /// ステージを回転させる処理
    /// </summary>
    private void RotateStage(Transform t, bool right)
    {
        float r = 0.0001f;
        if (right)
            r *= -1;

        if (t.localRotation.z <= 0.1f 
            && t.localRotation.z >= -0.1f)
        {
            t.rotation = new Quaternion(
                t.rotation.x,
                t.rotation.y,
                t.rotation.z + r,
                t.rotation.w);
        }

        if(t.localRotation.z > 0.1f)
            t.rotation = new Quaternion(t.rotation.x, t.rotation.y, 0.1f, t.rotation.w);
        else if (t.localRotation.z < -0.1f)
            t.rotation = new Quaternion(t.rotation.x, t.rotation.y, -0.1f, t.rotation.w);

    }
    //************************************************************************************
    //
    //  ネクストの処理
    //
    //************************************************************************************

    /// <summary>
    /// 次の種作成処理
    /// </summary>
    private void CreateNextSeed()
    {
        if (isNext)
        {
            //落とした種の数を数える
            seedDropCount++;

            //次のフラグ false
            isNext = false;

            //作成する種をセット
            AddSeedNumList();

            //時間差で種作成
            Invoke("CreateSeed", DELAY_CREATE_TIME);
        }
    }

    /// <summary>
    /// 種の作成する奴をランダムで決める
    /// </summary>
    /// <returns></returns>
    int SetNextSeedRandomNum()
    {
        return Random.Range(Create_Min, Create_Max);
    }

    /// <summary>
    /// ネクストの画像をセットする
    /// </summary>
    /// <param name="num"></param>
    void SetNextImage(int num)
    {
        Image n = GameObject.Find("Canvas/Next/NextImage").GetComponent<Image>();
        n.sprite = seedSprites[num];
    }

    /// <summary>
    /// ネクストのSeedNumを取得
    /// </summary>
    /// <returns></returns>
    public int GetNextSeedNum()
    {
        return seedNumList[seedDropCount + 1];
    }

    //************************************************************************************
    //
    //  スコア系の処理
    //
    //************************************************************************************

    /// <summary>
    /// スコアのテキストをセット
    /// </summary>
    private void AddScore(int score)
    {
        totalscore += score;
    }

    /// <summary>
    /// スコアのテキストをセット
    /// </summary>
    private void SetScore(int score)
    {
        totalscore = score;
    }

    /// <summary>
    /// スコアのテキストをセット
    /// </summary>
    private void SetScoreText(int score)
    {
        txtScore.text = score.ToString();
    }

    //************************************************************************************
    //
    //  効果音
    //
    //************************************************************************************

    /// <summary>
    /// 効果音を再生
    /// </summary>
    /// <param name="list">再生する効果音</param>
    public void PlaySE(SE_LIST list)
    {
        AudioSource audio = GameObject.Find("SE").GetComponent<AudioSource>();
        audio.clip = SE_clips[(int)list];
        audio.Play();
    }
}
