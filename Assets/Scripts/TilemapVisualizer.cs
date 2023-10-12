using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap, propTilemap;
    [SerializeField] private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;
    [SerializeField] private TileBase propTile1, propTile2;
    [SerializeField] private GameObject[] propPrefabs; // Add an array of prop prefabs

    public void PaintFloorTiles(IEnumerable<Vector3Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector3Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    public void PaintProps(IEnumerable<Vector3Int> propPositions)
    {
        PaintTiles(propPositions, propTilemap, propTile1); // Paint the props using the new Tilemap and TileBase
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector3Int position)
    {
        // Set the tile on the isometric tilemap
        tilemap.SetTile(position, tile);
    }

    internal void PaintSingleBasicWall(Vector3Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (tile != null)
        {
            // Set the tile on the isometric tilemap
            wallTilemap.SetTile(position, tile);
        }
    }

    private Vector3Int ConvertToIsometricCoordinates(Vector2Int position)
    {
        // Convert Cartesian coordinates to isometric coordinates
        int x = position.x - position.y;
        int y = (position.x + position.y) / 2;
        return new Vector3Int(x, y, 0);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        propTilemap.ClearAllTiles(); // Clear the prop Tilemap
    }

    internal void PlaceRandomProps(int numberOfProps)
    {
        Debug.Log("PlaceRandomProps called");

        // Get the bounds of the propTilemap to ensure props are placed within the room
        BoundsInt propBounds = propTilemap.cellBounds;

        for (int i = 0; i < numberOfProps; i++)
        {
            // Randomly select a prop prefab from the array
            GameObject propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];

            // Randomly choose a position within the propTilemap bounds
            Vector3Int randomPosition = new Vector3Int(
                Random.Range(propBounds.x, propBounds.x + propBounds.size.x),
                Random.Range(propBounds.y, propBounds.y + propBounds.size.y),
                0
            );

            Debug.Log("Random position: " + randomPosition);

            // Check if the selected position is empty (no existing tile or prop)
            if (propTilemap.GetTile(randomPosition) == null)
            {
                // Instantiate and place the selected prop prefab at the random position
                GameObject propInstance = Instantiate(propPrefab);
                propInstance.transform.position = propTilemap.GetCellCenterWorld(randomPosition);
            }
        }
    }

    internal void PaintSingleProp(Vector3Int position, TileBase propTile)
    {
        // Adjust the position to match isometric coordinates
        Vector3Int isoPosition = ConvertToIsometricCoordinates(new Vector2Int(position.x, position.y));

        // Set the tile on the prop Tilemap
        propTilemap.SetTile(isoPosition, propTile);
    }

    internal void PaintSingleCornerWall(Vector3Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        // Adjust the tile selection logic based on the isometric tileset
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            // Adjust the position to match isometric coordinates
            Vector3Int isoPosition = ConvertToIsometricCoordinates(new Vector2Int(position.x, position.y));
            wallTilemap.SetTile(isoPosition, tile);
        }
    }

    public bool IsFloorTile(Vector3Int position, HashSet<Vector3Int> floorPositions)
    {
        // In an isometric grid with Z-as-Y, check if the position exists in the floorPositions set.
        return floorPositions.Contains(position);
    }
}
