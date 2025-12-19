using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ListItem : MonoBehaviour
{
    [SerializeField]
    GameObject check, cross;

    [SerializeField]
    LocalizeStringEvent localization;

    // Start is called before the first frame update
    void Start()
    {
        check.SetActive(false);
        cross.SetActive(false);
    }

    public void SetName(LocalizedString localized)
    {
        localization.StringReference = localized;
    }

    public void SetObtained(bool obtained)
    {
        cross.SetActive(false);
        check.SetActive(obtained);
    }
    public void SetMissed(bool missed)
    {
        cross.SetActive(missed);
        check.SetActive(false);
    }
}
