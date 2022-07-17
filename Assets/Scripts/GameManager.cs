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
    private BasePlatform skippedPlatform = null;
    private bool isSetSkippedPlatform = false;

    [SerializeField]
    private GameObject ScoreTextGameObject;
    private TextMeshProUGUI ScoreText;
    private int Score = 0;

    private DiceManager diceManager = new DiceManager();

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

    private void OnLandHandler(Quaternion newRotation) {
        PlayerCube.canAction = true;

        var edge = GetEdge(newRotation);
        var platform = GetCurrentPlatform();

        AddNextPlatform();
        InitPlatformAction(skippedPlatform, edge, true);
        InitPlatformAction(platform, edge);
        InitEdgeAction(edge);


        AddScore(5);
        diceManager.DecreaseInvulnerability();
        diceManager.DecreaseBlocked();
    }

    private DiceEdge GetEdge(Quaternion rotation) {
        var x = Mathf.Round(rotation.eulerAngles.x / 90);
        var y = Mathf.Round(rotation.eulerAngles.y / 90);
        var z = Mathf.Round(rotation.eulerAngles.z / 90);

        x = x == 4 ? 0 : x;
        y = y == 4 ? 0 : y;
        z = z == 4 ? 0 : z;

        if ((x == 1 && y == 1 && z == 1) ||
            (x == 1 && y == 0 && z == 0) ||
            (x == 1 && y == 3 && z == 0) ||
            (x == 1 && y == 1 && z == 0) ||
            (x == 1 && y == 2 && z == 0) 
        ) {
            return diceManager.GetEdge(DiceEdgeType.Base);
        }

        if ((x == 0 && y == 0 && z == 2) ||
            (x == 0 && y == 2 && z == 2) ||
            (x == 0 && y == 1 && z == 2) ||
            (x == 0 && y == 3 && z == 2) ||
            (x == 1 && y == 2 && z == 0) ||
            (x == 0 && y == 2 && z == 2)
        ) {
            return diceManager.GetEdge(DiceEdgeType.Double);
        }

        if ((x == 0 && y == 0 && z == 3) ||
            (x == 0 && y == 3 && z == 3) ||
            (x == 0 && y == 2 && z == 3) ||
            (x == 0 && y == 3 && z == 2) ||
            (x == 0 && y == 1 && z == 3) ||
            (x == 0 && y == 2 && z == 3)
        ) {
            return diceManager.GetEdge(DiceEdgeType.Jump);
        }

        if ((x == 0 && y == 0 && z == 1) ||
            (x == 0 && y == 1 && z == 1) ||
            (x == 0 && y == 2 && z == 1) ||
            (x == 0 && y == 3 && z == 1) ||
            (x == 0 && y == 1 && z == 1)
        ) {
            return diceManager.GetEdge(DiceEdgeType.Time); 
        }

        if ((x == 0 && y == 0 && z == 0) ||
            (x == 0 && y == 2 && z == 0) ||
            (x == 0 && y == 3 && z == 0) ||
            (x == 0 && y == 1 && z == 0) ||
            (x == 0 && y == 2 && z == 0)
        ) {
            return diceManager.GetEdge(DiceEdgeType.Shield);
        }

        if ((x == 3 && y == 2 && z == 0) ||
            (x == 3 && y == 3 && z == 3) ||
            (x == 3 && y == 3 && z == 0) ||
            (x == 3 && y == 1 && z == 0) ||
            (x == 3 && y == 0 && z == 0)
        ) {
            return diceManager.GetEdge(DiceEdgeType.Score);
        }

        return default;
    }

    private void InitEdgeAction(DiceEdge edge) {
        if (edge.IsBlocked()) return;

        switch (edge.GetDiceEdgeType()) {
            case DiceEdgeType.Base:
                break;
            case DiceEdgeType.Double:
                break;
            case DiceEdgeType.Jump:
                if (isSetSkippedPlatform) return;

                InitJumpLong();
                break;
            case DiceEdgeType.Time:
                break;
            case DiceEdgeType.Shield:
                diceManager.AddShield();
                break;
            case DiceEdgeType.Score:
                AddScore(10);
                break;
        }
    }

    private void InitPlatformAction(BasePlatform platform, DiceEdge edge, bool onSkip = false) {

        if (platform == null) return;

        if (onSkip) {
            skippedPlatform = null;
            isSetSkippedPlatform = false;
        }

        if (onSkip && platform.GetTrigger() != PlatformTrigger.OnSkip) return;
        if (!onSkip && platform.GetTrigger() == PlatformTrigger.OnSkip) return;
        if (!platform.CanAction(edge)) return;
        if (!platform.IsPositive() && diceManager.IsInvulnerability()) return;

        switch (platform.GetPlatformType()) {
            case PlatformType.Block:
                edge.blockedSteps = 3;
                break;
            case PlatformType.TurnLimit:
                break;
            case PlatformType.BreaksEdgeOnSkip:
                if (!diceManager.RemoveShield()) {
                    if (edge.broken) {
                        GameOver();
                    } else {
                        edge.broken = true;
                    }
                }
                break;
            case PlatformType.BreaksEdgeOnHit:
                if (!diceManager.RemoveShield()) {
                    if (edge.broken) {
                        GameOver();
                    } else {
                        edge.broken = true;
                    }
                }
                break;
            case PlatformType.LoseControl:
                PlayerCube.canAction = false;
                break;
            case PlatformType.JumpOnHit:
                InitJumpLong();
                break;
            case PlatformType.RestoreEdge:
                edge.broken = false;
                break;
            case PlatformType.ScoreOnHit:
                AddScore(100);
                break;
            case PlatformType.ScoreOnSkip:
                AddScore(1000);
                break;
            case PlatformType.Shield:
                diceManager.AddShield();
                break;
            case PlatformType.Invulnerability:
                diceManager.AddInvulnerability();
                break;
        }
    }

    private void InitJumpLong() {
        PlayerCube.setNextJumpLong();
        skippedPlatform = GetCurrentPlatform();
        isSetSkippedPlatform = true;
        AddNextPlatform(); // пропускаем следующий блок
    }

    private BasePlatform GetCurrentPlatform() { return platforms[2]; }

    private BasePlatform AddNextPlatform() {
        var newPlatformObject = Instantiate(platformObject, new Vector3(platformX, -0.5f, 0f), Quaternion.identity);
        var platform = platformManager.GetNextPlatform(newPlatformObject);

        platforms.Add(platform);
        platformX += 5f;

        while (platforms.Count >= 9) {
            Destroy(platforms[0].GetObject());
            platforms.RemoveAt(0);
        }

        return platform;
    }

    private void AddScore(int value) {
        Score += value;
        ScoreText.SetText(Score.ToString());
    }

    private void GameOver() {
        Debug.Log("GAME OVER");
    }
}
