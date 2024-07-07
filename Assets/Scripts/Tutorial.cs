using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{

    public GameObject mano;
    public GameObject panel;
    public GameObject camisetaAmarilla;
    public GameObject panelInfoObject;
    public Animator manoAnimator;
    public GameObject camara;
    public string[] textoTutorial;
    TMP_Text texto;

    enum State { NONE, CLICK, DRAGNDROP, LUGGAGE, OVERINFO, PULLOVER, BACKTOROOM, DRAWER, BATHROOM, BACKTHROOM, END, NULL, CAMISETAAMARILLA,CLICKLIST, CLICKEDLIST,BACKFROMLIST }
    State state;
    // Use this for initialization
    void Start()
    {
        state = State.NONE;
        panel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state == State.CLICK)
            {
                state = State.DRAGNDROP;
                texto.text = textoTutorial[0];
                manoAnimator.SetInteger("step", 1);
            }
            else if (state == State.LUGGAGE)
            {
                state = State.OVERINFO;
                texto.text = textoTutorial[1];
            }
            else if (state == State.DRAWER)
            {
                texto.text = textoTutorial[2];
                
            }
            else if (state == State.BACKTHROOM)
            {
                state = State.CLICKLIST;
                panel.SetActive(true);
                texto.text = textoTutorial[3];
                manoAnimator.SetInteger("step", 8);
            }
            else if (state == State.NULL)
            {
                panel.SetActive(false);
                mano.SetActive(false);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (state == State.DRAGNDROP)
            {
                if (camisetaAmarilla.activeSelf)
                {
                    state = State.CLICK;
                    texto.text = textoTutorial[4];
                    manoAnimator.SetInteger("step", 0);
                }
                else
                {
                    state = State.LUGGAGE;
                    texto.text = textoTutorial[5];
                    manoAnimator.SetInteger("step", 2);
                }
            }
            if (state == State.PULLOVER)
            {
                if (camisetaAmarilla.activeSelf)
                {
                    state = State.BACKTOROOM;
                    texto.text = textoTutorial[6];
                    manoAnimator.SetInteger("step", 4);
                }
            }
            if(state == State.CAMISETAAMARILLA)
            {
                if (!camisetaAmarilla.activeSelf)
                {
                    state = State.DRAWER;
                    texto.text = textoTutorial[7];
                    manoAnimator.SetInteger("step", 5);
                }
            }
        }

        if (state == State.OVERINFO)
        {
            if (panelInfoObject.activeSelf)
            {
                state = State.PULLOVER;
                texto.text = textoTutorial[8];
                manoAnimator.SetInteger("step", 3);
            }
        }
        else if (state == State.CLICKLIST )
        {
            state = State.CLICKEDLIST;
            texto.text = textoTutorial[9];
            panel.SetActive(true);

        }
        else if (state == State.END && camara.gameObject.activeSelf)
        {
            state = State.NULL;
            texto.text = textoTutorial[10];
            panel.SetActive(true);
        }
    }
    public void ButtonList()
    {
        if (state == State.CLICKEDLIST)
        {
            state = State.BACKFROMLIST;
            texto.text = textoTutorial[11];
            mano.SetActive(false);
        }
        
    }
    public void ButtonBack()
    {
        if (state == State.BACKFROMLIST)
        {
            state = State.END;
            mano.SetActive(true);
            manoAnimator.SetInteger("step", 9);

        }

    }
    public void ButtonBackToRoom()
    {
        if (state == State.BACKTOROOM)
        {
            state = State.CAMISETAAMARILLA;
            texto.text = textoTutorial[12];
            manoAnimator.SetInteger("step", 2);
           

        }
        else if (state == State.DRAWER)
        {
            state = State.BATHROOM;
            texto.text = textoTutorial[13];
            manoAnimator.SetInteger("step", 6);
        }
    }
    public void ButtonBathRoom()
    {
        if (state == State.BATHROOM)
        {
            state = State.BACKTHROOM;
            texto.text = textoTutorial[14];
            manoAnimator.SetInteger("step", 7);

        }

    }
    public void ButtonBegin()
    {
        state = State.CLICK;
        texto = panel.GetComponentInChildren<TMP_Text>();
        texto.text = textoTutorial[15];
        manoAnimator.SetInteger("step", 0);
        panel.SetActive(true);
    }
   
    public void End()
    {
        panel.SetActive(false);
        mano.SetActive(false);
    }
}
