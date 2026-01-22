using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyButton : MonoBehaviour
{
    /// <summary>
    /// Instancia del GameManager
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// Numero del nivel
    /// </summary>
    [SerializeField]
    int levelNumber;

    /// <summary>
    /// Texto con el numero del nivel
    /// </summary>
    [SerializeField]
    TextMeshProUGUI levelText;

    /// <summary>
    /// Animacion que reproducen las estrellas desbloqueadas
    /// </summary>
    [SerializeField]
    Animation[] unlockedStarsAnimation;

    /// <summary>
    /// Icono de nivel bloqueado
    /// </summary>
    [SerializeField]
    GameObject lockedImg;

    /// <summary>
    /// Componente Button del objeto para bloquear la pulsacion si el nivel esta bloqueado
    /// </summary>
    [SerializeField]
    Button button;
    
    /// <summary>
    /// Si el nivel viene desbloqueado por defecto
    /// </summary>
    [SerializeField]
    bool preUnlocked = false;


    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        levelText.text = levelNumber.ToString();
    }

    private void OnEnable()
    {
        // Determina la id en las configuraciones del nivel actual y del anterior
        string prevLevelName = Defs.GetLevelSaveKey(levelNumber - 1, gameManager.Climate);
        string levelName = Defs.GetLevelSaveKey(levelNumber, gameManager.Climate);

        // Si es el primer nivel, el nivel anterior ha sido jugado, o esta desbloqueado por defecto, se desbloquea
        if (levelNumber == 1 || PlayerPrefs.HasKey(prevLevelName) || preUnlocked)
        {
            Unlock(true);
        }
        // Si no, se bloquea
        else
        {
            Unlock(false);
        }

        // Si hay informacion del nivel guardada en las configuraciones
        if (PlayerPrefs.HasKey(levelName))
        {
            // Se comprueba el numero de estrellas conseguidas
            int unlockedStars = PlayerPrefs.GetInt(levelName);

            // Se activan tantas estrellas como hay desbloqueadas
            for (int i = 0; i < unlockedStars; i++)
            {
                unlockedStarsAnimation[i].gameObject.SetActive(true);
                unlockedStarsAnimation[i].Play();
            }
        }
    }

    /// <summary>
    /// Bloquea/desbloquea el nivel
    /// </summary>
    private void Unlock(bool unlocked)
    {
        // Hace el boton interactuable y oculta el icono de bloqueado (o viceversa)
        button.interactable = unlocked;
        lockedImg.SetActive(!unlocked);

        // Activa las estrellas bloqueadas (las oculta si el nivel esta bloqueado) y oculta todas las estrellas obtenidas
        foreach (Animation star in unlockedStarsAnimation)
        {
            star.gameObject.transform.parent.gameObject.SetActive(unlocked);
            star.gameObject.SetActive(false);
        }
    }

    // Llamado al pulsar el boton
    public void Select()
    {
        gameManager.Level = levelNumber;
    }
}
