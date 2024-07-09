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
    
    public Dictionary<string, string> translationsDictionary { get; set; }

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

        LoadTranslations();
    }

    public void LoadTranslations()
    {
        string local = "Localization/";
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        local = string.Concat(local, '/');
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        TextAsset localizationFile = (TextAsset)Resources.Load(local, typeof(TextAsset));
        LoadTranslationTable(GetComponent<JSONReader>().LoadTranslationFile(localizationFile.text));
        Debug.Log(LocalizationSettings.SelectedLocale); 
        Debug.Log(localizationFile.text);
    }
    public void ChangeLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LoadTranslations();
    }

    private void LoadTranslationTable(TranslationInfo translations)
    {
        translationsDictionary = new Dictionary<string, string>();

        foreach (ObjectTranslation ti in translations.objects)
        {
            Debug.Log(ti.translation);
            translationsDictionary.Add(ti.id, ti.translation);
        }
    }

    public string getWord(string word)
    {
        Debug.Log(translationsDictionary.Count+" "+ translationsDictionary[word]);

        return translationsDictionary[word];
    }
}
