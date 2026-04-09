using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

// Static helper class containing algorithms used for dungeon generation
public static class ProceduralGeneration
{
    // Performs a random walk starting from a position.
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);

        Vector2Int previousPosition = startPosition;

        // Walk in random directions
        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int newPosition = previousPosition + Direction2D.GetRandomDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    // Splits the dungeon area recursively into smaller sections to create rooms
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            // Check if room can still be split
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                // Randomly decide whether to split horizontally or vertically
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int xSplit = Random.Range(1, room.size.x);

        BoundsInt room1 = new BoundsInt(
            room.min,
            new Vector3Int(xSplit, room.size.y, room.size.z)
        );

        BoundsInt room2 = new BoundsInt(
            new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)
        );

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int ySplit = Random.Range(1, room.size.y);

        BoundsInt room1 = new BoundsInt(
            room.min,
            new Vector3Int(room.size.x, ySplit, room.size.z)
        );

        BoundsInt room2 = new BoundsInt(
            new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)
        );

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }


    // Helper class for working with cardinal directions
    public static class Direction2D
    {
        public static List<Vector2Int> DirectionsList = new List<Vector2Int>
        {
            new Vector2Int(0, 1), //UP
            new Vector2Int(1, 0), //RIGHT
            new Vector2Int(0, -1),//DOWN
            new Vector2Int(-1, 0) //LEFT
        };

        // Returns a random direction from the list
        public static Vector2Int GetRandomDirection()
        {

            return DirectionsList[Random.Range(0, DirectionsList.Count)];
        }
    }
}