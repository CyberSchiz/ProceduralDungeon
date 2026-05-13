using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomDecorator : MonoBehaviour
{
    [SerializeField] private GameObject bookshelfPrefab;
    [SerializeField] private GameObject armorStandPrefab;
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private GameObject barrelPrefab;

    [Header("Generic Dungeon Decorations")]
    [SerializeField] private GameObject[] genericDecorPrefabs;
    [SerializeField] private int minGenericDecorPerRoom = 0;
    [SerializeField] private int maxGenericDecorPerRoom = 3;

    [Header("Generic Dungeon Gameplay Items")]
    [SerializeField] private GameObject[] genericItemEffects;
    [SerializeField] private int minGenericItemsPerRoom = 0;
    [SerializeField] private int maxGenericItemsPerRoom = 2;

    [Header("Enemies")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int minEnemiesPerRoom = 0;
    [SerializeField] private int maxEnemiesPerRoom = 2;

    public void AssignFurnitureToRooms(List<DungeonRoom> rooms, HashSet<Vector2Int> corridors)
    {
        foreach (var room in rooms)
        {
            if (room.isSpawnRoom || room.roomType == RoomType.None)
                continue;

            switch (room.roomType)
            {
                case RoomType.Library:
                    AddFurniturePlacements(room, bookshelfPrefab, 3, true);
                    break;

                case RoomType.Armory:
                    AddFurniturePlacements(room, armorStandPrefab, 2, true);
                    break;

                case RoomType.Wineary:
                    AddFurniturePlacements(room, cratePrefab, 4, false);
                    break;

                case RoomType.CrateStorage:
                    AddFurniturePlacements(room, barrelPrefab, 1, true);
                    break;
            }

            AssignGenericDecorToRoom(room, corridors);
            AssignGenericItemsToRoom(room, corridors);
            AssignEnemiesToRoom(room, corridors);
        }
    }

    private void AssignGenericDecorToRoom(DungeonRoom room, HashSet<Vector2Int> corridors)
    {
        AssignRandomObjectsToRoom(
            room,
            corridors,
            genericDecorPrefabs,
            minGenericDecorPerRoom,
            maxGenericDecorPerRoom
        );
    }

    private void AssignGenericItemsToRoom(DungeonRoom room, HashSet<Vector2Int> corridors)
    {
        AssignRandomObjectsToRoom(
            room,
            corridors,
            genericItemEffects,
            minGenericItemsPerRoom,
            maxGenericItemsPerRoom
        );
    }

    private void AssignRandomObjectsToRoom(
        DungeonRoom room,
        HashSet<Vector2Int> corridors,
        GameObject[] prefabArray,
        int minAmount,
        int maxAmount)
    {
        if (prefabArray == null || prefabArray.Length == 0)
            return;

        if (room.isSpawnRoom)
            return;

        int objectCount = Random.Range(minAmount, maxAmount + 1);

        List<Vector2Int> candidateTiles = new List<Vector2Int>(room.floorTiles);

        for (int i = 0; i < objectCount && candidateTiles.Count > 0; i++)
        {
            int tileIndex = Random.Range(0, candidateTiles.Count);
            Vector2Int chosenTile = candidateTiles[tileIndex];
            candidateTiles.RemoveAt(tileIndex);

            if (corridors.Contains(chosenTile))
                continue;

            int prefabIndex = Random.Range(0, prefabArray.Length);
            GameObject chosenPrefab = prefabArray[prefabIndex];

            if (chosenPrefab != null)
            {
                room.furniturePlacements.Add(new FurniturePlacement(chosenPrefab, chosenTile));
            }
        }
    }

    public void SpawnFurniture(List<DungeonRoom> rooms)
    {
        foreach (var room in rooms)
        {
            foreach (var furniture in room.furniturePlacements)
            {
                Instantiate(
                    furniture.prefab,
                    new Vector3(furniture.position.x + 0.5f, furniture.position.y + 0.5f, -1),
                    Quaternion.identity
                );
            }
        }
    }

    private void AddFurniturePlacements(DungeonRoom room, GameObject prefab, int count, bool nearWallOnly)
    {
        if (prefab == null)
            return;

        List<Vector2Int> candidateTiles = new List<Vector2Int>();

        foreach (var tile in room.floorTiles)
        {
            if (!nearWallOnly || IsNearWall(tile, room.floorTiles))
            {
                candidateTiles.Add(tile);
            }
        }

        for (int i = 0; i < count && candidateTiles.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, candidateTiles.Count);
            Vector2Int chosenTile = candidateTiles[randomIndex];
            candidateTiles.RemoveAt(randomIndex);

            room.furniturePlacements.Add(new FurniturePlacement(prefab, chosenTile));
        }
    }

    private void AssignEnemiesToRoom(DungeonRoom room, HashSet<Vector2Int> corridors)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        if (room.isSpawnRoom)
            return;

        int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);

        List<Vector2Int> candidateTiles = new List<Vector2Int>();

        foreach (var tile in room.floorTiles)
        {
            if (IsNextToCorridor(tile, corridors))
                continue;

            candidateTiles.Add(tile);
        }

        for (int i = 0; i < enemyCount && candidateTiles.Count > 0; i++)
        {
            int tileIndex = Random.Range(0, candidateTiles.Count);
            Vector2Int chosenTile = candidateTiles[tileIndex];
            candidateTiles.RemoveAt(tileIndex);

            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject chosenEnemy = enemyPrefabs[enemyIndex];

            if (chosenEnemy != null)
            {
                room.furniturePlacements.Add(new FurniturePlacement(chosenEnemy, chosenTile));
            }
        }
    }

    private bool IsNextToCorridor(Vector2Int tile, HashSet<Vector2Int> corridors)
    {
        Vector2Int[] directions =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        foreach (var dir in directions)
        {
            if (corridors.Contains(tile + dir))
                return true;
        }

        return false;
    }

    private bool IsNearWall(Vector2Int tile, HashSet<Vector2Int> roomTiles)
    {
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            if (!roomTiles.Contains(tile + dir))
                return true;
        }

        return false;
    }
}