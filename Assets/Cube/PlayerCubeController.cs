using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeController : MonoBehaviour
{
    Rigidbody rigidbody;
    
    Vector3 lastPos = new Vector3(0,0,;
    Vector3 nextPos;
    float speed = 10.0f;
    float height = 3f;


    void Start()
    {
        rigidbody = GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
    }

    void FixedUpdate()
    {

        // if (Input.GetKeyDown("space"))
        // {
        //     rigidbody.AddForce(Vector3.up * 300);
        //     rigidbody.AddTorque(Vector3.back * 10);
        // }
    }
}
