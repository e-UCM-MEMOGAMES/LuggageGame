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

    public void SelectLanguage()
    {
        LocalizationSettings.SelectedLocale = locale;
        int lid = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
        PlayerPrefs.SetInt(Defs.LANGUAGE_KEY, lid);

        trackerManager.TrySendStatement(AlternativeTracker.Instance.Selected(locale.LocaleName, "Language"));
        gameManager.ChangeScene(Defs.MENU_SCENE_NAME);
    }
}
