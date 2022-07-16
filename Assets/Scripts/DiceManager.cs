using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private readonly Dictionary<DiceEdgeType, DiceEdge> Edges;
    private int shield = 0;

    void Start()
    {
        InitEdges();
    }

    // TODO ���� ������� Edges � ���������������� ����������
    public Dictionary<DiceEdgeType, DiceEdge> InitEdges() {
        Edges.Clear();

        Edges.Add(DiceEdgeType.Base, new DiceEdge(DiceEdgeType.Base));
        Edges.Add(DiceEdgeType.Double, new DiceEdge(DiceEdgeType.Double));
        Edges.Add(DiceEdgeType.Jump, new DiceEdge(DiceEdgeType.Jump));
        Edges.Add(DiceEdgeType.Time, new DiceEdge(DiceEdgeType.Time));
        Edges.Add(DiceEdgeType.Shield, new DiceEdge(DiceEdgeType.Shield));
        Edges.Add(DiceEdgeType.Score, new DiceEdge(DiceEdgeType.Score));

        return Edges;
    }

    public bool OnShield() { return shield > 0; }
    public int GetShield() { return shield; }
    public void AddShield() { ++shield; }
    public void RemoveShield() { shield = shield > 0 ? shield - 1 : 0; }

}