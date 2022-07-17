using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private TextMeshPro description;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material grayMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material YellowMaterial;
    [SerializeField] private Material OrangeMaterial;
    [SerializeField] private Material BlueMaterial;

    private float baseX;
    private float baseY;
    private float randShakeX;
    private float randShakeY;
    private float randShakeSpeedX;
    private float randShakeSpeedY;
    private bool shake = false;

    public void SetText(string newText) {
        text.SetText(newText);
        text.color = Color.white;
    }

    public void SetPlatformColor(PlatformType platformType)
    {
        switch (platformType) {
            case PlatformType.BreaksEdgeOnHit:
                GetComponentInChildren<MeshRenderer>().material = redMaterial;
                break;
            case PlatformType.BreaksRandomEdge:
                GetComponentInChildren<MeshRenderer>().material = blackMaterial;
                text.SetText("?");
                break;
            case PlatformType.LoseControl:
                GetComponentInChildren<MeshRenderer>().material = purpleMaterial;
                text.color = Color.white;
                break;
            case PlatformType.JumpOnHit:
                GetComponentInChildren<MeshRenderer>().material = OrangeMaterial;
                break;
            case PlatformType.RestoreEdge:
                GetComponentInChildren<MeshRenderer>().material = greenMaterial;
                text.SetText("+");
                break;
            case PlatformType.ScoreOnHit:
                GetComponentInChildren<MeshRenderer>().material = YellowMaterial;
                break;
            case PlatformType.Invulnerability:
                GetComponentInChildren<MeshRenderer>().material = BlueMaterial;
                break;
            case PlatformType.Empty:
                GetComponentInChildren<MeshRenderer>().material = grayMaterial;
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

    public void FallDown()
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(Vector3.down * 100);
    }

    public void SomeShake()
    {
        shake = true;
        baseX = transform.position.x;
        baseY = transform.position.z;
        randShakeX = Random.Range(0f, 1f);
        randShakeY = Random.Range(0f, 1f);
        randShakeSpeedX = Random.Range(-10f, 10f);
        randShakeSpeedY = Random.Range(-10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            mesh.transform.position = new Vector3(
                baseX + Mathf.Sin(randShakeX + Time.time * (50 + randShakeSpeedX)) * 0.03f,
                mesh.transform.position.y,
                baseY + Mathf.Sin(randShakeY + Time.time * (50 + randShakeSpeedY)) * 0.03f);
        }
    }
}
