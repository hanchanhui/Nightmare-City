using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    [Tooltip("�ð� ǥ��")]
    int prevTime;
    [Tooltip("��ü �ð�")]
    private float surviveTime;
    [Tooltip("���� �ð� ������")]
    private bool surviveEnd = true;
    //[Tooltip("���� ����")]
    //int prevEventTime;
    [Tooltip("���� ���� ���� ����")]
    public float randomX;
    public float randomZ;
    int prevMonsterCheck;


    public GameObject monsterPrefab;


    void Update()
    {
        CreateMonster();
    }

    void CreateMonster()
    {
        surviveTime += Time.deltaTime;
        
        if(surviveTime % 5f <= 0.01f && prevMonsterCheck == 4)
        {
            float MoveX = Random.Range(-randomX, randomX);
            float MoveZ = Random.Range(-randomZ, randomZ);

            Vector3 randompos = new Vector3(transform.position.x + MoveX, transform.position.y, transform.position.z + MoveZ);
            Debug.Log("����");
            GameObject monster = Instantiate(monsterPrefab, randompos, Quaternion.identity);
            //monster.transform.parent = ;
            monster.transform.localPosition = randompos;
        }
        prevMonsterCheck = (int)(surviveTime % 5f);
    }
}