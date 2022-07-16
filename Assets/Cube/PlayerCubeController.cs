using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status
{
    START,
    FLY,
    STAY,
}

enum ActionDirection
{
    UP,
    RIGHT,
    DOWN,
    LEFT,
    NONE
}

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] float startSpeed = 10f;
    [SerializeField] float height = 3f;
    [SerializeField] float stayTime = 0.2f;
    [SerializeField] GameObject Cube;

    Vector3 lastPos = Vector3.zero;
    Vector3 nextPos = Vector3.zero;
    Transform cubeTransform;
    Quaternion lastRotatation;
    Quaternion newRotation;

    float speed;
    Vector3 apogee;
    float quadrA;
    float lastedTime = 0f;
    Status currentStatus = Status.START;
    ActionDirection nextAction = ActionDirection.NONE;

    void Start()
    {
        cubeTransform = Cube.GetComponent<Transform>();

        speed = startSpeed;
        lastRotatation = cubeTransform.rotation;
        Status currentStatus = Status.START;
        ActionDirection nextAction = ActionDirection.RIGHT;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) nextAction = ActionDirection.UP;
        if (Input.GetKeyDown(KeyCode.RightArrow)) nextAction = ActionDirection.RIGHT;
        if (Input.GetKeyDown(KeyCode.DownArrow)) nextAction = ActionDirection.DOWN;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) nextAction = ActionDirection.LEFT;

        switch (currentStatus)
        {
            case Status.START:
                countQuadrAttr();
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

                    countQuadrAttr();

                    switch (nextAction)
                    {
                        case (ActionDirection.UP):
                            newRotation = Quaternion.AngleAxis(-90f, Vector3.left);
                            break;
                        case (ActionDirection.RIGHT):
                            newRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
                            break;
                        case (ActionDirection.DOWN):
                            newRotation = Quaternion.AngleAxis(-90f, Vector3.right);
                            break;
                        case (ActionDirection.LEFT):
                            newRotation = Quaternion.AngleAxis(-90f, Vector3.back);
                            break;
                        default:
                            newRotation = Quaternion.AngleAxis(0, Vector3.forward);
                            break;
                    }
                    nextAction = ActionDirection.NONE;

                    lastedTime = 0f;
                    currentStatus = Status.FLY;
                }
                break;
        }
        
    }

    void countQuadrAttr()
    {
        apogee = lastPos + (nextPos - lastPos) / 2 + new Vector3(0, height, 0);
        quadrA = (lastPos.y - apogee.y) / ((lastPos.x - apogee.x) * (lastPos.x - apogee.x));
    }

    Vector3 getPosInQuadr(float x) // Восстанавливаем квадратичную функцию по нужным точкам
    {
        float y = quadrA * (x - apogee.x) * (x - apogee.x) + apogee.y;
        return new Vector3(x, y, 0);
    }
}
