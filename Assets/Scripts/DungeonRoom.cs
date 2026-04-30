using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public HashSet<Vector2Int> floorTiles;
    public Vector2Int center;
    public RoomType roomType;
    public bool isSpawnRoom;
    public List<FurniturePlacement> furniturePlacements;

    public DungeonRoom(HashSet<Vector2Int> floorTiles, Vector2Int center)
    {
        this.floorTiles = floorTiles;
        this.center = center;
        roomType = RoomType.None;
        isSpawnRoom = false;
        furniturePlacements = new List<FurniturePlacement>();
    }
}