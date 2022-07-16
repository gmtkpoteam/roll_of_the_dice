using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceEdgeType {
    Empty = 0,
    Base = 1,   // Без эффекта - x2 = ничего, бонус пропадает
    Double = 2, // Двойной эффект от грани на следующей платформе
    Jump = 3,   // Перепрыгивает следующую платформу - x2 = две платформы
    Time = 4,   // Замедление полета (скорость вращения остается прежней) - x2 = двойное замедление
    Shield = 5, // Дает 1 щита - x2 = 2 щита
    Score = 6   // Дает N очков - x2 = в два раза больше
}

public class DiceEdge
{
    private DiceEdgeType Type;
    public bool Broken = false;
    public int BlockedSteps = 0;

    public DiceEdge(DiceEdgeType type) {
        Type = type;
    }
    
    public DiceEdgeType GetDiceEdgeType() { return Type; }
    public bool IsBlocked() { return BlockedSteps > 0; }
}
