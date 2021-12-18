using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_test : MonoBehaviour
{
    void Update()
    {
        Escape();
    }

    [Tooltip("게임 씬에 나오는 오브젝트 그룹")]
    [SerializeField] private CanvasGroup gameGroup;
    [Tooltip("ESC 누르면 나오는 오브젝트 그룹")]
    [SerializeField] private CanvasGroup optionGroup;
    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasGroupOn(optionGroup); // 옵션 캔버스 그룹 온
            CanvasGroupOff(gameGroup); // 메인 캔버스 그룹 오프
            
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}