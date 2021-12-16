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

    public float health;
    public bool MonsterDir = false;

    // Patroling
    public Vector3 walkPoint;
    public bool walkPointSet = false;
    public float walkPointRange;


    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // State
    public float sightRange, attackRange; // eyeContactRange
    public bool playerInSightRange, playerInAttackRange;

    // Monster Sortation
    public bool Monster = false;
    public bool Boss = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
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
                //RecognitionPlayer();
                //ani.SetBool("IsWalk", false);
            }
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            if (Monster)
            {
                MonsterAni("IsWalkTrue");
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

    private void Patroling()
    {
        if (Monster)
        {
            MonsterAni("IsWalkFalse");
        }

        if (!walkPointSet)
        {
            SearchWalkPoint();
            if (Monster)
            {
                MonsterAni("IsTraceFalse");
            }
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = 0;
        float randomX = 0;

        // Calculate random point in range
        if (!MonsterDir)
        {
            randomZ = Random.Range(-walkPointRange, walkPointRange);
            MonsterDir = true;
        }
        else
        {
            randomX = Random.Range(-walkPointRange, walkPointRange);
            MonsterDir = false;
        }

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        // 지상에 있는지 확인
        Invoke("WalkPointSetting", 4f);

        /*
        Debug.DrawRay(transform.position, -transform.up, Color.blue, 2f);
        if (Physics.Raycast(walkPoint, -transform.up, 10f, whatIsGround))
        {
            Debug.Log("check");
            walkPointSet = true;
        }*/
    }

    private void WalkPointSetting()
    {
        walkPointSet = true;
        if (Monster)
        {
            MonsterAni("IsTraceTrue");
        }
    }
    
    /*
    private void RecognitionPlayer()
    {

        Vector3 pos = transform.position + Vector3.forward * 20f;
        if (Physics.CheckSphere(pos, eyeContactRange, whatIsPlayer))
        {
            Debug.Log("check");
            ani.SetBool("IsWalk", true);
            ChasePlayer();
        }

    }*/

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

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

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

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
            default:
                break;
        }
    }

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
            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position + Vector3.forward * 20f, eyeContactRange);
    }
}
