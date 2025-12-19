using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static JSONReader;
using TMPro;
using static CheckBox;
using Xasu;

public class LvlMngr : MonoBehaviour
{
    private AudioManager audioMng;
    enum State { BATHROOM, BEDROOM, DRAWER, LUGGAGE, FIRSTAIDKIT, END };
    public enum ObjectType { Clothes, Shoes, Others, ObjectTypeSize };
    string LevelNameGlobal;
    JSONReader jsonReader;

    [System.Serializable]
    public class ObjectsInfo
    {
        public string name;
        public GameObject go;
        public ObjectType type;

    }


    [System.Serializable]
    public class Storeinfo
    {
        public string name;
        public List<Transform> positions;
    }
    [System.Serializable]
    public class ListInfo
    {
        public List<string> objectList;
        public GameObject titleGO;
        public GameObject contentGO;
        public TMP_Text contentText;
        public RectTransform contentTr;
        public RectTransform titleTr;
    }

    public List<ObjectsInfo> objectsInfo;
    public List<Storeinfo> storeInfo;

    private Dictionary<string, ObjectsInfo> objectsDictionary;
    private Dictionary<string, CheckBox> checkBoxDictionary;
    private Dictionary<string, List<Transform>> storageDictionary;

    //Lista de objetos en pantalla
    [SerializeField]
    private List<ListInfo> objectLists;
    [SerializeField]
    private float firstTitleYOffset;
    [SerializeField]
    private float YOffsetBetweenLine;
    [SerializeField]
    private float YOffsetBetweenCheckBox;
    [SerializeField]
    private GameObject noteBookGO;
    private RectTransform noteBookTr;
    [SerializeField]
    private GameObject checkBoxPrefab;

    [SerializeField]
    private int maxcheckOpportunities = 3;
    private int checkOpportunities;

    [SerializeField]
    private TMP_Text opportunitiesText;


    public int level;
    public Camera roomCam;
    public Camera bathroomCam;
    public Camera drawerCam;
    public Camera firstAidKitCam;
    public GameObject roomButton;
    public Sprite[] roomsButton;
    [SerializeField]
    private List<Sprite> _tiposSuelos;
    [SerializeField]
    private GameObject _suelo;
    public GameObject buttonBackToRoom;
    public GameObject bttnEnd;
    public Luggage luggage;
    public GameObject drawerImage;
    public GameObject blackScreen;
    Vector3 initialLuggagePos;
    Vector3 initialLuggageScale;
    GameObject currentDrawer = null;
    State state;
    public List<Sprite> TiposSuelos { get => _tiposSuelos; set => _tiposSuelos = value; }
    public GameObject Suelo { get => _suelo; set => _suelo = value; }
    public GameObject[] stars;
    [SerializeField]
    private GameObject initialPanel, objectsPanel, noteBookPanel, endPanel;
    [SerializeField]
    private TMP_Text correctObjectsText, wrongObjectsText, finalOpportunitiesText;
    /// <summary>
    /// Panel donde se encuentra la lista de objetos a recoger.
    /// </summary>
    public GameObject PanelList { get => initialPanel; set => initialPanel = value; }
    /// <summary>
    /// Lista de objetos a recoger.
    /// </summary>
    public Text TextList { get; set; }

    // 1 = Room & 0 = Bathroom
    int myActualRoom = 1;
    void Start()
    {
        audioMng = AudioManager.Instance;
        audioMng.Play(GameSound.LevelBGM);
        audioMng.Play(GameSound.NoteBook);
        jsonReader = GetComponent<JSONReader>();

        for (int i = 0; i < objectLists.Count; i++)
        {
            objectLists[i].objectList = new List<string>();
        }
        level = 1;
        InicializeDictionary();
        // TextList = PanelList.GetComponentInChildren<Text>();

        noteBookTr = noteBookGO.GetComponent<RectTransform>();
        state = State.BEDROOM;
        roomCam.gameObject.SetActive(true);
     //   roomCam.GetComponent<AspectRatioUtility>().Adjust();
        bathroomCam.gameObject.SetActive(false);
        drawerCam.gameObject.SetActive(false);
        firstAidKitCam.gameObject.SetActive(false);
        roomButton.SetActive(true);
        buttonBackToRoom.SetActive(false);
        initialLuggagePos = luggage.transform.position;
        initialLuggageScale = luggage.transform.localScale;
        bttnEnd.gameObject.SetActive(true);

        initialPanel.SetActive(true);
        objectsPanel.SetActive(true);
        noteBookPanel.SetActive(false);
        endPanel.SetActive(false);
        checkOpportunities = maxcheckOpportunities;
        opportunitiesText.text = checkOpportunities.ToString();
        blackScreen.gameObject.SetActive(true);
    }
    private void InicializeDictionary()
    {
        objectsDictionary = new Dictionary<string, ObjectsInfo>();
        for (int i = 0; i < objectsInfo.Count; i++)
        {
            objectsDictionary.Add(objectsInfo[i].name, objectsInfo[i]);
        }
        storageDictionary = new Dictionary<string, List<Transform>>();
        for (int i = 0; i < storeInfo.Count; i++)
        {

            storageDictionary.Add(storeInfo[i].name, storeInfo[i].positions);

        }
        checkBoxDictionary = new Dictionary<string, CheckBox>();
    }

    /// <summary>
    /// Recoge del fichero las prendas según los parámetros.
    /// </summary>
    /// <param name="cola">Cola con la lista de objetos a procesar.</param>
    /// <param name="genero">Genero de la prenda a recoger.</param>
    /// <param name="fin">Hasta donde leemos del fichero.</param>
    private void ReadLevelInfo(LevelInfo info)
    {
        string gen = "";
        //objetos de escena
        for (int i = 0; i < info.storagePoints.Length; ++i)
        {
            //para cada punto de almacenamiento
            for (int j = 0; j < info.storagePoints[i].objects.Length; ++j)
            {
                if (info.storagePoints[i].objects[j].gender == gen || info.storagePoints[i].objects[j].gender == "N")
                {
                    string objectID = info.storagePoints[i].objects[j].name;
                    //GM.Gm.SceneObjects.Add(objectID);
                    Debug.Log(objectID);
                    storageDictionary.TryGetValue(info.storagePoints[i].name, out List<Transform> l);

                    GameObject go = objectsDictionary[objectID].go;
                    go.transform.SetParent(l[info.storagePoints[i].objects[j].position]);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.SetActive(true);
                    go.GetComponent<DraggNDrop>().setName(objectID, LocalizationManager.Lm.getWord(objectID));

                }
            }
        }
    }


    public void addToLuggage(string o)
    {
        checkBoxDictionary[o].SetCheckBoxState(CheckBoxState.Correct);
    }
    public void removefromLuggage(string o)
    {
        checkBoxDictionary[o].SetCheckBoxState(CheckBoxState.None);
    }
    //abrir armario de botiquin
    public void GoToFirstAidKit()
    {
        if (state != State.END)
        {
            audioMng.Play(GameSound.MedicineOpen);

            state = State.FIRSTAIDKIT;
            firstAidKitCam.gameObject.SetActive(true);
            bathroomCam.gameObject.SetActive(false);

            bttnEnd.gameObject.SetActive(false);
            buttonBackToRoom.SetActive(true);
            roomButton.gameObject.SetActive(false);
            if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)

                Xasu.HighLevel.GameObjectTracker.Instance.Interacted("FirstAidKit").WithResultExtensions(new Dictionary<string, object> { { "https://" + "open", "storagePoint" } });

        }
    }
    //abrir un cajon
    public void GoToDrawer(GameObject drawer)
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(false);
            state = State.DRAWER;

            drawerCam.gameObject.SetActive(true);

            if (myActualRoom == (int)State.BEDROOM)
            {
                roomCam.gameObject.SetActive(false);
            }
            else if (myActualRoom == (int)State.BATHROOM)
            {
                bathroomCam.gameObject.SetActive(false);
            }
            buttonBackToRoom.SetActive(true);
            roomButton.SetActive(false);

            if (drawer != null)
            {
                audioMng.Play(GameSound.DrawerOpen);

                drawerImage.SetActive(true);
                currentDrawer = drawer;
                currentDrawer.SetActive(true);
                luggage.transform.position = initialLuggagePos;
                luggage.transform.localScale = initialLuggageScale;
                if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)

                    Xasu.HighLevel.GameObjectTracker.Instance.Interacted(drawer.name).WithResultExtensions(new Dictionary<string, object> { { "https://" + "open", "storagePoint" } }); ;
            }
        }
    }


    public void End()
    {
        state = State.END;
        StringBuilder cad = new StringBuilder();


        //calcular las estrellas conseguidas
        int starsCompleted = 0;
        if (luggage.ObjetosGuardados.Count() > luggage.ObjetosList.Count() / 2.0f)
        {
            starsCompleted++;
            stars[0].transform.GetChild(0).gameObject.SetActive(true);
            if (luggage.ObjetosGuardados.Count() == luggage.ObjetosList.Count())
            {
                starsCompleted++;

                stars[1].transform.GetChild(0).gameObject.SetActive(true);
                if (luggage.ObjetosErroneosGuardados.Count() == 0)
                {
                    starsCompleted++;

                    stars[2].transform.GetChild(0).gameObject.SetActive(true);
                    if (checkOpportunities == maxcheckOpportunities)
                    {
                        starsCompleted++;

                        stars[3].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

            }
        }
        wrongObjectsText.text = luggage.ObjetosErroneosGuardados.Count().ToString();

        correctObjectsText.text = luggage.ObjetosGuardados.Count().ToString() + " / " + luggage.ObjetosList.Count().ToString();

        finalOpportunitiesText.text = (maxcheckOpportunities - checkOpportunities).ToString() + " / " + maxcheckOpportunities.ToString();

        Debug.Log("hsfuh"+PlayerPrefs.GetInt(LevelNameGlobal));
        Debug.Log(starsCompleted);
        if (PlayerPrefs.GetInt(LevelNameGlobal) <= starsCompleted)
        { 
            PlayerPrefs.SetInt(LevelNameGlobal, starsCompleted);
            Debug.Log("aaa" + PlayerPrefs.GetInt(LevelNameGlobal));
        }
       
        //actualizar el checkbox de los objetos de la lista
        for (int i = 0; i < /*GM.Gm.List.Count()*/ 10; i++)
        {
            CheckBox c = checkBoxDictionary[/*GM.Gm.List[i]*/"2"];
            if (c.GetCheckBoxState() == CheckBoxState.None)
                c.SetCheckBoxState(CheckBoxState.Wrong);
        }

        objectsPanel.SetActive(true);
        bttnEnd.gameObject.SetActive(false);
        roomButton.SetActive(false);
        endPanel.gameObject.SetActive(true);

        //mandar traza del estado del nivel
        if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)

            Xasu.HighLevel.CompletableTracker.Instance.Completed(LevelNameGlobal, Xasu.HighLevel.CompletableTracker.CompletableType.Level).WithSuccess(true).WithResultExtensions(new Dictionary<string, object> {
            {"https://" + "estrellasCompletas",starsCompleted },
            {"https://" + "wrongObjects", luggage.ObjetosErroneosGuardados.Count().ToString()},
            {"https://" + "correctObjects", luggage.ObjetosGuardados.Count().ToString() + " / " + luggage.ObjetosList.Count().ToString()},
            {"https://" +"checkListOpportunities", (maxcheckOpportunities - checkOpportunities).ToString() + " / " + maxcheckOpportunities.ToString()},
            {"https://" +"LuggageCorrectContent", luggage.ObjetosGuardados},
            {"https://" +"LuggageWrongContent", luggage.ObjetosErroneosGuardados}
        });

    }
}
