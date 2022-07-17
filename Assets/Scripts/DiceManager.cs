using System.Collections.Generic;
using UnityEngine;

public class DiceManager
{
    private readonly Dictionary<DiceEdgeType, DiceEdge> Edges = new Dictionary<DiceEdgeType, DiceEdge>();
    private int shield = 0;
    private int invulnerabilitySteps = 0;
    private int loseControlSteps = 0;

    public DiceManager() {
        InitEdges();
    }

    public void ResetAll()
    {
        shield = 0;
        invulnerabilitySteps = 0;
        loseControlSteps = 0;
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

    public int GetAliveEdgesCount() {
        var count = 0;
        foreach (var edge in Edges) {
            if (!edge.Value.broken) {
                count++;
            }
        }

        return count;
    }

    public DiceEdge GetRandomEdge(bool withBroken = true) {
        var filteredEdges = new Dictionary<int, DiceEdge>();
        var i = 0;
        foreach (var edge in Edges) {
            if (withBroken) {
                filteredEdges.Add(i, edge.Value);
                ++i;
                continue;
            }

            if (!edge.Value.broken) {
                filteredEdges.Add(i, edge.Value);
                ++i;
            }
        }

        return filteredEdges.Count > 0 ? filteredEdges[Random.Range(0, filteredEdges.Count - 1)] : default;
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

    public void DecreaseLoseControl() { loseControlSteps = loseControlSteps > 0 ? loseControlSteps - 1 : 0; }
    public void ClearLoseControl() { loseControlSteps = 0; }
    public void AddLoseControl() { loseControlSteps = 3; }
    public bool IsLoseControl() { return loseControlSteps > 0; }


}
