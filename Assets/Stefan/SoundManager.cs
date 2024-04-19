using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public float Volume;
    public AudioClip Clip;
    
}
public enum SoundName
{
    Button,
    Map,
    Menu,
    BattleStart,
    BattleLoop,
    Rest,
    Reward,
    CardUse,
    CardSacrifice,
    EnemyHit,
    CharHit,
    CardHover,
    CardDraw,
    ButtonClick,
    MapOpen,
    MapHover
}
[Serializable]
class SourceInfo
{
    public AudioSource AudioSource;
    public string Name;
    
    public SourceInfo(AudioSource audioSource, string name)
    {
        AudioSource = audioSource;
        Name = name;
    }
}

public class SoundManager : MonoBehaviour
{
    readonly List<SourceInfo> _activeSources = new();
    Stack<AudioSource> _inactiveSources;
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
            _inactiveSources = new Stack<AudioSource>(GetComponents<AudioSource>());
            foreach (AudioSource source in _inactiveSources)
                InitSource(source);
            Instance = this;

            DontDestroyOnLoad(this);
        }
    }

    [SerializeField, Range(0, 1)] float _masterVolume; 
    [SerializedDictionary("Clip Name", "Clip")] public SerializedDictionary<SoundName, SoundData> SoundClips;

    public bool ContainsSoundWithName(string name)
    {
        return _activeSources.Any(s => s.Name == name);
    }

    public void PlaySound(SoundName soundName)
    {
        if (!SoundClips.ContainsKey(soundName))
        {
            Debug.LogError($"There is no sound named {soundName}");
            return;
        }
        var source = GetSource();
        source.clip = SoundClips[soundName].Clip;
        source.volume *= _masterVolume;
        source.Play();
    }

    public void PlaySound(SoundName soundName, float volumeMult = 1, string name = null, bool loop = false)
    {
        if (!SoundClips.ContainsKey(soundName))
        {
            Debug.LogError($"There is no sound named {soundName}");
            return;
        }

        AudioSource source = GetSource(name);
        SoundData data = SoundClips[soundName];
        source.clip = data.Clip;
        source.volume = data.Volume * volumeMult * _masterVolume;
        source.loop = loop;
        source.Play();
    }

    public void StopSound(string name)
    {
        SourceInfo source = FindSourceInfoByName(name);
        if(source != null)
        {
            source.AudioSource.Stop();
            ReleaseSource(source);
        }
    }

    public void PlayBattleMusic(float volume = .3f)
    {
        var source = GetSource("BattleStart");
        source.clip = SoundClips[SoundName.BattleStart].Clip;
        source.volume = volume * _masterVolume;
        source.Play();

        var source2 = GetSource("BattleLoop");
        source2.clip = SoundClips[SoundName.BattleLoop].Clip;
        source2.loop = true;
        source2.volume = volume * _masterVolume;
        source2.PlayDelayed(source.clip.length);
    }

    public void StopBattleMusic()
    {
        SourceInfo start = FindSourceInfoByName("BattleStart");
        SourceInfo loop = FindSourceInfoByName("BattleLoop");

        if(start != null)
        {
            start.AudioSource.Stop();
            ReleaseSource(start);
        }
        if(loop != null)
        {
            loop.AudioSource.Stop();
            ReleaseSource(loop);
        }
    }


    void InitSource(AudioSource source)
    {
        source.playOnAwake = false;
    }

    AudioSource GetSource()
    {
        AudioSource source;
        if (_inactiveSources.Count == 0)
        {
            source = gameObject.AddComponent<AudioSource>();
            InitSource(source);
        }
        else
            source = _inactiveSources.Pop();

        _activeSources.Add(new SourceInfo(source,null));
        source.enabled = true;
        source.loop = false;
        source.volume = _masterVolume; 
        return source;

    }

    AudioSource GetSource(string nameSource)
    {
        AudioSource source;
        if (_inactiveSources.Count == 0)
        {
            source = gameObject.AddComponent<AudioSource>();
            InitSource(source);
        }
        else
            source = _inactiveSources.Pop();

        _activeSources.Add(new SourceInfo(source, nameSource));

        source.loop = false;
        source.volume = _masterVolume; 
        source.enabled = true;

        return source;

    }

    SourceInfo FindSourceInfoByName(string name)
    {
        return _activeSources.FirstOrDefault(s => s.Name == name);
    }

    void ReleaseSource(SourceInfo audioSource)
    {
        _activeSources.Remove(audioSource);
        audioSource.AudioSource.enabled = false;
        _inactiveSources.Push(audioSource.AudioSource);
    }

    void Update()
    {
        for (int i = 0; i < _activeSources.Count; i++)
        {
            var source = _activeSources[i];
            if (source.AudioSource.time >= source.AudioSource.clip.length || (source.AudioSource.time == 0 && !source.AudioSource.isPlaying))
            {
                ReleaseSource(source);
                i--;
            }
        }
    }
}
