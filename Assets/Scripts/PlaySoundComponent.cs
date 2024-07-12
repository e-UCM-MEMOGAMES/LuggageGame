using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//componente pensado para añadir a cualquier objeto interactuable de la escena donde se define el tipo de sonido desde el inspector
public class PlaySoundComponent : MonoBehaviour
{
    [SerializeField]
    GameSound gameSound;
    AudioManager audioMng;
  
    // Start is called before the first frame update
    void Start()
    {
        audioMng = AudioManager.Instance;
    }
    public void PlaySound()
    {
        audioMng.Play(gameSound);
    }


}
