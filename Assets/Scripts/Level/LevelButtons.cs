using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Xasu.HighLevel;

public class LevelButtons : MonoBehaviour
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
    /// Instancia del LevelManager
    /// </summary>
    LevelManager levelManager;


    [Header("UI")]
    /// <summary>
    /// Panel inicial con los objetivos
    /// </summary>
    [SerializeField] GameObject initialPanel;
    /// <summary>
    /// Lista de objetos necesarios
    /// </summary>
    [SerializeField] GameObject itemList;
    /// <summary>
    /// Fondo y boton al abrir la lista durante el nivel
    /// </summary>
    [SerializeField] GameObject notebookPanel;
    /// <summary>
    /// Panel final
    /// </summary>
    [SerializeField] GameObject endPanel;
    /// <summary>
    /// Panel de advertencia al intentar terminar el nivel
    /// </summary>
    [SerializeField] GameObject warning;

    /// <summary>
    /// Texto del indicador de usos restantes de la lista
    /// </summary>
    [SerializeField] TextMeshProUGUI remainingListUsesText;

    [Header("Scenario")]
    /// <summary>
    /// Vista normal de las habitaciones
    /// </summary>
    [SerializeField] GameObject roomsView;
    /// <summary>
    /// Vista de la maleta
    /// </summary>
    [SerializeField] GameObject caseView;
    /// <summary>
    /// Vista normal de la habitacion
    /// </summary>
    [SerializeField] GameObject bedroom;
    /// <summary>
    /// Vista normal del bano
    /// </summary>
    [SerializeField] GameObject bathroom;
    /// <summary>
    /// Vista de la maleta en la habitacion
    /// </summary>
    [SerializeField] GameObject bedroomCaseView;
    /// <summary>
    /// Vista de la maleta en el bano
    /// </summary>
    [SerializeField] GameObject bathroomCaseView;
    /// <summary>
    /// Vista de los armarios del bano
    /// </summary>
    [SerializeField] GameObject cabinet;
    /// <summary>
    /// Objeto con la imagen del cajon
    /// </summary>
    [SerializeField] GameObject drawer;
    /// <summary>
    /// Puntos con objetos "ocultos" (cajones e interior de armarios)
    /// </summary>
    [SerializeField] GameObject[] hiddenSpawnpoints;
    /// <summary>
    /// Objeto "oculto" que se esta mostrando
    /// </summary>
    GameObject currentHiddenItems = null;

    [Header("Case")]
    /// <summary>
    /// Transform de la maleta en la vista desde la arriba
    /// </summary>
    [SerializeField] RectTransform topViewCaseTr;
    /// <summary>
    /// Escala de la maleta al abrir los cajones
    /// </summary>
    [SerializeField] float caseScale;
    Vector3 topViewCaseDrawerScale;

    /// <summary>
    /// Posicion Y de la maleta al abrir los cajones
    /// </summary>
    [SerializeField] float caseYDrawer;
    Vector3 topViewCaseDrawerPosition;
    /// <summary>
    /// Escala normal de la maleta
    /// </summary>
    Vector3 topViewCaseNormalScale,
    /// <summary>
    /// Posicion normal de la maleta
    /// </summary>
    topViewCaseNormalPosition;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
        trackerManager = TrackerManager.Instance;
        
        audioManager.Play(GameSound.Notebook);

        levelManager = GetComponent<LevelManager>();
        remainingListUsesText.text = levelManager.RemainingListUses.ToString();

        // Se guardan las posiciones y escalas de la maleta tanto para los cajones como para la vista normal
        topViewCaseDrawerScale = new Vector3(caseScale, caseScale, caseScale);
        topViewCaseDrawerPosition = new Vector3(topViewCaseTr.localPosition.x, caseYDrawer, topViewCaseTr.localPosition.z);
        topViewCaseNormalScale = topViewCaseTr.localScale;
        topViewCaseNormalPosition = topViewCaseTr.localPosition;

        // Se ocultan todos los elementos de la UI excepto el panel inicial
        warning.SetActive(false);
        notebookPanel.SetActive(false);
        initialPanel.SetActive(true);
        endPanel.SetActive(false);
        itemList.SetActive(true);

        // Se inicia en la vista normal de la habitacion y se ocultan todos los elementos "ocultos"
        GoToBedroom();
        CloseCase();
        foreach (GameObject point in hiddenSpawnpoints)
        {
            point.SetActive(false);
        }
    }


    /// <summary>
    /// Llamado por el boton de empezar el juego de la pantalla inicial. Oculta dicha pantalla y la lista de objetos
    /// </summary>
    public void StartGame()
    {
        initialPanel.SetActive(false);
        itemList.SetActive(false);
    }

    /// <summary>
    /// Llamado por el boton de abrir la lista y el de volver que aparece cuando se abre la lista
    /// </summary>
    public void ToggleItemList()
    {
        // Si la lista esta cerrada y quedan intentos restantes
        if (!notebookPanel.activeSelf && levelManager.RemainingListUses > 0)
        {
            // Se actualizan los intentos restantes y el texto
            levelManager.RemainingListUses--;
            remainingListUsesText.text = levelManager.RemainingListUses.ToString();

            // Se activan los elementos de la lista
            notebookPanel.SetActive(true);
            itemList.SetActive(true);

            audioManager.Play(GameSound.Notebook);
        }
        // Si no, si esta activa, se oculta
        else if (notebookPanel.activeSelf)
        {
            notebookPanel.SetActive(false);
            itemList.SetActive(false);

            audioManager.Play(GameSound.Notebook);
        }
    }

    /// <summary>
    /// Llamado por el boton de ir a la habitacion que aparece estando en el bano
    /// </summary>
    public void GoToBedroom()
    {
        // Activa los elementos de la habitacion tanto para el escenario como para la vista desde la maleta
        bedroom.SetActive(true);
        bedroomCaseView.SetActive(true);

        // Desactiva los elementos del bano tanto para el escenario como para la vista desde la maleta
        bathroom.SetActive(false);
        bathroomCaseView.SetActive(false);

        // Desactiva los elementos del armario 
        cabinet.SetActive(false);

        try
        {
            trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed("Bedroom"));
        }
        catch { }
    }

    /// <summary>
    /// Llamado por el boton de ir al bano que aparece estando en la habitacion y por el boton de salir de los armarios
    /// </summary>
    public void GoToBathroom()
    {
        // Desactiva los elementos de la habitacion tanto para el escenario como para la vista desde la maleta
        bedroom.SetActive(false);
        bedroomCaseView.SetActive(false);

        // Activa los elementos del bano tanto para el escenario como para la vista desde la maleta
        bathroom.SetActive(true);
        bathroomCaseView.SetActive(true);

        // Desactiva los elementos del armario 
        cabinet.SetActive(false);

        try
        {
            trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed("Bathroom"));
        }
        catch { }
    }

    /// <summary>
    /// Llamado por los botones de los armarios del bano
    /// </summary>
    public void GoToCabinet(GameObject cabinetItems)
    {
        // Activa los elementos del armario y se muestran los objetos indicados
        cabinet.SetActive(true);
        ShowHiddenElements(cabinetItems);

        try
        {
            trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed("Cabinet")
                .WithResultExtensions(new Dictionary<string, object> {
                { "https://cabinet", cabinetItems.name }
                })
            );
        }
        catch { }
    }

    /// <summary>
    /// Llamado por los botones de los cajones
    /// </summary>
    public void GoToDrawer(GameObject drawerItems)
    {
        // Se muestra la maleta y se ajustan su posicion y escala
        OpenCase();
        topViewCaseTr.localScale = topViewCaseDrawerScale;
        topViewCaseTr.localPosition = topViewCaseDrawerPosition;

        // Activa los elementos del cajon y se muestran los objetos indicados
        drawer.SetActive(true);
        ShowHiddenElements(drawerItems);

        try
        {
            trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed("Drawer")
                .WithResultExtensions(new Dictionary<string, object> {
                { "https://drawer", drawerItems.name }
                })
            );
        }
        catch { }
    }

    /// <summary>
    /// Muestra los objetos "ocultos" indicados
    /// </summary>
    private void ShowHiddenElements(GameObject items)
    {
        HideHiddenElements();
        currentHiddenItems = items;
        currentHiddenItems.SetActive(true);
    }
    /// <summary>
    /// Llamado por los botones de salir de los cajones y armarios del bano para ocultar los elementos "ocultos"
    /// </summary>
    public void HideHiddenElements()
    {
        if (currentHiddenItems != null)
        {
            currentHiddenItems.SetActive(false);
        }
    }

    /// <summary>
    /// Llamado por el boton de la maleta en la vista normal
    /// </summary>
    public void OpenCase(bool openingDrawer = true)
    {
        roomsView.SetActive(false);
        caseView.SetActive(true);

        if (!openingDrawer)
        {
            try
            {
                trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed("Case", AccessibleTracker.AccessibleType.Inventory));
            }
            catch { }
        }
    }

    /// <summary>
    /// Llamado por el boton de cerrar en la vista de la maleta y en los cajones
    /// </summary>
    public void CloseCase()
    {
        HideHiddenElements();

        roomsView.SetActive(true);
        caseView.SetActive(false);

        if (drawer.activeSelf)
        {
            audioManager.Play(GameSound.DrawerClose);
        }
        drawer.SetActive(false);

        topViewCaseTr.localScale = topViewCaseNormalScale;
        topViewCaseTr.localPosition = topViewCaseNormalPosition;
    }

    /// <summary>
    /// Llamado por el boton de confirmar del panel de advertencia al intentar terminar el nivel
    /// </summary>
    public void EndGame()
    {
        endPanel.SetActive(true);
        itemList.SetActive(true);
        warning.SetActive(false);

        audioManager.Play(GameSound.AirPlane);
    }

    /// <summary>
    /// Llamado por el boton de volver de la pantalla final
    /// </summary>
    public void Return()
    {
        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
        audioManager.Play(GameSound.MenuBGM);
    }

}
