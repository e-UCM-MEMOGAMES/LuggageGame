using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;
    TrackerManager trackerManager;

    const int MAX_LIST_USES = 3;
    int remainingListUses = MAX_LIST_USES;
    public int RemainingListUses
    {
        get { return remainingListUses; }
        set { remainingListUses = value; }
    }


    /// <summary>
    /// Ruta de los archivos de localizacion de los niveles
    /// </summary>
    string levelItemsFilePath = Path.Combine("LevelInfo");


    [Header("Level settings")]
    [SerializeField] ItemsInfo itemsInfo;
    Dictionary<string, ItemProperties> itemsInfoDict;

    [SerializeField] SpawnpointsInfo spawnpointsInfo;
    Dictionary<string, Defs.SpawnType> spawnpointsInfoDict;

    Dictionary<string, ListItem> neededItems;
    HashSet<string> scenarioItems;
    HashSet<string> storedItems;

    [SerializeField] RectTransform itemsListTr;
    [SerializeField] RectTransform clothingTitleTr;
    [SerializeField] RectTransform footwearTitleTr;
    [SerializeField] RectTransform otherTitleTr;

    [SerializeField] GameObject listItemPrefab;

    [Header("Case")]
    [SerializeField] Image caseImg;
    [SerializeField] Sprite emptyTexture;
    [SerializeField] Sprite fullTexture;
    [SerializeField] GameObject normalCase;
    [SerializeField] GameObject topViewCase;

    [Header("Final panel")]
    [SerializeField] GameObject[] unlockedStars;
    [SerializeField] TextMeshProUGUI[] subObjectivesAmounts;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
        trackerManager = TrackerManager.Instance;

        for (int i = 0; i < unlockedStars.Length; i++)
        {
            unlockedStars[i].SetActive(false);
            unlockedStars[i].GetComponent<Animation>().Stop();
        }

        caseImg.sprite = emptyTexture;

        audioManager.Play(GameSound.LevelBGM);


        itemsInfoDict = new Dictionary<string, ItemProperties>();
        foreach (ItemProperties item in itemsInfo.List)
        {
            if (!itemsInfoDict.ContainsKey(item.Id))
            {
                itemsInfoDict.Add(item.Id, item);
            }
            else
            {
                if (Random.Range(0, 2) > 0)
                {
                    itemsInfoDict[item.Id] = item;
                }
            }
        }
        neededItems = new Dictionary<string, ListItem>();
        scenarioItems = new HashSet<string>();
        storedItems = new HashSet<string>();

        LoadItems();

        // TODO: TRACKER
    }

    private void LoadItems()
    {
        itemsInfoDict = new Dictionary<string, ItemProperties>();
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

        spawnpointsInfoDict = new Dictionary<string, Defs.SpawnType>();
        foreach (SpawnpointProperties point in spawnpointsInfo.List)
        {
            spawnpointsInfoDict.Add(point.Id, point.SpawnType);
        }

        neededItems = new Dictionary<string, ListItem>();
        scenarioItems = new HashSet<string>();
        storedItems = new HashSet<string>();

        // TEST
        gameManager.Level = 2;
        gameManager.Climate = Defs.Climate.WARM;

        string climate = gameManager.Climate.ToString().ToLower();
        climate = char.ToUpper(climate[0]) + climate.Substring(1);
        string fileName = gameManager.Level == 0 ? "Tutorial" : $"Level{gameManager.Level}{climate}";

        // TEST
        fileName = "Test";
        string filePath = Path.Combine(levelItemsFilePath, fileName);
        Debug.Log(filePath);


        TextAsset jsonFile = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        JObject jsonObject = JObject.Parse(jsonFile.text);
        Debug.Log(jsonFile);

        string genderInitial = (gameManager.PlayerGender.ToString()[0]).ToString();
        foreach (var obj in jsonObject)
        {
            JToken items = obj.Value;
            if (obj.Key == "objectList")
            {
                LoadNeededItems(items, genderInitial);
                LoadNeededItems(items, "N");
            }
            else if (obj.Key == "storagePoints")
            {
                LoadScenarioItems(items, genderInitial);
                LoadScenarioItems(items, "N");
            }
        }
    }

    private void LoadNeededItems(JToken items, string gender)
    {
        if (items[gender] != null)
        {
            List<string> list = items[gender].Values<string>().ToList();

            foreach (var item in list)
            {
                ItemProperties properties = itemsInfoDict.ContainsKey(item) ? itemsInfoDict[item] : null;
                RectTransform categoryTitleTr = otherTitleTr;

                if (properties != null) 
                {
                    if (properties.Category == Defs.ItemCategory.CLOTHING)
                    {
                        categoryTitleTr = clothingTitleTr;
                    }
                    else if (properties.Category == Defs.ItemCategory.FOOTWEAR)
                    {
                        categoryTitleTr = footwearTitleTr;
                    }

                    GameObject instance = Instantiate(listItemPrefab, itemsListTr);
                    instance.transform.SetSiblingIndex(categoryTitleTr.GetSiblingIndex() + 1);
                    ListItem listItem = instance.GetComponent<ListItem>();
                    listItem.SetName(properties.LocalizedName);

                    neededItems.Add(item, listItem);
                }
            }
        }
    }

    private void LoadScenarioItems(JToken items, string gender)
    {

    }

    public void StoreItem(string id)
    {
        storedItems.Add(id);
        audioManager.Play(GameSound.PutIn);
        caseImg.sprite = fullTexture;

        // TODO: TRACKER

        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(true);
        }
    }
    public void ReturnItem(string id)
    {
        storedItems.Remove(id);
        audioManager.Play(GameSound.TakeOut);
        if (storedItems.Count == 0)
        {
            caseImg.sprite = emptyTexture;
        }

        // TODO: TRACKER

        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(false);
        }
    }

    public void EndGame()
    {
        int totalStars = 0;
        int correctItems = 0;

        foreach (var item in neededItems)
        {
            if (storedItems.Contains(item.Key))
            {
                correctItems++;
            }
            else
            {
                item.Value.SetMissed(true);
            }
        }

        int incorrectItems = storedItems.Count - correctItems;
        int listUses = MAX_LIST_USES - remainingListUses;

        if (correctItems >= neededItems.Count)
        {
            totalStars++;

            if (correctItems == neededItems.Count)
            {
                totalStars++;

                if (incorrectItems == 0)
                {
                    totalStars++;

                    if (listUses == 0)
                    {
                        totalStars++;
                    }
                }
            }
        }

        for (int i = 0; i < totalStars; i++)
        {
            unlockedStars[i].SetActive(true);
            unlockedStars[i].GetComponent<Animation>().Play();
        }

        subObjectivesAmounts[0].text = $"{correctItems}/{neededItems.Count}";
        subObjectivesAmounts[1].text = $"{incorrectItems}";
        subObjectivesAmounts[2].text = $"{listUses}/{MAX_LIST_USES}";

        string levelName = Defs.GetLevelSaveKey(gameManager.Level, gameManager.Climate);

        if (!PlayerPrefs.HasKey(levelName) || (PlayerPrefs.HasKey(levelName) &&PlayerPrefs.GetInt(levelName) < totalStars))
        {
            PlayerPrefs.SetInt(levelName, totalStars);
        }


        // TODO: TRACKER
    }

}
