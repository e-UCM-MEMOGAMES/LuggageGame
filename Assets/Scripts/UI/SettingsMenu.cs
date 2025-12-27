using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Xasu.HighLevel;

public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;
    
    /// <summary>
    /// Instancia del AudioManager
    /// </summary>
    AudioManager audioManager;

    /// <summary>
    /// Instancia del TrackerManager
    /// </summary>
    TrackerManager trackerManager;


    /// <summary>
    /// Slider de volumen de la musica de fondo
    /// </summary>
    [SerializeField]
    Slider bgmSlider,

    /// <summary>
    /// Slider de volumen de los efectos de sonido
    /// </summary>
    sfxSlider;


    /// <summary>
    /// Dropdown con los idiomas disponibles
    /// </summary>
    [SerializeField]
    TMP_Dropdown languageDropdown,

    genderDropdown;

    /// <summary>
    /// Lista con las localizaciones disponibles
    /// </summary>
    List<Locale> lcs;


    private void Start()
    {
        gameManager = GameManager.Instance;

        audioManager = AudioManager.Instance;
        bgmSlider.value = audioManager.BGMVolume;
        sfxSlider.value = audioManager.SFXVolume;

        trackerManager = TrackerManager.Instance;
        lcs = LocalizationSettings.AvailableLocales.Locales;

        for (int i = 0; i < lcs.Count; ++i)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData() { text = lcs[i].LocaleName });
        }

        if (PlayerPrefs.HasKey(Defs.LANGUAGE_PREFS_KEY))
        {
            languageDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt(Defs.LANGUAGE_PREFS_KEY));
        }
        else
        {
            int lid = lcs.IndexOf(LocalizationSettings.SelectedLocale);
            languageDropdown.SetValueWithoutNotify(lid);
            PlayerPrefs.SetInt(Defs.LANGUAGE_PREFS_KEY, lid);
        }
        languageDropdown.RefreshShownValue();
    }


    public void ChangeLanguage()
    {
        LocalizationSettings.SelectedLocale = lcs[languageDropdown.value];
        PlayerPrefs.SetInt(Defs.LANGUAGE_PREFS_KEY, languageDropdown.value);

        trackerManager.TrySendStatement(AlternativeTracker.Instance.Selected("Language", lcs[languageDropdown.value].LocaleName));
    }
    public void ChangeGender()
    {
        gameManager.PlayerGender = (Defs.Gender)PlayerPrefs.GetInt(Defs.GENDER_PREFS_KEY);
    }
    public void SetBGMVolume()
    {
        audioManager.BGMVolume = bgmSlider.value;
    }

    public void setSFXVolume()
    {
        audioManager.SFXVolume = sfxSlider.value;
    }
}
