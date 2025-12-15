using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.Constantes;
using Xasu;
using System;


public class GM : MonoBehaviour
{
    #region Variables Unity

    [SerializeField]
    private int _clima;
    [SerializeField]
    private int _genero;
    [SerializeField]
    private int _level;

    #endregion

    #region Atributos

    /// <summary>
    /// Lista de objetos a poner en la maleta.
    /// </summary>
    public List<string> List { get; set; }

    /// <summary>
    /// Lista de objetos del nivel.
    /// </summary>
    public List<string> SceneObjects { get; set; }

    /// <summary>
    /// Manejado general del juego.
    /// </summary>
    public static GM Gm;

    public List<string> ObstaculosList { get; set; }

    /// <summary>
    /// Clima del juego.
    /// </summary>
    public Clima Clima
    {
        get => (Clima)_clima; set
        {
             if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
            {
                switch (value)
                {
                    case Clima.CALIDO:
                        Xasu.HighLevel.AlternativeTracker.Instance.Selected("climate", "Warm");
                        break;
                    case Clima.FRIO:
                        Xasu.HighLevel.AlternativeTracker.Instance.Selected("climate", "Cold");
                        break;
                    default:
                        Xasu.HighLevel.AlternativeTracker.Instance.Selected("climate", "Neutral");
                        break;
                }
            }
            _clima = (int)value;
        }
    }

    /// <summary>
    /// Género del juego.
    /// </summary>
    public Genero Genero
    {
        get => (Genero)_genero; set
        {

            _genero = (int)value;
        }
    }

    #endregion


    private void Awake()
    {
        if (Gm == null)
        {
            Gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Gm != this)
        {
            Destroy(gameObject);
        }
        List = new List<string>();
        ObstaculosList = new List<string>();
    }


    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }
}
