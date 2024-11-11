using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 총을 쏘게 하는 컴포넌트
/// </summary>
public class GunShooter : MonoBehaviour
{
    public Action shootAction;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable;
        TryGetComponent<XRGrabInteractable>(out grabInteractable);

        if (grabInteractable == null)
        {
            Debug.LogError("Grab Interacter가 존재하지 않음");
            return;
        }

        grabInteractable.activated.AddListener(Shoot);

    }

    public void Shoot(ActivateEventArgs args)
    {
        //TO DO: 총알 나가는 기능 구현

        //사격 액션 실행
        shootAction?.Invoke();
    }
}
