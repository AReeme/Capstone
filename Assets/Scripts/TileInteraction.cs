using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInteraction : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap containing wall tiles.

    public bool IsNextPositionWall(Vector2 nextPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(nextPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            CustomTile customTile = tile as CustomTile;
            if (customTile != null && customTile.isWall)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsNextPositionWall(transform.position) ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
    }
}