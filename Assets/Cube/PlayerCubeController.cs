using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Status
{
    START,
    FLY,
    STAY
}

public enum ActionDirection
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

    // UI Group
    [SerializeField] private GameObject[] CubeFacesStatuses;
    private List<Image> CubeFacesStatusesBacks;
    private Color colorActive = Color.green;
    private Color colorInactive = Color.black;
    private Color colorDisabled = Color.red;

    Vector3 lastPos = Vector3.zero;
    Vector3 nextPos = Vector3.zero;
    Transform cubeTransform;
    Quaternion lastRotation;
    Quaternion newRotation;

    float speed;
    Vector3 apogee;
    bool nextJumpLong = false;
    float quadrA;
    float lastedTime = 0f;
    Status currentStatus = Status.START;
    ActionDirection nextAction = ActionDirection.NONE;

    private bool isLandedNow = false;
    public delegate void OnLand();
    public event OnLand onLand;


    void Start()
    {
        cubeTransform = Cube.GetComponent<Transform>();

        speed = startSpeed;
        lastRotation = cubeTransform.rotation;

/*        for (var i = 0; i < CubeFacesStatuses.Length; i++)
        {
            CubeFacesStatusesBacks.Add(CubeFacesStatuses[i].GetComponent<Image>());
        }*/
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) nextAction = ActionDirection.ONE;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) nextAction = ActionDirection.TWO;
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) nextAction = ActionDirection.THREE;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) nextAction = ActionDirection.FOUR;
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) nextAction = ActionDirection.FIVE;
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) nextAction = ActionDirection.SIX;

        switch (currentStatus)
        {
            case Status.START:
                countQuadrAttr(height);
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
                    onLand.Invoke();
                    isLandedNow = false;
                }

                lastedTime += Time.deltaTime;

                if(lastedTime > stayTime)
                {
                    lastPos = nextPos;
                    if (nextJumpLong)
                    {
                        nextPos = nextPos + Vector3.right * 10;
                        countQuadrAttr(height * 1.5f);
                        nextJumpLong = false;
                    }
                    else
                    {
                        nextPos = nextPos + Vector3.right * 5;
                        countQuadrAttr(height);
                    }

                    lastRotation = cubeTransform.rotation;

                    switch (nextAction)
                    {
                        case (ActionDirection.ONE):
                            newRotation = Quaternion.Euler(90, 0, 0);
                            break;
                        case (ActionDirection.TWO):
                            newRotation = Quaternion.Euler(180, 0, 0);
                            break;
                        case (ActionDirection.THREE):
                            newRotation = Quaternion.Euler(180, 0, 90);
                            break;
                        case (ActionDirection.FOUR):
                            newRotation = Quaternion.Euler(180, 0, -90);
                            break;
                        case (ActionDirection.FIVE):
                            newRotation = Quaternion.Euler(0, 180, 0);
                            break;
                        case (ActionDirection.SIX):
                            newRotation = Quaternion.Euler(-90, 0, 0);
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

    void countQuadrAttr(float height)
    {
        apogee = lastPos + (nextPos - lastPos) / 2 + new Vector3(0, height, 0);
        quadrA = (lastPos.y - apogee.y) / ((lastPos.x - apogee.x) * (lastPos.x - apogee.x));
    }

    Vector3 getPosInQuadr(float x) // Восстанавливаем квадратичную функцию по нужным точкам
    {
        float y = quadrA * (x - apogee.x) * (x - apogee.x) + apogee.y;
        return new Vector3(x, y, 0);
    }

    public void setNextJumpLong()
    {
        nextJumpLong = true;
    }

    public void setNextAction(ActionDirection direction)
    {
        nextAction = direction;
    }
}
