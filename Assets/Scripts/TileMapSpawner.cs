using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


// Places tiles into Unity Tilemaps
public class TileMapSpawner : MonoBehaviour
{
    [SerializeField]
    public Tilemap floor, wall;

    [SerializeField]
    private TileBase floorTile, floorTile2, floorTile3, wallTop,
        lavaFloor1, lavaFloor2, lavaFloor3, lavaWall,
        iceFloor1, iceFloor2, iceFloor3, iceWall;

    Dictionary<BiomeType, TileBase[]> biomeTiles;

    public enum BiomeType
    {
        Forest,
        Lava,
        Ice
    }

    // Places floor tiles for every floor position
    public void SpawnFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        SpawnTiles(floorPos, floor, floorTile);

    }

    public void SpawnFloorBiomeRooms(HashSet<Vector2Int> floorPos)
    {
        biomeTiles = new Dictionary<BiomeType, TileBase[]>()
        {
            { BiomeType.Forest, new TileBase[] { floorTile, floorTile2, floorTile3 } },
            { BiomeType.Lava, new TileBase[] { lavaFloor1, lavaFloor2, lavaFloor3 } },
            { BiomeType.Ice, new TileBase[] { iceFloor1, iceFloor2, iceFloor3 } }
        };

        List<KeyValuePair<BiomeType, TileBase[]>> biomeList = biomeTiles.ToList();

        // Shuffle the biome order
        for (int i = biomeList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            var temp = biomeList[i];
            biomeList[i] = biomeList[randomIndex];
            biomeList[randomIndex] = temp;
        }

        foreach (var tile in floorPos)
        {

                TileBase[] biomeTileArray;

                if (tile.x <= 5 && tile.y <= 5)
                {
                    biomeTileArray = biomeList[0].Value;
                }
                else if (tile.x > 5 && tile.y <= 5)
                {
                    biomeTileArray = biomeList[1].Value;
                }
                else
                {
                    biomeTileArray = biomeList[2].Value;
                }

                TileBase tileToUse = biomeTileArray[Random.Range(0, biomeTileArray.Length)];

                SpawnSingleTile(floor, tileToUse, tile);
            
        }
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
