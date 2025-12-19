using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;

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
    [SerializeField] List<Defs.ItemInfo> itemsInfo;
    Dictionary<string, ItemProperties> itemsInfoDict;

    struct LevelItem
    {
        public ListItem ListItemComp;
        public bool Obtained;
        public GameObject ItemGameObject;
    }
    Dictionary<string, LevelItem> neededItems;

    [SerializeField] RectTransform itemsListTr;
    [SerializeField] RectTransform clothingTitleTr;
    [SerializeField] RectTransform footwearTitleTr;
    [SerializeField] RectTransform otherTitleTr;

    [SerializeField] GameObject listItemPrefab;

    public List<string> ObstaculosList { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;

        audioManager.Play(GameSound.LevelBGM);

        LoadItems();
    }

    private void LoadItems()
    {
        itemsInfoDict = new Dictionary<string, ItemProperties>();
        foreach (Defs.ItemInfo item in itemsInfo)
        {
            itemsInfoDict.Add(item.Id, item.item);
        }
        neededItems = new Dictionary<string, LevelItem>();

        // TEST
        gameManager.Level = 2;
        gameManager.Climate = Defs.Climate.WARM;

        string climate = gameManager.Climate.ToString().ToLower();
        climate = char.ToUpper(climate[0]) + climate.Substring(1);
        string fileName = gameManager.Level == 0 ? "Tutorial" : $"Level{gameManager.Level}{climate}";
        string filePath = Path.Combine(levelItemsFilePath, fileName);
        Debug.Log(filePath);

        // Carga el archivo como texto plano y se parsea a un objeto de JSON
        TextAsset jsonFile = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        JObject jsonObject = JObject.Parse(jsonFile.text);

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
                LoadNeededItems(items, "N");
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
                ItemProperties properties = itemsInfoDict.ContainsKey(item) ? itemsInfoDict[item] : new ItemProperties();
                RectTransform categoryTitleTr = otherTitleTr;

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

                LevelItem levelItem = new LevelItem();
                levelItem.ListItemComp = listItem;
                levelItem.Obtained = false;
                levelItem.ItemGameObject = null;

                neededItems.Add(item, levelItem);
            }
        }
    }

    private void LoadScenarioItems(JToken items, string gender)
    {

    }

    public void StoreItem(string id)
    {
        if (neededItems.ContainsKey(id))
        {
        }
    }
    public void ReturnItem(string id)
    {
        if (neededItems.ContainsKey(id))
        {
        }
    }

    public void EndGame()
    {

    }
}
