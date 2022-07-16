using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    Transform selfTransform;
    Transform followTransform;
    float distanceX;

    void Start()
    {
        selfTransform = GetComponent<Transform>();
        followTransform = objectToFollow.GetComponent<Transform>();
        distanceX = followTransform.position.x - selfTransform.position.x;
    }

    void Update()
    {
        selfTransform.position = new Vector3(followTransform.position.x + distanceX, selfTransform.position.y, selfTransform.position.z);
    }
}
