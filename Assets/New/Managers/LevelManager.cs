using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;

    AudioManager audioManager;

    [SerializeField]
    GameObject initialPanel, itemList, notebookPanel, endPanel, warning,
    bedroom, bathroom;

    const int MAX_LIST_USES = 3;
    int remainingListUses = MAX_LIST_USES;
    [SerializeField]
    TextMeshProUGUI remainingListUsesText;

    /// <summary>
    /// Ruta de los archivos de localizacion de los niveles
    /// </summary>
    string levelItemsFilePath = Path.Combine("LevelInfo");


    /// <summary>
    /// Lista de objetos del nivel
    /// </summary>
    List<string> levelItems;

    [SerializeField]
    ItemsInfo itemsInfo;
    Dictionary<string, ItemProperties> itemsInfoDict;

    struct LevelItem
    {
        public ListItem ListItemComp;
        public bool Obtained;
        public GameObject ItemGameObject;
    }
    Dictionary<string, LevelItem> neededItems;

    [SerializeField]
    RectTransform itemsListTr, clothingTitleTr, footwearTitleTr, otherTitleTr;

    [SerializeField]
    GameObject listItemPrefab;

    public List<string> ObstaculosList { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;

        audioManager.Play(GameSound.LevelBGM);

        bedroom.SetActive(true);
        bathroom.SetActive(false);

        warning.SetActive(false);
        notebookPanel.SetActive(false);
        initialPanel.SetActive(true);
        endPanel.SetActive(false);
        itemList.SetActive(true);

        remainingListUsesText.text = remainingListUses.ToString();

        LoadLevelItems();
    }

    private void LoadLevelItems()
    {
        itemsInfoDict = new Dictionary<string, ItemProperties>();
        foreach (Defs.ItemInfo item in itemsInfo.InfoList)
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

        foreach (var obj in jsonObject)
        {
            if (obj.Key == "objectList")
            {
                JToken items = obj.Value;
                string genderInitial = (gameManager.PlayerGender.ToString()[0]).ToString();

                LoadNeededItems(items, genderInitial);
                LoadNeededItems(items, "N");
            }
            else if (obj.Key == "storagePoints")
            {

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


    public void StartGame()
    {
        initialPanel.SetActive(false);
        itemList.SetActive(false);
    }

    public void ToggleItemList()
    {
        if (!notebookPanel.activeSelf && remainingListUses > 0)
        {
            remainingListUses--;
            remainingListUsesText.text = remainingListUses.ToString();
            notebookPanel.SetActive(true);
            itemList.SetActive(true);

            audioManager.Play(GameSound.NoteBook);

        }
        else if (notebookPanel.activeSelf)
        {
            notebookPanel.SetActive(false);
            itemList.SetActive(false);

            audioManager.Play(GameSound.NoteBook);
        }
    }
    public void EndGame()
    {
        endPanel.SetActive(true);
        itemList.SetActive(true);
        warning.SetActive(false);

        audioManager.Play(GameSound.AirPlane);
    }

    public void Return()
    {
        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
        audioManager.Play(GameSound.MenuBGM);
    }
}
