using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayScene : MonoBehaviour
{
    [Tooltip("Fade 오브젝트 받아오기")]
    [SerializeField] private UI_Died fade;

    // DiedScene으로 넘어오고 FadeOut하는 과정
    void Start()
    {
        StartCoroutine(toDiedScene());
    }
    public IEnumerator toDiedScene()
    {
        yield return new WaitForSeconds(0.5f);
        fade.Fade();
        yield return null;
    }
}
