using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Xasu;
using static JSONReader;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Lm;

    Dictionary<string, string> translationsDictionary;
    [SerializeField]  TranslationInfo INFO;
    private void Awake()
    {
        if (Lm == null)
        {
            Lm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Lm != this)
        {
            Destroy(gameObject);
        }
        Lm.translationsDictionary = new Dictionary<string, string>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("setLanguage")) ChangeLanguage(PlayerPrefs.GetInt("setLanguage"));
        else LoadTranslations();

    }

    //carga una tabla de traducciones del idioma seleccionado
    private void LoadTranslations()
    {
        string local = "Localization/";
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        local = string.Concat(local, '/');
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        TextAsset localizationFile = (TextAsset)Resources.Load(local, typeof(TextAsset));
        Debug.Log(localizationFile.text);
        Lm.INFO = (Lm.GetComponent<JSONReader>().LoadTranslationFile(localizationFile.text));
        LoadTranslationTable(Lm.GetComponent<JSONReader>().LoadTranslationFile(localizationFile.text));
    }
    //cambia el idioma de juego al indicado por index. 
    public void ChangeLanguage(int index)
    {
        if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
            Xasu.HighLevel.AlternativeTracker.Instance.Selected("language", LocalizationSettings.SelectedLocale.ToString());
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        Debug.Log("Language"+ index + LocalizationSettings.SelectedLocale);
        LoadTranslations();
        PlayerPrefs.SetInt("setLanguage", index);
    }
    //metodo auxiliar de LoadTranslations, se encarga de relacionar las traducciones con su correspondiente id
    private void LoadTranslationTable(TranslationInfo translations)
    {

        Lm.translationsDictionary.Clear();

            Debug.Log(translations.objects.Length);
       foreach (ObjectTranslation ti in translations.objects)
           Lm.translationsDictionary.Add(ti.id, ti.translation);
    }
    //devuelve la traduccion de la palabra con id "word"
    public string getWord(string word)
    {
        return Lm.translationsDictionary[word];
    }
}
