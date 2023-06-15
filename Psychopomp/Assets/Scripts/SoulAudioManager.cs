using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoulAudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public List<Sound> randomSounds = new List<Sound>();
    // private bool isRandomSoundPlaybackActive = false;
    private bool isPlayingRandomSound = false;
    List<Coroutine> randomSoundCoroutines = new List<Coroutine>();
    public float minSoundInterval = 1f; // Minimum time interval before playing the next sound
    public float maxSoundInterval = 5f; // Maximum time interval before playing the next sound

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    void Start()
    {

        foreach (Sound s in sounds)
        {
            if (s.playRandomly)
            {
                randomSounds.Add(s);
            }
        }
    }

    public void playSoundRandomly()
    {
        if (isPlayingRandomSound)
            return;

        Coroutine coroutine = StartCoroutine(PlayRandomSoundCoroutine());
        randomSoundCoroutines.Add(coroutine);
    }

    public void stopSoundRandomly()
    {
        if (!isPlayingRandomSound)
            return;

        foreach (Coroutine coroutine in randomSoundCoroutines)
        {
            StopCoroutine(coroutine);
        }

        isPlayingRandomSound = false;
    }

    IEnumerator PlayRandomSoundCoroutine()
    {
        isPlayingRandomSound = true;

        while (true)
        {
            Sound randomSound = randomSounds[UnityEngine.Random.Range(0, randomSounds.Count)];
            play(randomSound.name);

            yield return new WaitForSeconds(randomSound.clip.length);

            float randomInterval = UnityEngine.Random.Range(minSoundInterval, maxSoundInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void playSad()
    {
        StartCoroutine(playSadCoroutine());
    }

    IEnumerator playSadCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        //Stop all audioSource
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }

        play("Sad");
    }
}
