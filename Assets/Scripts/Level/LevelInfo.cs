using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// Instancia del LevelManager
    /// </summary>
    LevelManager levelManager;

    /// <summary>
    /// Ruta de los archivos de localizacion de los niveles
    /// </summary>
    string levelItemsFilePath = Path.Combine("LevelInfo");

    [Header("Items settings")]
    /// <summary>
    /// ScriptableObject con la informacion de los objetos
    /// </summary>
    [SerializeField] ItemsInfo itemsInfo;
    /// <summary>
    /// Lista de las propiedades de cada objeto, cada una asociada a su id
    /// </summary>
    Dictionary<string, ItemProperties> itemsInfoDict = new Dictionary<string, ItemProperties>();

    /// <summary>
    /// Clase para asignar desde el editor una serie de puntos 
    /// a un ScriptableObject con la informacion de un spawnpoint 
    /// </summary>
    [System.Serializable]
    public struct Spawnpoint
    {
        public SpawnpointProperties PointProperties;
        public RectTransform[] Points;
    }

    [Header("Spawnpoints settings")]
    /// <summary>
    /// Spawnpoints posibles 
    /// </summary>
    [SerializeField] Spawnpoint[] spawnpoints;
    /// <summary>
    /// Lista de los spawnpoints, cada uno asociado a su id
    /// </summary>
    Dictionary<string, Spawnpoint> spawnpointsInfoDict = new Dictionary<string, Spawnpoint>();

    /// <summary>
    /// Lista con las propiedades de todos los objetos necesarios 
    /// </summary>
    HashSet<ItemProperties> neededItems = new HashSet<ItemProperties>();


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        
        levelManager = GetComponent<LevelManager>();

        // Se guardan los elementos de la lista de itemsInfo en el diccionario
        foreach (ItemProperties item in itemsInfo.List)
        {
            if (!itemsInfoDict.ContainsKey(item.Id))
            {
                itemsInfoDict.Add(item.Id, item);
            }
        }
        levelManager.ItemsInfo = itemsInfoDict;

        // Se guardan los spawnpoints posibles en el diccionario
        foreach (Spawnpoint point in spawnpoints)
        {
            spawnpointsInfoDict.Add(point.PointProperties.Id, point);
        }

        // Se cargan todos los objetos del archivo correspondiente
        LoadItems();
    }

    /// <summary>
    /// Lee el archivo con la informacion del nivel para crear los objetos
    /// </summary>
    private void LoadItems()
    {
        // Se determina el nombre del nivel dependiendo del numero y el clima
        string fileName = "Tutorial";
        if (gameManager.Level > 0)
        {
            // Obtiene el nombre del clima en PascalCase
            string climate = gameManager.Climate.ToString().ToLower();
            climate = char.ToUpper(climate[0]) + climate.Substring(1);

            fileName = $"Level{gameManager.Level}{climate}";
        }
        // TEST
        //fileName = $"Level1Warm";

        // Se obtiene la ruta del archivo con la informacion del nivel
        string filePath = Path.Combine(levelItemsFilePath, fileName);

        // Carga el archivo como texto plano y se parsea a un objeto de JSON
        TextAsset jsonFile = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        JObject jsonObject = JObject.Parse(jsonFile.text);
        //Debug.Log(jsonFile);

        // Se obtiene la inicial del genero escogido (en string)
        string genderInitial = (gameManager.PlayerGender.ToString()[0]).ToString();

        // Se cargan los objetos de la lista para el genero escogido y para el neutral
        if (jsonObject.ContainsKey("objectList"))
        {
            JToken items = jsonObject["objectList"];
            LoadNeededItems(items, genderInitial);
            LoadNeededItems(items, "N");
        }
        levelManager.AddListItems(neededItems);

        // Se cargan e instancian los objetos del escenario para el genero escogido y para el neutral
        if (jsonObject.ContainsKey("spawnpoints"))
        {
            // Se va recorriendo cada spawnpoint del archivo
            foreach (var spawnpoint in jsonObject["spawnpoints"])
            {
                JProperty prop = (JProperty)spawnpoint;

                // Se obtiene el nombre del spawnpoint y el objeto con los arrays de objetos para cada genero
                string pointName = prop.Name;
                JToken gendersItems = prop.Value;

                // Si se pueden spawnear los objetos, se instancian en la escena
                if (spawnpointsInfoDict.ContainsKey(pointName))
                {
                    LoadSceneItems(gendersItems, pointName, genderInitial);
                    LoadSceneItems(gendersItems, pointName, "N");
                }
            }
        }

        // Se ordenan los objetos de la maleta
        levelManager.SortCaseItems();
    }

    /// <summary>
    /// Carga los objetos necesarios del genero indicado
    /// </summary>
    private void LoadNeededItems(JToken items, string gender)
    {
        // Si existe el array del genero, se recorren todos los elementos guardando la id de los objetos
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

    /// <summary>
    /// Carga los objetos del escenario del genero indicado
    /// </summary>
    private void LoadSceneItems(JToken items, string spawnpoint, string gender)
    {
        // Si existe el array del genero, se recorren todos los elementos
        if (items[gender] != null)
        {
            foreach (var itemInfo in items[gender])
            {
                // Si el objeto tiene las propiedades id y position, y la posicion en la que se va a
                // spawnear existe en la lista de puntos, se instancia el objeto en el escenario
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
