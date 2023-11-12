using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTest : MonoBehaviour
{
    public TileBase[] groundTiles;
    public TileBase[] propTiles;
    public int mapWidth = 20;
    public int mapHeight = 20;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap propTilemap;

    private List<RectInt> rooms;

    void Start()
    {
        if (groundTilemap == null || propTilemap == null)
        {
            Debug.LogError("Tilemaps not assigned! Please assign Tilemaps in the inspector.");
            return;
        }

        Debug.Log("Generating Map...");
        GenerateMap();
        Debug.Log("Generating Props...");
        GenerateProps();
        Debug.Log("Generation Complete!");
    }

    void GenerateMap()
    {
        groundTilemap.ClearAllTiles();
        rooms = new List<RectInt>();
        CreateRooms(new RectInt(0, 0, mapWidth, mapHeight));
        CreateCorridors();

        foreach (var room in rooms)
        {
            for (int x = room.x; x < room.x + room.width; x++)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[0]);
                }
            }
        }
    }

    void CreateRooms(RectInt area)
    {
        int minWidth = 4;
        int minHeight = 4;

        if (area.width < minWidth || area.height < minHeight)
            return;

        int split = Random.Range(0, 2); // 0 for horizontal split, 1 for vertical split

        if (split == 0)
        {
            int splitPoint = Random.Range(minHeight, area.height - minHeight + 1);
            rooms.Add(new RectInt(area.x, area.y, area.width, splitPoint));
            rooms.Add(new RectInt(area.x, area.y + splitPoint, area.width, area.height - splitPoint));
        }
        else
        {
            int splitPoint = Random.Range(minWidth, area.width - minWidth + 1);
            rooms.Add(new RectInt(area.x, area.y, splitPoint, area.height));
            rooms.Add(new RectInt(area.x + splitPoint, area.y, area.width - splitPoint, area.height));
        }

        CreateRooms(rooms[rooms.Count - 2]);
        CreateRooms(rooms[rooms.Count - 1]);
    }

    void CreateCorridors()
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int center1 = new Vector2Int(rooms[i].x + rooms[i].width / 2, rooms[i].y + rooms[i].height / 2);
            Vector2Int center2 = new Vector2Int(rooms[i + 1].x + rooms[i + 1].width / 2, rooms[i + 1].y + rooms[i + 1].height / 2);

            CreateCorridor(center1, center2);
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        int corridorWidth = 2; // Adjust as needed

        if (Random.value < 0.5f)
        {
            for (int x = Mathf.Min(start.x, end.x); x <= Mathf.Max(start.x, end.x); x++)
            {
                for (int y = start.y - corridorWidth / 2; y <= start.y + corridorWidth / 2; y++)
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[0]);
                }
            }
        }
        else
        {
            for (int y = Mathf.Min(start.y, end.y); y <= Mathf.Max(start.y, end.y); y++)
            {
                for (int x = start.x - corridorWidth / 2; x <= start.x + corridorWidth / 2; x++)
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[0]);
                }
            }
        }
    }

    void GenerateProps()
    {
        foreach (var room in rooms)
        {
            for (int x = room.x; x < room.x + room.width; x++)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    // Adjust the probability as needed
                    if (Random.value < 0.1f) // 10% chance to place a prop
                    {
                        TileBase propTile = propTiles[Random.Range(0, propTiles.Length)];
                        propTilemap.SetTile(new Vector3Int(x, y, 0), propTile);
                    }
                }
            }
        }
    }
}
