using UnityEngine;

public enum PlatformType {
    // ����������
    BreaksEdgeOnHit,  // ������ �����, ���� ����� �� ������ ������
    BreaksRandomEdge, // ������ ��������� �����
    LoseControl,      // �������� ���������� �� N �����

    //����������
    JumpOnHit,        // ������������� ��������� ���������, ���� ����� ������ ������
    RestoreEdge,      // ���������������� �����, ���� ����� ��������� ������
    ScoreOnHit,       // ���� ������ �����, ���� ����� ������ ������
    Invulnerability,  // ���� ������������� ���� ���������� �������� �� N �����
    Empty,            // ��� ��������

}

public enum PlatformTrigger {
    OnHit,            // ��� ��������� ����� ������
    OnHitWithEdge,    // ��� ��������� ���������� ������
    OnHitWithEdgeAlt, // ��� ��������� ����� ������, ����� ����������
    OnSkip            // ��� ��������������
}

public class BasePlatform {
    private GameObject PlatformObject;
    private PlatformType Type;
    private bool Positive;
    private DiceEdgeType EdgeType;
    private PlatformTrigger Trigger;
    private PlatformController Controller;

    public BasePlatform(GameObject platformObject, PlatformType type, string text, string description, bool positive, PlatformTrigger trigger, DiceEdgeType edgeType = DiceEdgeType.Empty) {
        PlatformObject = platformObject;
        Type = type;
        EdgeType = edgeType;
        Positive = positive;
        Trigger = trigger;

        Controller = PlatformObject.GetComponent<PlatformController>();

        Controller.SetText("" + (int)edgeType);
        Controller.SetPlatformColor(Type);
        Controller.SetDescription(description);
    }

    public bool CanAction(DiceEdge edge) {
        switch (Trigger) {
            case PlatformTrigger.OnHit: return true;
            case PlatformTrigger.OnHitWithEdge: return edge.GetDiceEdgeType() == EdgeType;
            case PlatformTrigger.OnHitWithEdgeAlt: return edge.GetDiceEdgeType() != EdgeType;
            case PlatformTrigger.OnSkip: return true;
        }

        return false;
    }

    public GameObject GetObject() { return PlatformObject; }
    public bool IsPositive() { return Positive; }
    public PlatformType GetPlatformType() { return Type; }
    public DiceEdgeType GetDiceEdgeType() { return EdgeType; }
    public PlatformTrigger GetTrigger() { return Trigger; }

}

public class PlatformInvulnerability : BasePlatform {
    public PlatformInvulnerability(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.Invulnerability, 
        "IN",
        "invulnerability",
        true, 
        PlatformTrigger.OnHitWithEdge, 
        edgeType
    ) { }
    public int Steps { get { return 3; } }
}

public class PlatformScoreOnHit : BasePlatform {
    public PlatformScoreOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.ScoreOnHit, 
        "SH", 
        "bonus score",
        true, 
        PlatformTrigger.OnHitWithEdge, 
        edgeType
    ) { }
}

public class PlatformBreaksEdgeOnHit : BasePlatform {
    public PlatformBreaksEdgeOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.BreaksEdgeOnHit, 
        "BH", 
        "damage",
        false, 
        PlatformTrigger.OnHitWithEdgeAlt, 
        edgeType
    ) { }
}

public class PlatformBreaksRandomEdge : BasePlatform {
    public PlatformBreaksRandomEdge(GameObject platformObject) : base(
        platformObject,
        PlatformType.BreaksRandomEdge,
        "BR",
        "random damage",
        false,
        PlatformTrigger.OnHit
    ) { }
}

public class PlatformJumpOnHit : BasePlatform {
    public PlatformJumpOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.JumpOnHit, 
        "JP", 
        "jump",
        true, 
        PlatformTrigger.OnHitWithEdge, 
        edgeType
    ) { }
}

public class PlatformLoseControl : BasePlatform {
    public PlatformLoseControl(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.LoseControl, 
        "LC", 
        "lose control",
        false, 
        PlatformTrigger.OnHitWithEdgeAlt, 
        edgeType
    ) { }
}

public class PlatformRestoreEdge : BasePlatform {
    public PlatformRestoreEdge(GameObject platformObject) : base(
        platformObject, 
        PlatformType.RestoreEdge, 
        "RE", 
        "restore",
        true, 
        PlatformTrigger.OnHit
    ) { }
}

public class PlatformEmpty : BasePlatform {
    public PlatformEmpty(GameObject platformObject) : base(
        platformObject, 
        PlatformType.Empty, 
        "EM", 
        "empty",
        true, 
        PlatformTrigger.OnHit
    ) { }
}

