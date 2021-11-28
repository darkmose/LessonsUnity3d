using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManager;
    private Dictionary<AudioClips, AudioClip> audioDictionary;
    private Dictionary<MusicClips, AudioClip> musicDictionary;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    public enum AudioClips { WindWistle, bonfire, environment, FireworkBoom, FireworkShift };
    public enum MusicClips { };


    private void Awake()
    {
        if (!audioManager)
        {
            audioManager = this;
        }
        audioDictionary = new Dictionary<AudioClips, AudioClip>();
        musicDictionary = new Dictionary<MusicClips, AudioClip>();
        audioDictionary.Add(AudioClips.bonfire, Resources.Load<AudioClip>("Sounds/bonfire"));
        audioDictionary.Add(AudioClips.FireworkBoom, Resources.Load<AudioClip>("Sounds/deepshot"));
        audioDictionary.Add(AudioClips.FireworkShift, Resources.Load<AudioClip>("Sounds/firework"));
        audioDictionary.Add(AudioClips.environment, Resources.Load<AudioClip>("Sounds/night-time"));

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

    }



    private IEnumerator DelayPlaySounds(AudioClips clip, float time)
    {
        yield return new WaitForSeconds(time);
        audioManager.audioSource.PlayOneShot(audioDictionary[clip]);
        yield return null;
    }

    private void PlaySFX_Delayed(AudioClips clip, float time)
    {
        StartCoroutine(DelayPlaySounds(clip, time));
    }


    public static void PlaySFX(AudioClips clip) 
    {
        audioManager.audioSource.PlayOneShot(audioManager.audioDictionary[clip]);
    }

    public static void PlaySFX(AudioClips clip, float time)
    {
        audioManager.PlaySFX_Delayed(clip, time);
    }

    public static void PlayMusic(MusicClips clip) 
    {
        audioManager.musicSource.PlayOneShot(audioManager.musicDictionary[clip]);
    }
}

