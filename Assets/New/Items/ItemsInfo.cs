using UnityEngine;


[CreateAssetMenu(fileName = "ItemsInfo", menuName = "LevelItems/ItemsInfo")]
public class ItemsInfo : ScriptableObject
{
    public ItemProperties[] List;
}
