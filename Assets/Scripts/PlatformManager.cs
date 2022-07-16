using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Custom-параметрами ты можешь четко указать, какую платформу ставить. Иначе - будет честный рандом.
    public BasePlatform GetNextPlatform(PlatformType customPlatformType = default, DiceEdgeType customEdgeType = default) {
        var platformType = customPlatformType != default 
            ? customPlatformType 
            : (PlatformType)Random.Range(0, System.Enum.GetNames(typeof(PlatformType)).Length - 1);

        var edgeType = customEdgeType != default 
            ? customEdgeType 
            : (DiceEdgeType)Random.Range(1, System.Enum.GetNames(typeof(DiceEdgeType)).Length - 1);

        switch (platformType) {
            case PlatformType.Block:            return new PlatformBlock();
            case PlatformType.TurnLimit:        return new PlatformTurnLimit(edgeType);
            case PlatformType.BreaksEdgeOnSkip: return new PlatformBreaksEdgeOnSkip();
            case PlatformType.BreaksEdgeOnHit:  return new PlatformBreaksEdgeOnHit(edgeType);
            case PlatformType.LoseControl:      return new PlatformLoseControl(edgeType);
            case PlatformType.JumpOnHit:        return new PlatformJumpOnHit(edgeType);
            case PlatformType.RestoreEdge:      return new PlatformRestoreEdge();
            case PlatformType.ScoreOnHit:       return new PlatformScoreOnHit(edgeType);
            case PlatformType.ScoreOnSkip:      return new PlatformScoreOnSkip();
            case PlatformType.Shield:           return new PlatformShield(edgeType);
            case PlatformType.Invulnerability:  return new PlatformInvulnerability(edgeType);
            default: return default;
        }
    }

    public BasePlatform GetNextPlatformWithoutDisabledEdges(
        Dictionary<DiceEdgeType, DiceEdge> edges, 
        PlatformType customPlatformType = default, 
        bool withoutBroken = true, 
        bool withoutBlocked = true
    ) {
        var filteredEdgeTypes = new List<DiceEdgeType>();

        foreach (var edge in edges) {
            if (withoutBroken && edge.Value.Broken) continue;
            if (withoutBlocked && edge.Value.IsBlocked()) continue;

            filteredEdgeTypes.Add(edge.Key);
        }

        var edgeType = filteredEdgeTypes.Count == 0 
            ? default
            : filteredEdgeTypes[Random.Range(0, filteredEdgeTypes.Count)];

        return GetNextPlatform(customPlatformType, edgeType);
    }
}
