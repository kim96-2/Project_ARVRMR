using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 spinAxis = Vector3.up;
        float spinSpeed = 40f;
        rb.angularVelocity = spinAxis * spinSpeed;

        Vector3 forceDirection = new Vector3(-.75f, 0.5f, 10.0f);
        int forceMagnitude = 130;
        GetComponent<Rigidbody>().AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.angularVelocity.magnitude > 0.1f)
        {
            Vector3 sideForce = new Vector3(rb.angularVelocity.y * 0.25f, 0, 0);
            rb.AddForce(sideForce);
        }
    }
}
