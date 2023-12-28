using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    //GameMain
    GameMain gameMain;

    readonly float mouseClickLimit_X = 5f;
    private Rigidbody2D _rb;
    public bool isMergeFlag = false;
    public bool isDrop = false;
    public int seedNo;

    //ゲームオーバーラインに留まっている時間計測
    public float stayTime = 0;

    private void Awake()
    {
        iTween.PunchScale(gameObject, iTween.Hash("x", 0.1f, "y", 0.1f, "time", 1f));
    }

    void Start()
    {
        //GameMainをFind　GameMainのコンポーネントを取得
        gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();
        //Rigidbody2Dを取得
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //マウスクリック時に種を落とす処理
        //ボウル外のクリックは種を落とさない
        if (Input.GetMouseButton(0)
            && gameMain.isDrop
            && isDrop == false
            && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < mouseClickLimit_X
            && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -mouseClickLimit_X)
        {
            Drop();
        }
        //isDropがtrueの場合、return
        if (isDrop) return;

        //可動域を設定
        LimitMoveSeed();
    }

    /// <summary>
    /// 種を落とす
    /// </summary>
    private void Drop()
    {
        isDrop = true;
        _rb.simulated = true;
        GameMain.Instance.isNext = true;
    }

    /// <summary>
    /// 同じ種に当たったら消して次の種を生成する
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colobj = collision.gameObject;
        if (colobj.CompareTag("Seed"))
        {
            Seed colseed = collision.gameObject.GetComponent<Seed>();
            //同じ種が当たった場合
            if (seedNo == colseed.seedNo &&
                !isMergeFlag &&
                !colseed.isMergeFlag &&
                seedNo < GameMain.Instance.MaxSeedNo - 1)
            {
                MergeSeed(colseed);
                gameMain.PlaySE(GameMain.SE_LIST.SEED_MERGE);
            }
            //一番最後の種が当たった場合
            else if(seedNo == colseed.seedNo &&
                !isMergeFlag &&
                !colseed.isMergeFlag &&
                seedNo == GameMain.Instance.MaxSeedNo - 1)
            {
                MergeMaxSeedNum(colseed);
                gameMain.PlaySE(GameMain.SE_LIST.SEED_MERGE);
            }
        }
    }

    /// <summary>
    /// マージ処理
    /// </summary>
    private void MergeSeed(Seed colseed)
    {
        isMergeFlag = true;
        colseed.isMergeFlag = true;
        GameMain.Instance.MergeNext(transform.position, seedNo);
        Destroy(gameObject);
        Destroy(colseed.gameObject);
    }

    /// <summary>
    /// マージしたのが一番大きい種の場合の処理
    /// </summary>
    /// <param name="colseed"></param>
    private void MergeMaxSeedNum(Seed colseed)
    {
        GameMain.Instance.MergeMax(seedNo);
        Destroy(gameObject);
        Destroy(colseed.gameObject);
    }

    /// <summary>
    /// 種の可動域
    /// </summary>
    private void LimitMoveSeed()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -gameMain.limitMouse_X, gameMain.limitMouse_X);
        mousePos.y = gameMain.limitMouse_Y;
        transform.position = mousePos;
    }
}
