using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xasu;

//Dropdown para cambiar el genero
public class GenderDropDown : MonoBehaviour
{
    TMPro.TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
        if (GM.Gm.Genero == Assets.Scripts.Constantes.Genero.HOMBRE)
        {
            dropdown.value = 0;
        }
        else
        {
            dropdown.value = 1;
        }
    }
    public void OnValueChanged()
    {

        if (dropdown.value == 0)
        {
            GM.Gm.Genero = Assets.Scripts.Constantes.Genero.HOMBRE;
            if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
                Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "man");

        }
        else
        {
            GM.Gm.Genero = Assets.Scripts.Constantes.Genero.MUJER;
            if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
                Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "woman");


        }

    }

}
