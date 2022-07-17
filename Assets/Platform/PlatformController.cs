using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private TextMeshPro description;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material grayMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material YellowMaterial;
    [SerializeField] private Material OrangeMaterial;
    [SerializeField] private Material BlueMaterial;

    public void SetText(string newText) {
        text.SetText(newText);
        text.color = Color.white;
    }

    public void SetPlatformColor(PlatformType platformType)
    {
        switch (platformType) {
            case PlatformType.BreaksEdgeOnHit:
                GetComponent<MeshRenderer>().material = redMaterial;
                break;
            case PlatformType.BreaksRandomEdge:
                GetComponent<MeshRenderer>().material = blackMaterial;
                text.SetText("?");
                break;
            case PlatformType.LoseControl:
                GetComponent<MeshRenderer>().material = purpleMaterial;
                text.color = Color.white;
                break;
            case PlatformType.JumpOnHit:
                GetComponent<MeshRenderer>().material = OrangeMaterial;
                break;
            case PlatformType.RestoreEdge:
                GetComponent<MeshRenderer>().material = greenMaterial;
                text.SetText("+");
                break;
            case PlatformType.ScoreOnHit:
                GetComponent<MeshRenderer>().material = YellowMaterial;
                break;
            case PlatformType.Invulnerability:
                GetComponent<MeshRenderer>().material = BlueMaterial;
                break;
            case PlatformType.Empty:
                GetComponent<MeshRenderer>().material = grayMaterial;
                break;
        }

        //if (newColor == "red")
        //{
        //    GetComponent<MeshRenderer>().material = redMaterial;
        //}
        //if (newColor == "green")
        //{
        //    GetComponent<MeshRenderer>().material = greenMaterial;
        //}
    }

    public void SetDescription(string newDescription) {
        description.SetText(newDescription);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
