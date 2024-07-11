using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xasu;

public class SliderTracker : MonoBehaviour
{
    Slider slider;
    [SerializeField]
    string sliderName;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(Interacted);

        slider.interactable = true;
    }

    public async void Interacted(float value)
    {
        if (XasuTracker.Instance.Status.State == TrackerState.Uninitialized) return;
            slider.interactable = false;
        await Xasu.HighLevel.GameObjectTracker.Instance.Interacted(sliderName);
        slider.interactable = true;
    }

}
