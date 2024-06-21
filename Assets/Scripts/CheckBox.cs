using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [SerializeField]
    GameObject correctGO, wrongGO;
    public enum CheckBoxState { Correct,Wrong,None}
    CheckBoxState state= CheckBoxState.None;
    public void SetCheckBoxState(CheckBoxState s)
    {
        state = s;
        if (s == CheckBoxState.Correct) {
            correctGO.SetActive(true);
            wrongGO.SetActive(false);
        }
        else if( s == CheckBoxState.Wrong)
        {
            correctGO.SetActive(false);
            wrongGO.SetActive(true);
        }
        else {
            correctGO.SetActive(false);
            wrongGO.SetActive(false);
        }

    }
    public CheckBoxState GetCheckBoxState()
    {
        return state;
    }
}
