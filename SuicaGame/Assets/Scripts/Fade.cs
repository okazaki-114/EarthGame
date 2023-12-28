using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }   //Fade のインスタンス

    Image fadePanel;
    readonly float fadeDuration = 1.5f;
    readonly float fadeInDuration = 0.15f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        fadePanel = gameObject.transform.Find("Fade").GetComponent<Image>();
        fadePanel.enabled = false;
    }

    public IEnumerator FadeIn()
    {
        StartCoroutine(FadeInOut(false));
        yield return null;
    }


    public IEnumerator FadeOutLoadScene(string scenename)
    {
        StartCoroutine (FadeInOut(true, scenename));
        yield return null;
    }

    private IEnumerator FadeInOut(bool fadeout, string scenename = null)
    {
        fadePanel.gameObject.SetActive(true);

        fadePanel.enabled = true;
        float elapsedTime = 0.0f;

        Color startColor = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        if (!fadeout)
        {
            fadePanel.color = Color.Lerp(startColor, endColor, 1f);
            yield return new WaitForSeconds(fadeInDuration);
        }


        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeout)
                fadePanel.color = Color.Lerp(startColor, endColor, t);
            else
                fadePanel.color = Color.Lerp(startColor, endColor, 1.0f - t);
            yield return null;
        }

        if(fadeout)
            fadePanel.color = endColor;
        fadePanel.enabled = false;
        yield return null;

        if(scenename != null)
            SceneManager.LoadSceneAsync(scenename);
    }
}
