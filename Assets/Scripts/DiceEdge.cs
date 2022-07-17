using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceEdgeType {
    Empty = 0,
    Base = 1,   // ��� ������� - x2 = ������, ����� ���������
    Double = 2, // ������� ������ �� ����� �� ��������� ���������
    Jump = 3,   // ������������� ��������� ��������� - x2 = ��� ���������
    Time = 4,   // ���������� ������ (�������� �������� �������� �������) - x2 = ������� ����������
    Shield = 5, // ���� 1 ���� - x2 = 2 ����
    Score = 6   // ���� N ����� - x2 = � ��� ���� ������
}

public class DiceEdge
{
    private DiceEdgeType Type;
    public bool broken = false;
    public int blockedSteps = 0;

    public DiceEdge(DiceEdgeType type) {
        Type = type;
    }
    
    public DiceEdgeType GetDiceEdgeType() { return Type; }
    public bool IsBlocked() { return blockedSteps > 0; }

    public void DecreaseBlocked() { blockedSteps = blockedSteps > 0 ? blockedSteps - 1 : 0; }
}
