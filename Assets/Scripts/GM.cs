using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.Constantes;
using Xasu;
using System;


public class GM : MonoBehaviour
{
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

    public int Level = 0;
    public Defs.Climate Clima = Defs.Climate.COLD;
    public Defs.Gender Genero = Defs.Gender.MALE;

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
}
