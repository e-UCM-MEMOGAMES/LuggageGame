using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject initialPanel, itemList,

        notebookPanel;

    /// <summary>
    /// Ruta de los archivos de localizacion de los niveles
    /// </summary>
    string levelItemsFilePath = Path.Combine("LevelInfo");


    /// <summary>
    /// Lista de objetos a poner en la maleta
    /// </summary>
    HashSet<string> neededItems = new HashSet<string>();

    /// <summary>
    /// Lista de objetos del nivel
    /// </summary>
    List<string> levelItems;

    public List<string> ObstaculosList { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        initialPanel.SetActive(true);
        itemList.SetActive(true);
        notebookPanel.SetActive(false);

        LoadLevelItems();
    }

    private void LoadLevelItems()
    {
        string climate = gameManager.Climate.ToString().ToLower();
        climate = char.ToUpper(climate[0]) + climate.Substring(1);
        string fileName = gameManager.Level == 0 ? "Tutorial" : $"Level{gameManager.Level}{climate}";
        fileName = "Level3Cold";
        string filePath = Path.Combine(levelItemsFilePath, fileName);
        //Debug.Log(filePath);

        // Carga el archivo como texto plano y se parsea a un array de JSON
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
                neededItems.Add(item);
            }
        }
    }


    public void StartGame()
    {
        initialPanel.SetActive(false);
        itemList.SetActive(false);
    }

    public void ActivateItemList(bool active)
    {
        notebookPanel.SetActive(active);
        itemList.SetActive(active);
    }
    public void EndGame()
    {
        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
    }
}
