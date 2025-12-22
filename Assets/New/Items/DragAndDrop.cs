using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    LevelManager levelManager;
    string id = "";
    GameObject oppositeObj;
    Vector3 initialPos;
    RectTransform originalParent;
    int originalIndex = 0;

    [SerializeField]
    bool stored = false;

    GameObject caseObj;
    bool dragging, colliding = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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


    public void Initialize(LevelManager lm, string i, GameObject opposite, GameObject caseO)
    {
        levelManager = lm;
        id = i;
        oppositeObj = opposite;

        caseObj = caseO;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        dragging = true;
        transform.parent = caseObj.transform.parent;
        transform.SetSiblingIndex(caseObj.transform.GetSiblingIndex() + 1);

        // TODO: TRACKER
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
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
        transform.position = initialPos;
        transform.parent = originalParent;
        transform.SetSiblingIndex(originalIndex);

        // TODO: TRACKER
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!dragging)
        {
            Debug.Log("PointerEnter");
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!dragging)
        {
            Debug.Log("PointerExit");
        }
    }
}
