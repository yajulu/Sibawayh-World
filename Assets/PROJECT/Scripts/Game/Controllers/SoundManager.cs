using System.Collections;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Tools;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {

        private string MUSIC_PREFS = "Music_Volume";

        public float MusicVolume
        {
            get => _audioSource.volume;
            set {
                _audioSource.volume = value;
                SaveUtility.SaveObject<float>(MUSIC_PREFS, value, true);
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _audioSource = GetComponent<AudioSource>();
            if (SaveUtility.HasKey(MUSIC_PREFS))
                _audioSource.volume = SaveUtility.LoadObject<float>(MUSIC_PREFS);
            else
                _audioSource.volume = 1;
        }

        private AudioSource _audioSource;

        public void PlaySound(AudioClip sound)
        {
            _audioSource.PlayOneShot(sound);
        }
    }

}