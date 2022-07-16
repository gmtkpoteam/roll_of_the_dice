using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCubeController PlayerCube;
    
    [SerializeField]
    private GameObject Platform;
    private float PlatformX = 0f;
    private List<GameObject> Platforms = new List<GameObject>();

    [SerializeField]
    private GameObject ScoreTextGameObject;
    private TextMeshProUGUI ScoreText;
    private int Score = 0;
    void Start()
    {
        PlayerCube.onLand += OnLandHandler;
        ScoreText = ScoreTextGameObject.GetComponent<TextMeshProUGUI>();
        ScoreText.SetText(Score.ToString());
        // Создаем начальные платформы
        for (var i = 0; i < 5; i++) {
            AddNextPlatform();
        }
    }

    private void OnLandHandler() {
        AddNextPlatform();
        AddScore(5);
    }

    private void AddNextPlatform() {
        var platform = Instantiate(Platform, new Vector3(PlatformX, -0.5f, 0f), Quaternion.identity);
        Platforms.Add(platform);
        PlatformX += 5f;

        if (Platforms.Count >= 9) {
            Destroy(Platforms[0]);
            Platforms.RemoveAt(0);
        }
    }

    private void AddScore(int value) {
        Score += value;
        ScoreText.SetText(Score.ToString());
    }
}
