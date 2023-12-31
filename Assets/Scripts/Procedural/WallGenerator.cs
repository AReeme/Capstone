using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector3Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction3D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction3D.diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector3Int> cornerWallPositions, HashSet<Vector3Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighborsBinaryType = GetNeighborsBinaryType(position, floorPositions, Direction3D.cardinalDirectionsList);
            tilemapVisualizer.PaintSingleCornerWall(position, neighborsBinaryType);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector3Int> basicWallPositions, HashSet<Vector3Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighborsBinaryType = GetNeighborsBinaryType(position, floorPositions, Direction3D.diagonalDirectionsList);
            tilemapVisualizer.PaintSingleBasicWall(position, neighborsBinaryType);
        }
    }

    private static string GetNeighborsBinaryType(Vector3Int position, HashSet<Vector3Int> floorPositions, List<Vector3Int> directionList)
    {
        string neighborsBinaryType = "";
        foreach (var direction in directionList)
        {
            var neighborPosition = position + direction;
            if (floorPositions.Contains(neighborPosition))
            {
                neighborsBinaryType += "1";
            }
            else
            {
                neighborsBinaryType += "0";
            }
        }
        return neighborsBinaryType;
    }

    private static HashSet<Vector3Int> FindWallsInDirections(HashSet<Vector3Int> floorPositions, List<Vector3Int> directionList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighborPosition = position + direction;
                if (!floorPositions.Contains(neighborPosition))
                {
                    wallPositions.Add(neighborPosition);
                }
            }
        }
        return wallPositions;
    }
}