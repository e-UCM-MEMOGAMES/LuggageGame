using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xasu.HighLevel;


public class Tutorial : MonoBehaviour
{
    /// <summary>
    /// Estados del tutorial (en que momentos se muestra cada panel)
    /// </summary>
    enum States
    {
        PRESS_ITEM, DRAG_IN, CHECK_SUITCASE, CHECK_NAME, DRAG_OUT, CLOSE_SUITCASE, PUT_BACK,
        CHECK_DRAWER, EXIT_DRAWER, GO_TO_BATHROOM, CHECK_LIST, LIST_TRIES, GO_TO_BEDROOM, FINISH, 
        LAST
    };

    /// <summary>
    /// Instancia del LevelManager
    /// </summary>
    [SerializeField]
    LevelManager levelManager;
    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;

    [Header("State info and animations")]
    /// <summary>
    /// Paneles con la informacion de cada estado del tutorial
    /// </summary>
    [SerializeField] GameObject[] infoPanels;
    /// <summary>
    /// Animator de la mano (para cambiar las animaciones segun el estado)
    /// </summary>
    [SerializeField] Animator handAnimator;

    [Header("Items and buttons to disable at the beginning")]
    /// <summary>
    /// Boton de ir al bano desde la habitacion
    /// </summary>
    [SerializeField] GameObject bathroomButton;
    /// <summary>
    /// Boton de ir a la habitacion desde el bano
    /// </summary>
    [SerializeField] GameObject bedroomButton;
    /// <summary>
    /// Boton de cerrar la maleta
    /// </summary>
    [SerializeField] GameObject exitCaseButton;
    /// <summary>
    /// Boton para abrir la lista de objetos
    /// </summary>
    [SerializeField] GameObject listButton;
    /// <summary>
    /// Botones de todos los cajones
    /// </summary>
    [SerializeField] Button[] drawers;


    [Header("Objects to check in each state")]
    /// <summary>
    /// Vista desde la maleta
    /// </summary>
    [SerializeField] GameObject caseView;
    /// <summary>
    /// Elementos del bano de la vista normal
    /// </summary>
    [SerializeField] GameObject bathroom;
    /// <summary>
    /// Boton de la maleta de la vista normal
    /// </summary>
    [SerializeField] Button caseButton;
    /// <summary>
    /// Lista de objetos
    /// </summary>
    [SerializeField] GameObject itemList;
    /// <summary>
    /// Elementos de la habitacion de la vista normal
    /// </summary>
    [SerializeField] GameObject bedroom;

    /// <summary>
    /// Instancia del objeto que meter a la maleta en la vista normal
    /// </summary>
    GameObject neededItem,
    /// <summary>
    /// Instancia del objeto que meter a la maleta en la vista de la maleta
    /// </summary>
    neededItemStored;
    /// <summary>
    /// Si se esta arrastrando el objeto necesario
    /// </summary>
    bool draggingNeededItem = false,
    /// <summary>
    /// Si se ha pasado el puntero por encima del objeto necesario
    /// </summary>
    pointerOverNeededItemStored = false;

    /// <summary>
    /// Paso del tutorial reproduciendose actualmente
    /// </summary>
    States currState;


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        trackerManager = TrackerManager.Instance;

        // Se desactivan todos los elementos que no se utilizan en el primer paso del tutorial
        caseButton.interactable = false;
        bathroomButton.SetActive(false);
        bedroomButton.SetActive(false);
        listButton.SetActive(false);
        exitCaseButton.SetActive(false);
        foreach (Button drawerButton in drawers)
        {
            drawerButton.interactable = false;
        }
        foreach (GameObject panel in infoPanels)
        {
            panel.SetActive(false);
        }

        currState = States.PRESS_ITEM;
        ChangeState(0);

        // Se configuran las instancias del objeto necesario
        StartCoroutine(SetupNeededItem());
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case States.PRESS_ITEM:
                if (draggingNeededItem)
                {
                    ChangeState(1);
                }
                break;
            case States.DRAG_IN:
                if (!draggingNeededItem && !neededItemStored.activeSelf)
                {
                    ChangeState(-1);
                }
                else if (!draggingNeededItem && neededItemStored.activeSelf)
                {
                    ChangeState(1);
                    caseButton.interactable = true;
                }
                break;
            case States.CHECK_SUITCASE:
                if (caseView.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case States.CHECK_NAME:
                if (pointerOverNeededItemStored)
                {
                    ChangeState(1);
                }
                break;
            case States.DRAG_OUT:
                if (neededItem.activeSelf)
                {
                    ChangeState(1);
                    exitCaseButton.SetActive(true);
                }
                break;
            case States.CLOSE_SUITCASE:
                if (!caseView.activeSelf)
                {
                    ChangeState(1);
                    caseButton.interactable = false;
                }
                break;
            case States.PUT_BACK:
                if (!neededItem.activeSelf)
                {
                    ChangeState(1);

                    foreach (Button drawerButton in drawers)
                    {
                        drawerButton.interactable = true;
                    }
                }
                break;
            case States.CHECK_DRAWER:
                if (caseView.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case States.EXIT_DRAWER:
                if (!caseView.activeSelf)
                {
                    ChangeState(1);
                    bathroomButton.SetActive(true);

                    foreach (Button drawerButton in drawers)
                    {
                        drawerButton.interactable = false;
                    }
                }
                break;
            case States.GO_TO_BATHROOM:
                if (bathroom.activeSelf)
                {
                    ChangeState(1);
                    listButton.SetActive(true);
                }
                break;
            case States.CHECK_LIST:
                if (itemList.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case States.LIST_TRIES:
                if (!itemList.activeSelf && Input.GetMouseButtonDown(0))
                {
                    ChangeState(1);
                    bedroomButton.SetActive(true);
                }
                break;
            case States.GO_TO_BEDROOM:
                if (bedroom.activeSelf)
                {
                    ChangeState(1);

                    caseButton.interactable = true;

                    foreach (Button drawerButton in drawers)
                    {
                        drawerButton.interactable = true;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Se configuran las instancias de los objetos necesarios
    /// (tiene que hacerse con retardo porque los objetos se instancian en el Start del LevelManager)
    /// </summary>
    private IEnumerator SetupNeededItem()
    {
        yield return new WaitForEndOfFrame();

        // Se obtiene el primer (y en teoria unico) elemento de la lista de objetos necesarios
        neededItem = levelManager.ScenarioStoredItemsPairs.First().Value.ScenarioItem;
        neededItemStored = levelManager.ScenarioStoredItemsPairs.First().Value.StoredItem;

        // Se anaden los eventos de arrastrar al objeto de la vista normal
        EventTrigger evtTrigger = neededItem.AddComponent<EventTrigger>();

        // Empezar a arrastrar
        EventTrigger.Entry pointerdown = new EventTrigger.Entry();
        pointerdown.eventID = EventTriggerType.PointerDown;
        pointerdown.callback.AddListener((data) =>
        {
            draggingNeededItem = true;
        });
        evtTrigger.triggers.Add(pointerdown);

        // Terminar de arrastrar
        EventTrigger.Entry pointerup = new EventTrigger.Entry();
        pointerup.eventID = EventTriggerType.PointerUp;
        pointerup.callback.AddListener((data) =>
        {
            draggingNeededItem = false;
        });
        evtTrigger.triggers.Add(pointerup);


        // Se anade el evento de pasar el puntero por el objeto de la vista de la maleta
        evtTrigger = neededItemStored.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerOver = new EventTrigger.Entry();
        pointerOver.eventID = EventTriggerType.PointerEnter;
        pointerOver.callback.AddListener((data) =>
        {
            pointerOverNeededItemStored = true;
        });
        evtTrigger.triggers.Add(pointerOver);
    }

    /// <summary>
    /// Actualiza el paso actual la cantidad de pasos indicada
    /// </summary>
    private void ChangeState(int increase)
    {
        // Oculta el panel del estado actual
        if ((int)currState >= 0 && infoPanels.Length > 0)
        {
            infoPanels[(int)currState].SetActive(false);
        }
        // Actualiza el estado
        currState += increase;
        // Muestra el panel del nuevo estado actual
        if ((int)currState < infoPanels.Length)
        {
            infoPanels[(int)currState].SetActive(true);
        }
        // Hace la transicion de la animacion a la del nuevo estado actual
        handAnimator.SetInteger("step", (int)currState);

        trackerManager.TrySendStatement(CompletableTracker.Instance.Progressed("Tutorial", CompletableTracker.CompletableType.Level, (float)currState / (int)States.LAST));
    }
}
