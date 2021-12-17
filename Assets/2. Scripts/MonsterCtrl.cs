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
    // Move On
    private void WalkPointSetting()
    {
       walkPointSet = true;
       MonsterAni("IsTraceTrue");  
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

    // Player Followed
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    // Player Attect
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void MonAttack()
    {
        //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
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

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // Zombi Animation
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
            default:
                break;
        }
    }
    // Player Check Range 
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
