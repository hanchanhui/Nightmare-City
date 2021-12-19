using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    Camera _camera;
    CharacterController _controller;
    //public float runspeed = 8f;
    //public float finalspeed;
    //public bool run;

    public float smoothness = 10f;

    //------------------------------------------------
    //private Rigidbody playerRigidbody;
    private float stop = 0f;
    public float speed = 10f;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    //����.
    private bool isWalk = true;
    private bool isRun = false;
    //private bool isGround = true;

    private Vector3 lastPos;
    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    Animator animator;

    [SerializeField]
    private AttackController theAttackController;
    private Crosshair thecrosshair; // ������.

    //------------------------------------------------���� 12.18��
    GameObject nearObject; // ������ ����
    GameObject equipGun; // ���� ������ ���� ���� ����
    public GameObject[] Gun; // ���� �������� ����
    public bool[] hasGun; // ���� ������ ��/��

    //-----------------------------------------------���� �÷��̾� ä��
    public int hp = 150;

    //-------------------------
    private AudioSource audioSource2;
    public AudioClip warking;
    public AudioClip ruuning;
    public AudioClip emp;

    private bool test = true;
    private bool test2 = true;
    // ======================================================����
    public UI_HpBar hpBar;
    Rigidbody rigid;
    public bool isDie = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //playerRigidbody = GetComponent<Rigidbody>();
        _controller = this.GetComponent<CharacterController>();

        applySpeed = speed;

        _camera = Camera.main;

        theAttackController = FindObjectOfType<AttackController>();
        thecrosshair = FindObjectOfType<Crosshair>(); // ������.

        rigid = GetComponent<Rigidbody>();

        audioSource2 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TryRun();
        PlayerMove(); 
        Swap(); // ���� ��ü�Ǵ� ��
        //MoveCheck();

        if(hp <= 0)
        {
            applySpeed = stop;
        }
    }
    private void LateUpdate()
    {
        // ī�޶� �κ�.
        Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);

    }

    //�޸��⸦ �ϴ���, ����ϴ��� ����
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isDie)
        {
            Running();
            if (test2)
            {
                PlaySE2(ruuning);
                test2 = false;
            }
            animator.SetBool("Run", isRun);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isDie)
        {
            RunningCancel();
            animator.SetBool("Run", isRun);
            test2 = true;
            PlaySE2(emp);
        }
    }

    //�޸���
    private void Running()
    {
        theAttackController.CancelFineSight(); // �����ؽ� �۶� �������� �����ǰԸ���

        isRun = true;
        thecrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        thecrosshair.RunningAnimation(isRun);
        applySpeed = speed;
    }

    private void PlayerMove()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * _moveDirZ + right * _moveDirX;

        _controller.Move(moveDirection.normalized * applySpeed * Time.deltaTime);

        if (!isRun)
        {
            if (theAttackController.isfineSightMode)
            {
                applySpeed = stop;
            }
            else
            {
                applySpeed = speed;
                if (_moveDirZ > 0.1f)
                {
                    animator.SetBool("Up", true);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test) 
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirZ <= -0.1f)
                {
                    
                    animator.SetBool("Up", true);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirX >= 0.1f)
                {
                    
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", true);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else if (_moveDirX <= -0.1f)
                {
                    
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", true);
                    animator.SetBool("Right", false);
                    isWalk = true;
                    if (test)
                    {
                        PlaySE2(warking);
                        test = false;
                    }
                }
                else
                {
                    animator.SetBool("Up", false);
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    isWalk = false;
                    test = true;
                    PlaySE2(emp);
                }
                thecrosshair.WalkingAnimation(isWalk);
            }
        }
    }
    

    // �տ� �� �� �ٲ��ִ� �� (Update()���� ���)
    void Swap()
    {
        if(hasGun[1])
        {
            equipGun = Gun[0]; // ������ ����.
            equipGun.SetActive(false); // ���� ����ִ� �� ����
            equipGun = Gun[1]; // 1�� �迭 ���� �ް�
            Gun[1].SetActive(true); // ���� ������ ��ü
        }
    }

    // Item ��ũ��Ʈ�� �� �޾ƿ��� �迭�� �����ϰ� ����
    public GameObject weaponImg1;
    public GameObject weaponImg2;
    static public int key = 0;
    public Text currentKey;
    void Interation()
    {
        if (nearObject != null)
        {
            if(nearObject.tag == "MachineGun")
            {
                Item item = nearObject.GetComponent<Item>();
                int GunIndex = item.value; // ���° �迭 ���������� �� �ް�
                hasGun[GunIndex] = true; // �� ���° ������ �Ծ��ٰ� Ȯ��

                weaponImg1.SetActive(false);
                weaponImg2.SetActive(true);

                Destroy(nearObject);// ����
            }
            else if (nearObject.tag == "Key")
            {
                key++;
                currentKey.text = key.ToString();
                Destroy(nearObject);
                if (key == 2 || key == 5)
                {
                    createPortal();
                }
            }
            else if (nearObject.tag == "Portal")
            {
                SceneManager.LoadScene("2stage");
            }
        }
    }

    public GameObject door;
    public GameObject portal;
    void createPortal()
    {
        door.SetActive(false);
        portal.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MachineGun")
        {
            nearObject = other.gameObject;
            Interation();
        }
        else if(other.tag == "Key")
        {
            nearObject = other.gameObject;
            Interation();
        }
        else if (other.tag == "Portal")
        {
            nearObject = other.gameObject;
            Interation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MachineGun")
        {
            nearObject = null;
        }
    }

    private void PlaySE2(AudioClip _clip)
    {
        audioSource2.clip = _clip;
        audioSource2.Play();
    }

    public UI_Died uiDied;
    public GameObject dieCamera;
    public void GetDamage(int damage) // �÷��̾� ����.
    {
        hp -= damage;
        hpBar.SetHP(hp);
        if (hp <= 0)
        {
            animator.SetTrigger("Die");
            dieCamera.GetComponent<CameraMove>().enabled = false; // �׾��� �� ī�޶� �������� �ʰ� �Ϸ��� �̸� ��������
            //rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            isDie = true;
            StartCoroutine(diePlayer());
            uiDied.playerDie();
        }
    }

    public IEnumerator diePlayer()
    {
        yield return new WaitForSeconds(1f);
        animator.enabled = false;
    }
}