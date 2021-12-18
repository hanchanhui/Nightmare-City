using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    [Tooltip("시간 표시")]
    int prevTime;
    [Tooltip("전체 시간")]
    private float surviveTime;
    [Tooltip("생성 시간 마무리")]
    private bool surviveEnd = true;
    //[Tooltip("실행 조건")]
    //int prevEventTime;
    [Tooltip("랜덤 생성 범위 설정")]
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
            Debug.Log("생성");
            GameObject monster = Instantiate(monsterPrefab, randompos, Quaternion.identity);
            //monster.transform.parent = ;
            monster.transform.localPosition = randompos;
        }
        prevMonsterCheck = (int)(surviveTime % 5f);
    }
}
