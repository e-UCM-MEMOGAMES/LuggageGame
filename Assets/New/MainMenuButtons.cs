using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using static GameManager;

public class MainMenuButtons : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject languageSelectorScreen, menuScreen, startScreen, speechScreen, genderScreen;

    [SerializeField]
    GameObject[] speechBubbles;

    int currSpeech;

    [SerializeField]
    bool deleteSavedGame = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (deleteSavedGame) PlayerPrefs.DeleteAll();
#endif

        gameManager = GameManager.Instance;

        menuScreen.SetActive(false);
        speechScreen.SetActive(false);
        genderScreen.SetActive(false);

        if (!PlayerPrefs.HasKey(NEW_GAME_KEY) || 
            (PlayerPrefs.HasKey(NEW_GAME_KEY) && PlayerPrefs.GetInt(NEW_GAME_KEY) == (int)LoadGameValues.NEW_GAME)) 
        {
            PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.NEW_GAME);

            languageSelectorScreen.SetActive(true);
            currSpeech = -1;
        }
        else
        {
            PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.LOAD_GAME);
            HideLanguageScreen();
        }
    }


    public void HideLanguageScreen()
    {
        languageSelectorScreen.SetActive(false);
        menuScreen.SetActive(true);

        startScreen.SetActive(true);
    }


    public void Play()
    {
        if (PlayerPrefs.GetInt(NEW_GAME_KEY) == (int)LoadGameValues.NEW_GAME)
        {
            startScreen.SetActive(false);
            speechScreen.SetActive(true);

            foreach (GameObject speech in speechBubbles)
            {
                speech.SetActive(false);
            }
            NextSpeech();
        }
        else
        {
            StartGame();
        }
    }

    public void NextSpeech() 
    {
        if (currSpeech >= 0 && currSpeech < speechBubbles.Length)
        {
            speechBubbles[currSpeech].SetActive(false);
        }
        currSpeech++;
        if (currSpeech >= 0 && currSpeech  < speechBubbles.Length)
        {
            speechBubbles[currSpeech].SetActive(true);
        }
    }

    public void ShowGenderSelector()
    {
        speechScreen.SetActive(false);
        genderScreen.SetActive(true);
    }

    public void ChooseGender(int gender)
    {
        PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.LOAD_GAME);
        PlayerPrefs.SetInt(GENDER_KEY, (int)(GameManager.Gender)gender);

        StartGame();
    }

    private void StartGame()
    {
        PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.NEW_GAME);
        gameManager.PlayerGender = (GameManager.Gender)PlayerPrefs.GetInt(GENDER_KEY);

        gameManager.ChangeScene(gameManager.LEVEL_SETTINGS_SCENE_NAME);
    }
}
