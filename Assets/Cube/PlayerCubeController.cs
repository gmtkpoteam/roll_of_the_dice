using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status
{
    START,
    FLY,
    STAY,
}

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] float startSpeed = 10f;
    [SerializeField] float height = 3f;
    [SerializeField] float stayTime = 0.2f;
    [SerializeField] GameObject Cube;

    Vector3 lastPos = Vector3.zero;
    Vector3 nextPos = Vector3.right * 5;
    Transform cubeTransform;
    Quaternion lastRotatation;
    Quaternion newRotation;

    float speed;
    Vector3 apogee = new Vector3(0, 0, 0);
    float quadrA = 0;
    float lastedTime = 0f;
    Status currentStatus = Status.START;

    void Start()
    {
        speed = startSpeed;
        cubeTransform = Cube.GetComponent<Transform>();
    }

    void Update()
    {

        switch (currentStatus)
        {
            case Status.START:
                lastPos = transform.position;
                nextPos = Vector3.zero;
                apogee = lastPos + (nextPos - lastPos) / 2 + new Vector3(0, height, 0);
                quadrA = (lastPos.y - apogee.y) / ((lastPos.x - apogee.x) * (lastPos.x - apogee.x));
                lastRotatation = cubeTransform.rotation;
                newRotation = Quaternion.AngleAxis(180f, Vector3.forward); // значения влияют на направление поворота
                currentStatus = Status.FLY;
                break;
            case Status.FLY:
                lastedTime += Time.deltaTime;
                
                transform.position = getPosInQuadr(Mathf.Lerp(lastPos.x, nextPos.x, lastedTime * speed / 10));
                cubeTransform.rotation = Quaternion.Lerp(lastRotatation, newRotation, lastedTime * speed / 10);

                if (Vector3.Distance(transform.position, nextPos) < 0.01f)
                {
                    lastedTime = 0f;
                    transform.position = nextPos;
                    currentStatus = Status.STAY;
                }
                break;
            case Status.STAY:
                lastedTime += Time.deltaTime;

                if(lastedTime > stayTime)
                {
                    lastPos = nextPos;
                    nextPos = nextPos + Vector3.right * 5;
                    apogee = lastPos + (nextPos - lastPos) / 2 + new Vector3(0, height, 0);
                    quadrA = (lastPos.y - apogee.y) / ((lastPos.x - apogee.x) * (lastPos.x - apogee.x));

                    newRotation = Quaternion.AngleAxis(180f, Vector3.forward); // значения влияют на направление поворота

                    lastedTime = 0f;
                    currentStatus = Status.FLY;
                }
                break;
        }
        
    }

    Vector3 getPosInQuadr(float x)
    {
        float y = quadrA * (x - apogee.x) * (x - apogee.x) + apogee.y;
        return new Vector3(x, y, 0);
    }
}
