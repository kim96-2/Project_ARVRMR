using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInteractionHandler : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    private bool isGrabbed = false;

    void Update()
    {
        if (isGrabbed)
        {
            ballRigidbody.isKinematic = true;
        }
        else
        {
            ballRigidbody.isKinematic = false;
        }
    }

    public void OnGrab()
    {
        isGrabbed = true;
    }

    public void OnRelease()
    {
        isGrabbed = false;
    }
}