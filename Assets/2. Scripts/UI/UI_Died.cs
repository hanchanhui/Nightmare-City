using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Died : MonoBehaviour
{
    public void playerDie()
    {
        StartCoroutine(Die());
    }

    [Tooltip("Fade ������Ʈ �޾ƿ���")]
    [SerializeField] private UI_Fade Fade;
    public IEnumerator Die()
    {
        StartCoroutine(onText());
        yield return new WaitForSeconds(3f);
        Fade.FadeIn();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Die Scene");
    }

    float time = 0f;
    float F_time = 1f;

    [Tooltip("you died �ؽ�Ʈ ������Ʈ")]
    [SerializeField] private Text text;
    public IEnumerator onText()
    {
        text.gameObject.SetActive(true);
        Color alpha = text.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            text.color = alpha;
            yield return null;
        }
        yield return null;
    }
}