using UnityEngine;

[System.Serializable]
public class FurniturePlacement
{
    public GameObject prefab;
    public Vector2Int position;

    public FurniturePlacement(GameObject prefab, Vector2Int position)
    {
        this.prefab = prefab;
        this.position = position;
    }
}