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
    GameObject titleScreen, speechScreen, genderScreen;

    [SerializeField]
    GameObject[] speechBubbles;

    [SerializeField]
    bool deleteSavedGame = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (deleteSavedGame) PlayerPrefs.DeleteAll();
#endif

        gameManager = GameManager.Instance;

        titleScreen.SetActive(true);
        speechScreen.SetActive(false);
        genderScreen.SetActive(false);

        foreach (GameObject speech in speechBubbles)
        {
            speech.SetActive(false);
        }

        if (!PlayerPrefs.HasKey(NEW_GAME_KEY) || 
            (PlayerPrefs.HasKey(NEW_GAME_KEY) && PlayerPrefs.GetInt(NEW_GAME_KEY) == (int)LoadGameValues.NEW_GAME)) 
        {
            PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.NEW_GAME);
        }
        else
        {
            PlayerPrefs.SetInt(NEW_GAME_KEY, (int)LoadGameValues.LOAD_GAME);
        }
    }


    public void Play()
    {
        if (PlayerPrefs.GetInt(NEW_GAME_KEY) == (int)LoadGameValues.NEW_GAME)
        {
            titleScreen.SetActive(false);
            speechScreen.SetActive(true);
            speechBubbles[0].SetActive(true);
        }
        else
        {
            StartGame();
        }
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
