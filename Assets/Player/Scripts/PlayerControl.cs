using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region 參數
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
    [Header("當前狀態")]
    public PlayerPosture Posture = PlayerPosture.Stand;
    public LocomotionState locomotionState = LocomotionState.Idle;


    public float moveSpeed;
    public float rotateSpeed;
    public float jumpHeight;
    public float fallMulti = 2.0f;
    [Header("地面檢測")]
    public LayerMask groundLayer;
    public LayerMask InteractLayer;
    public float groundCheckOffset = 0.5f;

    public float interactCheckDistance = 1f;

    private float moveSpeedMulti;
    private float walkSpeed = 1.0f;
    private float runSpeed = 2.0f;
    //private float airmoveSpeed = 0.8f;

    [Header("布林參數")]
    [SerializeField] private bool isGround;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isFall;
    private bool isJumpThisFrame = false;

    /// <summary>
    /// GetAxisRaw輸入
    /// </summary>
    Vector3 moveAxis;
    /// <summary>
    /// 對於相機方向的移動
    /// </summary>
    Vector3 cameraAxisMove;
    /// <summary>
    /// 真實速度
    /// </summary>
    Vector3 CharacterVelocity;
    /// <summary>
    /// 垂直速度
    /// </summary>
    float VerticalVelocity;

    //空中控制
    Vector3 average;

    //動畫Hash值
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

    #region Mono函式(Start Update...)
    void Start()
    {
        cameraTrasform = Camera.main.transform;
        gravity = Physics.gravity.y;
       // 獲取組件
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        
        // 根據骨骼調整動畫速度
        animator.SetFloat("MoveFactor", 1 / animator.humanScale);
        
        // 獲得哈希值
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
    /// 地面檢測
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
    /// 控制輸入 調整參數
    /// </summary>
    public void HandleParameter()
    {
        //方向類
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
    /// 計算重力
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
    /// 判斷跳躍 並附值
    /// </summary>
    public void Jump()
    {
        if (isGround && playerInput.GetKeyDownJump())
        {
            VerticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
    }
    /// <summary>
    /// 往相機正前的方向 進行旋轉
    /// </summary>
    private void Rotate()
    {
        if (moveAxis == Vector3.zero) return;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraAxisMove), rotateSpeed);
    }
    /// <summary>
    /// 計算速度(Update)
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
    /// 查看附近的可交互物件
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
    /// 切換當前狀態
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
    /// 設置狀態機參數
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
