using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public Sound[] sounds;

    private void Awake()
    {
        // Don't create AudioManager duplications in every scene
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Have the same AudioManager in all scenes
        DontDestroyOnLoad(gameObject);
        
        // Create AudioSource for every attached audio clip
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        // Play main theme sound during the gameplay
        Play("MainTheme");
    }

    public void Play(string name)
    {
        // Find sound in array
        var sound = Array.Find(sounds, sound => sound.name == name);

        // Make sure selected sound is available and is not currently playing
        if (sound != null && !sound.source.isPlaying)
        {
            // Play sound
            sound.source.Play();
        }
    }

    public bool IsPlaying(string name)
    {
        // Find sound in array
        var sound = Array.Find(sounds, sound => sound.name == name);

        // Return true if selected sound is playing
        return sound != null && sound.source.isPlaying;
    }
}
