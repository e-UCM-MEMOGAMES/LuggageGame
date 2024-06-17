﻿using RAGE.Analytics;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using static Assets.Scripts.Constantes;
using static JSONReader;
using TMPro;
using static CheckBox;

public class LevelManager : MonoBehaviour
{

    enum State { BATHROOM, ROOM, DRAWER, LUGGAGE, FIRSTAIDKIT, END };
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
    public GameObject buttonBathroom;
    public Sprite[] bttnBathroom;
    [SerializeField]
    private List<Sprite> _tiposSuelos;
    [SerializeField]
    private GameObject _suelo;
    public GameObject buttonBackToRoom;
    public GameObject bttnEnd;
    public Luggage luggage;
    public GameObject drawerImage;
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

    // 0 = Room & 1 = Bathroom
    int myActualRoom = 0;
    //public Image
    void Start()
    {
        jsonReader = GetComponent<JSONReader>();

        for (int i = 0; i < objectLists.Count; i++)
        {
            objectLists[i].objectList = new List<string>();
        }
        level = GM.Gm.Level;
        InicializeDictionary();
       // TextList = PanelList.GetComponentInChildren<Text>();

        noteBookTr = noteBookGO.GetComponent<RectTransform>();
        GM.Gm.Genero = (Genero)PlayerPrefs.GetInt("genre", -1);
        SetLevel();

        state = State.ROOM;
        roomCam.gameObject.SetActive(true);
        bathroomCam.gameObject.SetActive(false);
        drawerCam.gameObject.SetActive(false);
        firstAidKitCam.gameObject.SetActive(false);
        buttonBathroom.SetActive(true);
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
    private void SetLevel()
    {

        PanelList.SetActive(true);
        int l = GM.Gm.Level;
        LevelNameGlobal = string.Empty;
        Debug.Log(level +" "+l);
        if (l != 0)
        {
            switch (l)
            {
                case 1:
                    LevelNameGlobal = "Level1";
                    break;
                case 2:
                    LevelNameGlobal = "Level2";
                    break;
                case 3:
                    LevelNameGlobal = "Level3";
                    break;
            }


            switch (GM.Gm.Clima)
            {
                case Clima.CALIDO:
                    LevelNameGlobal = string.Concat(LevelNameGlobal, "Warm");
                    break;
                case Clima.FRIO:
                    LevelNameGlobal = string.Concat(LevelNameGlobal, "Cold");
                    break;
            }


        }
        else LevelNameGlobal = "Tutorial";
        LoadList(LevelNameGlobal);
        Tracker.T.Completable.Initialized(LevelNameGlobal, CompletableTracker.Completable.Level);

    }

    /// <summary>
    /// Carga la lista de objetos a poner en la maleta.
    /// </summary>
    /// <param name="name">Nombre del fichero donde se van a cargar los datos.</param>
    private void LoadList(string name)
    {
        GM.Gm.List = new List<string>();
        GM.Gm.SceneObjects = new List<string>();

        TextAsset jsonFile = (TextAsset)Resources.Load(string.Concat("Lists/", name), typeof(TextAsset));
        Debug.Log(jsonFile.text);
        ReadLevelInfo(jsonReader.LoadFile(jsonFile.text));

        showList();

        luggage.InicializeList();
    }

    public void Ready()
    {

        initialPanel.SetActive(false);
        objectsPanel.SetActive(false);


    }
    public void Checklist()
    {
        if (checkOpportunities > 0)
        {
            checkOpportunities--;
            objectsPanel.SetActive(true);
            noteBookPanel.SetActive(true);
            opportunitiesText.text = checkOpportunities.ToString();
        }
    }
    public void ReturnFromCheckList()
    {

        objectsPanel.SetActive(false);
        noteBookPanel.SetActive(false);
    }

    /// <summary>
    /// Añadir a la categoria correspondiente de objeto
    /// </summary>
    private void addToList(string o)
    {
        ObjectType type = objectsDictionary[o].type;
        objectLists[(int)type].objectList.Add(o);
    }

    /// <summary>
    /// Generar la lista d eobjetos a poner en la maleta en UI
    /// </summary>
    private void showList()
    {

        float actualLineYPosition = 0;
        float actualCheckboxYPosition;

        for (int i = 0; i < objectLists.Count; i++)
        {
            if (objectLists[i].objectList.Count == 0)
            {
                objectLists[i].contentGO.SetActive(false);
                objectLists[i].titleGO.SetActive(false);
            }
            else
            {
                StringBuilder finalList = new StringBuilder();

                objectLists[i].titleTr.localPosition += actualLineYPosition * Vector3.up;
                objectLists[i].contentTr.localPosition += actualLineYPosition * Vector3.up;
                actualCheckboxYPosition = objectLists[i].contentTr.localPosition.y + 4;
                for (int j = 0; j < objectLists[i].objectList.Count; j++)
                {
                    GameObject checkbox = Instantiate(checkBoxPrefab, new Vector3(0, 0, 0),
                    checkBoxPrefab.transform.rotation, noteBookTr);

                    RectTransform checkboxTr = checkbox.GetComponent<RectTransform>();
                    checkboxTr.localPosition = new Vector2(objectLists[i].contentTr.localPosition.x - 35, actualCheckboxYPosition);
                    CheckBox c = checkbox.GetComponent<CheckBox>();
                    c.SetCheckBoxState(CheckBoxState.None);
                    checkBoxDictionary.Add(objectLists[i].objectList[j], c);

                    finalList.AppendLine(objectLists[i].objectList[j]);
                    actualLineYPosition += YOffsetBetweenLine;
                    actualCheckboxYPosition += YOffsetBetweenCheckBox;
                }
                objectLists[i].contentText.text = string.Concat(objectLists[i].contentText.text, finalList.ToString());

            }
            actualLineYPosition += YOffsetBetweenLine;
        }


    }


    /// <summary>
    /// Recoge del fichero las prendas según los parámetros.
    /// </summary>
    /// <param name="cola">Cola con la lista de objetos a procesar.</param>
    /// <param name="genero">Genero de la prenda a recoger.</param>
    /// <param name="fin">Hasta donde leemos del fichero.</param>
    private void ReadLevelInfo(LevelInfo info)
    {
        string gen;
        if (GM.Gm.Genero == Genero.HOMBRE) gen = "M";
        else gen = "F";

        //lectura de lista de objetos a introducir a la maleta
        if (gen == "M")
        {
            for (int i = 0; i < info.objectList_M.Length; ++i)
            {
                GM.Gm.List.Add(info.objectList_M[i]);
                addToList(info.objectList_M[i]);
            }
        }
        else
        {
            for (int i = 0; i < info.objectList_F.Length; ++i)
            {
                GM.Gm.List.Add(info.objectList_F[i]);
                addToList(info.objectList_F[i]);
            }
        }

        for (int i = 0; i < info.objectList_N.Length; ++i)
        {
            GM.Gm.List.Add(info.objectList_N[i]);
            addToList(info.objectList_N[i]);
        }

        //objetos de escena
        for (int i = 0; i < info.storagePoints.Length; ++i)
        {
            //para cada punto de almacenamiento
            for (int j = 0; j < info.storagePoints[i].objects.Length; ++j)
            {
                if (info.storagePoints[i].objects[j].gender == gen || info.storagePoints[i].objects[j].gender == "N")
                {
                    string objectName = info.storagePoints[i].objects[j].name;
                    GM.Gm.SceneObjects.Add(objectName);
                    storageDictionary.TryGetValue(info.storagePoints[i].name, out List<Transform> l);

                    GameObject go = objectsDictionary[objectName].go;
                    go.transform.SetParent(l[info.storagePoints[i].objects[j].position]);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.SetActive(true);

                }
            }
        }
    }

    /// <summary>
    /// Recoge del fichero los objetos de escena según los parámetros.
    /// </summary>
    /// <param name="cola">Cola con la lista de objetos a procesar.</param>
    /// <param name="genero">Genero de la prenda a recoger.</param>
    /// <param name="fin">Hasta donde leemos del fichero.</param>
    private void GetSceneObj(Queue<string> cola, Genero genero, string fin)
    {
        if (GM.Gm.Genero == genero || genero == Genero.NEUTRAL)
        {
            string objeto = cola.Dequeue();

            List<string> listObj;
            int j = 0;
            listObj = new List<string>(objeto.Split(',').Select(o => o.Trim()));
            Debug.Log(listObj[0]);
            while (!listObj[0].Equals(fin))
            {

                Debug.Log("&&&&&&&&&&&&&&& List : " + listObj[0] + " " + listObj[1] + " " + listObj[2]);
                if (listObj.Count == 3)
                {
                    GM.Gm.SceneObjects.Add(listObj[0]);

                    storageDictionary.TryGetValue(listObj[1], out List<Transform> l);

                    objectsDictionary[listObj[0]].go.transform.SetParent(l[int.Parse(listObj[2])]);
                    objectsDictionary[listObj[0]].go.transform.localPosition = new Vector3(0, 0, 0);
                    objectsDictionary[listObj[0]].go.SetActive(true);
                }

                objeto = cola.Dequeue();
                Debug.Log("a");
                listObj = new List<string>(objeto.Split(',').Select(o => o.Trim()));
                j++;
            }

        }
        else
        {
            string objeto = cola.Dequeue();
            while (!objeto.Equals(fin))
            {
                objeto = cola.Dequeue();
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
    public void GoToFirstAidKit()
    {
        if (state != State.END)
        {
            state = State.FIRSTAIDKIT;
            bathroomCam.gameObject.SetActive(false);
            firstAidKitCam.gameObject.SetActive(true);

            bttnEnd.gameObject.SetActive(false);
            buttonBackToRoom.SetActive(true);
            buttonBathroom.gameObject.SetActive(false);

            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            Tracker.T.Accessible.Accessed("FirstAidKit", AccessibleTracker.Accessible.Screen);
        }
    }

    public void GoToDrawer(GameObject drawer)
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(false);
            state = State.DRAWER;
            roomCam.gameObject.SetActive(false);
            drawerCam.gameObject.SetActive(true);

            if (myActualRoom == 0)
            {
                buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
                Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[0];
            }
            else if (myActualRoom == 1)
            {
                buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
                Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            }
            buttonBackToRoom.SetActive(true);
            buttonBathroom.SetActive(false);
            drawerImage.SetActive(true);

            if (drawer != null)
            {
                currentDrawer = drawer;
                currentDrawer.SetActive(true);
                luggage.transform.position = initialLuggagePos;
                luggage.transform.localScale = initialLuggageScale;
                Tracker.T.Accessible.Accessed(drawer.name, AccessibleTracker.Accessible.Screen);
            }
        }
    }

    public void GoToLuggage()
    {
        if (state != State.END)
        {
            if (currentDrawer != null) currentDrawer.SetActive(false);
            GoToDrawer(null);
            drawerImage.SetActive(false);
            state = State.LUGGAGE;
            luggage.gameObject.SetActive(true);
            luggage.transform.position = new Vector3(0, 19f, luggage.transform.position.z);
            luggage.transform.localScale = new Vector3(3.5f, 3.5f, 1);
            Tracker.T.Accessible.Accessed("Luggage", AccessibleTracker.Accessible.Screen);
        }

    }

    public void BackToRoom()
    {
        // boton salir de la maleta
        if (state != State.END)
        {
            if (myActualRoom == 0)
            {
                state = State.ROOM;
                roomCam.gameObject.SetActive(true);
                bathroomCam.gameObject.SetActive(false);
            }
            else if (myActualRoom == 1)
            {
                state = State.BATHROOM;
                roomCam.gameObject.SetActive(false);
                bathroomCam.gameObject.SetActive(true);
            }
            buttonBathroom.SetActive(true);
            drawerCam.gameObject.SetActive(false);
            if (currentDrawer != null)
                currentDrawer.SetActive(false);
            buttonBackToRoom.SetActive(false);
            bttnEnd.gameObject.SetActive(true);
        }
    }

    void GoToBathroom()
    {
        if (state != State.END)
        {
            bttnEnd.gameObject.SetActive(true);
            state = State.BATHROOM;
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[1];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[1];
            drawerCam.gameObject.SetActive(false);
            roomCam.gameObject.SetActive(false);
            bathroomCam.gameObject.SetActive(true);
            buttonBackToRoom.SetActive(false);
            Tracker.T.Accessible.Accessed("Bathroom", AccessibleTracker.Accessible.Screen);
        }

    }
    public void BathRoomButton()
    {
        if (currentDrawer != null) currentDrawer.SetActive(false);
        if (myActualRoom == 1)
        {
            myActualRoom = 0;
            Tracker.T.setVar("RoomButtom", 1);
            BackToRoom();
            buttonBathroom.GetComponent<Image>().sprite = bttnBathroom[0];
            Suelo.GetComponent<SpriteRenderer>().sprite = TiposSuelos[0];
        }
        else
        {
            myActualRoom = 1;
            Tracker.T.setVar("BathRoomButton", 1);
            GoToBathroom();
        }
    }

    public void End()
    {
        state = State.END;
        StringBuilder cad = new StringBuilder();

      //  cad.AppendLine(luggage.Check(level));

        //string s = luggage.GetObjetosErroneos();
        //if (s.Length > 0)
        //    cad.Append(s);

        int starsCompleted=0;
        if (luggage.ObjetosGuardados.Count() == luggage.ObjetosList.Count())
        {
            starsCompleted++;
            stars[0].transform.GetChild(0).gameObject.SetActive(true);

            if (luggage.ObjetosErroneosGuardados.Count() == 0)
            {
                starsCompleted++;
                stars[1].transform.GetChild(0).gameObject.SetActive(true);
              
                if (checkOpportunities == maxcheckOpportunities)
                {
                    starsCompleted++;
                    stars[2].transform.GetChild(0).gameObject.SetActive(true);

                }
            }

        }
        wrongObjectsText.text = luggage.ObjetosErroneosGuardados.Count().ToString();
     
        correctObjectsText.text = luggage.ObjetosGuardados.Count().ToString() + " / " + luggage.ObjetosList.Count().ToString();
       
        finalOpportunitiesText.text = (maxcheckOpportunities - checkOpportunities).ToString() + " / " + maxcheckOpportunities.ToString();

        if (PlayerPrefs.GetInt(LevelNameGlobal)<= starsCompleted)
            PlayerPrefs.SetInt(LevelNameGlobal, starsCompleted);
        Debug.Log(LevelNameGlobal + " " + PlayerPrefs.GetInt(LevelNameGlobal));
        for(int i = 0; i < GM.Gm.List.Count(); i++) {
            CheckBox c = checkBoxDictionary[GM.Gm.List[i]];
            if (c.GetCheckBoxState() == CheckBoxState.None)
                c.SetCheckBoxState(CheckBoxState.Wrong);
        }
        objectsPanel.SetActive(true);
        bttnEnd.gameObject.SetActive(false);
        buttonBathroom.SetActive(false);
        endPanel.gameObject.SetActive(true);
        //endPanel.GetComponentInChildren<Text>().text = cad.ToString();
        Tracker.T.setVar("EndButton", 1);
      //  Tracker.T.setVar("Resultado: " + cad.Length, cad.ToString());
        Tracker.T.Completable.Completed(LevelNameGlobal, CompletableTracker.Completable.Level, true);
    }
}
