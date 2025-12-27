using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Xasu.HighLevel;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    LevelManager levelManager;
    TrackerManager trackerManager;

    string id = "";
    GameObject oppositeObj;

    Vector3 initialPos;
    RectTransform rectTr, originalParent;
    int originalIndex = 0;

    [SerializeField]
    bool stored = false;
    [SerializeField] GameObject caseObj;
    bool dragging, colliding = false;


    // Start is called before the first frame update
    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalParent = gameObject.transform.parent.GetComponent<RectTransform>();
        originalIndex = transform.GetSiblingIndex();

        dragging = false;
        colliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliding = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }


    public void Initialize(LevelManager lm, string itemId, GameObject opposite, GameObject caseO)
    {
        levelManager = lm;
        id = itemId;
        initialPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = initialPos;
        oppositeObj = opposite;

        caseObj = caseO;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        dragging = true;
        transform.SetParent(caseObj.transform.parent, false);
        transform.SetSiblingIndex(caseObj.transform.GetSiblingIndex() + 1);

        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(stored ? "storedItem" : "scenarioItem")
            .WithResultExtensions(new Dictionary<string, object> {
                {"https://dragging",  id }
            })
        );
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        levelManager.PointerOutItem();
        transform.position = new Vector3(pointerEventData.position.x, pointerEventData.position.y, initialPos.z);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        dragging = false;
        if ((!stored && colliding) || (stored && !colliding))
        {
            gameObject.SetActive(false);
            oppositeObj.SetActive(true);

            if (!stored)
            {
                levelManager.StoreItem(id);
            }
            else
            {
                levelManager.ReturnItem(id);
            }
        }
        transform.localPosition = initialPos;
        transform.SetParent(originalParent, false);
        transform.SetSiblingIndex(originalIndex);

        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(stored ? "storedItem" : "scenarioItem")
            .WithResultExtensions(new Dictionary<string, object> {
                {"https://dropping",  id }
            })
        );
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!dragging)
        {
            levelManager.PointerEnterItem(id, rectTr);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!dragging)
        {
            levelManager.PointerOutItem();
        }
    }
}
