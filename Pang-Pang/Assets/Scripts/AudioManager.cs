using UnityEngine;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Data")]
    [SerializeField] private SFXTable sfxTable;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private Dictionary<string, AudioClip> sfxdict = new Dictionary<string, AudioClip> ();

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitSFXTable();
        LoadVolume();
    }

    void InitSFXTable()
    {
        sfxdict.Clear();
        foreach(var sfx in sfxTable.sfxList)
        {
            if(!sfxdict.ContainsKey(sfx.key))
            {
                sfxdict.Add(sfx.key, sfx.clip);
            }
        }
    }

    void LoadVolume()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);

        bgmSource.volume = bgmVolume;
    }

    public void PlaySFX(string key)
    {
        if(!sfxdict.ContainsKey(key))
        {
            Debug.LogWarning($"SFX Key Not Found: {key}");
            return;
        }

        sfxSource.PlayOneShot(sfxdict[key], sfxVolume);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFX_VOLUME", value);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("BGM_VOLUME", value);
    }


}
