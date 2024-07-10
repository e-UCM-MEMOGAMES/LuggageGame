using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (dropdown.value==0)
        {
            GM.Gm.Genero=Assets.Scripts.Constantes.Genero.HOMBRE;
            Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "man");

        }
        else
        {
            GM.Gm.Genero=Assets.Scripts.Constantes.Genero.MUJER;
            Xasu.HighLevel.AlternativeTracker.Instance.Selected("gender", "woman");


        }

    }

}
