using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCubeController PlayerCube;
    
    [SerializeField]
    private GameObject platformObject;
    private float platformX = -20f;
    private List<BasePlatform> platforms = new List<BasePlatform>();
    private PlatformManager platformManager = new PlatformManager();

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
        for (var i = 0; i < 9; i++) {
            var newPlatform = AddNextPlatform();
            if (i <= 3) {
                newPlatform.GetObject().SetActive(false);
            }
            
        }
    }

    private void OnLandHandler() {
        AddNextPlatform();
        Debug.Log(GetCurrentPlatform().GetObject().transform.position.x.ToString());
        InitPlatformAction();
        AddScore(5);
    }

    private void InitPlatformAction() {
        var platform = GetCurrentPlatform();

        switch (platform.GetPlatformType()) {
            case PlatformType.Block:
                break;
            case PlatformType.TurnLimit:
                break;
            case PlatformType.BreaksEdgeOnSkip:
                break;
            case PlatformType.BreaksEdgeOnHit:
                break;
            case PlatformType.LoseControl:
                break;
            case PlatformType.JumpOnHit:
                break;
            case PlatformType.RestoreEdge:
                break;
            case PlatformType.ScoreOnHit:
                PlatformScoreOnHit pl = platform as PlatformScoreOnHit;
                AddScore(pl.Score);
                break;
            case PlatformType.ScoreOnSkip:
                break;
            case PlatformType.Shield:
                break;
            case PlatformType.Invulnerability:
                break;
        }
    }

    private BasePlatform GetCurrentPlatform() { return platforms[2];  }

    private BasePlatform AddNextPlatform() {
        var newPlatformObject = Instantiate(platformObject, new Vector3(platformX, -0.5f, 0f), Quaternion.identity);
        var platform = platformManager.GetNextPlatform(newPlatformObject);
        Debug.Log("start: " + platformX.ToString());

        platforms.Add(platform);
        platformX += 5f;

        if (platforms.Count >= 9) {
            Destroy(platforms[0].GetObject());
            platforms.RemoveAt(0);
        }

        return platform;
    }

    private void AddScore(int value) {
        Score += value;
        ScoreText.SetText(Score.ToString());
    }
}
