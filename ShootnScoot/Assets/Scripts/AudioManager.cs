using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    
    public float defaultVolume = .5f;
    public UnityEngine.UI.Slider SFXslider;
    public UnityEngine.UI.Slider BGMslider;



    public int SFXsoundSelector = 0;
    public int BGMsoundSelector = 0;

    public static AudioManager instance;

    public AudioClip[] sfxClips;
    public AudioSource sfxAudioSource;
    [Range(0, 1)] public float sfxVolume = 0.5f;

    public AudioClip[] bgmClips;
    public AudioSource bgmAudioSource;
    [Range(0, 1)] public float bgmVolume = 0.5f;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayBGM(0);
    }

    // Update is called once per frame
    void Update()
    {
        sfxAudioSource.volume = sfxVolume;
        bgmAudioSource.volume = bgmVolume;

    }

    public void PlaySFX(int clip)
    {
        sfxAudioSource.PlayOneShot(sfxClips[clip]);
    }

    public void PlayBGM(int clip)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = bgmClips[clip];
        bgmAudioSource.Play();
    }

    

    public void SFXPressToPlay()
    {
        PlaySFX(SFXsoundSelector);
    }
    public void SFXVolumeControl(float pizzaVOLUME)
    {
        sfxVolume = pizzaVOLUME;
    }
    

    

    public void BGMPressToPlay()
    {
        PlayBGM(BGMsoundSelector);
    }
    public void BGMVolumeControl(float pizzaVOLUME)
    {
        bgmVolume = pizzaVOLUME;
    }

    public void DefaultVolume()
    {
        sfxVolume = defaultVolume;
        bgmVolume = defaultVolume;

        SFXslider.value = sfxVolume;
        BGMslider.value = bgmVolume;
    }



   
}
