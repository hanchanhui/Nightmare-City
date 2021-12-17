using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{

    public void Criate()
    {
        gameObject.SetActive(true);
    }    

    public void Destroy()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
