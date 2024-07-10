﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xasu;
using Xasu.HighLevel;
using static Assets.Scripts.Constantes;
using System.Threading.Tasks;
using UnityEngine.UI;
public class InitialScene : MonoBehaviour
{
    public GameObject loginScene;
    public GameObject playScene;
    public GameObject playMini;
    public GameObject genre;
    public GameObject languageSelector;
    public GameObject[] speechBubbles;
    public bool saveGame;
    private AudioManager audioManager;
    int speech;
    void Start()
    {
#if UNITY_EDITOR
        if (!saveGame) PlayerPrefs.DeleteAll();
#endif
        speech = 0;
        playMini.SetActive(false);
        genre.SetActive(false);
        foreach (GameObject go in speechBubbles) go.SetActive(false);
        audioManager = AudioManager.Instance;
        audioManager.Play((int)GameSound.MenuBGM);

        if (!PlayerPrefs.HasKey("newGame")) PlayerPrefs.SetInt("newGame", 1);
        else PlayerPrefs.SetInt("newGame", 0);
        languageSelector.SetActive((PlayerPrefs.GetInt("newGame") == 1));
    }
    public async void InitTracker()
    {
        playScene.SetActive(false);
        loginScene.SetActive(true);
        await XasuTracker.Instance.Init();
        await Task.Yield();
        while (XasuTracker.Instance.Status.State == TrackerState.Uninitialized)
        {
            await Task.Yield();
        }
        loginScene.SetActive(false);
        SetSpeechBubble();
        //await Xasu.HighLevel.CompletableTracker.Instance.Initialized("MyGame", Xasu.HighLevel.CompletableTracker.CompletableType.Game);

    }

    public void SetSpeechBubble()
    {
        if (speech == 0)
        {
            if ( PlayerPrefs.GetInt("newGame")==1)
            {

                speechBubbles[speech].SetActive(true);
                speech++;
            }
            else
                GM.Gm.LoadScene("LevelSelector");


        }
        else
        {
            speechBubbles[speech - 1].SetActive(false);
            speechBubbles[speech].SetActive(true);
            speech++;
        }

        if (speech == speechBubbles.Length) playMini.SetActive(true);
    }

    public void ConfigGenre()
    {
        Debug.Log((Genero)PlayerPrefs.GetInt("genre"));

        playMini.SetActive(false);
        speechBubbles[speech - 1].SetActive(false);
        genre.SetActive(true);


    }
}
