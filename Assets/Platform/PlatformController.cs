using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;

    public void SetText(string newText, Color color) {
        text.SetText(newText);
        //text.color = color;
    }

    public void SetPlatformColor(string newColor)
    {
        if (newColor == "red")
        {
            GetComponent<MeshRenderer>().material = redMaterial;
        }
        if (newColor == "green")
        {
            GetComponent<MeshRenderer>().material = greenMaterial;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
