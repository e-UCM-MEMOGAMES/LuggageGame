using UnityEngine;

public class LevelSelectorButtons : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    GameObject climates, levels;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        climates.SetActive(true);
        levels.SetActive(false);
    }


    public void Return()
    {
        if (levels.activeSelf)
        {
            climates.SetActive(true);
            levels.SetActive(false);
        }
        else
        {
            gameManager.ChangeScene(Defs.MENU_SCENE_NAME);
        }
    }


    public void StartTutorial()
    {
        gameManager.Climate = Defs.Climate.BOTH;
        gameManager.Level = 0;
        gameManager.ChangeScene(Defs.TUTORIAL_SCENE_NAME);
    }


    public void SelectClimate(int climate)
    {
        gameManager.Climate = (Defs.Climate)climate;

        climates.SetActive(false);
        levels.SetActive(true);
    }

    public void SelectLevel()
    {
        gameManager.ChangeScene(Defs.GAME_SCENE_NAME);
    }
}
