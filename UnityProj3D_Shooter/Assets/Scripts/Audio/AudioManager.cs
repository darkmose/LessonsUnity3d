using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string MusicPath = "AudioSystem/Music";
    private const string SoundsPath = "AudioSystem/Sounds";
    static private AudioManager _instance;
    static public AudioManager Instance { get; }


    private Dictionary<string, AudioClip> _sounds;
    private Dictionary<string, AudioClip> _musics;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _musicSource;

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            _instance = this;
        }
        _sounds = InitializeSounds();
        _musics = InitializeMusics();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private Dictionary<string, AudioClip> InitializeMusics()
    {
        AudioClip[] musicClips = Resources.LoadAll<AudioClip>(MusicPath);
        var musics = new Dictionary<string, AudioClip>();

        for (int i = 0; i < musicClips.Length; i++)
        {
            musics.Add(musicClips[i].name, musicClips[i]);
        }
        Debug.Log($"Musics count: {musicClips.Length}");

        return musics;
    }

    public static void AudioMute(bool mute) 
    {
        _instance._audioSource.mute = mute;
    }
    public static void MusicMute(bool mute) 
    {
        _instance._musicSource.mute = mute;
    }


    private Dictionary<string, AudioClip> InitializeSounds()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(SoundsPath);
        var sounds = new Dictionary<string, AudioClip>();

        for (int i = 0; i < audioClips.Length; i++)
        {
            sounds.Add(audioClips[i].name, audioClips[i]);
        }
        Debug.Log($"Sounds count: {audioClips.Length}");

        return sounds;
    }

    public static void StopAudio() 
    {
        _instance._audioSource.Stop();
    }
    public static void StopMusic() 
    { 
        _instance._musicSource.Stop();
    }

    public static AudioClip GetAudio(string sound) 
    {
        return _instance._sounds[sound];
    }
    public static AudioClip GetMusic(string music) 
    {
        return _instance._musics[music];
    }

    public static void PlayClip(AudioClip clip) 
    {
        _instance._audioSource.PlayOneShot(clip);
    }

    static public void PlaySound(string sound)
    {        
        if (_instance._sounds.ContainsKey(sound))
        {
            var _soundFx = _instance._sounds[sound];
            _instance._audioSource.PlayOneShot(_soundFx);
        }
        else
        {
            Debug.Log($"Sound {sound} doesn't exist!");
        }


    }
    static public void PlayMusic(string music) 
    {        
        if (_instance._musics.ContainsKey(music))
        {
            var _soundFx = _instance._musics[music];
            _instance._musicSource.PlayOneShot(_soundFx);
        }
        else
        {
            Debug.Log($"Music {music} doesn't exist!");
        }
    }




}
