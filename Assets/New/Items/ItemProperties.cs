using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Item", menuName = "LevelItems/Item")]
public class ItemProperties : ScriptableObject
{
    public string Id;
    public LocalizedString LocalizedName;
    public GameObject ItemPrefab, StoredItemPrefab;
    public Defs.ItemCategory Category;
}
