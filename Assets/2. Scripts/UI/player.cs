using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ŭ������ System.Serializable�̶�� ��Ʈ����Ʈ(Attribute)�� ����ؾ� Inspector�信 �����
[System.Serializable]
public class Anim
{
    [Tooltip("���� �ִϸ��̼�")]
    public AnimationClip idle;
    [Tooltip("���� �ִϸ��̼�")]
    public AnimationClip runForward;
    [Tooltip("���� �ִϸ��̼�")]
    public AnimationClip runBackward;
    [Tooltip("������ �̵� �ִϸ��̼�")]
    public AnimationClip runRight;
    [Tooltip("���� �̵� �ִϸ��̼�")]
    public AnimationClip runLeft;
}

public class player : MonoBehaviour
{
    // ��� ���� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr; // �����ؾ� �ϴ� ������Ʈ�� �ݵ�� ������ �Ҵ��� �Ŀ� ���
    [Tooltip("�̵� �ӵ� ����")]
    public float moveSpeed = 10.0f;
    [Tooltip("ȸ�� �ӵ� ����")]
    public float rotSpeed = 100.0f;

    // ù ��° ����Ʈ1�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    private bool islcPressed = false;
    private bool islsPressed = false;
    // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    // ��� �Լ� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    void Start()
    {
        // ��ũ��Ʈ ó���� Transform ������Ʈ �Ҵ�
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
    }

    void Move() // Player �����Ӱ� ���õ� �Լ�
    {
        h = Input.GetAxis("Horizontal"); // A, D, Left, Right Ű�� ������ ��
        v = Input.GetAxis("Vertical"); // W, S, Up, Down Ű�� ������ ��

        // �� �� �� �� �̵� ���� ���� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(�̵� ���� * �ӵ� * ������ *  Time.deltaTime, ���� ��ǥ)
        //tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

        //// Vector3.up ���� �������� rotSpeed��ŭ�� �ӵ��� ȸ��
        //tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        // ù ��° ����Ʈ2�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
        // ���� Ctrl Ű�� ������ ���� ���
        if (!islsPressed && Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed *= 1.5f;
            islcPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeed = 10f;
            islcPressed = false;
        }

        // ���� Shift Ű�� ������ ���� ���
        if (!islcPressed && Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            moveSpeed *= 0.5f;
            islsPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
            islsPressed = false;
        }
        // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    }

    static public int key = 0;
    public int stageNum = 0;
    public Text currentKey;
    void OnTriggerEnter(Collider coll)
    {
        //// �浹�� Collider�� ������ PUNCH�̸� Player�� HP ����
        //if(coll.gameObject.tag == "PUNCH")
        //{
        //    hp -= 10;

        //    // Image UI �׸��� fillAmount �Ӽ��� ������ ���� ������ �� ����
        //    imgHpbar.fillAmount = (float)hp / (float)initHp;

        //    Debug.Log("Player HP = " + hp.ToString());
        //    // Player�� ������ 0 �����̸� ��� ó��
        //    if(hp <= 0)
        //    {
        //        PlayerDie();
        //    }
        //}
        if(coll.gameObject.tag == "Key")
        {
            key++;
            currentKey.text = key.ToString();
            Destroy(coll.gameObject);
            if (key == 2 || key == 5)
            {
                createPortal();
            }
        }
        else if(coll.gameObject.tag == "Portal")
        {
            stageNum++;
            SceneManager.LoadScene("test " + stageNum);
        }
    }

    public GameObject portal;
    void createPortal()
    {
        portal.SetActive(true);
    }
}