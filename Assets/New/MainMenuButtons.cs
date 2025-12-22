using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    GameManager gameManager;
    TrackerManager trackerManager;

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
        trackerManager = TrackerManager.Instance;

        titleScreen.SetActive(true);
        speechScreen.SetActive(false);
        genderScreen.SetActive(false);

        foreach (GameObject speech in speechBubbles)
        {
            speech.SetActive(false);
        }

        if (!PlayerPrefs.HasKey(Defs.NEW_GAME_PREFS_KEY) || 
            (PlayerPrefs.HasKey(Defs.NEW_GAME_PREFS_KEY) && PlayerPrefs.GetInt(Defs.NEW_GAME_PREFS_KEY) == (int)(Defs.LoadGameValues.NEW_GAME))) 
        {
            PlayerPrefs.SetInt(Defs.NEW_GAME_PREFS_KEY, (int)(Defs.LoadGameValues.NEW_GAME));
        }
        else
        {
            PlayerPrefs.SetInt(Defs.NEW_GAME_PREFS_KEY, (int)(Defs.LoadGameValues.LOAD_GAME));
        }
    }


    public void Play()
    {
        if (PlayerPrefs.GetInt(Defs.NEW_GAME_PREFS_KEY) == (int)(Defs.LoadGameValues.NEW_GAME))
        {
            titleScreen.SetActive(false);
            speechScreen.SetActive(true);
            speechBubbles[0].SetActive(true);
        }
        else
        {
            StartGame(PlayerPrefs.GetInt(Defs.GENDER_PREFS_KEY));
        }
    }


    public void ChooseGender(int gender)
    {
        StartGame(gender);
    }

    private void StartGame(int gender)
    {
        PlayerPrefs.SetInt(Defs.NEW_GAME_PREFS_KEY, (int)(Defs.LoadGameValues.LOAD_GAME));
        gameManager.PlayerGender = (Defs.Gender)gender;

        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
    }
}
