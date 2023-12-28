using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    readonly float speaker_0 = 0.33f;
    readonly float speaker_1 = 0.66f;

    [SerializeField] Slider audioSlider;
    [SerializeField] Image[] speakerImages;

    [SerializeField] GameObject[] Audio_objs;
    [SerializeField] Sprite[] SpeakerSprites;
    
    AudioSource[] audioSources = new AudioSource[2];

    public static float audioVolume = 0f; 
    bool audio_flag;

    enum SPEAKER_NUM
    {
        VOL_MAX,
        VOL_01,
        VOL_02,
        VOL_MIN
    }

    // Start is called before the first frame update
    void Start()
    {
        audio_flag = false;

        FindAudioSource();

        foreach (AudioSource a in audioSources)
            a.volume = audioVolume;

        audioSlider.value = audioVolume;
        SetSpeakerImg();
    }

    // Update is called once per frame
    void Update()
    {
        SetAudioValue();
        SetSpeakerImg();
    }

    /// <summary>
    /// audioSourcesを設定する
    /// </summary>
    private bool FindAudioSource()
    {
        audioSources[0] = GameObject.Find("BGM").GetComponent<AudioSource>();
        audioSources[1] = GameObject.Find("SE").GetComponent<AudioSource>();

        foreach (AudioSource source in audioSources) 
        {
            if (source == null)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 音量設定ボタンの処理
    /// </summary>
    public void OnSettingButton()
    {
        if (!audio_flag)
        {
            audio_flag = true;
            SetActiveSettingObj(audio_flag);
        }
        else
        {
            audio_flag = false;
            SetActiveSettingObj(audio_flag);
        }
    }

    /// <summary>
    ///音量設定の画面を表示・非表示
    /// </summary>
    /// <param name="flag">true = 音量設定表示/param>
    private void SetActiveSettingObj(bool flag)
    {
        Audio_objs[0].SetActive(!flag);
        Audio_objs[1].SetActive(flag);
    }

    private void SetAudioValue()
    {
        foreach (AudioSource a in audioSources)
            a.volume = audioSlider.value;

        audioVolume = audioSlider.value;
    }

    /// <summary>
    /// 音量の大きさでスピーカーの画像を変更
    /// </summary>
    private void SetSpeakerImg()
    {
        if (audioSlider.value == 0)
        {
            foreach (Image img in speakerImages)
                img.sprite = SpeakerSprites[(int)SPEAKER_NUM.VOL_MIN];
        }
        else if (audioSlider.value > 0 && audioSlider.value < speaker_0)
        {
            foreach (Image img in speakerImages)
                img.sprite = SpeakerSprites[(int)SPEAKER_NUM.VOL_02];
        }
        else if (audioSlider.value >= speaker_0 && audioSlider.value < speaker_1)
        {
            foreach (Image img in speakerImages)
                img.sprite = SpeakerSprites[(int)SPEAKER_NUM.VOL_01];
        }
        else if (audioSlider.value == 1)
        {
            foreach (Image img in speakerImages)
                img.sprite = SpeakerSprites[(int)SPEAKER_NUM.VOL_MAX];
        }
    }

}
