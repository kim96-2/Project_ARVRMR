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
    public Action<ActivateEventArgs> shootAction;

    [SerializeField] Transform shootPos;
    [SerializeField] Transform shootDirPos;

    [Space(15f)]
    [SerializeField] float shootMaxDistance = 100f;
    [SerializeField] LayerMask shootMask;

    [Space(15f)]
    [SerializeField] GameObject hitParticle;

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

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPos.position, shootDirPos.position - shootPos.position, out hit, shootMaxDistance, shootMask))
        {
            Debug.DrawLine(shootPos.position, hit.point,Color.blue);
        }
        else
        {
            Debug.DrawRay(shootPos.position, (shootDirPos.position - shootPos.position) * 5f, Color.red);
        }
            
    }

    public void Shoot(ActivateEventArgs args)
    {
        //TO DO: 총알 나가는 기능 구현
        RaycastHit hit;
        if(Physics.Raycast(shootPos.position,shootDirPos.position - shootPos.position, out hit, shootMaxDistance, shootMask))
        {
            //Debug.Log(hit.collider.gameObject.name);

            //총알 hit effect 생성
            Instantiate(hitParticle, hit.point - (shootDirPos.position - shootPos.position).normalized * 0.1f, hitParticle.transform.rotation);

            ShootingTarget target;
            if(hit.collider.gameObject.TryGetComponent<ShootingTarget>(out target))
            {
                target.Hit(hit.point);
            }
        }

        //사격 액션 실행
        shootAction?.Invoke(args);
    }
}
