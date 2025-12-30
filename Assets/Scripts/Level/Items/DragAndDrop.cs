using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Xasu.HighLevel;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Instancia del LevelManager
    /// </summary>
    LevelManager levelManager;
    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;

    /// <summary>
    /// Id del objeto
    /// </summary>
    string id = "";
    /// <summary>
    /// Objeto opuesto (si este objeto es el de la vista normal, 
    /// el objeto opuesto es el de la vista de la maleta)
    /// </summary>
    GameObject oppositeObj;

    /// <summary>
    /// Posicion en la que aparece el objeto
    /// </summary>
    Vector3 initialPos;
    /// <summary>
    /// RectTransform del objeto
    /// </summary>
    RectTransform rectTr,
    /// <summary>
    /// RectTransform del objeto padre en el que se instancia el objeto
    /// </summary>
    originalParent;
    /// <summary>
    /// Posicion en la jerarquia en la que se instancia el objeto
    /// </summary>
    int originalIndex = 0;

    /// <summary>
    /// Si el objeto esta dentro de la maleta o no
    /// </summary>
    [SerializeField]
    bool stored = false;

    /// <summary>
    /// Maleta (si este objeto es el de la vista normal, la maleta sera 
    /// la de la vista normal y lo mismo con la vista desde la maleta)
    /// </summary>
    [SerializeField] 
    GameObject caseObj;
    /// <summary>
    /// Si se esta arrastrando el objeto
    /// </summary>
    bool dragging,
    /// <summary>
    /// Si el objeto esta colisionando con la zona de la maleta
    /// </summary>
    colliding = false;


    // Start is called before the first frame update
    void Start()
    {
        trackerManager = TrackerManager.Instance;

        rectTr = GetComponent<RectTransform>();
        originalParent = gameObject.transform.parent.GetComponent<RectTransform>();
        originalIndex = transform.GetSiblingIndex();

        dragging = false;
        colliding = false;

    }


    /* No se filtra entre los objetos con los que colisiona/deja de colisionar
     * porque el unico trigger de la escena es el de la maleta y los objetos 
     * del escenario estan configurados para colisionar solo con la maleta)
    */

    /// <summary>
    /// Llamado cuando el objeto entra a un trigger
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliding = true;
    }
    /// <summary>
    /// Llamado cuando el objeto sale de un trigger
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }


    /// <summary>
    /// Llamado por el LevelManager al instanciar el objeto. Configura 
    /// los elementos necesarios para que el DragAndDrop funcione
    /// </summary>
    public void Initialize(LevelManager lm, string itemId, GameObject opposite, GameObject caseO)
    {
        levelManager = lm;
        id = itemId;
        oppositeObj = opposite;
        caseObj = caseO;

        initialPos = transform.localPosition;
    }

    /// <summary>
    /// Llamado al empezar a arrastrar el objeto
    /// </summary>
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        dragging = true;

        // Se mueve el objeto en la jerarquia para que se renderice por encima de la maleta
        transform.SetParent(caseObj.transform.parent, false);
        transform.SetSiblingIndex(caseObj.transform.GetSiblingIndex() + 1);

        string location = stored ? "suitcase" : "scenario";
        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(id)
            .WithResultExtensions(new Dictionary<string, object> {
                { $"https://draggingFrom", location}
            })
        );
    }
    /// <summary>
    /// Llamado al arrastrar el objeto. Cambia su posicion a la del puntero
    /// </summary>
    public void OnDrag(PointerEventData pointerEventData)
    {
        transform.position = pointerEventData.position;
    }
    /// <summary>
    /// Llamado al acabar de arrastrar el objeto
    /// </summary>
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        dragging = false;
        // Si estaba en la vista normal y colisiona con la maleta o si
        // estaba en la vista de la maleta y no esta dentro de su colision
        if ((!stored && colliding) || (stored && !colliding))
        {
            // Desactiva el objeto y activa su opuesto
            gameObject.SetActive(false);
            oppositeObj.SetActive(true);

            // Si no estab en la maleta, lo guarda
            if (!stored)
            {
                levelManager.StoreItem(id);
            }
            // Si estaba en la maleta, lo saca
            else
            {
                levelManager.ReturnItem(id);
            }

            levelManager.PointerOutItem();
        }

        // Devuelve el objeto a su posicion inicial tanto en la escena como en la jerarquia
        transform.localPosition = initialPos;
        transform.SetParent(originalParent, false);
        transform.SetSiblingIndex(originalIndex);


        string location = stored ? "suitcase" : "scenario";
        trackerManager.TrySendStatement(GameObjectTracker.Instance.Interacted(id)
            .WithResultExtensions(new Dictionary<string, object> {
                { $"https://droppingFrom", location}
            })
        );
    }

    /// <summary>
    /// Llamado al pasar el puntero por encima del objeto
    /// </summary>
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // Si no se esta arrastrando, avisa al LevelManager
        if (!dragging)
        {
            levelManager.PointerEnterItem(id, rectTr);
        }
    }
    /// <summary>
    /// Llamado al quitar el puntero de encima del objeto
    /// </summary>
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // Si no se esta arrastrando, avisa al LevelManager
        if (!dragging)
        {
            levelManager.PointerOutItem();
        }
    }
}
