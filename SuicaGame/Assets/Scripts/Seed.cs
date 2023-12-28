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

    //�Q�[���I�[�o�[���C���ɗ��܂��Ă��鎞�Ԍv��
    public float stayTime = 0;

    private void Awake()
    {
        iTween.PunchScale(gameObject, iTween.Hash("x", 0.1f, "y", 0.1f, "time", 1f));
    }

    void Start()
    {
        //GameMain��Find�@GameMain�̃R���|�[�l���g���擾
        gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();
        //Rigidbody2D���擾
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //�}�E�X�N���b�N���Ɏ�𗎂Ƃ�����
        //�{�E���O�̃N���b�N�͎�𗎂Ƃ��Ȃ�
        if (Input.GetMouseButton(0)
            && gameMain.isDrop
            && isDrop == false
            && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < mouseClickLimit_X
            && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -mouseClickLimit_X)
        {
            Drop();
        }
        //isDrop��true�̏ꍇ�Areturn
        if (isDrop) return;

        //�����ݒ�
        LimitMoveSeed();
    }

    /// <summary>
    /// ��𗎂Ƃ�
    /// </summary>
    private void Drop()
    {
        isDrop = true;
        _rb.simulated = true;
        GameMain.Instance.isNext = true;
    }

    /// <summary>
    /// ������ɓ�������������Ď��̎�𐶐�����
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colobj = collision.gameObject;
        if (colobj.CompareTag("Seed"))
        {
            Seed colseed = collision.gameObject.GetComponent<Seed>();
            //�����킪���������ꍇ
            if (seedNo == colseed.seedNo &&
                !isMergeFlag &&
                !colseed.isMergeFlag &&
                seedNo < GameMain.Instance.MaxSeedNo - 1)
            {
                MergeSeed(colseed);
                gameMain.PlaySE(GameMain.SE_LIST.SEED_MERGE);
            }
            //��ԍŌ�̎킪���������ꍇ
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
    /// �}�[�W����
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
    /// �}�[�W�����̂���ԑ傫����̏ꍇ�̏���
    /// </summary>
    /// <param name="colseed"></param>
    private void MergeMaxSeedNum(Seed colseed)
    {
        GameMain.Instance.MergeMax(seedNo);
        Destroy(gameObject);
        Destroy(colseed.gameObject);
    }

    /// <summary>
    /// ��̉���
    /// </summary>
    private void LimitMoveSeed()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -gameMain.limitMouse_X, gameMain.limitMouse_X);
        mousePos.y = gameMain.limitMouse_Y;
        transform.position = mousePos;
    }
}
