using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsInfo", menuName = "LevelItems/ItemsInfo")]
public class ItemsInfo : ScriptableObject
{
    public List<Defs.ItemInfo> InfoList;
}