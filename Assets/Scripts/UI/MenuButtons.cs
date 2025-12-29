using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;


    //Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void ChangeScene(string sceneName)
    {
        gameManager.ChangeScene(sceneName);
    }

    public void ExitGame()
    {
        gameManager.ExitGame();
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        gameManager.ChangeScene(Defs.LANGUAGE_SCENE_NAME);
    }
}
