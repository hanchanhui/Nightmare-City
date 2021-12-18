using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_test : MonoBehaviour
{
    void Update()
    {
        Escape();
    }

    [Tooltip("���� ���� ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup gameGroup;
    [Tooltip("ESC ������ ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup optionGroup;
    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasGroupOn(optionGroup); // �ɼ� ĵ���� �׷� ��
            CanvasGroupOff(gameGroup); // ���� ĵ���� �׷� ����
            
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