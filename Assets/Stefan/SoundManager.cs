using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public struct SoundData
{
    public float Volume;

    public SoundData(float volume)
    {
        Volume = volume;
    }
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
    [SerializedDictionary("Clip Name", "Clip")] public SerializedDictionary<SoundName, AudioClip> SoundClips;

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

    public void PlayBattleMusic(float volume = .5f)
    {
        var source = GetSource("BattleStart");
        source.clip = SoundClips[SoundName.BattleStart];
        source.volume = volume;
        source.Play();
        var source2 = GetSource("BattleLoop");
        source2.clip = SoundClips[SoundName.BattleLoop];
        source2.loop = true;
        source2.volume = volume;
        source2.PlayDelayed(source.clip.length);
    }

    public void StopBattleMusic()
    {
        var start = FindSourceByName("BattleStart");
        var loop = FindSourceByName("BattleLoop");
        if(start != null)
            start.Stop();
        if(loop != null)
            loop.Stop();
    }

   

    public void PlayButtonSound(float volume = 0.5f)
    {
        var source = GetSource("ButtonClick");
        source.clip = SoundClips[SoundName.ButtonClick];
        source.volume = volume;
        source.Play();
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
        source.enabled = true;
        return source;

    }

    AudioSource FindSourceByName(string name)
    {
        var info = _activeSources.FirstOrDefault(s => s.Name == name);
        if(info != null)
            return info.AudioSource;
        return null;
    }

    void ReleaseSource(SourceInfo audioSource)
    {
        _activeSources.Remove(audioSource);
        audioSource.AudioSource.enabled = false;
        _inactiveSources.Push(audioSource.AudioSource);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < _activeSources.Count; i++)
        {
            var source = _activeSources[i];
            if (!source.AudioSource.isPlaying)
            {
                ReleaseSource(source);
                i--;
            }
        }
    }
}
