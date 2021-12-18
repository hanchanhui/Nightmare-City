using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum BTNType
{
    Next,
    Restart,
    MainMenu,
    Back,
    Continue_Yes,
    Continue_No
}

public class UI_Click : MonoBehaviour
{
    [Tooltip("�� ��ư�� ��� ����")]
    [SerializeField] private BTNType currentType;
    public UI_test test;
    [Tooltip("���� ���� ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup gameGroup;
    [Tooltip("ESC ������ ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup optionGroup;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Next:
                //SceneManager.LoadScene("Play Scene");
                SceneManager.LoadScene("test");
                break;
            case BTNType.Restart:
                //SceneManager.LoadScene("Play Scene");
                SceneManager.LoadScene("test");
                break;
            case BTNType.MainMenu:
                SceneManager.LoadScene("Main Menu");
                break;
            case BTNType.Back:
                test.CanvasGroupOn(gameGroup);
                test.CanvasGroupOff(optionGroup);
                break;
            case BTNType.Continue_Yes:
                //SceneManager.LoadScene("Play Scene");
                SceneManager.LoadScene("test");
                break;
            case BTNType.Continue_No:
                SceneManager.LoadScene("Main Menu");
                break;
        }
    }
}