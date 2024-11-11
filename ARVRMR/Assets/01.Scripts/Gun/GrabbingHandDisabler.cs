using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 잡는 오브젝트 상호작용 시 손 없어지게 하는 컴포넌트
/// </summary>
public class GrabbingHandDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable;
        TryGetComponent<XRGrabInteractable>(out grabInteractable);

        if(grabInteractable == null)
        {
            Debug.LogError("Grab Interacter가 존재하지 않음");
            return;
        }

        //이벤트 추가
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);

    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        ActionBasedController controller;

        args.interactorObject.transform.TryGetComponent<ActionBasedController>(out controller);

        if(controller != null)
        {
            controller.model.gameObject.SetActive(false);
            Debug.Log("손 비활성화");
        }

        
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        ActionBasedController controller;

        args.interactorObject.transform.TryGetComponent<ActionBasedController>(out controller);

        if (controller != null)
        {
            controller.model.gameObject.SetActive(true);
            Debug.Log("손 활성화");
        }
    }
}
