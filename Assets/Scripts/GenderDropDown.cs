using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;
using Xasu;
using static Assets.Scripts.Constantes;

//Dropdown para cambiar el genero
public class GenderDropDown : MonoBehaviour
{
    TMPro.TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
        //Debug.Log("GM: "+GM.Gm.Genero);
       // Debug.Log(PlayerPrefs.GetInt("genre"));
        if (PlayerPrefs.GetInt("genre")== (int)Assets.Scripts.Constantes.Genero.HOMBRE)
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
            PlayerPrefs.SetInt("genre",(int) GM.Gm.Genero);
            if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
                Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "man");

        }
        else
        {

            GM.Gm.Genero = Assets.Scripts.Constantes.Genero.MUJER;
            PlayerPrefs.SetInt("genre",(int) GM.Gm.Genero);
            if (XasuTracker.Instance.Status.State != TrackerState.Uninitialized)
                Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "woman");


        }

    }

}
