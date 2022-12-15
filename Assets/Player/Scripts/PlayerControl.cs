using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region �Ѽ�
    public enum PlayerPosture {        
        Stand,
        MidAir
    }
    public enum LocomotionState
    {
        Idle,
        Walk,
        Run,
        JumpUp,
        Fall
    }
    [Header("��e���A")]
    public PlayerPosture Posture = PlayerPosture.Stand;
    public LocomotionState locomotionState = LocomotionState.Idle;


    public float moveSpeed;
    public float rotateSpeed;
    public float jumpHeight;
    public float fallMulti = 2.0f;
    [Header("�a���˴�")]
    public LayerMask groundLayer;
    public LayerMask InteractLayer;
    public float groundCheckOffset = 0.5f;

    public float interactCheckDistance = 1f;

    private float moveSpeedMulti;
    private float walkSpeed = 1.0f;
    private float runSpeed = 2.0f;
    //private float airmoveSpeed = 0.8f;

    [Header("���L�Ѽ�")]
    [SerializeField] private bool isGround;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isFall;
    private bool isJumpThisFrame = false;

    /// <summary>
    /// GetAxisRaw��J
    /// </summary>
    Vector3 moveAxis;
    /// <summary>
    /// ���۾���V������
    /// </summary>
    Vector3 cameraAxisMove;
    /// <summary>
    /// �u��t��
    /// </summary>
    Vector3 CharacterVelocity;
    /// <summary>
    /// �����t��
    /// </summary>
    float VerticalVelocity;

    //�Ť�����
    Vector3 average;

    //�ʵeHash��
    private int State_Hash;
    private int Speed_Hash;
    private int Direction_Hash;
    private int VerticalSpeed_Hash;

    private readonly float standThreshold = 0.0f;
    private readonly float midAirThreshold = 1.0f;

    private Collider[] colliders = new Collider[5];
    private float gravity;
    Transform cameraTrasform;
    PlayerInput playerInput;
    Animator animator;
    CharacterController characterController;
    #endregion

    #region Mono�禡(Start Update...)
    void Start()
    {
        cameraTrasform = Camera.main.transform;
        gravity = Physics.gravity.y;
       // ����ե�
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        
        // �ھڰ��f�վ�ʵe�t��
        animator.SetFloat("MoveFactor", 1 / animator.humanScale);
        
        // ��o���ƭ�
        State_Hash = Animator.StringToHash("State");
        Speed_Hash = Animator.StringToHash("Speed");
        Direction_Hash = Animator.StringToHash("Direction");
        VerticalSpeed_Hash = Animator.StringToHash("VerticalSpeed");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GameFacade.Instance.PushPanel(UIPanelType.PausePanel);
        if (Input.GetKeyDown(KeyCode.L))
            GameFacade.Instance.PushPanel(UIPanelType.LosePanel);
        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
            GameFacade.Instance.PushPanel(UIPanelType.LosePanel);
        }
        HandleParameter();
        CalculateGravity();
        Jump();
        Rotate();
        CalculateVeclocity();
        SwitchPlayerState();
        SetAnimator();
        CheckAroundInteract();
        
    }

    //private void OnAnimatorMove()
    //{
    //    if (Posture == PlayerPosture.Stand)
    //    {
    //        CharacterVelocity = animator.deltaPosition;
    //        CharacterVelocity.y = VerticalVelocity * Time.deltaTime;
    //        characterController.Move(CharacterVelocity);
    //        average = AverageVelocity(animator.velocity);
    //    }
    //    else if (Posture == PlayerPosture.MidAir)
    //    {
    //        average.y = VerticalVelocity;
    //        CharacterVelocity = average * Time.deltaTime;
    //        characterController.Move(CharacterVelocity);
    //    }
    //}
    #endregion

    /// <summary>
    /// �a���˴�
    /// </summary>
    public void GroundCheck()
    {
        
        if (Physics.SphereCast(transform.position + transform.up * groundCheckOffset
            , characterController.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - characterController.radius +  characterController.skinWidth
            , groundLayer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
    /// <summary>
    /// �����J �վ�Ѽ�
    /// </summary>
    public void HandleParameter()
    {
        //��V��
        moveAxis = playerInput.GetMove();

        cameraAxisMove = cameraTrasform.TransformDirection(moveAxis).normalized;
        cameraAxisMove.y = 0;
        cameraAxisMove = cameraAxisMove.normalized;

        // 
        isRun = playerInput.GetKeyHeldRun();
        isJumpThisFrame = playerInput.GetKeyDownJump();
        GroundCheck();
    }
    /// <summary>
    /// �p�⭫�O
    /// </summary>
    private void CalculateGravity()
    {
        if (isGround)
        {
            VerticalVelocity = gravity * Time.deltaTime;
        }
        else
        {
            VerticalVelocity += gravity * fallMulti * Time.deltaTime; 
        }
    }
    /// <summary>
    /// �P�_���D �ê���
    /// </summary>
    public void Jump()
    {
        if (isGround && playerInput.GetKeyDownJump())
        {
            VerticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
    }
    /// <summary>
    /// ���۾����e����V �i�����
    /// </summary>
    private void Rotate()
    {
        if (moveAxis == Vector3.zero) return;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraAxisMove), rotateSpeed);
    }
    /// <summary>
    /// �p��t��(Update)
    /// </summary>
    private void CalculateVeclocity()
    {
        if (Posture == PlayerPosture.Stand)
        {
            
            CharacterVelocity = cameraAxisMove * moveSpeed * moveSpeedMulti *  Time.deltaTime;
            CharacterVelocity.y = VerticalVelocity * Time.deltaTime;
            characterController.Move(CharacterVelocity);
            average = CharacterVelocity;
        }
        else if (Posture == PlayerPosture.MidAir)
        {
            CharacterVelocity = average;
            CharacterVelocity.y = VerticalVelocity * Time.deltaTime;
            characterController.Move(CharacterVelocity );
        }
    }
    /// <summary>
    /// �d�ݪ��񪺥i�椬����
    /// </summary>
    private void CheckAroundInteract()
    {
        if (playerInput.GetKeyDownInteract())
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position + characterController.center 
                , interactCheckDistance , colliders , InteractLayer,QueryTriggerInteraction.Ignore);
            if (count != 0)
            {
                for(int i = 0; i < count; i++)
                {
                    if (colliders[i].GetComponent<IInteract>() == null)
                        continue;
                    colliders[i].GetComponent<IInteract>().Interact();
                    Debug.Log("Interact");
                    break;
                }
            }
        }
    }
    /// <summary>
    /// ������e���A
    /// </summary>
    public void SwitchPlayerState()
    {
        if(isGround){
            Posture = PlayerPosture.Stand;
            if (moveAxis.sqrMagnitude == 0)
            {
                moveSpeedMulti = 0;
                locomotionState = LocomotionState.Idle;
            }
            else if (!isRun)
            {
                moveSpeedMulti = walkSpeed;
                locomotionState = LocomotionState.Walk;
            }
            else
            {
                moveSpeedMulti = runSpeed;
                locomotionState = LocomotionState.Run;
            }
        }
        else{
            Posture = PlayerPosture.MidAir;
        }
        
    }
    /// <summary>
    /// �]�m���A���Ѽ�
    /// </summary>
    public void SetAnimator()
    {
        if (Posture == PlayerPosture.Stand)
        {
            animator.SetFloat(State_Hash, standThreshold);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(Speed_Hash, 0.0f, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(Speed_Hash,  walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(Speed_Hash,  runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (Posture == PlayerPosture.MidAir)
        {
            animator.SetFloat(State_Hash, midAirThreshold);
            animator.SetFloat(VerticalSpeed_Hash, VerticalVelocity, 0.1f, Time.deltaTime);
        }
        animator.SetFloat(Direction_Hash , moveAxis.x );
    }


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IInteract>()?.Interact();
    }

    public bool Ground()
    {
        return isGround;
    }
}
