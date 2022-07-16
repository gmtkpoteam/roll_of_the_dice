using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    Transform followTransform;
    float distanceX;

    void Start()
    {
        followTransform = objectToFollow.GetComponent<Transform>();
        distanceX = followTransform.position.x - transform.position.x;
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, followTransform.position.x - distanceX, Time.deltaTime * 2),
            transform.position.y,
            transform.position.z);
    }
}
