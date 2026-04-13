using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

public class RoomFirstGenerator : DungeonGenerator
{

    //min room size
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    //dungeon size for bsp splitting
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;


    [SerializeField]
    private GameObject player;

    [Button]
    public override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        //use bsp to split the dungeon
        var roomsList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        //store rom centers to later connect corridors and spawn player
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            //set player spawn point
            player.transform.position = new Vector3(roomCenters[0].x, roomCenters[0].y, 0);
            Debug.Log("Player position is " + player.transform.position);
        }

        //Create corridors
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);


        //spawn tiles
        tileMapSpawner.SpawnFloorBiomeRooms(floor);
        WallGenerator.CreateWalls(floor, tileMapSpawner);
    }

    //connect the rooms with corridors
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        //start with random room center
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        //connect them all until none are left
        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPoint(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    //create organic rooms using random walk
    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(
                Mathf.RoundToInt(roomBounds.center.x),
                Mathf.RoundToInt(roomBounds.center.y)
            );

            int seed = Random.Range(0, 1000);
            var roomFloor = RunRandomWalk(paramatersSO, roomCenter, seed);

            HashSet<Vector2Int> filteredRoom = new HashSet<Vector2Int>();

            foreach (var position in roomFloor)
            {
                if ((position.x >= (roomBounds.xMin + offset)) &&
                    (position.x <= (roomBounds.xMax - offset)) &&
                    (position.y >= (roomBounds.yMin + offset)) &&
                    (position.y <= (roomBounds.yMax - offset)))
                {
                    filteredRoom.Add(position);
                    floor.Add(position);
                }
            }

        }

        return floor;
    }
    // create L shaped corridor
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y) 
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if(destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    //find the closest room center to the current room
    private Vector2Int FindClosestPoint(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }

        }
        return closest;
    }

    //Create Simple rectangular rooms
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
