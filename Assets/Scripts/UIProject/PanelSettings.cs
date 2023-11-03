using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogame;

public class PanelSettings : UIPanel
{

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private EventFloat onMasterVolumeChange;
    [SerializeField] private EventFloat onBGMVolumeChange;
    [SerializeField] private EventFloat onSFXVolumeChange;

    protected override void initialize()
    {
        base.initialize();

        masterVolumeSlider.value = ModelManager.Instance.UserGameData.master_volume;
        bgmVolumeSlider.value = ModelManager.Instance.UserGameData.bgm_volume;
        sfxVolumeSlider.value = ModelManager.Instance.UserGameData.sfx_volume;

        masterVolumeSlider.onValueChanged.AddListener(OnChangeVolumeMaster);
        bgmVolumeSlider.onValueChanged.AddListener(OnChangeVolumeBGM);
        sfxVolumeSlider.onValueChanged.AddListener(OnChangeVolumeSFX);

    }

    public void OnChangeVolumeMaster(float value)
    {
        ModelManager.Instance.UserGameData.master_volume = value;
        onMasterVolumeChange.Invoke(value);
    }

    public void OnChangeVolumeBGM(float value)
    {
        ModelManager.Instance.UserGameData.bgm_volume = value;
        onBGMVolumeChange.Invoke(value);
    }

    public void OnChangeVolumeSFX(float value)
    {
        ModelManager.Instance.UserGameData.sfx_volume = value;
        onSFXVolumeChange.Invoke(value);
    }





}
