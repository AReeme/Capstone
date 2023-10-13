using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> potentialRoomPositions = new HashSet<Vector3Int>();

        List<List<Vector3Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector3Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector3Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        foreach (var corridor in corridors)
        {
            corridor.AddRange(IncreaseCorridorSizeByOne(corridor));
            floorPositions.UnionWith(corridor);
        }

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        // WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private List<Vector3Int> IncreaseCorridorSizeByOne(List<Vector3Int> corridor)
    {
        List<Vector3Int> newCorridor = new List<Vector3Int>();
        Vector3Int previousDirection = Vector3Int.zero;

        for (int i = 1; i < corridor.Count; i++)
        {
            Vector3Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector3Int.zero && directionFromCell != previousDirection)
            {
                // Handle corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector3Int(x, y, 0)); // Adjust for Z-as-Y
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                // Add cells on both sides of the original path
                Vector3Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
                newCorridor.Add(corridor[i - 1] - newCorridorTileOffset);

                // Additional cells for increased width
                newCorridor.Add(corridor[i - 1] + 2 * newCorridorTileOffset);
                newCorridor.Add(corridor[i - 1] - 2 * newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    private Vector3Int GetDirection90From(Vector3Int direction)
    {
        if (direction == Vector3Int.up) return new Vector3Int(1, 0, -1); // Adjust for Z-as-Y
        if (direction == Vector3Int.right) return new Vector3Int(0, -1, -1); // Adjust for Z-as-Y
        if (direction == Vector3Int.down) return new Vector3Int(-1, 0, -1); // Adjust for Z-as-Y
        if (direction == Vector3Int.left) return new Vector3Int(0, 1, -1); // Adjust for Z-as-Y

        return Vector3Int.zero;
    }

    private void CreateRoomsAtDeadEnd(List<Vector3Int> deadEnds, HashSet<Vector3Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (!roomFloors.Contains(position))
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector3Int> FindAllDeadEnds(HashSet<Vector3Int> floorPositions)
    {
        List<Vector3Int> deadEnds = new List<Vector3Int>();
        foreach (var position in floorPositions)
        {
            int neighborsCount = 0;
            foreach (var direction in Direction3D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighborsCount++;
                }
            }

            if (neighborsCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector3Int> CreateRooms(HashSet<Vector3Int> potentialRoomPositions)
    {
        HashSet<Vector3Int> roomPositions = new HashSet<Vector3Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector3Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private List<List<Vector3Int>> CreateCorridors(HashSet<Vector3Int> floorPositions, HashSet<Vector3Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector3Int>> corridors = new List<List<Vector3Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);

            foreach (var cell in corridor)
            {
                // Add cells to make the corridor three times as wide
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector3Int widenedCell = cell + new Vector3Int(x, y, 0);
                        floorPositions.Add(widenedCell);
                    }
                }
            }

            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
        }
        return corridors;
    }
}