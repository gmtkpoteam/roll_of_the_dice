using System.Collections.Generic;
using UnityEngine;

public class PlatformManager
{
    // Custom-??????????? ?? ?????? ????? ???????, ????? ????????? ???????. ????? - ????? ??????? ??????.
    public BasePlatform GetNextPlatform(GameObject platformObject, PlatformType customPlatformType = default, DiceEdgeType customEdgeType = default) {
        var platformType = customPlatformType != default 
            ? customPlatformType 
            : (PlatformType)Random.Range(0, System.Enum.GetNames(typeof(PlatformType)).Length - 1);

        var edgeType = customEdgeType != default 
            ? customEdgeType 
            : (DiceEdgeType)Random.Range(1, System.Enum.GetNames(typeof(DiceEdgeType)).Length - 1);

        switch (platformType) {
            case PlatformType.Empty:            return new PlatformEmpty(platformObject);
            case PlatformType.BreaksEdgeOnHit:  return new PlatformBreaksEdgeOnHit(platformObject, edgeType);
            case PlatformType.BreaksRandomEdge: return new PlatformBreaksRandomEdge(platformObject);
            case PlatformType.LoseControl:      return new PlatformLoseControl(platformObject, edgeType);
            case PlatformType.JumpOnHit:        return new PlatformJumpOnHit(platformObject, edgeType);
            case PlatformType.RestoreEdge:      return new PlatformRestoreEdge(platformObject);
            case PlatformType.ScoreOnHit:       return new PlatformScoreOnHit(platformObject, edgeType);
            case PlatformType.Invulnerability:  return new PlatformInvulnerability(platformObject, edgeType);
            default: return default;
        }
    }

    public BasePlatform GetNextPlatformWithoutDisabledEdges(
        GameObject platformObject,
        Dictionary<DiceEdgeType, DiceEdge> edges, 
        PlatformType customPlatformType = default, 
        bool withoutBroken = true
    ) {
        var filteredEdgeTypes = new List<DiceEdgeType>();

        foreach (var edge in edges) {
            if (withoutBroken && edge.Value.broken) continue;

            filteredEdgeTypes.Add(edge.Key);
        }

        var edgeType = filteredEdgeTypes.Count == 0 
            ? default
            : filteredEdgeTypes[Random.Range(0, filteredEdgeTypes.Count)];

        return GetNextPlatform(platformObject, customPlatformType, edgeType);
    }
}
