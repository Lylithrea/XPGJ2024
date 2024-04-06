using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct SoundData
{
    public float Volume;
}
public enum SoundName
{
    Button,
    Map,
    Menu,
    Battle,
    Rest,
    Reward,
    FollowerUse,
    FollowerSacrifice,
    EnemyHit,
    CharHit,
    CardHover,
    CardDraw,
    ButtonClick
}

public class SoundManager : MonoBehaviour
{
    List<AudioSource> activeSources = new();
    Stack<AudioSource> inactiveSources;
    public static SoundManager Instance { get; private set; }
    void Awake()
    {
        //Debug.Log("song awake");
        if (Instance != null && Instance != this)
        {
            Debug.Log("destroying song manager");
            Destroy(this);
        }
        else
        {
            inactiveSources = new Stack<AudioSource>(GetComponents<AudioSource>());
            foreach (AudioSource source in inactiveSources)
                InitSource(source);
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }


    [SerializedDictionary("Clip Name", "Clip")]
    public SerializedDictionary<SoundName, AudioClip> SoundClips;

    public void PlaySound(SoundName soundName)
    {
        if (!SoundClips.ContainsKey(soundName))
        {
            Debug.LogError($"There is no sound named {soundName}");
            return;
        }
        var source = GetSource();
        source.clip = SoundClips[soundName];
        source.Play();
    }

    public void PlaySound(SoundName soundName, SoundData data)
    {
        if (!SoundClips.ContainsKey(soundName))
        {
            Debug.LogError($"There is no sound named {soundName}");
            return;
        }
        var source = GetSource();
        source.clip = SoundClips[soundName];
        source.volume = data.Volume;
        source.Play();
    }

    void InitSource(AudioSource source)
    {
        source.playOnAwake = false;
    }

    AudioSource GetSource()
    {
        AudioSource source;
        if (inactiveSources.Count == 0)
        {
            source = gameObject.AddComponent<AudioSource>();
            InitSource(source);
        }
        else
            source = inactiveSources.Pop();

        activeSources.Add(source);
        return source;

    }

    void ReleaseSource(AudioSource audioSource)
    {
        activeSources.Remove(audioSource);
        inactiveSources.Push(audioSource);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < activeSources.Count; i++)
        {
            var source = activeSources[i];
            if (!source.isPlaying)
            {
                ReleaseSource(source);
                i--;
            }
        }
    }
}
