using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float speed = 10f;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    //상태.
    private bool isWalk = true;
    private bool isRun = false;
    //private bool isGround = true;

    private Vector3 lastPos;
    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    Animator animator;

    [SerializeField]
    private AttackController theAttackController;
    private Crosshair thecrosshair; // 조준점.


    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //playerRigidbody = GetComponent<Rigidbody>();
        _controller = this.GetComponent<CharacterController>();

        applySpeed = speed;

        _camera = Camera.main;

        theAttackController = FindObjectOfType<AttackController>();
        thecrosshair = FindObjectOfType<Crosshair>(); // 조준점.

    }

    // Update is called once per frame
    void Update()
    {
        TryRun();
        PlayerMove();
        //MoveCheck();
    }
    private void LateUpdate()
    {
        // 카메라 부분.
        Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);

    }

    //달리기를 하는지, 취소하는지 결정
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
            animator.SetBool("Run", isRun);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
            animator.SetBool("Run", isRun);

        }
    }

    //달리기
    private void Running()
    {
        theAttackController.CancelFineSight(); // 정조준시 뛸때 정조준이 해제되게만듦

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
            if (_moveDirZ > 0.1f)
            {
                animator.SetBool("Up", true);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                isWalk = true;
            }
            else if (_moveDirZ <= -0.1f)
            {
                animator.SetBool("Up", true);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                isWalk = true;
            }
            else if (_moveDirX >= 0.1f)
            {
                animator.SetBool("Up", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", true);
                isWalk = true;
            }
            else if (_moveDirX <= -0.1f)
            {
                animator.SetBool("Up", false);
                animator.SetBool("Left", true);
                animator.SetBool("Right", false);
                isWalk = true;
            }
            else
            {
                animator.SetBool("Up", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                isWalk = false;
            }
            thecrosshair.WalkingAnimation(isWalk);
        }


        //Vector3 _moveHorizontal = transform.right * _moveDirX;
        //Vector3 _moveVertical = transform.forward * _moveDirZ;

        //Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //playerRigidbody.MovePosition(moveDirection.normalized + moveDirection * Time.deltaTime);


    }

    //private void MoveCheck()
    //{
    //    if (!isRun && isGround)
    //    {
    //        if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
    //        {
    //            Debug.Log(Vector3.Distance(lastPos, transform.position));
    //            animator.SetBool("Up", true);
    //            //isWalk = true;
    //        }
    //        else
    //        {
    //            animator.SetBool("Up", false);
    //            //isWalk = false;
    //        }
    //        //thecrosshair.WalkingAnimation(isWalk);
    //        lastPos = transform.position;
    //    }
    //}

    //void InputMovement()
    //{

    //    float _dirX = Input.GetAxisRaw("Horizontal");
    //    float _dirZ = Input.GetAxisRaw("Vertical");

    //    Vector3 direction = new Vector3(_dirX, 0f, _dirZ);
    //    isMove = false;
    //    Debug.Log(direction.y);


    //    if (direction != Vector3.zero)
    //    {
    //        isMove = true;
    //        this.transform.Translate(direction.normalized * speed * Time.deltaTime);
    //    }

    //    anim.SetBool("Move", isMove);
    //    anim.SetFloat("DirX", direction.x);
    //    anim.SetFloat("DirZ", direction.z);
    //}


}
