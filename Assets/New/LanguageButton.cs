using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
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
        trackerManager.TrySendStatement(AlternativeTracker.Instance.Selected(locale.LocaleName, "Language"));
        gameManager.ChangeScene(gameManager.MENU_SCENE_NAME);
    }

}
