using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageButton : MonoBehaviour
{
    [SerializeField]
    Locale locale;

    public void SelectLanguage()
    {
        LocalizationSettings.SelectedLocale = locale;
    }

}
