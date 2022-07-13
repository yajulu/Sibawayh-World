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

    protected override void Awake()
    {
        base.Awake();
        musicSlider.onValueChanged.AddListener(UpdateAudioSource);
        musicSlider.value = SoundManager.Instance.MusicVolume;
    }

    private void UpdateAudioSource(float value)
    {
        SoundManager.Instance.MusicVolume = value;
    }

    [Button]
    protected override void SetRefs()
    {
        base.SetRefs();
        musicSlider = transform.FindDeepChild<RectTransform>("MusicSlider").GetComponentInChildren<Slider>(true);
    }
}
