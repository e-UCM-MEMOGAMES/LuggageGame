using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Constantes;

public class WeatherButton : MonoBehaviour
{
   // public int _level;
  //  public GameObject[] stars;
    public LevelSelector lvlSelector;
    public Clima clima;
   

    public void SetWeather()
    {
       
        lvlSelector.SetWeather((int)clima);

    }
}
