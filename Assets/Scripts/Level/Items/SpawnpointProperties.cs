using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "LevelItems/SpawnpointProperties")]
public class SpawnpointProperties : ScriptableObject
{
    public string Id;
    public Defs.SpawnType SpawnType;
    public Defs.SpawnPivot SpawnPivot;
}
