using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Status
{
    START,
    FLY,
    STAY
}

enum ActionDirection
{
    NONE,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX
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
    Quaternion lastRotation;
    Quaternion newRotation;

    float speed;
    Vector3 apogee;
    float quadrA;
    float lastedTime = 0f;
    Status currentStatus = Status.START;
    ActionDirection nextAction = ActionDirection.NONE;

    private bool isLandedNow = false;
    public delegate void OnLand(Quaternion newRotation);
    public event OnLand onLand;


    void Start()
    {
        cubeTransform = Cube.GetComponent<Transform>();

        speed = startSpeed;
        lastRotation = cubeTransform.rotation;
        Status currentStatus = Status.START;
        ActionDirection nextAction = ActionDirection.NONE;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) nextAction = ActionDirection.ONE;
        if (Input.GetKeyDown(KeyCode.Alpha2)) nextAction = ActionDirection.TWO;
        if (Input.GetKeyDown(KeyCode.Alpha3)) nextAction = ActionDirection.THREE;
        if (Input.GetKeyDown(KeyCode.Alpha4)) nextAction = ActionDirection.FOUR;
        if (Input.GetKeyDown(KeyCode.Alpha5)) nextAction = ActionDirection.FIVE;
        if (Input.GetKeyDown(KeyCode.Alpha6)) nextAction = ActionDirection.SIX;

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
                cubeTransform.rotation = Quaternion.Lerp(lastRotation, newRotation, lastedTime * speed / 10);

                if (Vector3.Distance(transform.position, nextPos) < 0.01f)
                {
                    lastedTime = 0f;
                    transform.position = nextPos;
                    currentStatus = Status.STAY;
                    isLandedNow = true;
                }
                break;
            case Status.STAY:
                if (isLandedNow) {
                    onLand.Invoke(newRotation);
                    isLandedNow = false;
                }

                lastedTime += Time.deltaTime;

                if(lastedTime > stayTime)
                {
                    lastPos = nextPos;
                    nextPos = nextPos + Vector3.right * 5;

                    countQuadrAttr();
                    lastRotation = cubeTransform.rotation;

                    switch (nextAction)
                    {
                        case (ActionDirection.ONE):
                            newRotation = Quaternion.Euler(0, 0, 0);
                            break;
                        case (ActionDirection.TWO):
                            newRotation = Quaternion.Euler(90, 0, 0);
                            break;
                        case (ActionDirection.THREE):
                            newRotation = Quaternion.Euler(90, -90, 0);
                            break;
                        case (ActionDirection.FOUR):
                            newRotation = Quaternion.Euler(90, 90, 0);
                            break;
                        case (ActionDirection.FIVE):
                            newRotation = Quaternion.Euler(90, 180, 0);
                            break;
                        case (ActionDirection.SIX):
                            newRotation = Quaternion.Euler(0, 180, 180);
                            break;
                        default:
                            newRotation = Quaternion.Euler(0, 0, -90f) * lastRotation;
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
