using TMPro;
using UnityEngine;
using UnityEngine.UI;

//boton para seleccionar el nivel y mostrar las estrellas conseguidas
public class LevelButton : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    int levelNumber;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    GameObject[] obtainedStars;

    [SerializeField]
    GameObject lockedImg;

    [SerializeField]
    bool preUnlocked = false;

    Button button;


    void Start()
    {
        gameManager = GameManager.Instance;
        button = GetComponent<Button>();

        levelText.text = levelNumber.ToString();

        Activate(false);

        string levelName = $"{Defs.LEVEL_NAME_PREFS_KEY}_{levelNumber.ToString()}_{gameManager.Climate.ToString()}";
        if (preUnlocked || PlayerPrefs.HasKey(levelName)) {
            Activate(true);

            int unlockedStars = PlayerPrefs.GetInt(levelName);
            for (int i = 0; i < unlockedStars; i++)
            {
                obtainedStars[i].SetActive(true);
            }
        }
    }

    private void Activate(bool active)
    {
        button.interactable = active;
        lockedImg.SetActive(!active);

        foreach (GameObject star in obtainedStars)
        {
            star.transform.parent.gameObject.SetActive(active);
        }
    }

    public void Select()
    {
        gameManager.Level = levelNumber;
    }
}
