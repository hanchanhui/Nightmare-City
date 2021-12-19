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
    public UI_test uiTest;
    public GameObject crossHair;
    [Tooltip("ESC ������ ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup optionGroup;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Next:
                SceneManager.LoadScene("1stage");
                break;
            case BTNType.Restart:
                SceneManager.LoadScene("1stage");
                break;
            case BTNType.MainMenu:
                SceneManager.LoadScene("Main Menu");
                break;
            case BTNType.Back:
                uiTest.CanvasGroupOff(optionGroup);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                crossHair.SetActive(true);
                uiTest.CameraOn();
                break;
            case BTNType.Continue_Yes:
                SceneManager.LoadScene("Play Scene");
                break;
            case BTNType.Continue_No:
                SceneManager.LoadScene("Main Menu");
                break;
        }
    }
}