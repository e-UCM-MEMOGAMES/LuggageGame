using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class AudioManager : SingletonMonoBehaviour<AudioManager>
{


    [SerializeField]
    private AudioSource _musicSource,
                        _uiSoundSource,
                        _sceneSoundSource;

    //[SerializeField]
    //private AudioClip _buttonClip,
    //                  _mainMenuClip,
    //                  _levelClip;
    // [SerializeField]
    // private VolumeControl _volumeController;

    /// <summary>
    /// This nested class holds all data related to the playback of a single AudioClip. Instances of this class are exposed to the Inspector through the SoundManager class.
    /// </summary>
    [Serializable]
    public class SoundEntity
    {
#if UNITY_EDITOR
        /// <summary>
        /// These members are part of the compilation only in the Unity Editor; i.e. they won't compile into your game.
        /// The string replaces the default 'Element' string displayed as the name of the entities inside the array.
        /// But, at the same time the field itself is hidden from editing.
        /// </summary>
        [HideInInspector]
        public string Name;
        public void SetName()
            => Name = soundType.ToString();
#endif
        public ChannelType channelType;
        public GameSound soundType;
        public AudioClip audioClip;

    }



    [SerializeField]
    private SoundEntity[] _soundList = default;

    private Dictionary<GameSound, SoundEntity> _soundMap = new Dictionary<GameSound, SoundEntity>();

    private bool _initialized = false;


#if UNITY_EDITOR
    private void OnValidate()
    {
        // We're only interested in changes made in the Inspector
        if (!GUI.changed)
            return;

       // Set / update the names of entities to replace the generic 'element' name in Unity Inspector's array view
        foreach (var s in _soundList)
            s.SetName();

        // If changes were made in the Inspector while the game is running, and we're past initialization,
        // process again the list of sounds to make sure our runtime representation is up to date.
        if (EditorApplication.isPlaying && _initialized)
            PopulateSoundMap();
    }
#endif

    /// <summary>
    /// Converts the Editor-compatible array into a fast-lookup dictionary map.
    /// Creates a list for each sound type, to support multiple sounds of the same type.
    /// </summary>
    private void PopulateSoundMap()
    {
        foreach (var s in _soundList)
        {
            // Silently skip entries where 'None' is selected as soundtype
            if (s.soundType == GameSound.None)
                continue;

            else
            {
                _soundMap.Add(s.soundType, s);
                Debug.Log(s.soundType);
            }
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

    }

    public void Play(int s)
    {
        Play((GameSound)s);

    }
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

    public void PlayBGMSound(AudioClip clip)
    {
        StopMusic();
        if (clip != _musicSource.clip)
            _musicSource.clip = clip;
        _musicSource.Play();

    }

    public void PlayUISound(AudioClip clip)
    {
        Debug.Log("UI");
        _uiSoundSource.PlayOneShot(clip);
    }

    public void PlaySceneSound(AudioClip clip)
    {
        Debug.Log("scene");

        _sceneSoundSource.PlayOneShot(clip);
    }


    //public void FadeAudio()
    //{
    //    _volumeController.FadeAudio();
    //}




    public void StopMusic()
    {
        _musicSource.Stop();
    }


    // Start is called before the first frame update
    void Start()
    {
        //_volumeController = _volumeController.GetComponent<VolumeControl>();

        //_backgroundSource.Play();
    }
}


