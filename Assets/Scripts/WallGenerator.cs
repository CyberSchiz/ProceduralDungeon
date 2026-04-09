using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Creates walls around floor tiles
public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapSpawner tilemapSpawner)
    {
        //find all empty tiles surrounding floors
        var basicWallPositions = FindWallsInDirections(floorPositions, ProceduralGeneration.Direction2D.DirectionsList);
        //spawn walls in those positions
        foreach (var position in basicWallPositions) {

            tilemapSpawner.SpawnSingleWall(position);
        }
    }

    // Finds empty neighbouring tiles that should contain walls
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)

        {
            foreach( var direction in directionList)
            {
                var neighbourPosition = position + direction;
                // If neighbour is not floor, it becomes a wall
                if (floorPositions.Contains(neighbourPosition) == false)
                        wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }
}
