using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
    [SerializeField] private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

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
