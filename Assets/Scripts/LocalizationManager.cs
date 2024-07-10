using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static JSONReader;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Lm;
    
    public static Dictionary<string, string> translationsDictionary { get; set; }

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
    }

    private void Start()
    {
        translationsDictionary = new Dictionary<string, string>();
        LoadTranslations();
    }

    //carga una tabla de traducciones del idioma seleccionado
    public void LoadTranslations()
    {
        string local = "Localization/";
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        local = string.Concat(local, '/');
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        TextAsset localizationFile = (TextAsset)Resources.Load(local, typeof(TextAsset));
        LoadTranslationTable(GetComponent<JSONReader>().LoadTranslationFile(localizationFile.text));
    }
    //cambia el idioma de juego al indicado por index. 
    public void ChangeLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LoadTranslations();
    }
    //metodo auxiliar de LoadTranslations, se encarga de relacionar las traducciones con su correspondiente id
    private void LoadTranslationTable(TranslationInfo translations)
    {
        translationsDictionary.Clear();

        foreach (ObjectTranslation ti in translations.objects)
            translationsDictionary.Add(ti.id, ti.translation);
    }
    //devuelve la traduccion de la palabra con id "word"
    public string getWord(string word)
    {
        return translationsDictionary[word];
    }
}
