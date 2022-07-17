using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    private readonly Dictionary<DiceEdgeType, DiceEdge> Edges = new Dictionary<DiceEdgeType, DiceEdge>();
    private int shield = 0;
    private int invulnerabilitySteps = 0;

    public DiceManager() {
        InitEdges();
    }

    
    // TODO надо связать Edges с соответствующими монобехами
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

    public DiceEdge GetEdge(DiceEdgeType edgeType) {
        Edges.TryGetValue(edgeType, out var edge);
        return edge;
    }

    public bool OnShield() { return shield > 0; }
    public int GetShield() { return shield; }
    public void AddShield() { shield = 1; }
    public bool RemoveShield() {
        if (shield == 0) return false;

        shield = 0;
        return true;
    }
    public bool IsInvulnerability() { return invulnerabilitySteps > 0; }
    public void DecreaseInvulnerability() { invulnerabilitySteps = invulnerabilitySteps > 0 ? invulnerabilitySteps - 1 : 0; }
    public void AddInvulnerability() { invulnerabilitySteps = 3; }

    public void DecreaseBlocked() {
        foreach (var edge in Edges) {
            edge.Value.DecreaseBlocked();
        }
    }

}
