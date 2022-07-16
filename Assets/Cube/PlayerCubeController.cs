using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeController : MonoBehaviour
{
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            rigidbody.AddForce(Vector3.up * 300);
            rigidbody.AddTorque(Vector3.back * 10);
        }
    }
}
