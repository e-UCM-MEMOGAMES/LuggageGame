using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Item", menuName = "LevelItems/Item")]
public class Item : ScriptableObject
{
    public LocalizedString LocalizedName;
    public Texture Img, StoredImg;
    public Defs.ItemCategory Category;
}

[CreateAssetMenu(fileName = "ItemsInfo", menuName = "LevelItems/ItemsInfo")]
public class ItemsInfo : ScriptableObject
{
    [System.Serializable]
    public struct ItemInfo
    {
        public string Id;
        public Item item;
    }

    public List<ItemInfo> itemsInfoList;
}