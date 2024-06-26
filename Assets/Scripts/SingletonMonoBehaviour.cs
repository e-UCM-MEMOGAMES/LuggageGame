using System;
using UnityEngine;


    /// <summary>
    /// This is just a very minimal singleton helper class, facilitating easy access to the SoundManager class.
    /// Replace or customize it to your liking.
    /// </summary>
    [DisallowMultipleComponent]
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance => _instance ?? throw new InvalidOperationException($"Pre-instantiation missing. An instance of {typeof(T).Name} is required to exist in the scene.");
        protected static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
               DontDestroyOnLoad(gameObject);
        }
            else if (_instance != (T)this)
            {
                Destroy(gameObject);
            }
          
        }
    }
