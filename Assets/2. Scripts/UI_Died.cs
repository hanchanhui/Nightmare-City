using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Died : MonoBehaviour
{
    [Tooltip("페이드인 할 이미지")]
    [SerializeField] private Image Image;
    [Tooltip("페이드인 할 텍스트")]
    [SerializeField] private Text text;

    float time = 0f;
    float F_time = 1f;

    public void Fade()
    {
        //StartCoroutine(FadeOutText());
        //StartCoroutine(FadeInBackground());
    }

    IEnumerator FadeOutText()
    {
        text.gameObject.SetActive(true);
        Color alpha = text.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            text.color = alpha;
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeInBackground()
    {
        yield return new WaitForSeconds(3f);

        Image.gameObject.SetActive(true);
        Color alpha = Image.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Image.color = alpha;
            yield return null;
        }
        yield return null;
    }
}