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
    private List<KeyValuePair<BiomeType, TileBase[]>> biomeList;

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

        biomeList = biomeTiles.ToList();
        TileBase tileToUse;
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
            if (tile.x <= 50 && tile.y <= 50)
            {

                biomeTileArray = biomeList[0].Value;
            }
            else if (tile.x > 60 && tile.y >= 60)
            {
                biomeTileArray = biomeList[1].Value;
            }
            else
            {
                biomeTileArray = biomeList[2].Value;
            }

            // using perlin noise to get a random tile from the selected biome

            float noise = Mathf.PerlinNoise(tile.x * 0.2f, tile.y * 0.2f);

            int index = Mathf.FloorToInt(noise * biomeTileArray.Length);

            index = Mathf.Clamp(index, 0, biomeTileArray.Length - 1);
             Debug.Log($"noise={noise}, index={index}, length={biomeTileArray.Length}");
            tileToUse = biomeTileArray[index];
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
        TileBase wallTileToUse;

        if (position.x <= 50 && position.y <= 50)
        {
            wallTileToUse = GetWallTileFromBiome(biomeList[0].Key);
        }
        else if (position.x > 60 && position.y >= 60)
        {
            wallTileToUse = GetWallTileFromBiome(biomeList[1].Key);
        }
        else
        {
            wallTileToUse = GetWallTileFromBiome(biomeList[2].Key);
        }

        SpawnSingleTile(wall, wallTileToUse, position);
    }

    private TileBase GetWallTileFromBiome(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Forest:
                return wallTop;
            case BiomeType.Lava:
                return lavaWall;
            case BiomeType.Ice:
                return iceWall;
            default:
                return wallTop;
        }
    }
}
