using RAGE.Analytics;
using System;
using System.Collections.Generic;
using System.Text;
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
        GM.Gm.Genero = (Genero)PlayerPrefs.GetInt("genre", -1);

        if (!PlayerPrefs.HasKey("firstime"))
        {
            PlayerPrefs.SetInt("firstime", 1);
            SelectWeather(0);
        }

        //if (PlayerPrefs.GetInt("level3C") + PlayerPrefs.GetInt("level3W") >= 1) creditsButton.SetActive(true);
        //else creditsButton.SetActive(false);
    }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Establece el clima en el que se cargarán los datos.
    /// </summary>
    /// <param name="w">Valor númerico del clima: 0 -> AMBOS, 1 -> CÁLIDO, 2 -> FRÍO</param>
    public void SetWeather(int w)
    {
        GM.Gm.Clima = (Clima)w;
        for (int i = 0; i < levels.Count; i++)
            levels[i].CalcStars();
        ClimaButtons.SetActive(false);
        levelButtons.SetActive(true);
    }

    /// <summary>
    /// Establece el nivel de dificultad en el que se cargaran los datos del fichero.
    /// </summary>
    /// <param name="l">Nivel de dificultad del juego: 0 -> Tutorial</param>
    public void SetLevel(int l)
    {
        GM.Gm.Level = l;
        _decoration.SetActive(false);
        ClimaButtons.SetActive(false);

        if (l == 0)
        {
            GM.Gm.Genero = Genero.NEUTRAL;
            GM.Gm.Clima = Clima.AMBOS;
        }

        Play();
    }

    /// <summary>
    /// Comienza el juego según los parámetros establecidos.
    /// </summary>
    public void Play()
    {
        string levelPlay = (GM.Gm.Level != 0) ? "Level" : "Tutorial";
        SceneManager.LoadScene(levelPlay);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Créditos");
    }

    public void SelectWeather(int level)
    {

        if (level != 0)
        {
            levelButtons.SetActive(false);
            ClimaButtons.SetActive(true);
        }
        else
        {
            SetLevel(level);
            levelButtons.SetActive(false);
            ClimaButtons.SetActive(false);

        }
        levelSelected = level;

    }
    public void BackToWeather()
    {
        levelButtons.SetActive(false);
        ClimaButtons.SetActive(true);
    }
    #endregion

    #region Métodos privados



    #endregion

}
