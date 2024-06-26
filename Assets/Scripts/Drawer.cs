﻿using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
public class Drawer : MonoBehaviour
{

    public LevelManager lvlMngr;
    public GameObject drawer;
    SpriteRenderer spRenderer;

    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Cuando el ratón pasa por encima del objeto.
    /// </summary>
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        spRenderer.enabled = true;
    }

    /// <summary>
    /// Cuando el ratón sale del objeto.
    /// </summary>
    private void OnMouseExit()
    {
        spRenderer.enabled = false;
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonUp(0))
        {

            lvlMngr.GoToDrawer(drawer);
        }
    }
}
