﻿using RAGE.Analytics;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.Constantes;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggNDrop : MonoBehaviour
{
    #region Variables Unity

    [SerializeField]
    private Item _objetoMaleta;
    [SerializeField]
    private int _clima;
    [SerializeField]
    private int _genero;

    #endregion

    #region Constantes

    private float OFFSET_Z { get { return 10.0f; } }

    #endregion

    #region Atributos

    /// <summary>
    /// Objeto que referencia al guardado en la maleta.
    /// </summary>
    public Item ObjetoMaleta { get => _objetoMaleta; set => _objetoMaleta = value; }

    /// <summary>
    /// Clima del objeto.
    /// </summary>
    public Clima Clima { get => (Clima)_clima; set => _clima = (int)value; }

    /// <summary>
    /// Género del objeto.
    /// </summary>
    public Genero Genero { get => (Genero)_genero; set => _genero = (int)value; }

    /// <summary>
    /// Maleta del juego.
    /// </summary>
    public Luggage Maleta { get; set; }

    public bool ItsInTarget { get; set; }

    /// <summary>
    /// Posición inicial del movimiento.
    /// </summary>
    private Vector3 StartPoint { get; set; }

    /// <summary>
    /// Posición actual del movimiento.
    /// </summary>
    private Vector3 Offset { get; set; }
    public Genero GENERO;
    #endregion

    #region Eventos

    void Start()
    {
    
        //GENERO = GM.Gm.Genero;
        ItsInTarget = false;
        Maleta = ObjetoMaleta.transform.parent.gameObject.GetComponent<Luggage>();
    }

    /// <summary>
    /// Evento cuando se clicka el objeto.
    /// </summary>
    private void OnMouseDown()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;

        Tracker.T.setVar("Click en objeto", 1);
        Xasu.HighLevel.GameObjectTracker.Instance.Interacted("clickOnObject-"+_objetoMaleta.name);
        StartPoint = transform.position;
        Offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, OFFSET_Z));
    }

    /// <summary>
    /// Evento cuando se mantiene pulsado el objeto.
    /// </summary>
    private void OnMouseDrag()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;
     
        Tracker.T.setVar("Objeto pulsado", 1);
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, OFFSET_Z);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + Offset;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    /// <summary>
    /// Evento cuando se deja de clickar el objeto.
    /// </summary>
    private void OnMouseUp()
    {
        // if (EventSystem.current.IsPointerOverGameObject()) return;
        Xasu.HighLevel.GameObjectTracker.Instance.Interacted("dropObject-" + _objetoMaleta.name);
        Tracker.T.setVar("Deja de clickar en objeto", 1);
        transform.position = StartPoint;
        if (ItsInTarget)
        {
            Xasu.HighLevel.GameObjectTracker.Instance.Interacted("saveObjectInLuggage-" + _objetoMaleta.name);

            Maleta.SaveObject(ObjetoMaleta);
            ObjetoMaleta.SetTwin(gameObject);
            gameObject.SetActive(false);
        }
        ItsInTarget = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            throw new ArgumentNullException(nameof(collision));
        Debug.Log("Ontrigger");
        ItsInTarget = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
            throw new ArgumentNullException(nameof(collision));

        ItsInTarget = false;
    }

    #endregion

}