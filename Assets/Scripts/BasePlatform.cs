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
    private PlatformType Type;
    private bool Positive;
    private DiceEdgeType EdgeType;
    private PlatformTrigger Trigger;

    public BasePlatform(PlatformType type, bool positive, PlatformTrigger trigger, DiceEdgeType edgeType = DiceEdgeType.Empty) {
        Type = type;
        EdgeType = edgeType;
        Positive = positive;
        Trigger = trigger;
    }

    public bool IsPositive() { return Positive; }
    public PlatformType GetPlatformType() { return Type; }
    public DiceEdgeType GetDiceEdgeType() { return EdgeType; }
    public PlatformTrigger GetTrigger() { return Trigger; }

}

public class PlatformBlock : BasePlatform {
    public PlatformBlock() : base(PlatformType.Block, false, PlatformTrigger.OnHit) { }
    public int Steps { get { return 3; } }
}

public class PlatformInvulnerability : BasePlatform {
    public PlatformInvulnerability(DiceEdgeType edgeType) : base(PlatformType.Invulnerability, true, PlatformTrigger.OnHitWithEdge, edgeType) { }
    public int Steps { get { return 3; } }
}

public class PlatformScoreOnHit : BasePlatform {
    public PlatformScoreOnHit(DiceEdgeType edgeType) : base(PlatformType.ScoreOnHit, true, PlatformTrigger.OnHitWithEdge, edgeType) { }
    public int Score { get { return 5; } }
}

public class PlatformScoreOnSkip : BasePlatform {
    public PlatformScoreOnSkip() : base(PlatformType.ScoreOnSkip, true, PlatformTrigger.OnSkip) { }
    public int Score { get { return 5; } }
}

public class PlatformBreaksEdgeOnHit : BasePlatform {
    public PlatformBreaksEdgeOnHit(DiceEdgeType edgeType) : base(PlatformType.BreaksEdgeOnHit, false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

public class PlatformBreaksEdgeOnSkip : BasePlatform {
    public PlatformBreaksEdgeOnSkip() : base(PlatformType.BreaksEdgeOnSkip, false, PlatformTrigger.OnSkip) { }
}

public class PlatformJumpOnHit : BasePlatform {
    public PlatformJumpOnHit(DiceEdgeType edgeType) : base(PlatformType.JumpOnHit, true, PlatformTrigger.OnHitWithEdge, edgeType) { }
}

public class PlatformLoseControl : BasePlatform {
    public PlatformLoseControl(DiceEdgeType edgeType) : base(PlatformType.LoseControl, false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

public class PlatformRestoreEdge : BasePlatform {
    public PlatformRestoreEdge() : base(PlatformType.RestoreEdge, false, PlatformTrigger.OnHit) { }
}

public class PlatformShield : BasePlatform {
    public PlatformShield(DiceEdgeType edgeType) : base(PlatformType.Shield, true, PlatformTrigger.OnHitWithEdge, edgeType) { }
}

public class PlatformTurnLimit : BasePlatform {
    public PlatformTurnLimit(DiceEdgeType edgeType) : base(PlatformType.TurnLimit, false, PlatformTrigger.OnHitWithEdgeAlt, edgeType) { }
}

