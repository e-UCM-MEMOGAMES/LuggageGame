using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Item", menuName = "LevelItems/Item")]
public class ItemProperties : ScriptableObject
{
    public LocalizedString LocalizedName;
    public GameObject ItemPrefab, StoredItemPrefab;
    public Defs.ItemCategory Category;
}