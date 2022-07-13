using System;
using System.Collections;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Tools;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Settings : UIPanelBase
{

    [SerializeField] private Slider musicSlider;
    private AudioSource source;

    private string MUSIC_PREFS = "Music_Volume";

    protected override void Awake()
    {
        base.Awake();
        source = DataPersistenceManager.Instance.gameObject.GetComponent<AudioSource>();
        if (SaveUtility.HasKey(MUSIC_PREFS))
            source.volume = SaveUtility.LoadObject<float>(MUSIC_PREFS);
        else
            source.volume = 1;
        musicSlider.onValueChanged.AddListener(UpdateAudioSource);
        musicSlider.value = source.volume;
    }

    private void UpdateAudioSource(float value)
    {
        source.volume = value;
        SaveUtility.SaveObject(MUSIC_PREFS, source.volume, true);
    }

    [Button]
    protected override void SetRefs()
    {
        base.SetRefs();
        musicSlider = transform.FindDeepChild<RectTransform>("MusicSlider").GetComponentInChildren<Slider>(true);
    }
}
