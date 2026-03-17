using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Xasu.HighLevel;

public class LanguageButton : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;
    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;

    /// <summary>
    /// Asset con el locale que activara el boton
    /// </summary>
    [SerializeField]
    Locale locale;


    void Start()
    {
        gameManager = GameManager.Instance;
        trackerManager = TrackerManager.Instance;
    }

    /// <summary>
    /// Llamado al pulsar el boton
    /// </summary>
    public void SelectLanguage()
    {
        // Cambia el idioma seleccionado por el asignado en el boton
        LocalizationSettings.SelectedLocale = locale;
        int lid = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);

        // Se guarda el idioma elegido en las configuraciones
        PlayerPrefs.SetInt(Defs.LANGUAGE_PREFS_KEY, lid);

        try
        {
            trackerManager.TrySendStatement(AlternativeTracker.Instance.Selected(locale.LocaleName, "Language"));
        }
        catch { }
        // Se pasa al menu principal
        gameManager.ChangeScene(Defs.MENU_SCENE_NAME);
    }
}
