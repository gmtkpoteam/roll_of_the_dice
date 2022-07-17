using UnityEngine;

public enum PlatformType {
    // ����������
    Block,            // ��������� ����� ��� ��������� �� N �������
    TurnLimit,        // �� ��������� ������ ������������ �������� ������ �� 2 ��������
    BreaksEdgeOnSkip, // ������ �����, ���� �����������
    BreaksEdgeOnHit,  // ������ �����, ���� ����� �� ������ ������
    LoseControl,      // �� ��������� ������ ����� ������ ����������, ����� �������� ��������

    //����������
    JumpOnHit,        // ������������� ��������� ���������, ���� ����� ������ ������
    RestoreEdge,      // ���������������� �����, ���� ����� ��������� ������
    ScoreOnHit,       // ���� ������ �����, ���� ����� ������ ������
    ScoreOnSkip,      // ���� ������ �����, ���� �����������
    Shield,           // ���� ���, ���� ����� ������ ������
    Invulnerability   // ���� ������������� ���� ���������� �������� �� N �����
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

    public BasePlatform(GameObject platformObject, PlatformType type, string text, bool positive, PlatformTrigger trigger, DiceEdgeType edgeType = DiceEdgeType.Empty) {
        PlatformObject = platformObject;
        Type = type;
        EdgeType = edgeType;
        Positive = positive;
        Trigger = trigger;

        Controller = PlatformObject.GetComponent<PlatformController>();

        Controller.SetText(text + (int)edgeType, positive ? Color.green : Color.red);
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

public class PlatformBlock : BasePlatform {
    public PlatformBlock(GameObject platformObject) : base(platformObject, PlatformType.Block, "BL", false, PlatformTrigger.OnHit) { }
    public int Steps { get { return 3; } }
}

public class PlatformInvulnerability : BasePlatform {
    public PlatformInvulnerability(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.Invulnerability, "IN", true, PlatformTrigger.OnHitWithEdge, edgeType) { }
    public int Steps { get { return 3; } }
}

public class PlatformScoreOnHit : BasePlatform {
    public PlatformScoreOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.ScoreOnHit, "SH", true, PlatformTrigger.OnHitWithEdge, edgeType) { }
}

public class PlatformScoreOnSkip : BasePlatform {
    public PlatformScoreOnSkip(GameObject platformObject) : base(platformObject, PlatformType.ScoreOnSkip, "SS", true, PlatformTrigger.OnSkip) { }
}

public class PlatformBreaksEdgeOnHit : BasePlatform {
    public PlatformBreaksEdgeOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.BreaksEdgeOnHit, "BH", false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

public class PlatformBreaksEdgeOnSkip : BasePlatform {
    public PlatformBreaksEdgeOnSkip(GameObject platformObject) : base(platformObject, PlatformType.BreaksEdgeOnSkip, "BS", false, PlatformTrigger.OnSkip) { }
}

public class PlatformJumpOnHit : BasePlatform {
    public PlatformJumpOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.JumpOnHit, "JP", true, PlatformTrigger.OnHitWithEdge, edgeType) { }
}

public class PlatformLoseControl : BasePlatform {
    public PlatformLoseControl(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.LoseControl, "LC", false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

public class PlatformRestoreEdge : BasePlatform {
    public PlatformRestoreEdge(GameObject platformObject) : base(platformObject, PlatformType.RestoreEdge, "RE", true, PlatformTrigger.OnHit) { }
}

public class PlatformShield : BasePlatform {
    public PlatformShield(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.Shield, "SD", true, PlatformTrigger.OnHitWithEdge, edgeType) { }
}

public class PlatformTurnLimit : BasePlatform {
    public PlatformTurnLimit(GameObject platformObject, DiceEdgeType edgeType) : base(platformObject, PlatformType.TurnLimit, "TL", false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

