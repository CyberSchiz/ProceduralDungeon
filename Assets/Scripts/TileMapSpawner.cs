using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

// Places tiles into Unity Tilemaps
public class TileMapSpawner : MonoBehaviour
{
    [SerializeField]
    public Tilemap floor, wall;

    [SerializeField]
    private TileBase floorTile, wallTop;

    // Places floor tiles for every floor position
    public void SpawnFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        SpawnTiles(floorPos, floor, floorTile);

    }

    // Generic tile spawning method
    public void SpawnTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var pos in positions)
        {
            SpawnSingleTile(tilemap, tile, pos);
        }

    }
    // Places a single tile in the tilemap
    private void SpawnSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    internal void SpawnSingleWall(Vector2Int position)
    {
        SpawnSingleTile(wall, wallTop, position);
    }
}
