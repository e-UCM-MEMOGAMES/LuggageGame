using System.Diagnostics;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using Xasu.HighLevel;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>
    /// Nombre de la escena del de idiomas
    /// </summary>
    public string LANGUAGE_SCENE_NAME = "",
    /// <summary>
    /// Nombre de la escena del menu principal
    /// </summary>
    MENU_SCENE_NAME = "",
    /// <summary>
    /// Nombre de la escena de configuracion
    /// </summary>
    SETTINGS_SCENE_NAME = "",
    /// <summary>
    /// Nombre de la escena de creditos
    /// </summary>
    CREDITS_SCENE_NAME = "",
    /// <summary>
    /// Nombre de la escena de opciones del nivel
    /// </summary>
    LEVEL_SETTINGS_SCENE_NAME = "",
    /// <summary>
    /// Nombre de la escena de juego
    /// </summary>
    GAME_SCENE_NAME = "";

    public const string NEW_GAME_KEY = "newGame",
                        GENDER_KEY = "gender";

    public enum LoadGameValues { NEW_GAME = 0, LOAD_GAME = 1 };

    public enum Gender { MALE = 0, FEMALE = 1 };


    /// <summary>
    /// Temporizador para medir el tiempo que se tarda en completar el nivel
    /// </summary>
    Stopwatch watch = Stopwatch.StartNew();

    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;

    string COMPLETABLE_ID = "game";
    //CompletableTracker.CompletableType COMPLETABLE_TYPE = CompletableTracker.CompletableType.Game;


    Gender playerGender;
    public Gender PlayerGender
    {
        get { return playerGender; }
        set { playerGender = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        trackerManager = TrackerManager.Instance;
        //trackerManager.TrySendStatement(CompletableTracker.Instance.Initialized(COMPLETABLE_ID, COMPLETABLE_TYPE));
        //trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed(MENU_SCENE_NAME));

        watch.Start();

        if (PlayerPrefs.HasKey("language"))
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("language")];
        }

        //Application.wantsToQuit += WantsToQuit;
    }

    /// <summary>
    /// Llamado al generar el evento de que se quiere salir de la aplicacion
    /// (tanto al pulsar el boton de salir como al salir pulsando la X)
    /// </summary>
    async void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Quitting GameManager");
        watch.Stop();

        //trackerManager.TrySendStatement(CompletableTracker.Instance.Completed(COMPLETABLE_ID, COMPLETABLE_TYPE, watch.ElapsedMilliseconds));
        await trackerManager.Quit();
    }

    /// <summary>
    /// Llamado al pulsar el boton de salir. Se encarga de cerrar el juego
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    /// <summary>
    /// Se cambia a la escena indicada por el parametro que se pasa
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        //trackerManager.TrySendStatement(AccessibleTracker.Instance.Accessed(sceneName));
        SceneManager.LoadScene(sceneName);
    }
}
