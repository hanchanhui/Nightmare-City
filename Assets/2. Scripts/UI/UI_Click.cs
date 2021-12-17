using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum BTNType
{
    Next,
    Continue_Yes,
    Continue_No
}

public class UI_Click : MonoBehaviour
{
    [Tooltip("�� ��ư�� ��� ����")]
    [SerializeField] private BTNType currentType;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Next:
                SceneManager.LoadScene("Play Scene");
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