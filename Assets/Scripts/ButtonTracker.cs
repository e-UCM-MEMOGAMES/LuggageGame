using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xasu;

//mandar trazas con XasuTracker cuando interactua con el boton
public class ButtonTracker : MonoBehaviour
{
    Button button;
    [SerializeField]
    string buttonName;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Interacted);

        button.interactable = true;
    }

    public async void Interacted()
    {
        if (XasuTracker.Instance.Status.State == TrackerState.Uninitialized) return;

        //si no es null manda la traza
        if (button != null)
        {
            button.interactable = false;
            await Xasu.HighLevel.GameObjectTracker.Instance.Interacted(buttonName);
        }
        if (button != null)

            button.interactable = true;

    }

}
