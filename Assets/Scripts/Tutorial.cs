using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Tutorial : MonoBehaviour
{
    enum State
    {
        PRESS_ITEM, DRAG_IN, CHECK_SUITCASE, CHECK_NAME, DRAG_OUT, CLOSE_SUITCASE, PUT_BACK,
        CHECK_DRAWER, EXIT_DRAWER, GO_TO_BATHROOM, CHECK_LIST, LIST_TRIES, GO_TO_BEDROOM, FINISH
    };

    [SerializeField]
    LevelManager levelManager;

    [Header("State info and animations")]
    [SerializeField] GameObject[] infoPanels;
    [SerializeField] Animator handAnimator;

    [Header("Items and buttons to disable at the beginning")]
    [SerializeField] GameObject bathroomButton;
    [SerializeField] GameObject bedroomButton;
    [SerializeField] GameObject listButton;
    [SerializeField] GameObject exitCaseButton;

    [Header("Objects to check in each state")]
    [SerializeField] GameObject caseView;
    [SerializeField] GameObject bathroom;
    [SerializeField] Button caseButton;
    [SerializeField] Button[] drawers;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject bedroom;
    GameObject neededItem, neededItemStored;
    bool draggingNeededItem = false, pointerOverNeededItemStored = false;

    State currState;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
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

        currState = State.PRESS_ITEM;
        ChangeState(0);

        StartCoroutine(SetupNeededItem());
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case State.PRESS_ITEM:
                if (draggingNeededItem)
                {
                    ChangeState(1);
                }
                break;
            case State.DRAG_IN:
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
            case State.CHECK_SUITCASE:
                if (caseView.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case State.CHECK_NAME:
                if (pointerOverNeededItemStored)
                {
                    ChangeState(1);
                }
                break;
            case State.DRAG_OUT:
                if (neededItem.activeSelf)
                {
                    ChangeState(1);
                    exitCaseButton.SetActive(true);
                }
                break;
            case State.CLOSE_SUITCASE:
                if (!caseView.activeSelf)
                {
                    ChangeState(1);
                    caseButton.interactable = false;
                }
                break;
            case State.PUT_BACK:
                if (!neededItem.activeSelf)
                {
                    ChangeState(1);

                    foreach (Button drawerButton in drawers)
                    {
                        drawerButton.interactable = true;
                    }
                }
                break;
            case State.CHECK_DRAWER:
                if (caseView.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case State.EXIT_DRAWER:
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
            case State.GO_TO_BATHROOM:
                if (bathroom.activeSelf)
                {
                    ChangeState(1);
                    listButton.SetActive(true);
                }
                break;
            case State.CHECK_LIST:
                if (itemList.activeSelf)
                {
                    ChangeState(1);
                }
                break;
            case State.LIST_TRIES:
                if (!itemList.activeSelf && Input.GetMouseButtonDown(0))
                {
                    ChangeState(1);
                    bedroomButton.SetActive(true);
                }
                break;
            case State.GO_TO_BEDROOM:
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

    private IEnumerator SetupNeededItem()
    {
        yield return new WaitForEndOfFrame();

        neededItem = levelManager.ScenarioStoredItemsPairs.First().Value.ScenarioItem;
        AddDragEvents(neededItem);

        neededItemStored = levelManager.ScenarioStoredItemsPairs.First().Value.StoredItem;
        AddDragEvents(neededItemStored);

        EventTrigger evtTrigger = neededItemStored.GetComponent<EventTrigger>();
        EventTrigger.Entry pointerOver = new EventTrigger.Entry();
        pointerOver.eventID = EventTriggerType.PointerEnter;
        pointerOver.callback.AddListener((data) =>
        {
            pointerOverNeededItemStored = true;
        });
        evtTrigger.triggers.Add(pointerOver);
    }

    private void AddDragEvents(GameObject item)
    {
        EventTrigger evtTrigger = item.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerdown = new EventTrigger.Entry();
        pointerdown.eventID = EventTriggerType.PointerDown;
        pointerdown.callback.AddListener((data) =>
        {
            draggingNeededItem = true;
        });
        evtTrigger.triggers.Add(pointerdown);

        EventTrigger.Entry pointerup = new EventTrigger.Entry();
        pointerup.eventID = EventTriggerType.PointerUp;
        pointerup.callback.AddListener((data) =>
        {
            draggingNeededItem = false;
        });
        evtTrigger.triggers.Add(pointerup);
    }

    private void ChangeState(int increase)
    {
        if ((int)currState >= 0 && infoPanels.Length > 0)
        {
            infoPanels[(int)currState].SetActive(false);
        }
        currState += increase;
        if ((int)currState < infoPanels.Length)
        {
            infoPanels[(int)currState].SetActive(true);
        }
        handAnimator.SetInteger("step", (int)currState);
    }
}
