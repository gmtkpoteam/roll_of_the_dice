using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum FaceStatus
{
    INACTIVE,
    ACTIVE,
    DISABLED
}

class FaceStruct
{
    public FaceStruct(int v, Image i)
    {
        value = v;
        curStatus = FaceStatus.INACTIVE;
        img = i;
    }

    public int value { get; }
    public FaceStatus curStatus { get; set; }
    public Image img { get; }
}

public class UIStatuses : MonoBehaviour
{
    [SerializeField] private List<GameObject> CubeFacesStatuses;
    private Color colorActive = new Color(0, 1, 0, 0.5f);
    private Color colorInactive = new Color(0, 0, 0, 0.5f);
    private Color colorDisabled = new Color(1, 0, 0, 0.5f);

    private List<FaceStruct> faces = new List<FaceStruct>();

    void Start()
    {
        for(var i = 0; i < 6; i++)
        {
            Image img = CubeFacesStatuses[i].GetComponent<Image>();
            faces.Add(new FaceStruct(i + 1, img));
        }
    }

    public void resetActiveFace()
    {
        for (var i = 0; i < 6; i++)
        {
            FaceStruct face = faces[i];
            if (face.curStatus == FaceStatus.ACTIVE)
            {
                face.img.color = colorInactive;
                face.curStatus = FaceStatus.INACTIVE;
            }
        }
    }

    public void setActiveFace(int num)
    {
        for(var i = 0; i < 6; i++)
        {
            FaceStruct face = faces[i];
            if (face.curStatus == FaceStatus.ACTIVE)
            {
                face.img.color = colorInactive;
                face.curStatus = FaceStatus.INACTIVE;
            }
            if (i + 1 == num && face.curStatus != FaceStatus.DISABLED)
            {
                face.img.color = colorActive;
                face.curStatus = FaceStatus.ACTIVE;
            }
        }
    }

    public void toggleDisabledFace(int num)
    {
        for (var i = 0; i < 6; i++)
        {
            FaceStruct face = faces[i];
            if (i + 1 == num && face.curStatus != FaceStatus.DISABLED)
            {
                face.img.color = colorDisabled;
                face.curStatus = FaceStatus.DISABLED;
            }
            if (i + 1 == num && face.curStatus == FaceStatus.DISABLED)
            {
                face.img.color = colorInactive;
                face.curStatus = FaceStatus.INACTIVE;
            }
        }
    }
    public void toggleDisabledFace(int num, bool disable)
    {
        for (var i = 0; i < 6; i++)
        {
            FaceStruct face = faces[i];
            if (i + 1 == num && disable)
            {
                face.img.color = colorDisabled;
                face.curStatus = FaceStatus.DISABLED;
            }
            if (i + 1 == num && !disable)
            {
                face.img.color = colorInactive;
                face.curStatus = FaceStatus.INACTIVE;
            }
        }
    }

    public void resetAllStatuses()
    {
        for (var i = 0; i < 6; i++)
        {
            FaceStruct face = faces[i];
            face.img.color = colorInactive;
            face.curStatus = FaceStatus.INACTIVE;
        }
    }
}
