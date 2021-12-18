using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_test : MonoBehaviour
{
    void Update()
    {
        Escape();
    }

    [Tooltip("ESC ������ ������ ������Ʈ �׷�")]
    [SerializeField] private CanvasGroup optionGroup;
    public GameObject dieCamera;
    public GameObject crossHair;
    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CameraOff();
            crossHair.SetActive(false);
            CanvasGroupOn(optionGroup); // �ɼ� ĵ���� �׷� ��
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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

    public void CameraOn()
    {
        dieCamera.GetComponent<CameraMove>().enabled = true; // �׾��� �� ī�޶� �������� �ʰ� �Ϸ��� �̸� ��������
    }
    public void CameraOff()
    {
        dieCamera.GetComponent<CameraMove>().enabled = false; // �׾��� �� ī�޶� �������� �ʰ� �Ϸ��� �̸� ��������
    }
}