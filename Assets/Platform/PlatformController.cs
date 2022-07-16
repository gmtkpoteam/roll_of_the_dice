using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;

    public void SetText(string newText, Color color) {
        text.SetText(newText);
        text.color = color;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
