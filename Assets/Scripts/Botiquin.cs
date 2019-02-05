﻿using UnityEngine;

/// <summary>
/// Script para las funcionalidades del botiquín.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class Botiquin : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private LevelManager levelManager;

    #endregion

    #region Atributos

    /// <summary>
    /// Objeto que maneja el nivel.
    /// </summary>
    public LevelManager LevelManager { get => levelManager; set => levelManager = value; }

    /// <summary>
    /// Sprite del objeto.
    /// </summary>
    public SpriteRenderer SpRenderer { get ; set; }

    #endregion

    #region Eventos

    /// <summary>
    /// Cuando comienza el objeto
    /// </summary>
    void Start()
    {
        SpRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Cuando el ratón pasa por encima del objeto.
    /// </summary>
    private void OnMouseEnter()
    {
        SpRenderer.enabled = true;
    }

    /// <summary>
    /// Cuando el ratón sale del objeto.
    /// </summary>
    private void OnMouseExit()
    {
        SpRenderer.enabled = false;
    }

    /// <summary>
    /// Cuando se hace click sobre el objeto.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            LevelManager.GoToFirstAidKit();
        }
    }

    #endregion

}
