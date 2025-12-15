using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Scripts.Constantes;
// para leer de txt
using System.IO;
using System.Linq;
using UnityEngine.Analytics;

public class LevelSelector : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private GameObject _climaButtons;
    [SerializeField]
    private GameObject _decoration;
    [SerializeField]
    private GameObject _levelButtons;
    [SerializeField]
    private GameObject _panelList;

    public GameObject creditsButton;

    #endregion

    #region Atributos

    /// <summary>
    /// Objeto que contiene los elementos decorativos del menú. Imagen del hombre, panel, bocadillo...
    /// </summary>
    public GameObject decoration { get => _decoration; set => _decoration = value; }

    /// <summary>
    /// Botones para seleccionar la dificultad.
    /// </summary>
    public GameObject levelButtons { get => _levelButtons; set => _levelButtons = value; }

    /// <summary>
    /// Botones para elegir clima.
    /// </summary>
    public GameObject ClimaButtons { get => _climaButtons; set => _climaButtons = value; }

    /// <summary>
    /// Panel donde se encuentra la lista de objetos a recoger.
    /// </summary>
    public GameObject PanelList { get => _panelList; set => _panelList = value; }

    /// <summary>
    /// Nombre completo del nivel.
    /// </summary>
    public static string LevelNameGlobal { get; set; } = string.Empty;

    /// <summary>
    /// Lista de objetos a recoger.
    /// </summary>
    public Text TextList { get; set; }

    /// <summary>
    /// Nivel establecido por el jugador.
    /// </summary>
    public int Level { get; set; }

    private int levelSelected;
    [SerializeField]
    private List<LevelButton> levels;
    #endregion

    #region Eventos

    private void Start()
    {
        levelSelected = -1;
        ClimaButtons.SetActive(true);
        levelButtons.SetActive(false);
 
    }

    #endregion


}
