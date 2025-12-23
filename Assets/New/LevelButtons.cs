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


    [Header("Case")]
    [SerializeField] RectTransform topViewCase;
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

        topViewCaseScale = new Vector3(topViewCase.localScale.x, topViewCase.localScale.y, topViewCase.localScale.z);
        topViewCaseY = topViewCase.position.y;

        GoToBedroom();
        CloseCase();

        warning.SetActive(false);
        notebookPanel.SetActive(false);
        initialPanel.SetActive(true);
        endPanel.SetActive(false);
        itemList.SetActive(true);

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

    public void GoToCabinet()
    {
        bedroom.SetActive(false);
        bedroomCaseView.SetActive(false);

        bedroom.SetActive(false);
        bathroomCaseView.SetActive(false);

        cabinet.SetActive(true);
    }

    public void GoToDrawer(GameObject drawerItems)
    {
        OpenCase();

        drawer.SetActive(true);
        
        topViewCase.localScale = caseScaleDrawer;
        topViewCase.position = new Vector3(topViewCase.position.x, caseYDrawer, topViewCase.position.z);
    }


    public void OpenCase()
    {
        roomsView.SetActive(false);
        caseView.SetActive(true);
    }
    public void CloseCase()
    {
        roomsView.SetActive(true);
        caseView.SetActive(false);

        drawer.SetActive(false);

        topViewCase.localScale = topViewCaseScale;
        topViewCase.position = new Vector3(topViewCase.position.x, topViewCaseY, topViewCase.position.z);
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
