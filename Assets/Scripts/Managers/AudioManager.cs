using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using anogame;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip bgmMain;
    public List<AudioClip> attackClipPlayerList;
    public List<AudioClip> attackClipEnemyList;
    public AudioClip coinGetClip;
    public AudioClip itemGetClip;
    public AudioClip levelUpClip;
    public AudioClip gachaClip;

    private float GetDecibel(float volume)
    {
        volume = Mathf.Clamp01(volume);

        float decibel = 20f * Mathf.Log10(volume);
        decibel = Mathf.Clamp(decibel, -80f, 0f);
        return decibel;

    }


    public void OnChangeVolumeBGM(float volume)
    {
        audioMixer.SetFloat("BGM", GetDecibel(volume));
    }
    public void OnChangeVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFX", GetDecibel(volume));
    }

    public void OnChangeVolumeMaster(float volume)
    {
        Debug.Log("OnChangeVolumeMaster:" + volume);
        audioMixer.SetFloat("Master", GetDecibel(volume));
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        PlayBGM(bgmMain);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomAttackPlayerSFX()
    {
        PlaySFX(attackClipPlayerList[Random.Range(0, attackClipPlayerList.Count)]);
    }

    public void PlayRandomAttackEnemySFX()
    {
        PlaySFX(attackClipEnemyList[Random.Range(0, attackClipEnemyList.Count)]);
    }

    public void PlayCoinGetSFX()
    {
        PlaySFX(coinGetClip);
    }

    public void PlayItemGetSFX()
    {
        PlaySFX(itemGetClip);
    }

    public void PlayLevelUpSFX()
    {
        PlaySFX(levelUpClip);
    }

    public void PlayGachaSFX()
    {
        PlaySFX(gachaClip);
    }

}
