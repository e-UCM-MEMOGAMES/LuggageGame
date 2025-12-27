using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    GameManager gameManager;
    
    [SerializeField] LevelManager levelManager;

    /// <summary>
    /// Ruta de los archivos de localizacion de los niveles
    /// </summary>
    string levelItemsFilePath = Path.Combine("LevelInfo");


    [Header("Items settings")]
    [SerializeField] ItemsInfo itemsInfo;
    Dictionary<string, ItemProperties> itemsInfoDict = new Dictionary<string, ItemProperties>();


    [Header("Spawnpoints settings")]
    [SerializeField] Spawnpoint[] spawnpoints;

    [System.Serializable]
    public struct Spawnpoint
    {
        public SpawnpointProperties PointProperties;
        public RectTransform[] Points;
    }
    Dictionary<string, Spawnpoint> spawnpointsInfoDict = new Dictionary<string, Spawnpoint>();


    List<ItemProperties> neededItems = new List<ItemProperties>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        foreach (ItemProperties item in itemsInfo.List)
        {
            if (!itemsInfoDict.ContainsKey(item.Id))
            {
                itemsInfoDict.Add(item.Id, item);
            }
            else if (Random.Range(0, 2) > 0)
            {
                itemsInfoDict[item.Id] = item;
            }
        }
        levelManager.LoadItemsInfo(itemsInfoDict);

        foreach (Spawnpoint point in spawnpoints)
        {
            spawnpointsInfoDict.Add(point.PointProperties.Id, point);
        }


        LoadItems();

    }


    private void LoadItems()
    {
        string climate = gameManager.Climate.ToString().ToLower();
        climate = char.ToUpper(climate[0]) + climate.Substring(1);
        string fileName = gameManager.Level == 0 ? "Tutorial" : $"Level{gameManager.Level}{climate}";

        // TEST
        //fileName = $"Level1Warm";
        string filePath = Path.Combine(levelItemsFilePath, fileName);

        TextAsset jsonFile = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        JObject jsonObject = JObject.Parse(jsonFile.text);
        //Debug.Log(jsonFile);

        string genderInitial = (gameManager.PlayerGender.ToString()[0]).ToString();

        if (jsonObject.ContainsKey("objectList"))
        {
            JToken items = jsonObject["objectList"];
            LoadNeededItems(items, genderInitial);
            LoadNeededItems(items, "N");
        }
        levelManager.AddListItems(neededItems);

        if (jsonObject.ContainsKey("spawnpoints"))
        {
            foreach (var spawnpoint in jsonObject["spawnpoints"])
            {
                JProperty prop = (JProperty)spawnpoint;
                string pointName = prop.Name;
                JToken gendersItems = prop.Value;

                if (spawnpointsInfoDict.ContainsKey(pointName))
                {
                    LoadSceneItems(gendersItems, pointName, genderInitial);
                    LoadSceneItems(gendersItems, pointName, "N");
                }
            }
        }
        levelManager.SortCaseItems();
    }

    private void LoadNeededItems(JToken items, string gender)
    {
        if (items[gender] != null)
        {
            List<string> list = items[gender].Values<string>().ToList();
            foreach (var item in list)
            {
                if (itemsInfoDict.ContainsKey(item))
                {
                    neededItems.Add(itemsInfoDict[item]);
                }
            }
        } 
    }

    private void LoadSceneItems(JToken items, string spawnpoint, string gender)
    {
        if (items[gender] != null)
        {
            foreach (var itemInfo in items[gender])
            {
                if (itemInfo["id"] != null && itemInfo["position"] != null)
                {
                    int position = (int)itemInfo["position"];
                    if (spawnpointsInfoDict[spawnpoint].Points.Count() > position)
                    {
                        levelManager.AddScenarioItem(
                            (string)itemInfo["id"], 
                            spawnpointsInfoDict[spawnpoint].Points[position],
                            spawnpointsInfoDict[spawnpoint].PointProperties.SpawnType,
                            spawnpointsInfoDict[spawnpoint].PointProperties.SpawnPivot
                        );
                    }
                }
            }
        }
    }
}
