using TMPro;
using UnityEngine;

public class LevelButtons : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;

    LevelManager levelManager;

    [Header("UI")]
    [SerializeField] GameObject initialPanel;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject notebookPanel;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject warning;

    [SerializeField] TextMeshProUGUI remainingListUsesText;

    [Header("Scenario")]
    [SerializeField] GameObject roomsView;
    [SerializeField] GameObject caseView;
    [SerializeField] GameObject bedroom;
    [SerializeField] GameObject bathroom;
    [SerializeField] GameObject bedroomCaseView;
    [SerializeField] GameObject bathroomCaseView;
    [SerializeField] GameObject cabinet;
    [SerializeField] GameObject drawer;
    [SerializeField] GameObject[] hiddenSpawnpoints;
    GameObject currentHiddenItems = null;

    [Header("Case")]
    [SerializeField] RectTransform caseOpened;
    [SerializeField] Vector3 caseScaleDrawer;
    [SerializeField] float caseYDrawer;
    Vector3 topViewCaseScale;
    float topViewCaseY;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;

        levelManager = GetComponent<LevelManager>();

        topViewCaseScale = new Vector3(caseOpened.localScale.x, caseOpened.localScale.y, caseOpened.localScale.z);
        topViewCaseY = caseOpened.localPosition.y;

        GoToBedroom();
        CloseCase();

        warning.SetActive(false);
        notebookPanel.SetActive(false);
        initialPanel.SetActive(true);
        endPanel.SetActive(false);
        itemList.SetActive(true);

        foreach (GameObject point in hiddenSpawnpoints)
        {
            point.SetActive(false);
        }

        audioManager.Play(GameSound.NoteBook);

        remainingListUsesText.text = levelManager.RemainingListUses.ToString();
    }


    public void StartGame()
    {
        initialPanel.SetActive(false);
        itemList.SetActive(false);
    }

    public void ToggleItemList()
    {
        if (!notebookPanel.activeSelf && levelManager.RemainingListUses > 0)
        {
            levelManager.RemainingListUses--;
            remainingListUsesText.text = levelManager.RemainingListUses.ToString();
            notebookPanel.SetActive(true);
            itemList.SetActive(true);

            audioManager.Play(GameSound.NoteBook);

        }
        else if (notebookPanel.activeSelf)
        {
            notebookPanel.SetActive(false);
            itemList.SetActive(false);

            audioManager.Play(GameSound.NoteBook);
        }
    }


    public void GoToBedroom()
    {
        bedroom.SetActive(true);
        bedroomCaseView.SetActive(true);

        bathroom.SetActive(false);
        bathroomCaseView.SetActive(false);

        cabinet.SetActive(false);
    }

    public void GoToBathroom()
    {
        bedroom.SetActive(false);
        bedroomCaseView.SetActive(false);

        bathroom.SetActive(true);
        bathroomCaseView.SetActive(true);

        cabinet.SetActive(false);
    }

    public void GoToCabinet(GameObject cabinetItems)
    {
        GoToBathroom();
        cabinet.SetActive(true);

        ShowHiddenElements(cabinetItems);
    }
    public void GoToDrawer(GameObject drawerItems)
    {
        OpenCase();

        drawer.SetActive(true);
        
        caseOpened.localScale = caseScaleDrawer;
        caseOpened.localPosition = new Vector3(caseOpened.localPosition.x, caseYDrawer, caseOpened.localPosition.z);

        ShowHiddenElements(drawerItems);
    }
    private void ShowHiddenElements(GameObject items)
    {
        HideHiddenElements();
        currentHiddenItems = items;
        currentHiddenItems.SetActive(true);
    }
    public void HideHiddenElements()
    {
        if (currentHiddenItems != null)
        {
            currentHiddenItems.SetActive(false);
        }
    }

    public void OpenCase()
    {
        roomsView.SetActive(false);
        caseView.SetActive(true);
    }
    public void CloseCase()
    {
        HideHiddenElements();

        roomsView.SetActive(true);
        caseView.SetActive(false);

        if (drawer.activeSelf)
        {
            audioManager.Play(GameSound.DrawerClose);
        }
        drawer.SetActive(false);

        caseOpened.localScale = topViewCaseScale;
        caseOpened.localPosition = new Vector3(caseOpened.localPosition.x, topViewCaseY, caseOpened.localPosition.z);
    }

    public void EndGame()
    {
        endPanel.SetActive(true);
        itemList.SetActive(true);
        warning.SetActive(false);

        audioManager.Play(GameSound.AirPlane);
    }

    public void Return()
    {
        gameManager.ChangeScene(Defs.LEVEL_SETTINGS_SCENE_NAME);
        audioManager.Play(GameSound.MenuBGM);
    }

}
