using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private Animator ani;
    private Animator BossAni;
    private AudioSource audioSource;

    //HP
    public float health;

    // Patroling
    public Vector3 walkPoint;
    public bool walkPointSet = false;
    public float walkPointRange;
    
    // Move
    public int MonsterMoveX = 0;
    public int MonsterMoveZ;
    private int MoveCheck = 0;
    public bool MonsterDir = false;
    public bool MonsterMoveCheck = false;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // State
    public float sightRange, attackRange; // eyeContactRange
    public bool playerInSightRange, playerInAttackRange;
    public float sightRangecharge;

    // Monster Sortation
    public bool Monster = false;
    public bool Boss = false;

    // Effect
    public GameObject bloodEffect;

    // Die Check
    public bool MonsterDie = false;
    public bool MonsterDieCheck = false;

    // Sound
    public AudioClip MonsterIdle;
    public AudioClip MonsterWalk;
    public AudioClip MonsterDied;
    public AudioClip BossWav;
    public AudioClip BossDie;
    public bool ZonbieSounds = true;
    public bool BossSounds = true;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (Monster)
        {
            ani = GetComponent<Animator>();
        }
        if(Boss)
        {
            BossAni = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (Monster)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
                MonsterMoveCheck = false;
            }
            if (ZonbieSounds)
            {
                MonsterSound("Idle");
                ZonbieSounds = false;
            }
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            
            if (Monster)
            {
                MonsterMoveCheck = true;
                sightRange = sightRangecharge;
                MonsterAni("IsWalkTrue");
            }
            if(Boss)
            {
                if (BossSounds)
                {
                    BossSound("Wav");
                    Debug.Log("Boss����");
                    BossSounds = false;
                }
            }
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
            if (Monster)
            {
                MonsterAni("IsAttackTrue");
            }
            if(Boss)
            {
                BossAnimator("IsAttackTrue");
            }
        }
    }

    // Freedom Move
    private void Patroling()
    {
       
        MonsterAni("IsWalkFalse");
        

        if (!walkPointSet)
        {
            SearchWalkPoint();
            MonsterAni("IsTraceFalse"); 
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            //MonsterSound("Walk");
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            if (MoveCheck == 0)
            {
                MonsterDir = true;
                MoveCheck = 1;
            }
            else if (MoveCheck == 1)
            {
                MonsterDir = false;
                MoveCheck = 0;
            }
        }
    }
    // Move X,Z
    private void SearchWalkPoint()
    {
        int MoveX;
        int MoveZ;

        // Calculate random point in range
        if (!MonsterDir)
        {
            MoveX = MonsterMoveX;
            MoveZ = MonsterMoveZ;
        }
        else
        {
            MoveX = -MonsterMoveX;
            MoveZ = -MonsterMoveZ;
        }
        

        walkPoint = new Vector3(transform.position.x + MoveX, transform.position.y, transform.position.z + MoveZ);

        
        // ���� �ִ��� Ȯ��
        Invoke("WalkPointSetting", 4f);
  
    }
    // Move On
    private void WalkPointSetting()
    {
       walkPointSet = true;
        if(MonsterMoveCheck || MonsterMoveX != 0 || MonsterMoveZ != 0)
        {
            MonsterAni("IsTraceTrue");
        }
    }
    

    // Player Followed
    private void ChasePlayer()
    {
        if (!MonsterDie)
        {
            agent.SetDestination(player.position);
            MonsterSound("Walk");
        }
    }

    // Player Attect
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void MonAttack()
    {
        projectile.SetActive(true);
    }

    public void AttackDestroy()
    {
        projectile.SetActive(false);
    }

    // Attack Cool time
    private void ResetAttack()
    {
        alreadyAttacked = false;
        if (Monster)
        {
            MonsterAni("IsAttackFalse");
        }
        if (Boss)
        {
            BossAnimator("IsAttackFalse");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            MonsterDieCheck = true;
            if (Monster)
            {
                MonsterAni("IsDeadTrue");
                MonsterDie = true;
                ZonbieSounds = true;
                gameObject.tag = "DieEnemy";
                if (ZonbieSounds)
                {
                    MonsterSound("Die");
                    ZonbieSounds = false;
                }
            }
            if (Boss)
            {
                BossAnimator("IsDeadTrue");
                BossSounds = true;
                gameObject.tag = "DieEnemy";
                if (BossSounds)
                {
                    BossSound("Die");
                    BossSounds = false;
                }
            }
            Invoke(nameof(DestroyEnemy), 0.7f);
        }
    }


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // Zombie Animation
    private void MonsterAni(string Ani)
    {
        switch (Ani)
        {
            case "IsTraceTrue":
                ani.SetBool("IsTrace", true);
                break;
            case "IsTraceFalse":
                ani.SetBool("IsTrace", false);
                break;
            case "IsWalkTrue":
                ani.SetBool("IsWalk", true);
                break;
            case "IsWalkFalse":
                ani.SetBool("IsWalk", false);
                break;
            case "IsAttackTrue":
                ani.SetBool("IsAttack", true);
                break;
            case "IsAttackFalse":
                ani.SetBool("IsAttack", false);
                break;
            case "IsDeadTrue":
                ani.SetTrigger("IsDead");
                break;
            default:
                break;
        }
    }

    // Boss Animation
    private void BossAnimator(string Ani)
    {
        switch (Ani)
        {
            case "IsAttackTrue":
                BossAni.SetBool("IsAttack", true);
                break;
            case "IsAttackFalse":
                BossAni.SetBool("IsAttack", false);
                break;
            case "IsDeadTrue":
                BossAni.SetTrigger("IsDead");
                break;
            default:
                break;
        }
    }

    // Monster Sound
    private void MonsterSound(string action)
    {
        switch(action)
        {
            case "Idle":
                audioSource.clip = MonsterIdle;
                break;
            case "Walk":
                audioSource.clip = MonsterWalk;
                break;
            case "Die":
                audioSource.clip = MonsterDied;
                break;
            default:
                Debug.Log("ERROR : �ش� ���� �� ����");
                break;
        }
        audioSource.Play();
    }
    // Boss Sound
    private void BossSound(string action)
    {
        switch (action)
        {
            case "Wav":
                audioSource.clip = BossWav;
                break;
            case "Die":
                audioSource.clip = BossDie;
                break;
            default:
                Debug.Log("ERROR : �ش� ���� �� ����");
                break;
        }
        audioSource.Play();
    }

    // Particle
    public void CreateBloodEffect()
    {
        // ���� ����
        if (Monster)
        {
            Vector3 pos = transform.position + transform.up * 5f;
            GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
            Destroy(blood1, 1.0f);
        }
        else if (Boss)
        {
            Vector3 pos = transform.position + transform.up * 7f;
            GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
            Destroy(blood1, 1.0f);
        }

    }

    // Player Check Range 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
