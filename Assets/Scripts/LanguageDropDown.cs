using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.UI;

//Drop down para cambiar de idioma
public class LanguegeDropDown : MonoBehaviour
{
    TMPro.TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown=GetComponent<TMP_Dropdown>();
        dropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
    }

    public void OnDropDownChanged()
    {
        LocalizationManager.Lm.ChangeLanguage(dropdown.value);
    }
}