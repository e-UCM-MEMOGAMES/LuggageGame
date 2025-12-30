using UnityEngine;

public class LevelSelectorButtons : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;

    [SerializeField]
    /// <summary>
    /// Elementos de la seleccion de clima
    /// </summary>
    GameObject climates,
    /// <summary>
    /// Elementos de la seleccion de nivel
    /// </summary>
    levels;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        climates.SetActive(true);
        levels.SetActive(false);
    }


    /// <summary>
    /// Llamado por el boton de volver
    /// </summary>
    public void Return()
    {
        // Si se esta eligiendo nivel, se ocultan los elementos de la
        // seleccion de nivel y se muestran los de la seleccion de clima
        if (levels.activeSelf)
        {
            climates.SetActive(true);
            levels.SetActive(false);
        }
        // Si no, se vuelve al menu principal
        else
        {
            gameManager.ChangeScene(Defs.MENU_SCENE_NAME);
        }
    }

    /// <summary>
    /// Llamado por el boton de tutorial
    /// </summary>
    public void StartTutorial()
    {
        // Se pone el clima a ambos y el numero del nivel a 0
        gameManager.Climate = Defs.Climate.BOTH;
        gameManager.Level = 0;

        SelectLevel();
    }

    /// <summary>
    /// Llamado por los botones de clima
    /// </summary>
    public void SelectClimate(int climate)
    {
        gameManager.Climate = (Defs.Climate)climate;

        // Se ocultan los elementos de seleccion de clima y se muestran los de seleccion de nivel
        climates.SetActive(false);
        levels.SetActive(true);
    }

    /// <summary>
    /// Llamado por los botones de los niveles
    /// </summary>
    public void SelectLevel()
    {
        // Cambia a la escena de juego (el numero del nivel se configura en DifficultyButton)
        gameManager.ChangeScene(Defs.GAME_SCENE_NAME);
    }
}
