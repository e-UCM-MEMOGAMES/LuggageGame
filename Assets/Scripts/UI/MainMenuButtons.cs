using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// Elementos de la pantalla del titulo
    /// </summary>
    [SerializeField]
    GameObject titleScreen,
    /// <summary>
    /// Elementos de la pantalla con el texto introductorio
    /// </summary>
    speechScreen,
    /// <summary>
    /// Elementos del panel de seleccion de genro
    /// </summary>
    genderScreen;

    /// <summary>
    /// Burbujas de con el texto introductorio
    /// </summary>
    [SerializeField]
    GameObject[] speechBubbles;

    /// <summary>
    /// Opcion para borrar todos los datos guardados al iniciar la escena (solo en el editor)
    /// </summary>
    [SerializeField]
    bool deleteSavedGame = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (deleteSavedGame) PlayerPrefs.DeleteAll();
#endif

        gameManager = GameManager.Instance;

        // Se ocultan todos los elementos excepto los de la pantalla de titulo
        titleScreen.SetActive(true);
        speechScreen.SetActive(false);
        genderScreen.SetActive(false);
        foreach (GameObject speech in speechBubbles)
        {
            speech.SetActive(false);
        }

        // Si no hay informacion en la configuracion sobre si crear una partida
        // o cargarla, se guarda con la opcion de crear una nueva partida
        if (!PlayerPrefs.HasKey(Defs.NEW_GAME_PREFS_KEY))
        {
            PlayerPrefs.SetInt(Defs.NEW_GAME_PREFS_KEY, (int)(Defs.LoadGameValues.NEW_GAME));
        }
    }

    /// <summary>
    /// Llamado por el boton de jugar de la pantalla de titulo
    /// </summary>
    public void Play()
    {
        // Si la configuracion guardada indica que hay que crear una nueva partida, se oculta
        // la pantalla de titulo y se muestra la pantalla con el texto introductorio
        if (PlayerPrefs.GetInt(Defs.NEW_GAME_PREFS_KEY) == (int)(Defs.LoadGameValues.NEW_GAME))
        {
            titleScreen.SetActive(false);
            speechScreen.SetActive(true);
            speechBubbles[0].SetActive(true);
        }
        // Si no, se inicia el juego directamente
        else
        {
            // Se intenta cargar el genero de la configuracion guardada
            int gender = (int)Defs.Gender.NEUTRAL;
            if (PlayerPrefs.HasKey(Defs.GENDER_PREFS_KEY))
            {
                gender = PlayerPrefs.GetInt(Defs.GENDER_PREFS_KEY);
            }
            StartGame(gender);
        }
    }

    /// <summary>
    /// Llamado por los botones de cada genero
    /// </summary>
    public void ChooseGender(int gender)
    {
        StartGame(gender);
    }

    /// <summary>
    /// Inicia el juego con la configuracion elegida
    /// </summary>
    private void StartGame(int gender)
    {
        PlayerPrefs.SetInt(Defs.NEW_GAME_PREFS_KEY, (int)(Defs.LoadGameValues.LOAD_GAME));
        gameManager.PlayerGender = (Defs.Gender)gender;

        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
    }
}
