using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractive : MonoBehaviour
{
    public PlayerController controller;

    PlayerInput input;

    public float useGageCount;

    float ScreenXC;
    float ScreenYC;
    Vector3 CenterVector;

    public float sightDistance = 0.5f;
    public InteractiveAction target;

    Action UseGageCal;

    Ray ray;

    private void Awake()
    {
        CenterizedVector();
        input = new PlayerInput();
        UseGageCal = () => { };
    }

    private void OnEnable()
    {
        input.Interactive.Enable();
        input.Interactive.Interacte.performed += UseAction;
        input.Interactive.Interacte.canceled += StopAction;
    }


    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(CenterVector);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, sightDistance))
        {
            if (hit.collider.gameObject.CompareTag("Interactive"))
            {
                target = hit.collider.gameObject.GetComponent<InteractiveAction>();
            }
            else
            {
                target = null;
            }
        }
        UseGageCal();
    }

    /// <summary>
    /// 상호작용 실행
    /// </summary>
    /// <param name="context"></param>
    private void UseAction(InputAction.CallbackContext context)
    {
        if (target != null)
        {
            GageCal(target.gageOfUsage, true);
        }
    }

    /// <summary>
    /// 상호작용 중도 취소
    /// </summary>
    /// <param name="context"></param>
    private void StopAction(InputAction.CallbackContext context)
    {
        if (target != null)
        {
            GageCal(0f, false);
        }
    }

    /// <summary>
    /// 화면 가운데 계산 함수
    /// </summary>
    void CenterizedVector()
    {
        ScreenXC = Screen.width * 0.5f;
        ScreenYC = Screen.height * 0.5f;
        CenterVector.x = ScreenXC;
        CenterVector.y = ScreenYC;
        CenterVector.z = 0f;
    }

    void GageCal(float GageParamiter, bool Active)
    {
        useGageCount = GageParamiter;
        if (useGageCount == 0 && Active)
        {
            target.IsThisAction = !target.IsThisAction;
        }
        else
        {
            if (Active)
            {
                UseGageCal += UsingState;
            }
            else
            {
                UseGageCal -= UsingState;
            }
        }
        void UsingState()
        {
            if (useGageCount > 0f)
            {
                useGageCount -= Time.deltaTime;
            }
            else
            {
                UseGageCal -= UsingState;
                target.IsThisAction = !target.IsThisAction;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * sightDistance);
    }
#endif
}
