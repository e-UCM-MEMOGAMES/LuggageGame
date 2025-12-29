using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
    enum State { PANEL1, PANEL2, PANEL3 };

    [SerializeField]
    LevelManager levelManager;

    [SerializeField]
    GameObject[] infoPanels;
    
    [SerializeField]
    Animator handAnimator;

    [SerializeField] 
    GameObject neededItemPrefab, neededItemStoredPrefab;
    GameObject neededItem, neededItemStored;
    bool draggingNeededItem = false;

    State currState;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject panel in infoPanels)
        {
            panel.SetActive(false);
        }

        currState = State.PANEL1;
        ChangeState(0);

        StartCoroutine(SetupNeededItem());
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == State.PANEL1 && draggingNeededItem)
        {
            ChangeState(1);
            handAnimator.SetInteger("step", (int)State.PANEL2);
        }

        if (currState == State.PANEL2)
        {
            if (!draggingNeededItem && !neededItemStored.activeSelf)
            {
                ChangeState(-1);
                handAnimator.SetInteger("step", (int)State.PANEL1);
            }
            else if (!draggingNeededItem && neededItemStored.activeSelf)
            {
                ChangeState(1);
                handAnimator.SetInteger("step", (int)State.PANEL3);
            }
        }
    }

    private IEnumerator SetupNeededItem()
    {
        yield return new WaitForEndOfFrame();

        neededItem = AddDragEvents(neededItemPrefab);
        neededItemStored = AddDragEvents(neededItemStoredPrefab);
    }

    private GameObject AddDragEvents(GameObject prefab)
    {
        string itemName = $"{prefab.name}(Clone)";
        if (levelManager.ScenarioItems.ContainsKey(itemName))
        {
            GameObject item = levelManager.ScenarioItems[itemName].gameObject;
            EventTrigger evtTrigger = item.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerdown = new EventTrigger.Entry();
            pointerdown.eventID = EventTriggerType.PointerDown;
            pointerdown.callback.AddListener((data) => {
                draggingNeededItem = true;
            });
            evtTrigger.triggers.Add(pointerdown);

            EventTrigger.Entry pointerup = new EventTrigger.Entry();
            pointerup.eventID = EventTriggerType.PointerUp;
            pointerup.callback.AddListener((data) => {
                draggingNeededItem = false;
            });
            evtTrigger.triggers.Add(pointerup);

            return item;
        }
        return null;
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
    }
}
