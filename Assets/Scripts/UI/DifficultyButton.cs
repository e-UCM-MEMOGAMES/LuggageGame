using TMPro;
using UnityEngine;
using UnityEngine.UI;

//boton para seleccionar el nivel y mostrar las estrellas conseguidas
public class DifficultyButton : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    int levelNumber;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    Animation[] unlockedStarsAnimation;


    [SerializeField]
    GameObject lockedImg;

    [SerializeField]
    bool preUnlocked = false;

    Button button;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        button = GetComponent<Button>();
    }

    void Start()
    {
        levelText.text = levelNumber.ToString();
    }


    private void OnEnable()
    {
        string prevLevelName = Defs.GetLevelSaveKey(levelNumber - 1, gameManager.Climate);
        string levelName = Defs.GetLevelSaveKey(levelNumber, gameManager.Climate);
        if (levelNumber == 1 || PlayerPrefs.HasKey(prevLevelName) || preUnlocked)
        {
            Unlock(true);
        }
        else
        {
            Unlock(false);
        }

        if (PlayerPrefs.HasKey(levelName))
        {
            int unlockedStars = PlayerPrefs.GetInt(levelName);
            for (int i = 0; i < unlockedStars; i++)
            {
                unlockedStarsAnimation[i].gameObject.SetActive(true);
                unlockedStarsAnimation[i].Play();
            }
        }
    }

    private void Unlock(bool unlocked)
    {
        button.interactable = unlocked;
        lockedImg.SetActive(!unlocked);

        foreach (Animation star in unlockedStarsAnimation)
        {
            star.gameObject.SetActive(false);
            star.gameObject.transform.parent.gameObject.SetActive(unlocked);
        }
    }

    public void Select()
    {
        gameManager.Level = levelNumber;
    }
}
