using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParamaters_",menuName = "PCG/RandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEverytime = true;
}
