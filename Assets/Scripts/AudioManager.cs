using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


//Se encarga de guardar todos los clips de sonido, gestionar la lista de sonidos y su volumen
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{


    [SerializeField]
    private AudioSource _musicSource,
                        _uiSoundSource,
                        _sceneSoundSource;

    [Serializable]
    public class SoundEntity
    {
#if UNITY_EDITOR
 
        [HideInInspector]
        public string Name;
        public void SetName()
            => Name = soundType.ToString();
#endif
        public ChannelType channelType; //Indica el audioSource al que pertenece
        public GameSound soundType; //tipo de sonido en el juego
        public AudioClip audioClip;//clip correspondiente

    }


    [SerializeField]
    private SoundEntity[] _soundList = default;

    private Dictionary<GameSound, SoundEntity> _soundMap = new Dictionary<GameSound, SoundEntity>(); //para facil acceso segun el tipo de sonido dado

    private bool _initialized = false;


#if UNITY_EDITOR
    private void OnValidate()
    {

        if (!GUI.changed)
            return;

        //establecer el nombre de los sonidos
        foreach (var s in _soundList)
            s.SetName();

       //inicializar el mapa de sonidos
        if (EditorApplication.isPlaying && _initialized)
            PopulateSoundMap();
    }
#endif


    private void PopulateSoundMap()
    {
        foreach (var s in _soundList)
        {
            if (s.soundType == GameSound.None)
                continue;

            else _soundMap.Add(s.soundType, s);
            
        }


    }
    /// <summary>
    /// Initialization.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        if (_initialized == true)
            return;

        PopulateSoundMap();
        _initialized = true;

        _musicSource.loop = true;


        //cargar volumen si ha habido cambios
        if (PlayerPrefs.HasKey("musicVolume"))
            _musicSource.volume = PlayerPrefs.GetFloat("musicVolume");

        if (PlayerPrefs.HasKey("soundVolume"))
            _musicSource.volume = PlayerPrefs.GetFloat("soundVolume");

    }

    //play de la musica con el indice dado
    public void Play(int s)
    {
        Play((GameSound)s);

    }
    ///play de la musica con el tipo de sonido dado
    public void Play(GameSound sound)
    {
        if (_soundMap.ContainsKey(sound))
        {
            ChannelType type = _soundMap[sound].channelType;
            if (type == ChannelType.BGM)
            {
                PlayBGMSound(_soundMap[sound].audioClip);
            }
            else if (type == ChannelType.UI)
            {
                PlayUISound(_soundMap[sound].audioClip);
            }
            else if (type == ChannelType.SceneSound)
            {
                PlaySceneSound(_soundMap[sound].audioClip);
            }
        }

    }

    //musica de fondo
    public void PlayBGMSound(AudioClip clip)
    {
        StopMusic();
        if (clip != _musicSource.clip)
            _musicSource.clip = clip;
        _musicSource.Play();

    }

    //sonidos de ui

    public void PlayUISound(AudioClip clip)
    {
        _uiSoundSource.PlayOneShot(clip);
    }
    //efectos de sonido de los objetos del escenario

    public void PlaySceneSound(AudioClip clip)
    {

        _sceneSoundSource.PlayOneShot(clip);
    }

    //volumen de musica de fondo
    public void BGMVolume(float volume)
    {
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);

    }

    //volumen de efectos de sonido
    public void SoundEffectVolume(float volume)
    {
        _sceneSoundSource.volume = volume;
        _uiSoundSource.volume = volume;
        PlayerPrefs.SetFloat("soundVolume", volume);

    }
    public float GetBGMVolume()
    {
        return _musicSource.volume;
    }
    public float GetSoundEffectVolume()
    {
        return _sceneSoundSource.volume;
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }


}


