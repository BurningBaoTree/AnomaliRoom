using System;
using UnityEngine;

/// <summary>
/// 플레이어 이동만 관리하는 코드
/// </summary>
public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 플레이어 컨트롤러
    /// </summary>
    public PlayerController controller;
    /// <summary>
    /// 플레이어 상태 열거
    /// </summary>
    public enum PlayerState
    {
        Idel = 0,
        walk,
        run,
        jump
    }

    /// <summary>
    /// 커서락 상태용 bool 변수
    /// </summary>
    public bool cursorLock = true;

    /// <summary>
    /// 속도
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// 시야 흔들림용 변수
    /// </summary>
    float updonwspeed = 0f;

    /// <summary>
    /// 시야 흔들림 수치
    /// </summary>
    public float updonwAdd = 10f;

    /// <summary>
    /// 시야 흔들림 속도
    /// </summary>
    public float updonwImpulse = 0.01f;

    /// <summary>
    /// 시야 회전 속도
    /// </summary>
    public float headrotationSpeed = 20.0f;

    float xis = 0;
    float yis = 0;

    float xxis;
    float yxis;
    int ani;

    /// <summary>
    /// 카메라 트랜스폼
    /// </summary>
    public Transform Cameratransform;

    /// <summary>
    /// 업다운용 카메라 트랜스폼
    /// </summary>
    public Transform CamUpdownTransform;

    /// <summary>
    /// 물리연산 변수
    /// </summary>
    Rigidbody rig;

    /// <summary>
    /// 플레이어 인풋시스템 변수
    /// </summary>
    PlayerInput playerinput;

    Vector3 posi;
    Vector2 MoveDir;
    Vector3 defoltTransform;

    /*    public Animator animator;*/

    /// <summary>
    /// 업데이트용 델리게이트
    /// </summary>
    Action checker;

    /// <summary>
    /// 상태를 내보내는 델리게이트
    /// </summary>
    public Action<string> stateChange;



    #region 프로퍼티
    /// <summary>
    /// 현재 플레이어 상태
    /// </summary>
    string statename;
    /// <summary>
    /// 현재 플레이어 상태 프로퍼티
    /// </summary>
    public string StateName
    {
        get
        {
            return statename;
        }
        set
        {
            statename = value;
            stateChange?.Invoke(statename);
        }
    }

    /// <summary>
    /// 플레이어 상태 변수
    /// </summary>
    public PlayerState playerstate = 0;
    /// <summary>
    /// 플레이어 상태 프로퍼티
    /// </summary>
    public PlayerState Playerstate
    {
        get
        {
            return playerstate;
        }
        set
        {
            switch (playerstate)
            {
                case PlayerState.Idel:

                    break;
                case PlayerState.walk:

                    break;
                case PlayerState.run:

                    break;
                case PlayerState.jump:

                    break;
                default:
                    break;
            }
            playerstate = value;
            switch (playerstate)
            {
                case PlayerState.Idel:
                    StateName = "일반";
                    break;
                case PlayerState.walk:
                    StateName = "걷기";
                    break;
                case PlayerState.run:
                    StateName = "달리기";
                    break;
                case PlayerState.jump:
                    StateName = "점프";
                    break;
                default:
                    playerstate = 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 업다운 변수
    /// </summary>
    bool updowncheck = false;
    /// <summary>
    /// 걸을때 시야가 위아래로 왔다갔다하는 프로퍼티
    /// </summary>
    bool UpdownCheck
    {
        get
        {
            return updowncheck;
        }
        set
        {
            if (updowncheck != value)
            {
                updowncheck = value;
                if (updowncheck)
                {
                    checker += updowncam;
                }
                else
                {
                    checker -= updowncam;
                    slowlycomback(CamUpdownTransform.localPosition, defoltTransform, 10);
                }
            }
        }
    }

    bool walkActive = false;
    /// <summary>
    /// 걷기 프로퍼티
    /// </summary>
    bool WalkActive
    {
        get
        {
            return walkActive;
        }
        set
        {
            if (walkActive != value)
            {
                walkActive = value;
                if (walkActive)
                {
                    UpdownCheck = true;
                    checker += WalkAction;
                }
                else
                {
                    UpdownCheck = false;
                    checker -= WalkAction;
                }
            }
        }
    }

    /// <summary>
    /// 플레이어가 공중에 있는지 체크
    /// </summary>
    bool inAir = false;

    /// <summary>
    /// 플레이어가 점프중인지 체크
    /// </summary>
    bool jumpcheck = false;
    /// <summary>
    /// 점프 프로퍼티
    /// </summary>
    bool JumpCheck
    {
        get
        {
            return jumpcheck;
        }
        set
        {
            if (jumpcheck != value)
            {
                jumpcheck = value;
                if (jumpcheck)
                {
                    if (!inAir)
                    {
                        rig.AddForce(Vector3.up * 5, ForceMode.Impulse);
                    }
                    UpdownCheck = false;
                }
                else if (RunCheck)
                {
                    UpdownCheck = true;
                }
                else if (WalkActive)
                {
                    UpdownCheck = true;
                }
                else
                {
                    UpdownCheck = false;
                }
            }
        }
    }

    /// <summary>
    /// 플레이어가 달리는 중인지 체크
    /// </summary>
    bool runcheck = false;
    /// <summary>
    /// 플레이어 달리는 중인지 확인하는 프로퍼티
    /// </summary>
    bool RunCheck
    {
        get
        {
            return runcheck;
        }
        set
        {
            if (runcheck != value)
            {
                runcheck = value;
            }
        }
    }

    #endregion

    private void Awake()
    {
        defoltTransform = CamUpdownTransform.localPosition;
        rig = GetComponent<Rigidbody>();
        playerinput = new PlayerInput();
        xxis = 0;
        yxis = 0;
        /*        animator = GetComponent<Animator>();*/
        checker += GroundCheck;
        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
    #region OnEnable & OnDisable
    private void OnEnable()
    {
        playerinput.Move.Enable();
        playerinput.Move.Head.performed += HeadBanging;
        playerinput.Move.WASD.performed += MoveAction;
        playerinput.Move.WASD.canceled += MoveAction;
        playerinput.Move.Dash.performed += RunActive;
        playerinput.Move.Dash.canceled += RunDeActive;
        playerinput.Move.Jump.performed += JumpAction;
    }

    private void Start()
    {
        Playerstate = PlayerState.Idel;
    }

    private void OnDisable()
    {
        playerinput.Move.Jump.performed -= JumpAction;
        playerinput.Move.Dash.canceled -= RunDeActive;
        playerinput.Move.Dash.performed -= RunActive;
        playerinput.Move.WASD.canceled -= MoveAction;
        playerinput.Move.WASD.performed -= MoveAction;
        playerinput.Move.Head.performed -= HeadBanging;
        playerinput.Move.Disable();
    }
    #endregion

    private void FixedUpdate()
    {
        //초과회전 방지
        xxis = ClampAngleY(xis, -90, 90);
        yxis = ClampAngle(yis, float.MinValue, float.MaxValue);

        //Y를 축으로 돌아가는 몸통
        this.transform.rotation = Quaternion.Euler(0, yxis, 0);

        //시야 회전
        Cameratransform.rotation = Quaternion.Euler(xxis, yxis, 0);

        //업데이트 델리게이트
        checker();
    }

    /// <summary>
    /// 시야 회전 함수
    /// </summary>
    /// <param name="context">델타값</param>
    private void HeadBanging(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 pos = Vector3.zero;

        pos = context.ReadValue<Vector2>();
        float multiply = headrotationSpeed * Time.deltaTime;
        xis += multiply * (-pos.y);
        yis += multiply * (pos.x);
        if (xis > 90)
        {
            xis = 90;
        }
        if (xis < -90)
        {
            xis = -90;
        }
    }

    /// <summary>
    /// 점프 활성화
    /// </summary>
    /// <param name="context"></param>
    private void JumpAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        JumpCheck = true;
    }

    /// <summary>
    /// 이동 활성화
    /// </summary>
    /// <param name="context"></param>
    private void MoveAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        WalkActive = true;
        Vector2 move = context.ReadValue<Vector2>();
        MoveDir = move;
        ani = (int)MoveDir.y;
        posi.x = MoveDir.x;
        posi.z = MoveDir.y;
        if (MoveDir.sqrMagnitude < 0.8f)
        {
            WalkActive = false;
        }
    }

    /// <summary>
    /// 땅이 있는지 없는지 체크하는 함수
    /// </summary>
    void GroundCheck()
    {
        if (rig.velocity.y < 0.5f && rig.velocity.y > -0.5f)
        {
            inAir = false;
            JumpCheck = false;
            if (walkActive)
            {
                Playerstate = PlayerState.walk;
            }
            else if (runcheck)
            {
                Playerstate = PlayerState.run;
            }
            else
            {
                Playerstate = PlayerState.Idel;
            }
        }
        else
        {
            inAir = true;
            UpdownCheck = false;
            Playerstate = PlayerState.jump;
        }
    }

    /// <summary>
    /// 걷기 활성화
    /// </summary>
    void WalkAction()
    {
        //이동 
        transform.Translate(speed * Time.fixedDeltaTime * posi);
    }

    /// <summary>
    /// 카메라 원위치
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="headto"></param>
    /// <param name="speed"></param>
    void slowlycomback(Vector3 pos, Vector3 headto, float speed)
    {
        Vector3 distance;
        void comback()
        {
            distance = pos - headto;
            CamUpdownTransform.localPosition = Vector3.MoveTowards(pos, headto, speed * Time.fixedDeltaTime);
            if (distance.sqrMagnitude < 0.1f)
            {
                checker -= comback;
            }
        }
        checker += comback;
    }

    /// <summary>
    /// 달리기 시작
    /// </summary>
    /// <param name="context"></param>
    private void RunActive(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        RunCheck = true;
        speed *= 2;
        ani = (int)(ani * 2.1f);
        updonwAdd *= 2;
        updonwImpulse *= 4;
    }

    /// <summary>
    /// 달리기 초기화
    /// </summary>
    /// <param name="context"></param>
    private void RunDeActive(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        RunCheck = false;
        speed *= 0.5f;
        ani = (int)(ani * 0.5f);
        updonwAdd *= 0.5f;
        updonwImpulse *= 0.25f;

    }

    /// <summary>
    /// 이동시 카메라를 위 아래로 흔듭니다.
    /// </summary>
    void updowncam()
    {
        updonwspeed = Mathf.Cos(Time.time * updonwAdd);
        CamUpdownTransform.Translate(Vector3.up * updonwspeed * updonwImpulse, Space.Self);
    }

    /// <summary>
    /// 360 초과회전 방지 함수 X축
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        //ifAngle값이 360이라는 숫자를 넘지 안도록 막아주는 조건문
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// 360 초과회전 방지 함수Y축
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngleY(float lfAngle, float lfMin, float lfMax)
    {
        //ifAngle값이 360이라는 숫자를 넘지 안도록 막아주는 조건문
        if (lfAngle < lfMin) lfAngle = lfMin;
        if (lfAngle > lfMax) lfAngle = lfMax;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// null 방지
    /// </summary>
    void WWN()
    {

    }
}
