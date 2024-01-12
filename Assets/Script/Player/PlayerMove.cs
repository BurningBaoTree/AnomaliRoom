using System;
using UnityEngine;

/// <summary>
/// �÷��̾� �̵��� �����ϴ� �ڵ�
/// </summary>
public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ�
    /// </summary>
    public PlayerController controller;
    /// <summary>
    /// �÷��̾� ���� ����
    /// </summary>
    public enum PlayerState
    {
        Idel = 0,
        walk,
        run,
        jump
    }

    /// <summary>
    /// Ŀ���� ���¿� bool ����
    /// </summary>
    public bool cursorLock = true;

    /// <summary>
    /// �ӵ�
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// �þ� ��鸲�� ����
    /// </summary>
    float updonwspeed = 0f;

    /// <summary>
    /// �þ� ��鸲 ��ġ
    /// </summary>
    public float updonwAdd = 10f;

    /// <summary>
    /// �þ� ��鸲 �ӵ�
    /// </summary>
    public float updonwImpulse = 0.01f;

    /// <summary>
    /// �þ� ȸ�� �ӵ�
    /// </summary>
    public float headrotationSpeed = 20.0f;

    float xis = 0;
    float yis = 0;

    float xxis;
    float yxis;
    int ani;

    /// <summary>
    /// ī�޶� Ʈ������
    /// </summary>
    public Transform Cameratransform;

    /// <summary>
    /// ���ٿ�� ī�޶� Ʈ������
    /// </summary>
    public Transform CamUpdownTransform;

    /// <summary>
    /// �������� ����
    /// </summary>
    Rigidbody rig;

    /// <summary>
    /// �÷��̾� ��ǲ�ý��� ����
    /// </summary>
    PlayerInput playerinput;

    Vector3 posi;
    Vector2 MoveDir;
    Vector3 defoltTransform;

    /*    public Animator animator;*/

    /// <summary>
    /// ������Ʈ�� ��������Ʈ
    /// </summary>
    Action checker;

    /// <summary>
    /// ���¸� �������� ��������Ʈ
    /// </summary>
    public Action<string> stateChange;



    #region ������Ƽ
    /// <summary>
    /// ���� �÷��̾� ����
    /// </summary>
    string statename;
    /// <summary>
    /// ���� �÷��̾� ���� ������Ƽ
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
    /// �÷��̾� ���� ����
    /// </summary>
    public PlayerState playerstate = 0;
    /// <summary>
    /// �÷��̾� ���� ������Ƽ
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
                    StateName = "�Ϲ�";
                    break;
                case PlayerState.walk:
                    StateName = "�ȱ�";
                    break;
                case PlayerState.run:
                    StateName = "�޸���";
                    break;
                case PlayerState.jump:
                    StateName = "����";
                    break;
                default:
                    playerstate = 0;
                    break;
            }
        }
    }

    /// <summary>
    /// ���ٿ� ����
    /// </summary>
    bool updowncheck = false;
    /// <summary>
    /// ������ �þ߰� ���Ʒ��� �Դٰ����ϴ� ������Ƽ
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
    /// �ȱ� ������Ƽ
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
    /// �÷��̾ ���߿� �ִ��� üũ
    /// </summary>
    bool inAir = false;

    /// <summary>
    /// �÷��̾ ���������� üũ
    /// </summary>
    bool jumpcheck = false;
    /// <summary>
    /// ���� ������Ƽ
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
    /// �÷��̾ �޸��� ������ üũ
    /// </summary>
    bool runcheck = false;
    /// <summary>
    /// �÷��̾� �޸��� ������ Ȯ���ϴ� ������Ƽ
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
        //�ʰ�ȸ�� ����
        xxis = ClampAngleY(xis, -90, 90);
        yxis = ClampAngle(yis, float.MinValue, float.MaxValue);

        //Y�� ������ ���ư��� ����
        this.transform.rotation = Quaternion.Euler(0, yxis, 0);

        //�þ� ȸ��
        Cameratransform.rotation = Quaternion.Euler(xxis, yxis, 0);

        //������Ʈ ��������Ʈ
        checker();
    }

    /// <summary>
    /// �þ� ȸ�� �Լ�
    /// </summary>
    /// <param name="context">��Ÿ��</param>
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
    /// ���� Ȱ��ȭ
    /// </summary>
    /// <param name="context"></param>
    private void JumpAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        JumpCheck = true;
    }

    /// <summary>
    /// �̵� Ȱ��ȭ
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
    /// ���� �ִ��� ������ üũ�ϴ� �Լ�
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
    /// �ȱ� Ȱ��ȭ
    /// </summary>
    void WalkAction()
    {
        //�̵� 
        transform.Translate(speed * Time.fixedDeltaTime * posi);
    }

    /// <summary>
    /// ī�޶� ����ġ
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
    /// �޸��� ����
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
    /// �޸��� �ʱ�ȭ
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
    /// �̵��� ī�޶� �� �Ʒ��� ���ϴ�.
    /// </summary>
    void updowncam()
    {
        updonwspeed = Mathf.Cos(Time.time * updonwAdd);
        CamUpdownTransform.Translate(Vector3.up * updonwspeed * updonwImpulse, Space.Self);
    }

    /// <summary>
    /// 360 �ʰ�ȸ�� ���� �Լ� X��
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        //ifAngle���� 360�̶�� ���ڸ� ���� �ȵ��� �����ִ� ���ǹ�
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// 360 �ʰ�ȸ�� ���� �Լ�Y��
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngleY(float lfAngle, float lfMin, float lfMax)
    {
        //ifAngle���� 360�̶�� ���ڸ� ���� �ȵ��� �����ִ� ���ǹ�
        if (lfAngle < lfMin) lfAngle = lfMin;
        if (lfAngle > lfMax) lfAngle = lfMax;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// null ����
    /// </summary>
    void WWN()
    {

    }
}
