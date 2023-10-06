using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1; // Added offset variable
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt(Vector3Int.zero, new Vector3Int(dungeonWidth, dungeonHeight, 1)), minRoomWidth, minRoomHeight);

        HashSet<Vector3Int> floor = new HashSet<Vector3Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector3Int> roomCenters = new List<Vector3Int>();
        foreach (var room in roomsList)
        {
            // Convert the room center to Vector3Int
            Vector3Int roomCenter = new Vector3Int((int)room.center.x, (int)room.center.y, 0);
            roomCenters.Add(roomCenter);
        }

        HashSet<Vector3Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        // WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector3Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector3Int> floor = new HashSet<Vector3Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            for (int x = roomBounds.x; x < roomBounds.x + roomBounds.size.x; x++)
            {
                for (int y = roomBounds.y; y < roomBounds.y + roomBounds.size.y; y++)
                {
                    // Convert the position to Vector3Int
                    Vector3Int position = new Vector3Int(x, y, 0);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector3Int> ConnectRooms(List<Vector3Int> roomCenters)
    {
        HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector3Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector3Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector3Int> CreateCorridor(Vector3Int currentRoomCenter, Vector3Int destination)
    {
        HashSet<Vector3Int> corridor = new HashSet<Vector3Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position.y++;
            }
            else if (destination.y < position.y)
            {
                position.y--;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position.x++;
            }
            else if (destination.x < position.x)
            {
                position.x--;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector3Int FindClosestPointTo(Vector3Int currentRoomCenter, List<Vector3Int> roomCenters)
    {
        Vector3Int closest = Vector3Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector3.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector3Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector3Int> floor = new HashSet<Vector3Int>();
        foreach (var room in roomsList)
        {
            for (int x = room.x + offset; x < room.x + room.size.x - offset; x++)
            {
                for (int y = room.y + offset; y < room.y + room.size.y - offset; y++)
                {
                    // Convert the position to Vector3Int
                    Vector3Int position = new Vector3Int(x, y, 0);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}