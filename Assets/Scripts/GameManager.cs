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

    [SerializeField]
    private GameObject StatusTextGameObject;
    private TextMeshProUGUI StatusText;

    private DiceManager diceManager = new DiceManager();

    private UIStatuses uiStatuses;

    private AudioSource backroundMusic;

    void Start()
    {
        uiStatuses = gameObject.GetComponent<UIStatuses>();

        PlayerCube.onLand += OnLandHandler;
        ScoreText = ScoreTextGameObject.GetComponent<TextMeshProUGUI>();
        ScoreText.SetText(Score.ToString());

        StatusText = StatusTextGameObject.GetComponent<TextMeshProUGUI>();

        backroundMusic = GetComponent<AudioSource>();
        StartCoroutine(PlayBackgroundMusic()); // перенести в старт игры.

        // Создаем начальные платформы
        for (var i = 0; i < 9; i++) {
            var newPlatform = AddNextPlatform();
            if (i <= 3) {
                newPlatform.GetObject().SetActive(false);
            }
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        yield return new WaitForSeconds(0.95f);
        backroundMusic.Play();
    }

    private void OnLandHandler(Quaternion newRotation) {
        if (!diceManager.IsLoseControl()) PlayerCube.canAction = true;

        var edge = GetEdge(newRotation);
        var platform = GetCurrentPlatform();

        AddNextPlatform();
        InitPlatformAction(skippedPlatform, edge, true);
        InitPlatformAction(platform, edge);

        AddScore(5);
        diceManager.DecreaseInvulnerability();
        diceManager.DecreaseLoseControl();

        if (!diceManager.IsLoseControl()) RemoveLoseControl();

        if (diceManager.IsInvulnerability()) {
            StatusText.SetText("неуязвимый");
        } else if (diceManager.IsLoseControl()) {
            StatusText.SetText("потеря контроля");
        } else if (diceManager.OnShield()) {
            StatusText.SetText("защита щитом");
        } else {
            StatusText.SetText("");
        }

        CheckGameOver();
    }

    private void CheckGameOver() {
        if (diceManager.GetAliveEdgesCount() == 0) {
            GameOver();
        }
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
            case PlatformType.Empty:
                break;
            case PlatformType.BreaksEdgeOnSkip:
                if (!diceManager.RemoveShield()) {
                    if (edge.broken) {
                        GameOver();
                    } else {
                        BreakEdge(edge);
                    }
                }
                break;
            case PlatformType.BreaksEdgeOnHit:
                if (!diceManager.RemoveShield()) {
                    if (edge.broken) {
                        GameOver();
                    } else {
                        BreakEdge(edge);
                    }
                }
                break;
            case PlatformType.BreaksRandomEdge:
                var randomEdge = diceManager.GetRandomEdge(false);
                BreakEdge(randomEdge);
                break;
            case PlatformType.LoseControl:
                InitLoseControl();
                break;
            case PlatformType.JumpOnHit:
                InitJumpLong();
                break;
            case PlatformType.RestoreEdge:
                RestoreEdge(edge);
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
                RemoveLoseControl();
                break;
        }
    }

    private void BreakEdge(DiceEdge edge) {
        edge.broken = true;
        uiStatuses.toggleDisabledFace((int)edge.GetDiceEdgeType(), true);
    }

    private void RestoreEdge(DiceEdge edge) {
        edge.broken = false;
        uiStatuses.toggleDisabledFace((int)edge.GetDiceEdgeType(), false);
    }

    private void InitLoseControl() {
        PlayerCube.canAction = false;
        diceManager.AddLoseControl();

        //uiStatuses.toggleDisabledFace(1, true);
        //uiStatuses.toggleDisabledFace(2, true);
        //uiStatuses.toggleDisabledFace(3, true);
        //uiStatuses.toggleDisabledFace(4, true);
        //uiStatuses.toggleDisabledFace(5, true);
        //uiStatuses.toggleDisabledFace(6, true);
    }

    private void RemoveLoseControl() {
        diceManager.ClearLoseControl();
        PlayerCube.canAction = true;

        //uiStatuses.toggleDisabledFace(1, false);
        //uiStatuses.toggleDisabledFace(2, false);
        //uiStatuses.toggleDisabledFace(3, false);
        //uiStatuses.toggleDisabledFace(4, false);
        //uiStatuses.toggleDisabledFace(5, false);
        //uiStatuses.toggleDisabledFace(6, false);
    }

    private void InitJumpLong() {
        PlayerCube.setNextJumpLong();
        skippedPlatform = GetCurrentPlatform();
        isSetSkippedPlatform = true;
        AddNextPlatform(); // пропускаем следующий блок
    }

    private BasePlatform GetCurrentPlatform() {
        return platforms[4]; 
    }

    private BasePlatform AddNextPlatform() {
        var newPlatformObject = Instantiate(platformObject, new Vector3(platformX, -0.5f, 0f), Quaternion.identity);
        var platform = platformManager.GetNextPlatform(newPlatformObject);
        // Костыль, чтобы почаще были негативные платформы
        if (platform.IsPositive()) platform = platformManager.GetNextPlatform(newPlatformObject);

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
