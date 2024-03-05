using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    private const string SFX_PARENT_NAME = "SFX";
    private const string SFX_NAME_FORMAT = "SFX - [{0}]";
    public const float TRACK_TRANSITION_SPEED = 1f;

    public static AudioManager Instance { get; private set; }

    public Dictionary<int, AudioChannel> channels = new Dictionary<int, AudioChannel>();

    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    private Transform sfxRoot;

    private void Awake()
    {
        if(Instance == null)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        sfxRoot = new GameObject(SFX_PARENT_NAME).transform;
        sfxRoot.SetParent(transform);
    }

    public AudioSource PlaySoundEffect(string filePath, float volume = 1, bool loop = false)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if(clip == null)
        {
            Debug.LogError($"Could not load audio from path '{filePath}'. Please ensure it exists within Resources!");
            return null;
        }

        return PlaySoundEffect(clip, volume, loop);
    }

    public AudioSource PlaySoundEffect(AudioClip clip, float volume = 1, bool loop = false)
    {
        AudioSource effectSource = new GameObject(string.Format(SFX_NAME_FORMAT, clip.name)).AddComponent<AudioSource>();
        effectSource.transform.SetParent(sfxRoot);
        effectSource.transform.position = sfxRoot.position;

        effectSource.clip = clip;
        effectSource.outputAudioMixerGroup = sfxMixer;
        effectSource.volume = volume;
        effectSource.loop = loop;
        effectSource.spatialBlend = 0;

        effectSource.Play();

        if(!loop)
            Destroy(effectSource.gameObject, clip.length + 1);

        return effectSource;
    }

    public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);

    public void StopSoundEffect(string soundName)
    {
        soundName = soundName.ToLower();

        AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
        foreach(var source in sources)
        {
            if(source.clip.name.ToLower() == soundName)
            {
                Destroy(source.gameObject);
                return;
            }
        }
    }

    public AudioTrack PlayTrack(string filePath, int channel = 0, float volumeCap = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if(clip == null)
        {
            Debug.LogError($"Could not load audio from path '{filePath}'. Please ensure it exists within Resources!");
            return null;
        }

        return PlayTrack(clip, channel, volumeCap, filePath);
    }

    public AudioTrack PlayTrack(AudioClip clip, int channel = 0, float volumeCap = 1f, string filePath = "")
    {
        AudioChannel audioChannel = TryGetChannel(channel, createIfDoesNotExist: true);
        AudioTrack track = audioChannel.PlayTrack(clip, volumeCap, filePath);
        return track;
    }

    public void StopTrack(int channel)
    {
        AudioChannel c = TryGetChannel(channel, createIfDoesNotExist: false);

        if(c == null)
            return;

        c.StopTrack();
    }

    public void StopTrack(string trackName)
    {
        trackName = trackName.ToLower();

        foreach(var channel in channels.Values)
        {
            if(channel.activeTrack != null && channel.activeTrack.name.ToLower() == trackName)
            {
                channel.StopTrack();
                return;
            }
        }
    }

    public AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false)
    {
        AudioChannel channel;

        if(channels.TryGetValue(channelNumber, out channel))
        {
            return channel;
        }
        else if(createIfDoesNotExist)
        {
            channel = new AudioChannel(channelNumber);

            channels.Add(channelNumber, channel);
            return channel;
        }

        return null;
    }
}
