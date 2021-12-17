using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fade : MonoBehaviour
{
    [Tooltip("���̵��� �Ǵ� ���̵�ƿ� �� �̹���")]
    [SerializeField] private Image Image;

    float time = 0f;
    float F_time = 1f;

    // Fade in�� ���� �Լ� //
    public void FadeIn()
    {
        StartCoroutine(FadeInController());
    }
    IEnumerator FadeInController()
    {
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
    // ================ //

    // Fade out�� ���� �Լ� //
    public void FadeOut()
    {
        StartCoroutine(FadeOutController());
    }
    IEnumerator FadeOutController()
    {
        Color alpha = Image.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Image.color = alpha;
            yield return null;
        }
        Image.gameObject.SetActive(false);
        yield return null;
    }
    // ================ //
}