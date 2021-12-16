using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Press : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        StartCoroutine(FadeInText());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Play Scene");
        }
    }

    public IEnumerator FadeOutText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeInText());
    }
    public IEnumerator FadeInText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.2f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeOutText());
    }
}