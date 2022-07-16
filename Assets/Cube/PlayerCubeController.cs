using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status
{
    FLY,
    STAY,
    COUNT_JUMP
}

public class PlayerCubeController : MonoBehaviour
{
    Transform trnsfrm;

    
    [SerializeField] float startSpeed = 10f;
    [SerializeField] float height = 3f;

    Vector3 lastPos = new Vector3(0, 0, 0);
    Vector3 nextPos = new Vector3(5, 0, 0);

    float speed;
    Vector3 apogee = new Vector3(0, 0, 0);
    float quadrA = 0;
    float lastedTime = 0f;
    Status currentStatus = Status.COUNT_JUMP;

    void Start()
    {
        trnsfrm = GetComponent<Transform>();
        speed = startSpeed;
    }

    void Update()
    {
        switch (currentStatus)
        {
            case Status.FLY:
                lastedTime += Time.deltaTime;
                //trnsfrm.position = new Vector3(Mathf.Lerp(lastPos.x, nextPos.x, lastedTime * speed), 0, 0);
                trnsfrm.position = getPosInQuadr(Mathf.Lerp(lastPos.x, nextPos.x, lastedTime * speed / 10));
                if (Vector3.Distance(trnsfrm.position, nextPos) < 0.01)
                {
                    lastPos = nextPos;
                    nextPos = nextPos + Vector3.right * 5;
                    currentStatus = Status.COUNT_JUMP;
                }
                break;
            case Status.COUNT_JUMP:
                apogee = lastPos + (nextPos - lastPos) / 2 + new Vector3(0, height, 0);
                quadrA = (lastPos.y - apogee.y) / ((lastPos.x - apogee.x) * (lastPos.x - apogee.x));
                lastedTime = 0f;
                currentStatus = Status.FLY;
                break;
        }
        
    }

    Vector3 getPosInQuadr(float x)
    {
        float y = quadrA * (x - apogee.x) * (x - apogee.x) + apogee.y;
        return new Vector3(x, y, 0);
    }
}
