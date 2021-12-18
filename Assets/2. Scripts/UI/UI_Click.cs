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
    [Tooltip("각 버튼의 기능 선택")]
    [SerializeField] private BTNType currentType;
    public UI_test test;
    [Tooltip("게임 씬에 나오는 오브젝트 그룹")]
    [SerializeField] private CanvasGroup gameGroup;
    [Tooltip("ESC 누르면 나오는 오브젝트 그룹")]
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