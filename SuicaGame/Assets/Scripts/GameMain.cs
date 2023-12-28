using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance { get; private set; }   //GameMain �̃C���X�^���X
    public bool isDrop { get; private set; }                //��𗎂Ƃ��邩�t���O
    public bool isNext { get; set; }                        //���̎�𐶐����邩�ǂ����̃t���O
    public int MaxSeedNo { get; private set; }              //��̎�ޏ��
    public int seedNum { get; private set; }                //����������index
    public float limitMouse_X { get; private set; }         //�헎�Ƃ��Ƃ��̉��� X
    public float limitMouse_Y { get; private set; }         //�헎�Ƃ��Ƃ��̉��� Y

    //����������̎�� �������
    int Create_Max = 3;
    int Create_Min = 0;

    //����������̎�� �������
    int CharaID;

    //�X�R�A�̗ݏ�
    readonly int SCORE_POW = 3;
    //���̎�̐��l
    readonly int NEXT_SEED = 1;

    //�}�E�X�̐������� �w�W
    readonly float LIMIT_MOUSE_X_POS = 3f;
    readonly float LIMIT_MOUSE_Y_POS = 4f;

    //�����f�B���C
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
        SEED_0, //������
        SEED_1, //��
        SEED_2, //����
        SEED_3, //�ΐ�
        SEED_4, //����
        SEED_5, //�n��
        SEED_6, //�C����
        SEED_7, //�V����
        SEED_8, //�y��
        SEED_9, //�ؐ�
        SEED_10 //���z
    }

    //�X�R�A
    private int totalscore;
    public int TotalScore { get { return totalscore; } }
    //�n�C�X�R�A
    private int highscore;

    private int seedDropCount;
    List<int> seedNumList = new List<int>();

    //�}�E�X�̐�������
    float[] mouseLimitX = new float[] { 0.60f, 0.5f, 0.25f, 0f };

    public enum SE_LIST
    {
        SEED_CREATE = 0,
        SEED_MERGE,
        GAME_OVER
    }

    private void Awake()
    {
        //������
        Initialized();

    }
    void Start()
    {
        //SeedNum��ǉ�
        AddSeedNumList();
        //Next����ǉ�
        AddSeedNumList();                   

        //�t�F�[�h�C������
        StartCoroutine(Fade.Instance.FadeIn());

        //�쐬�������Z�b�g
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
    //  ���������̏���
    //
    //************************************************************************************

    /// <summary>
    /// ������
    /// </summary>
    private void Initialized()
    {
        Instance = this;                    //GameMain �C���X�^���X

        CharaID = CharaSelect.CharaID;      //CharaID�ݒ�
        highscore = UserData.Instance.GetHighScore(); //�n�C�X�R�A�Z�b�g


        seedNumList.Clear();                //List������
        seedDropCount = 0;                  //��̗��Ƃ��Ă���J�E���g��������
        SetCreateMaxMin();                  //��ސ��ݒ�
        isDrop = true;                      //��𗎂Ƃ��邩�t���O
        isNext = false;                     //�l�N�X�g�̗��Ƃ��t���O��False
        MaxSeedNo = seedPrefab.Length;      //��̎�ނ̍ő��ݒ�
        SetScore(0);                        //�X�R�A��0�ɐݒ�
        SetScoreText(totalscore);           //�X�R�A�̃e�L�X�g���X�V
        SetLimitMouse(LIMIT_MOUSE_X_POS, LIMIT_MOUSE_Y_POS);    //�}�E�X�̉���ݒ�
        SetCharaSprite();                   //�L�����N�^�[�̉摜��ݒ�

    }

    private void SetCharaSprite()
    {
        charaPosition.GetComponent<SpriteRenderer>().sprite = charaSprites[CharaID];
    }

    /// <summary>
    /// �쐬�����̎�ސ���ݒ�
    /// </summary>
    void SetCreateMaxMin()
    {
        switch(CharaID) 
        {
            case (int)CHARA_ID.GOD:
            case (int)CHARA_ID.KAIROS:
                Create_Max = (int)SEED_TYPE.SEED_3; //�쐬�����̍ő���
                Create_Min = (int)SEED_TYPE.SEED_0; //�쐬�����̍ŏ����
                break;
            case (int)CHARA_ID.THEMIS:
                Create_Max = (int)SEED_TYPE.SEED_4; //�쐬�����̍ő���
                Create_Min = (int)SEED_TYPE.SEED_1; //�쐬�����̍ŏ����
                break;
        }
    }

    /// <summary>
    /// �쐬�����̃��X�g
    /// </summary>
    void AddSeedNumList()
    {
        seedNumList.Add(SetNextSeedRandomNum());
    }

    /// <summary>
    /// �}�E�X�̉����ݒ�
    /// </summary>
    void SetLimitMouse(float x, float y)
    {
        limitMouse_X = x;
        limitMouse_Y = y;
    }

    /// <summary>
    /// ��̉���
    /// </summary>
    private void LimitMoveSeed()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -limitMouse_X, limitMouse_X);
        mousePos.y = limitMouse_Y;
        charaPosition.position = new Vector3(mousePos.x + 0.3f, mousePos.y - 0.1f, 10);
    }

    /// <summary>
    /// SeedNum���擾
    /// </summary>
    /// <returns></returns>
    public int GetSeedNum()
    {
        return seedNumList[seedDropCount];
    }

    /// <summary>
    /// ��𗎂Ƃ��邩�t���O��ݒ�
    /// </summary>
    public void SetIsDrop(bool b)
    {
        isDrop = b;
    }

    //************************************************************************************
    //
    //  ���C������
    //
    //************************************************************************************

    /// <summary>
    /// ��쐬����
    /// </summary>
    private void CreateSeed()
    {
        //�������̌��ʉ��Đ�
        PlaySE(GameMain.SE_LIST.SEED_CREATE);

        //����쐬
        Seed seedIns = Instantiate(seedPrefab[GetSeedNum()], seedPosition);
        seedIns.seedNo = GetSeedNum();
        seedIns.gameObject.SetActive(true);

        //�}�E�X�̉���ݒ�
        SetLimitMouse(LIMIT_MOUSE_X_POS + mouseLimitX[GetSeedNum()], LIMIT_MOUSE_Y_POS);

        //�l�N�X�g�̃C���[�W��ݒ�
        SetNextImage(seedNumList[seedDropCount + 1]);
    }

    /// <summary>
    /// �킪���̂ɕς�鏈��
    /// </summary>
    public void MergeNext(Vector3 target, int seedNo)
    {
        //���̎�𐶐�����
        Seed seedIns = Instantiate(seedPrefab[seedNo + NEXT_SEED], target, Quaternion.identity, seedPosition);
        
        //��̃X�e�[�^�X���Z�b�g
        seedIns.seedNo = seedNo + NEXT_SEED;
        seedIns.isDrop = true;
        seedIns.GetComponent<Rigidbody2D>().simulated = true;
        seedIns.gameObject.SetActive(true);

        //�X�R�A�𑝂₷
        AddScore((int)Mathf.Pow(SCORE_POW, seedNo));
        SetScoreText(totalscore);
    }

    public void MergeMax(int seedNo) 
    {
        //�X�R�A�𑝂₷
        AddScore((int)Mathf.Pow(SCORE_POW, seedNo));
        SetScoreText(totalscore);
    }

    /// <summary>
    /// �L�[���������ꍇ�X�e�[�W����]������
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
    /// �X�e�[�W����]�����鏈��
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
    //  �l�N�X�g�̏���
    //
    //************************************************************************************

    /// <summary>
    /// ���̎�쐬����
    /// </summary>
    private void CreateNextSeed()
    {
        if (isNext)
        {
            //���Ƃ�����̐��𐔂���
            seedDropCount++;

            //���̃t���O false
            isNext = false;

            //�쐬�������Z�b�g
            AddSeedNumList();

            //���ԍ��Ŏ�쐬
            Invoke("CreateSeed", DELAY_CREATE_TIME);
        }
    }

    /// <summary>
    /// ��̍쐬����z�������_���Ō��߂�
    /// </summary>
    /// <returns></returns>
    int SetNextSeedRandomNum()
    {
        return Random.Range(Create_Min, Create_Max);
    }

    /// <summary>
    /// �l�N�X�g�̉摜���Z�b�g����
    /// </summary>
    /// <param name="num"></param>
    void SetNextImage(int num)
    {
        Image n = GameObject.Find("Canvas/Next/NextImage").GetComponent<Image>();
        n.sprite = seedSprites[num];
    }

    /// <summary>
    /// �l�N�X�g��SeedNum���擾
    /// </summary>
    /// <returns></returns>
    public int GetNextSeedNum()
    {
        return seedNumList[seedDropCount + 1];
    }

    //************************************************************************************
    //
    //  �X�R�A�n�̏���
    //
    //************************************************************************************

    /// <summary>
    /// �X�R�A�̃e�L�X�g���Z�b�g
    /// </summary>
    private void AddScore(int score)
    {
        totalscore += score;
    }

    /// <summary>
    /// �X�R�A�̃e�L�X�g���Z�b�g
    /// </summary>
    private void SetScore(int score)
    {
        totalscore = score;
    }

    /// <summary>
    /// �X�R�A�̃e�L�X�g���Z�b�g
    /// </summary>
    private void SetScoreText(int score)
    {
        txtScore.text = score.ToString();
    }

    //************************************************************************************
    //
    //  ���ʉ�
    //
    //************************************************************************************

    /// <summary>
    /// ���ʉ����Đ�
    /// </summary>
    /// <param name="list">�Đ�������ʉ�</param>
    public void PlaySE(SE_LIST list)
    {
        AudioSource audio = GameObject.Find("SE").GetComponent<AudioSource>();
        audio.clip = SE_clips[(int)list];
        audio.Play();
    }
}
