using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;


// Base class for all dungeon generation systems
// Contains shared functionality used by different generators
public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    //So's with parameters
    [SerializeField]
    protected SimpleRandomWalkSO paramatersSO;

    [SerializeField]
    protected TileMapSpawner tileMapSpawner;

    [SerializeField]
    protected int seed = 0;
    
    //main generation method (can be overrided)
    [Button]
    public virtual void RunProceduralGeneration()
    {
        //this can be commented out to use the same seed and debug but in order to create random rooms the seed is also randomly generated
        seed = UnityEngine.Random.Range(0, 10000);
        Debug.Log("Change seed to" + seed);
        //Generate random floor with random walk
        HashSet<Vector2Int> floorPos = RunRandomWalk(paramatersSO, startPosition, seed);
        //spawn tiles
        tileMapSpawner.SpawnFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tileMapSpawner);
    }

    [Button]
    public void ResetTileMap()
    {
        tileMapSpawner.floor.ClearAllTiles();
    }
    [Button]
    public void ResetWallTileMap()
    {
        tileMapSpawner.wall.ClearAllTiles();
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO paramaters, Vector2Int position, int seed)
    {
        Random.InitState(seed);

        var currentPos = position;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < paramaters.iterations; i++)
        {
            var path = ProceduralGeneration.SimpleRandomWalk(currentPos, paramaters.walkLength);
            floorPos.UnionWith(path);

            if (paramaters.startRandomlyEverytime)
                currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
        }

        return floorPos;
    }
}
