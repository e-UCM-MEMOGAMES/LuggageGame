using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xasu;
using static System.Net.WebRequestMethods;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    GameObject twin;
    public GameObject panelInfo;
    TMP_Text nameInfo;
    Vector3 StartPoint;
    Vector3 Offset;
    bool hasExit;
    Luggage luggage;

    string id = " ";
    string translatedName=" ";

    void Start()
    {
        StartPoint = transform.localPosition;
        nameInfo = panelInfo.GetComponentInChildren<TMP_Text>();
        hasExit = false;
        luggage = transform.parent.GetComponent<Luggage>();
    }

    public void SetTwin(GameObject ob)
    {
        twin = ob;
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        nameInfo.text = translatedName;
        panelInfo.SetActive(true);
    }
    private void OnMouseExit()
    {
        panelInfo.SetActive(false);
    }

    private  void  OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x == Screen.width - 1 || Input.mousePosition.y == Screen.height - 1)
        {
            hasExit = false;
            transform.localPosition = StartPoint;
        }
        if (Input.GetMouseButtonUp(0))
        {
      

            if (hasExit)
            {
                panelInfo.SetActive(false);
                luggage.RemoveObject(this);
                twin.SetActive(true);
            }
            else if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
                Xasu.HighLevel.GameObjectTracker.Instance.Interacted(id).WithResultExtensions(new Dictionary<string, object> { { "https://"+ "dropBack", "luggageObject" } });

            hasExit = false;
            transform.localPosition = StartPoint;

        }
    }

    /// <summary>
    /// Evento cuando se clicka el objeto.
    /// </summary>
    private  void OnMouseDown()
    {
        if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
            Xasu.HighLevel.GameObjectTracker.Instance.Interacted(id).WithResultExtensions(new Dictionary<string, object> { {  "https://" + "clickOn", "luggageObject" } });

        panelInfo.SetActive(true);
        //StartPoint = transform.localPosition;
        Offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
    }
 
    /// <summary>
    /// Evento cuando se mantiene pulsado el objeto.
    /// </summary>
    private void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + Offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }
    private void OnMouseUp()
    {
        panelInfo.SetActive(false);


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
        {
            throw new System.ArgumentNullException(nameof(collision));
        }

        hasExit = true;

    }
    public void SetName(string i,string n)
    {
        id = i;
        translatedName = n;
    }
    public string GetID()
    {
        return id;
    }

}
