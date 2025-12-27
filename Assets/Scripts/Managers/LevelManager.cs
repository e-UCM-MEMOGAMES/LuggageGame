using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xasu.HighLevel;


public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;

    protected Stopwatch watch = Stopwatch.StartNew();
    TrackerManager trackerManager;
    CompletableTracker.CompletableType COMPLETABLE_TYPE = CompletableTracker.CompletableType.Level;

    const int MAX_LIST_USES = 3;
    int remainingListUses = MAX_LIST_USES;
    public int RemainingListUses
    {
        get { return remainingListUses; }
        set { remainingListUses = value; }
    }

    Dictionary<string, ItemProperties> itemsInfo = new Dictionary<string, ItemProperties>();
    Dictionary<string, ListItem> neededItems = new Dictionary<string, ListItem>();
    HashSet<string> storedItems = new HashSet<string>();

    [Header("Item list")]
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

    [Header("Item name")]
    [SerializeField] RectTransform uiRectTr;
    [SerializeField] GameObject itemNamePanel;
    [SerializeField] TextMeshProUGUI itemNamePanelText;
    RectTransform itemNamePanelTr;
    const float NAME_PANEL_BORDER_OFFSET = 50.0f;

    [Header("Final panel")]
    [SerializeField] GameObject[] unlockedStars;
    [SerializeField] TextMeshProUGUI[] subObjectivesAmounts;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;

        trackerManager = TrackerManager.Instance;
        watch.Start();

        foreach (GameObject star in unlockedStars)
        {
            star.SetActive(false);
            star.GetComponent<Animation>().Stop();
        }

        caseImg.sprite = emptyTexture;

        audioManager.Play(GameSound.LevelBGM);


        itemNamePanel.SetActive(false);
        itemNamePanelTr = itemNamePanel.GetComponent<RectTransform>();

        trackerManager.TrySendStatement(CompletableTracker.Instance.Initialized(Defs.GetLevelSaveKey(gameManager.Level, gameManager.Climate), COMPLETABLE_TYPE));
    }

    public void LoadItemsInfo(Dictionary<string, ItemProperties> info)
    {
        itemsInfo = info;
    }


    public void AddListItems(List<ItemProperties> neededItemsInfo)
    {
        foreach (ItemProperties properties in neededItemsInfo)
        {
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

            neededItems.Add(properties.Id, listItem);
        }
    }

    public void AddScenarioItem(string id, RectTransform spawnpoint, Defs.SpawnType spawnType, Defs.SpawnPivot pivot)
    {
        if (itemsInfo.ContainsKey(id))
        {
            GameObject scenarioItem = 
                Instantiate(spawnType == Defs.SpawnType.REGULAR ? itemsInfo[id].SceneItemPrefab : itemsInfo[id].DrawerItemPrefab, spawnpoint);
            GameObject caseItem = Instantiate(itemsInfo[id].StoredItemPrefab, topViewCase.transform);
            caseItem.SetActive(false);

            RectTransform scenarioTr = scenarioItem.GetComponent<RectTransform>();
            Vector3 itemTr = scenarioTr.localPosition;
            float height = scenarioTr.sizeDelta.y / 2;
            if (Mathf.Abs(scenarioTr.eulerAngles.z) >= 70)
            {
                height = scenarioTr.sizeDelta.x / 2;
            }

            if (pivot == Defs.SpawnPivot.BOTTOM)
            {
                itemTr.y += height;
            }
            else if (pivot == Defs.SpawnPivot.TOP)
            {
                itemTr.y -= height;
            }
            scenarioTr.localPosition = itemTr;

            DragAndDrop scenarioDragNdrop = scenarioItem.GetComponent<DragAndDrop>();
            DragAndDrop caseDragNDrop = caseItem.GetComponent<DragAndDrop>();

            scenarioDragNdrop.Initialize(this, id, caseItem, spawnType == Defs.SpawnType.STORED ? topViewCase : normalCase);
            caseDragNDrop.Initialize(this, id, scenarioItem, topViewCase);
        }
    }

    public void SortCaseItems()
    {
        Transform parent = topViewCase.transform; // or any parent transform
        RectTransform[] children = new RectTransform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = (RectTransform)parent.GetChild(i);
        }
        Array.Sort(children, (a, b) => (b.sizeDelta.x * b.sizeDelta.y).CompareTo(a.sizeDelta.x * a.sizeDelta.y));
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }


    public void PointerEnterItem(string id, RectTransform objectRectTr)
    {
        // Se activa el panel y se pone el nombre del objeto (si esta localizado, se traduce, y si no, aparece la id del objeto)
        itemNamePanel.SetActive(true);
        itemNamePanelText.text = id;
        if (itemsInfo.ContainsKey(id))
        {
            itemNamePanelText.text = itemsInfo[id].LocalizedName.GetLocalizedString();
        }

        // Se coloca el panel encima del objeto
        Vector3 panelPos = objectRectTr.position;
        float offset = objectRectTr.offsetMax.y - objectRectTr.localPosition.y;
        panelPos.y += offset + itemNamePanelTr.sizeDelta.y / 2;
        itemNamePanelTr.position = panelPos;

        // Se calcula si el panel se sale por algun lado de la pantalla
        panelPos = itemNamePanelTr.position;

        // Si el lateral superior del panel se sale por arriba
        if (uiRectTr.TransformPoint(itemNamePanelTr.offsetMax).y >= uiRectTr.offsetMax.y - NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.y = uiRectTr.offsetMax.y - NAME_PANEL_BORDER_OFFSET - itemNamePanelTr.sizeDelta.y / 2;
        }
        // Si el lateral inferior del panel se sale por abajo
        else if (uiRectTr.TransformPoint(itemNamePanelTr.offsetMin).y <= uiRectTr.offsetMin.y + NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.y = uiRectTr.offsetMin.y + NAME_PANEL_BORDER_OFFSET + itemNamePanelTr.sizeDelta.y / 2;
        }

        // Si el lateral derecho del panel se sale por la derecha
        if (uiRectTr.TransformPoint(itemNamePanelTr.offsetMax).x >= uiRectTr.offsetMax.x - NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.x = uiRectTr.offsetMax.x - NAME_PANEL_BORDER_OFFSET - itemNamePanelTr.sizeDelta.x / 2;
        }
        // Si el lateral izquierdo del panel se sale por la izquierda
        else if (uiRectTr.TransformPoint(itemNamePanelTr.offsetMin).x <= uiRectTr.offsetMin.x + NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.x = uiRectTr.offsetMin.x + NAME_PANEL_BORDER_OFFSET + itemNamePanelTr.sizeDelta.x / 2;
        }
        
        itemNamePanelTr.position = panelPos;
    }

    public void PointerOutItem()
    {
        itemNamePanel.SetActive(false);
        itemNamePanelText.text = "";
    }


    public void StoreItem(string id)
    {
        storedItems.Add(id);
        audioManager.Play(GameSound.PutIn);
        caseImg.sprite = fullTexture;

        string contextExtension = "incorrectItemProgression";
        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(true);
            contextExtension = "correctItemProgression";
        }

        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(id)
            .WithResultExtensions(new Dictionary<string, object> {
                {"https://storeIn",  "suitcase" }
            })
            .WithContextExtensions(new Dictionary<string, object> {
                {$"https://{contextExtension}",  $"{storedItems.Count / (double)neededItems.Count}" }
            })
        );
    }
    public void ReturnItem(string id)
    {
        storedItems.Remove(id);
        audioManager.Play(GameSound.TakeOut);
        if (storedItems.Count == 0)
        {
            caseImg.sprite = emptyTexture;
        }

        string contextExtension = "incorrectItemProgression";
        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(false);
            contextExtension = "correctItemProgression";
        }

        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(id)
            .WithResultExtensions(new Dictionary<string, object> {
                {"https://removeFrom",  "suitcase" }
            })
            .WithContextExtensions(new Dictionary<string, object> {
                {$"https://{contextExtension}",  $"{storedItems.Count / (double)neededItems.Count}" }
            })
        );
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

        if (correctItems >= neededItems.Count / 2.0f)
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

        watch.Stop();
        long completionTime = watch.ElapsedMilliseconds;

        List<string> correctItemsList = new List<string>();
        List<string> wrongItemsList = new List<string>();

        foreach (string item in storedItems) {
            if (neededItems.ContainsKey(item))
            {
                correctItemsList.Add(item);
            }
            else
            {
                wrongItemsList.Add(item);
            }
        }

        trackerManager.TrySendStatement(
            CompletableTracker.Instance.Completed(levelName, COMPLETABLE_TYPE, watch.ElapsedMilliseconds)
            .WithSuccess(true)
            .WithResultExtensions(new Dictionary<string, object> {
                {"https://stars", totalStars },
                {"https://wrongItems", incorrectItems },
                {"https://correctItems", $"{correctItems}/{neededItems.Count}" },
                {"https://wrongItemsList", wrongItemsList },
                {"https://correctItemsList", correctItemsList },
                {"https://checkListOpportunities", $"{listUses}/{MAX_LIST_USES}" },
            })
        );
    }

}
