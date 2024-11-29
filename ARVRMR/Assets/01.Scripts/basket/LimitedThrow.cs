using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class LimitedThrow : MonoBehaviour
{
    public int maxThrows = 5; // 최대 던질 수 있는 횟수
    private int currentThrows = 0; // 현재 던진 횟수
    public TextMeshProUGUI throwCountText; // UI 텍스트 참조

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
        UpdateThrowCountText(); // 초기 텍스트 설정
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (currentThrows < maxThrows)
        {
            currentThrows++;
            UpdateThrowCountText();
            Debug.Log("Throw Count: " + currentThrows);
        }

        if (currentThrows >= maxThrows)
        {
            Debug.Log("Finished");
            if (throwCountText != null)
            {
                throwCountText.text = "Finished";
            }
            grabInteractable.enabled = false; // 더 이상 던질 수 없게 처리
        }
    }

    private void UpdateThrowCountText()
    {
        if (throwCountText != null)
        {
            throwCountText.text = "Throws Left: " + (maxThrows - currentThrows);
        }
    }
}