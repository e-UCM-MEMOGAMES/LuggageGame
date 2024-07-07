using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static JSONReader;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Lm;
    JSONReader jsonReader;
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
        jsonReader = GetComponent<JSONReader>();
        translationsDictionary = new Dictionary<string, string>();
        LoadTranslations();
    }

    void LoadTranslations()
    {
        string local = "Localization/";
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);
        local = string.Concat(local, '/');
        local = string.Concat(local, LocalizationSettings.SelectedLocale.Identifier.Code);

        Debug.Log(local);
        TextAsset localizationFile = (TextAsset)Resources.Load(local, typeof(TextAsset));
        LoadTranslationTable(jsonReader.LoadTranslationFile(localizationFile.text));
    }

    private void LoadTranslationTable(TranslationInfo translations)
    {
        foreach (ObjectTranslation ti in translations.objects)
        {
            translationsDictionary.Add(ti.id, ti.translation);
        }
    }

    public string getWord(string word)
    {
        return translationsDictionary[word];
    }
}
