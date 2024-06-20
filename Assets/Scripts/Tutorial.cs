using System.Collections.Generic;
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

    Text texto;

    enum State { NONE,CLICK, DRAGNDROP, LUGGAGE, OVERINFO, PULLOVER, BACKTOROOM, DRAWER, BATHROOM, BACKTHROOM, END, NULL, CAMISETAAMARILLA,CLICKLIST, CLICKEDLIST,BACKFROMLIST }
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
                texto.text = "Sin soltar, arrastre la camiseta hasta la maleta y luego suelte.";
                manoAnimator.SetInteger("step", 1);
            }
            else if (state == State.LUGGAGE)
            {
                state = State.OVERINFO;
                texto.text = "Si pone el cursor sobre el objeto, aparecerá su nombre.";
            }
            else if (state == State.DRAWER)
            {
                texto.text = "No todos los cajones tendrán algo dentro. Vuelva a la habitación y revise el resto.";
                
            }
            else if (state == State.BACKTHROOM)
            {
                state = State.CLICKLIST;
                panel.SetActive(true);
                texto.text = "Aqui puedes echar un ojo a la lista de objetos en caso de que se te ha olvidado.";
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
                    texto.text = "Pulse sobre la camiseta amarilla y mantenga pulsado.";
                    manoAnimator.SetInteger("step", 0);
                }
                else
                {
                    state = State.LUGGAGE;
                    texto.text = "Haga click en la maleta para ver lo que ha metido.";
                    manoAnimator.SetInteger("step", 2);
                }
            }
            if (state == State.PULLOVER)
            {
                if (camisetaAmarilla.activeSelf)
                {
                    state = State.BACKTOROOM;
                    texto.text = "Haga click sobre el icono de abajo para cerrar la maleta y volver atrás.";
                    manoAnimator.SetInteger("step", 4);
                }
            }
            if(state == State.CAMISETAAMARILLA)
            {
                if (!camisetaAmarilla.activeSelf)
                {
                    state = State.DRAWER;
                    texto.text = "Puede ver el contenido de los cajones haciendo click en ellos.";
                    manoAnimator.SetInteger("step", 5);
                }
            }
        }

        if (state == State.OVERINFO)
        {
            if (panelInfoObject.activeSelf)
            {
                state = State.PULLOVER;
                texto.text = "Puede arrastrar el objeto fuera de la maleta para sacarlo.";
                manoAnimator.SetInteger("step", 3);
            }
        }
        else if (state == State.CLICKLIST )
        {
            state = State.CLICKEDLIST;
            texto.text = "Aqui puedes echar un ojo a la lista de objetos en caso de que se te ha olvidado.";
            panel.SetActive(true);

        }
        else if (state == State.END && camara.gameObject.activeSelf)
        {
            state = State.NULL;
            texto.text = "Revise su equipaje y haga click sobre el icono del avión para terminar y marcharse de viaje.";
            panel.SetActive(true);
        }
    }
    public void ButtonList()
    {
        if (state == State.CLICKEDLIST)
        {
            state = State.BACKFROMLIST;
            texto.text = "Cuidado que cada revisión resta una oportunidad. Ya no podrás volver a revisar cuando agoten todos los intentos.";
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
            texto.text = "Vuelva a meter la camiseta amarilla en la maleta.";
            manoAnimator.SetInteger("step", 2);
           

        }
        else if (state == State.DRAWER)
        {
            state = State.BATHROOM;
            texto.text = "Puede hacer click en el icono de la derecha para ir al baño.";
            manoAnimator.SetInteger("step", 6);
        }
    }
    public void ButtonBathRoom()
    {
        if (state == State.BATHROOM)
        {
            state = State.BACKTHROOM;
            texto.text = "Revise los cajones y haga click en el icono señalado para volver a la habitación";
            manoAnimator.SetInteger("step", 7);

        }

    }
    public void ButtonBegin()
    {
        state = State.CLICK;
        texto = panel.GetComponentInChildren<Text>();
        texto.text = "Pulse sobre la camiseta amarilla y mantenga pulsado.";
        manoAnimator.SetInteger("step", 0);
        panel.SetActive(true);
    }
   
    public void End()
    {
        panel.SetActive(false);
        mano.SetActive(false);
    }
}
