using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int level;
    public GameObject[] stars;
    public GameObject blocked;
    public LevelSelector lvlSelector;

    bool isBlocked;
    void Start()
    {
        isBlocked = false;   
    }

    public void CalcStars()
    {
        int numStars = 0;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (GM.Gm.Clima == Assets.Scripts.Constantes.Clima.CALIDO)
        {
            Debug.Log(PlayerPrefs.GetInt("Level" + level.ToString() + "Warm", 0));
            numStars = PlayerPrefs.GetInt("Level" + level.ToString() + "Warm", 0);
        }
        else { 
            PlayerPrefs.GetInt("Level" + level.ToString() + "Cold", 0);
            Debug.Log(PlayerPrefs.GetInt("Level" + level.ToString() + "Cold", 0));
        }

        for (int i = 0; i < numStars; i++)
        {
            stars[i].transform.GetChild(0).gameObject.SetActive(true);
        }
       
       
    }
    public void OnSelected()
    {
        if (!isBlocked)
        {
            lvlSelector.SetLevel(level);
            
        }
    }

}
