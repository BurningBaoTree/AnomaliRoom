using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAction : MonoBehaviour
{
    public bool isThisAction = false;
    public float gageOfUsage;

    public Action<bool> InteractiveUse;

    /// <summary>
    /// 상호작용 버튼형 작동
    /// </summary>
    public bool IsThisAction
    {
        get => isThisAction;
        set
        {
            if (IsThisAction != value)
            {
                isThisAction = value;
                InteractiveUse?.Invoke(isThisAction);
            }
        }
    }
}
