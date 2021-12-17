using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Press : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(offText());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(PressAnyKey());
        }
    }

    // �ƹ� Ű�� ������ ����Ǵ� ���� ��� //
    [Tooltip("Fade ������Ʈ �޾ƿ���")]
    [SerializeField] private UI_Fade Fade;
    [Tooltip("Background_Objects ������Ʈ")]
    [SerializeField] private GameObject Bg_Objs;
    [Tooltip("First ������Ʈ")]
    [SerializeField] private GameObject first;
    [Tooltip("Tutorial �̹��� ������Ʈ")]
    [SerializeField] private GameObject tutorialImg;
    [Tooltip("Next �ؽ�Ʈ ������Ʈ")]
    [SerializeField] private GameObject nextBtn;
    public IEnumerator PressAnyKey()
    {
        Fade.FadeIn();
        yield return new WaitForSeconds(1.5f);
        Bg_Objs.SetActive(false);
        first.SetActive(false);

        tutorialImg.SetActive(true);
        nextBtn.SetActive(true);
    }
    // ================================ //

    // �ؽ�Ʈ �����̴� ȿ�� //
    [Tooltip("PressAnyKey �ؽ�Ʈ ������Ʈ")]
    [SerializeField] private Text text;
    public IEnumerator onText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(offText());
    }
    public IEnumerator offText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.2f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(onText());
    }
    // ================== //
}