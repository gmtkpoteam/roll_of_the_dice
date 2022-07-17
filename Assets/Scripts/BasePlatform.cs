using UnityEngine;

public enum PlatformType {
    // Негативные
    BreaksEdgeOnSkip, // Ломает грань, если перепрыгнул
    BreaksEdgeOnHit,  // Ломает грань, если попал не нужной гранью
    BreaksRandomEdge, // Ломает случайную грань
    LoseControl,      // Теряется управление на N ходов

    //Позитивные
    JumpOnHit,        // Перепрыгивает следующую платформу, если попал нужной гранью
    RestoreEdge,      // Восстанавлливает грань, если попал сломанной гранью
    ScoreOnHit,       // Дает больше очков, если попал нужной гранью
    ScoreOnSkip,      // Дает больше очков, если перепрыгнул
    Shield,           // Дает щит, если попал нужной гранью
    Invulnerability,  // Дает сопротивление всем негативным эффектам на N ходов
    Empty,            // Без действий

}

public enum PlatformTrigger {
    OnHit,            // При попадании любой гранью
    OnHitWithEdge,    // При попадании конкретной гранью
    OnHitWithEdgeAlt, // При попадании любой гранью, кроме конкретной
    OnSkip            // При перепрыгивании
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

        Controller.SetText(text + (int)edgeType, positive ? Color.green : Color.red);
        Controller.SetPlatformColor(positive ? "green" : "red");
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
        "неуязвимость\nесли УСПЕХ",
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
        "доп очки\nесли УСПЕХ",
        true, 
        PlatformTrigger.OnHitWithEdge, 
        edgeType
    ) { }
}

public class PlatformScoreOnSkip : BasePlatform {
    public PlatformScoreOnSkip(GameObject platformObject) : base(
        platformObject, 
        PlatformType.ScoreOnSkip, 
        "SS", 
        "доп очки\nесли ПРЫЖОК",
        true, 
        PlatformTrigger.OnSkip
    ) { }
}

public class PlatformBreaksEdgeOnHit : BasePlatform {
    public PlatformBreaksEdgeOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.BreaksEdgeOnHit, 
        "BH", 
        "ломает грань\nесли ПРОВАЛ",
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
        "ломает грань\nслучайно",
        false,
        PlatformTrigger.OnHit
    ) { }
}

public class PlatformBreaksEdgeOnSkip : BasePlatform {
    public PlatformBreaksEdgeOnSkip(GameObject platformObject) : base(
        platformObject, 
        PlatformType.BreaksEdgeOnSkip, 
        "BS", 
        "ломает грань\nесли ПРЫЖОК",
        false, 
        PlatformTrigger.OnSkip
    ) { }
}

public class PlatformJumpOnHit : BasePlatform {
    public PlatformJumpOnHit(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.JumpOnHit, 
        "JP", 
        "прыжок\nесли УСПЕХ",
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
        "потеря контроля\nесли ПРОВАЛ",
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
        "чинит грань",
        true, 
        PlatformTrigger.OnHit
    ) { }
}

public class PlatformShield : BasePlatform {
    public PlatformShield(GameObject platformObject, DiceEdgeType edgeType) : base(
        platformObject, 
        PlatformType.Shield, 
        "SD", 
        "щит\nесли УСПЕХ",
        true, 
        PlatformTrigger.OnHitWithEdge, 
        edgeType
    ) { }
}

public class PlatformEmpty : BasePlatform {
    public PlatformEmpty(GameObject platformObject) : base(
        platformObject, 
        PlatformType.Empty, 
        "EM", 
        "ничего",
        true, 
        PlatformTrigger.OnHit
    ) { }
}

