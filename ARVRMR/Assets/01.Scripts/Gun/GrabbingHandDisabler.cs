using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// ��� ������Ʈ ��ȣ�ۿ� �� �� �������� �ϴ� ������Ʈ
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
            Debug.LogError("Grab Interacter�� �������� ����");
            return;
        }

        //�̺�Ʈ �߰�
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
            Debug.Log("�� ��Ȱ��ȭ");
        }

        
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        ActionBasedController controller;

        args.interactorObject.transform.TryGetComponent<ActionBasedController>(out controller);

        if (controller != null)
        {
            controller.model.gameObject.SetActive(true);
            Debug.Log("�� Ȱ��ȭ");
        }
    }
}
