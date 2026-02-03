using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xasu.HighLevel;


public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;
    /// <summary>
    /// Instancia del AudioManager
    /// </summary>
    AudioManager audioManager;
    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;

    /// <summary>
    /// Temporizador para medir el tiempo que se tarda en completar el nivel
    /// </summary>
    Stopwatch watch = Stopwatch.StartNew();
    CompletableTracker.CompletableType COMPLETABLE_TYPE = CompletableTracker.CompletableType.Level;


    [Header("Tutorial")]
    /// <summary>
    /// Objeto con todos lo elementos del tutorial
    /// </summary>
    [SerializeField]
    GameObject tutorial;

    /// <summary>
    /// Numero maximo de usos de la lista
    /// </summary>
    const int MAX_LIST_USES = 3;
    /// <summary>
    /// Usos de la lista restantes
    /// </summary>
    int remainingListUses = MAX_LIST_USES;
    public int RemainingListUses
    {
        get { return remainingListUses; }
        set { remainingListUses = value; }
    }

    /// <summary>
    /// Propiedades de cada objeto asociadas a su id
    /// </summary>
    Dictionary<string, ItemProperties> itemsInfo = new Dictionary<string, ItemProperties>();
    public Dictionary<string, ItemProperties> ItemsInfo
    {
        get { return itemsInfo; }
        set { itemsInfo = value; }
    }

    /// <summary>
    /// Componentes ListItem de los objetos de la lista asociados a su id
    /// </summary>
    Dictionary<string, ListItem> neededItems = new Dictionary<string, ListItem>();
    HashSet<string> storedItems = new HashSet<string>();


    public class ItemPair
    {
        public ItemPair(GameObject scenario, GameObject stored)
        {
            ScenarioItem = scenario;
            StoredItem = stored;
        }
        public GameObject ScenarioItem, StoredItem;
    }
    /// <summary>
    /// Parejas de objeto de la vista normal y objeto de la vista de la maleta asociadas a su ide
    /// </summary>
    Dictionary<string, ItemPair> scenarioStoredItemsPairs = new Dictionary<string, ItemPair>();
    public Dictionary<string, ItemPair> ScenarioStoredItemsPairs
    {
        get { return scenarioStoredItemsPairs; }
        private set { }
    }


    [Header("Item list")]
    /// <summary>
    /// RectTransform de la lista de objetos
    /// </summary>
    [SerializeField] RectTransform itemsListTr;
    /// <summary>
    /// RectTransform del titulo de la categoria de ropa
    /// </summary>
    [SerializeField] RectTransform clothingTitleTr;
    /// <summary>
    /// RectTransform del titulo de la categoria de calzado
    /// </summary>
    [SerializeField] RectTransform footwearTitleTr;
    /// <summary>
    /// RectTransform del titulo de la categoria otros
    /// </summary>
    [SerializeField] RectTransform otherTitleTr;
    /// <summary>
    /// Prefab para los elementos de la lista
    /// </summary>
    [SerializeField] GameObject listItemPrefab;


    [Header("Case")]
    /// <summary>
    /// Componente Image de la maleta en la vista normal
    /// </summary>
    [SerializeField] Image caseImg;
    /// <summary>
    /// Textura de la maleta vacia
    /// </summary>
    [SerializeField] Sprite emptyTexture;
    /// <summary>
    /// Textura de la maleta con algun elemento
    /// </summary>
    [SerializeField] Sprite fullTexture;
    /// <summary>
    /// Maleta de la vista normal
    /// </summary>
    [SerializeField] GameObject normalCase;
    /// <summary>
    /// Maleta de la vista de la maleta
    /// </summary>
    [SerializeField] GameObject topViewCase;


    [Header("Item name")]
    /// <summary>
    /// CanvasScaler del canvas de la UI
    /// </summary>
    [SerializeField] CanvasScaler uiCanvasScaler;
    /// <summary>
    /// RectTransform de la UI
    /// </summary>
    [SerializeField] RectTransform uiRectTr;
    /// <summary>
    /// Lateral superior del canvas
    /// </summary>
    float canvasTop,
    /// <summary>
    /// Lateral inferior del canvas
    /// </summary>
    canvasBottom,
    /// <summary>
    /// Lateral derecho del canvas
    /// </summary>
    canvasRight,
    /// <summary>
    /// Lateral izquierdo del canvas
    /// </summary>
    canvasLeft;
    /// <summary>
    /// Panel con el nombre de los objetos
    /// </summary>
    [SerializeField] GameObject itemNamePanel;
    /// <summary>
    /// Texto del nombre de los objetos
    /// </summary>
    [SerializeField] TextMeshProUGUI itemNamePanelText;
    /// <summary>
    /// RectTransform del panel del nombre
    /// </summary>
    RectTransform itemNamePanelTr;
    
    /// <summary>
    /// Distancia entre el panel del nombre y los limites del canvas
    /// </summary>
    const float NAME_PANEL_BORDER_OFFSET = 50.0f;


    [Header("Final panel")]
    /// <summary>
    /// Estrellas de los objetivos desbloqueadas
    /// </summary>
    [SerializeField] GameObject[] unlockedStars;
    /// <summary>
    /// Textos con las cantidades de los objetivos "secundarios"
    /// </summary>
    [SerializeField] TextMeshProUGUI[] subObjectivesAmounts;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        audioManager = AudioManager.Instance;
        audioManager.Play(GameSound.LevelBGM);

        trackerManager = TrackerManager.Instance;
        watch.Start();

        // Se calculan los limites del canvas
        float width = uiCanvasScaler.referenceResolution.x;
        float height = uiCanvasScaler.referenceResolution.y;
        canvasRight = width / 2;
        canvasLeft = canvasRight - width;
        canvasBottom = -height / 2;
        canvasTop = canvasBottom + height;

        // Se activa el tutorial si el nivel es el 0
        tutorial.SetActive(gameManager.Level == 0);

        // Se ocultan todos los titulos de las categorias de la lista
        otherTitleTr.gameObject.SetActive(false);
        clothingTitleTr.gameObject.SetActive(false);
        footwearTitleTr.gameObject.SetActive(false);

        // Se ocultan todas las estrellas desbloqueadas
        foreach (GameObject star in unlockedStars)
        {
            star.SetActive(false);
            star.GetComponent<Animation>().Stop();
        }

        caseImg.sprite = emptyTexture;

        itemNamePanel.SetActive(false);
        itemNamePanelTr = itemNamePanel.GetComponent<RectTransform>();


        trackerManager.TrySendStatement(CompletableTracker.Instance.Initialized(Defs.GetLevelSaveKey(gameManager.Level, gameManager.Climate), COMPLETABLE_TYPE));
    }

    /// <summary>
    /// Llamado por LevelInfo una vez se han leido todos los objetos necesarios. 
    /// Crea el elemento en la lista correspondiente a dicho objeto
    /// </summary>
    public void AddListItems(HashSet<ItemProperties> neededItemsInfo)
    {
        int clothingItems = 0,
            footwearItems = 0,
            otherItems = 0;

        // Recorre todos los objetos
        foreach (ItemProperties properties in neededItemsInfo)
        {
            // Determina el titulo de la categoria del objeto
            RectTransform categoryTitleTr = otherTitleTr;
            int index = 0;
            if (properties.Category == Defs.ItemCategory.CLOTHING)
            {
                categoryTitleTr = clothingTitleTr;
                clothingTitleTr.gameObject.SetActive(true);
                clothingItems++;
                index = clothingItems;
            }
            else if (properties.Category == Defs.ItemCategory.FOOTWEAR)
            {
                categoryTitleTr = footwearTitleTr;
                footwearTitleTr.gameObject.SetActive(true);
                footwearItems++;
                index = footwearItems;
            }
            else
            {
                otherTitleTr.gameObject.SetActive(true);
                otherItems++;
                index = otherItems;
            }

            // Se instancia el elemento como hijo de la lista de objetos
            GameObject instance = Instantiate(listItemPrefab, itemsListTr);

            // Se coloca debajo del ultimo elemento instanciado en esa categoria
            instance.transform.SetSiblingIndex(categoryTitleTr.GetSiblingIndex() + index);

            // Se establece el nombre del objeto
            ListItem listItem = instance.GetComponent<ListItem>();
            listItem.SetName(properties.LocalizedName);

            // Se guarda en la lista de objetos necesarios
            neededItems.Add(properties.Id, listItem);
        }
    }
    /// <summary>
    /// Llamado por LevelInfo segun se van leyendo los objetos del escenario.
    /// Crea el objeto indicado tanto en la vista normal como en la vista de la maleta
    /// </summary>
    public void AddScenarioItem(string id, RectTransform spawnpoint, Defs.SpawnType spawnType, Defs.SpawnPivot pivot)
    {
        // Si hay informacion del objeto
        if (itemsInfo.ContainsKey(id))
        {
            // Se instancia el objeto en el estado correspondiente (normal o en cajon) de la vista normal como hijo del spawnpoint
            GameObject scenarioItem =
                Instantiate(spawnType == Defs.SpawnType.REGULAR ? itemsInfo[id].SceneItemPrefab : itemsInfo[id].DrawerItemPrefab, spawnpoint);

            // Se instancia el objeto guardado como hijo de la maleta en la vista de la maleta y se oculta
            GameObject caseItem = Instantiate(itemsInfo[id].StoredItemPrefab, topViewCase.transform);
            caseItem.SetActive(false);


            RectTransform scenarioTr = scenarioItem.GetComponent<RectTransform>();

            // Se determina la "altura" del objeto de la vista normal dependiendo de su rotacion
            Vector3 itemTr = scenarioTr.localPosition;
            float height = scenarioTr.sizeDelta.y / 2;
            if (Mathf.Abs(scenarioTr.eulerAngles.z) >= 70)
            {
                height = scenarioTr.sizeDelta.x / 2;
            }

            // Se desplaza el objeto dependiendo del pivote del spawnpoint
            if (pivot == Defs.SpawnPivot.BOTTOM)
            {
                itemTr.y += height;
            }
            else if (pivot == Defs.SpawnPivot.TOP)
            {
                itemTr.y -= height;
            }
            scenarioTr.localPosition = itemTr;

            // Se inicializa el drag and drop del objeto en ambas vistas
            DragAndDrop scenarioDragNdrop = scenarioItem.GetComponent<DragAndDrop>();
            DragAndDrop caseDragNDrop = caseItem.GetComponent<DragAndDrop>();
            scenarioDragNdrop.Initialize(this, id, caseItem, spawnType == Defs.SpawnType.STORED ? topViewCase : normalCase);
            caseDragNDrop.Initialize(this, id, scenarioItem, topViewCase);

            // Se guarda la pareja de objetos
            scenarioStoredItemsPairs.Add(id, new ItemPair(scenarioItem, caseItem));
        }
    }
    /// <summary>
    /// Llamado por LevelInfo una vez se han instanciado todos los objetos del escenario. Ordena los objetos de la 
    /// maleta de manera que los que tienen una mayor area se rendericen por debajo de los que tienen menor area
    /// </summary>
    public void SortCaseItems()
    {
        Transform parent = topViewCase.transform;
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

    /// <summary>
    /// Llamado por el DragAndDrop de un objeto. Muestra el panel del nombre del objeto encima de este
    /// </summary>
    public void PointerEnterItem(string id, RectTransform objectRectTr)
    {
        // Se activa el panel y se pone el nombre del objeto (si esta localizado, se traduce, y si no, aparece la id del objeto)
        itemNamePanel.SetActive(true);
        itemNamePanelText.text = id;
        if (itemsInfo.ContainsKey(id))
        {
            itemNamePanelText.text = itemsInfo[id].LocalizedName.GetLocalizedString();
        }

        // Se coloca el panel encima del objeto (se convierten las coordenadas
        // globales del objeto en coordenadas locales del canvas con la UI)
        Vector3 panelPos = uiRectTr.InverseTransformPoint(objectRectTr.position);
        float offset = objectRectTr.offsetMax.y - objectRectTr.localPosition.y;
        panelPos.y += offset + itemNamePanelTr.sizeDelta.y / 2;
        itemNamePanelTr.localPosition = panelPos;


        // Se calcula si el panel se sale por algun lado de la pantalla

        // Si el lateral superior del panel se sale por arriba
        if (itemNamePanelTr.offsetMax.y > canvasTop - NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.y = canvasTop - NAME_PANEL_BORDER_OFFSET - itemNamePanelTr.sizeDelta.y / 2;
        }
        // Si el lateral inferior del panel se sale por abajo
        else if (itemNamePanelTr.offsetMin.y < canvasBottom + NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.y = canvasBottom + NAME_PANEL_BORDER_OFFSET + itemNamePanelTr.sizeDelta.y / 2;
        }

        // Si el lateral derecho del panel se sale por la derecha
        if (itemNamePanelTr.offsetMax.x > canvasRight - NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.x = canvasRight - NAME_PANEL_BORDER_OFFSET - itemNamePanelTr.sizeDelta.x / 2;
        }
        // Si el lateral izquierdo del panel se sale por la izquierda
        else if (itemNamePanelTr.offsetMin.x < canvasLeft + NAME_PANEL_BORDER_OFFSET)
        {
            panelPos.x = canvasLeft + NAME_PANEL_BORDER_OFFSET + itemNamePanelTr.sizeDelta.x / 2;
        }

        itemNamePanelTr.localPosition = panelPos;
    }
    /// <summary>
    /// Llamado por el DragAndDrop de un objeto. Desactiva el panel del nombre del objeto
    /// </summary>
    public void PointerOutItem()
    {
        itemNamePanel.SetActive(false);
        itemNamePanelText.text = "";
    }

    /// <summary>
    /// Llamado por el DragAndDrop de un objeto. Mete el objeto indicado en la maleta
    /// </summary>
    public void StoreItem(string id)
    {
        storedItems.Add(id);
        audioManager.Play(GameSound.PutIn);

        // Se pone la textura de la maleta llena
        caseImg.sprite = fullTexture;


        string contextExtension = "incorrectItemProgression";
        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(true);
            contextExtension = "correctItemProgression";
        }

        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(id)
            .WithResultExtensions(new Dictionary<string, object> {
                { "https://storeIn",  "suitcase" }
            })
            .WithContextExtensions(new Dictionary<string, object> {
                { $"https://{contextExtension}",  $"{storedItems.Count / (double)neededItems.Count}" }
            })
        );
    }
    /// <summary>
    /// Llamado por el DragAndDrop de un objeto. Saca el objeto indicado de la maleta
    /// </summary>
    public void ReturnItem(string id)
    {
        storedItems.Remove(id);
        audioManager.Play(GameSound.TakeOut);

        // Si la maleta se queda vacia, cambia la textura
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
                { "https://removeFrom",  "suitcase" }
            })
            .WithContextExtensions(new Dictionary<string, object> {
                { $"https://{contextExtension}",  $"{storedItems.Count / (double)neededItems.Count}" }
            })
        );
        if (neededItems.ContainsKey(id))
        {
            neededItems[id].SetObtained(false);
        }
    }

    /// <summary>
    /// Llamado por el boton de confirmar del panel de advertencia al intentar terminar el nivel
    /// </summary>
    public void EndGame()
    {
        int totalStars = 0;
        int correctItems = 0;

        // Se obtiene el numero de objetos correctos
        foreach (var item in neededItems)
        {
            if (storedItems.Contains(item.Key))
            {
                correctItems++;
            }
            // Si el objeto no esta guardado, se muestra que se ha perdido
            else
            {
                item.Value.SetMissed(true);
            }
        }

        int incorrectItems = storedItems.Count - correctItems;
        int listUses = MAX_LIST_USES - remainingListUses;

        // Se va calculando el numero de estrellas obtenidas. Como es necesario completar todos los objetivos
        // previos para conseguir la estrella de cada objetivo, en cuanto no se cumple uno, se deja de comprobar
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

        // Se activa la cantidad de estrellas desbloqueadas
        for (int i = 0; i < totalStars; i++)
        {
            unlockedStars[i].SetActive(true);
            unlockedStars[i].GetComponent<Animation>().Play();
        }

        // Se actualiza el texto de los objetivos secundarios
        subObjectivesAmounts[0].text = $"{correctItems}/{neededItems.Count}";
        subObjectivesAmounts[1].text = $"{incorrectItems}";
        subObjectivesAmounts[2].text = $"{listUses}/{MAX_LIST_USES}";

        // Se guarda la puntuacion si no habia puntuacion guardada o es mayor a esta
        string levelName = Defs.GetLevelSaveKey(gameManager.Level, gameManager.Climate);
        if (!PlayerPrefs.HasKey(levelName) || (PlayerPrefs.HasKey(levelName) && PlayerPrefs.GetInt(levelName) < totalStars))
        {
            PlayerPrefs.SetInt(levelName, totalStars);
        }


        watch.Stop();
        long completionTime = watch.ElapsedMilliseconds;

        List<string> correctItemsList = new List<string>();
        List<string> wrongItemsList = new List<string>();

        foreach (string item in storedItems)
        {
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
            CompletableTracker.Instance.Completed(levelName, COMPLETABLE_TYPE, completionTime)
            .WithSuccess(true)
            .WithResultExtensions(new Dictionary<string, object> {
                { "https://stars", totalStars },
                { "https://wrongItems", incorrectItems },
                { "https://correctItems", $"{correctItems}/{neededItems.Count}" },
                { "https://wrongItemsList", wrongItemsList },
                { "https://correctItemsList", correctItemsList },
                { "https://checkListOpportunities", $"{listUses}/{MAX_LIST_USES}" },
            })
        );
    }
}

