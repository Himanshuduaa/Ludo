using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializedDictionary("Audio SName", "Audio")]
    public SerializedDictionary<string, AudioClip> audioDictionary = new SerializedDictionary<string, AudioClip>();
    private AudioSource audioSource;

    private int currentTrackIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    Destroy(gameObject);
        //}

        audioSource = GetComponent<AudioSource>();
        FillAudioDictionary();
    }

    private void FillAudioDictionary()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioDictionary[audioClips[i].name] = audioClips[i];
        }
    }

    public void PlayAudio(string audioName)
    {
        if (audioDictionary.ContainsKey(audioName))
        {
            bool play = true;
            audioSource.Stop();
            audioSource.clip = audioDictionary[audioName];
            if(audioName=="BackgroundMusic")
            {
                audioSource.GetComponent<AudioSource>().loop = true;
                if (LudoManager.Instance.gameInfo.gameSettings.music == false)
                {
                    play = false;
                }
            }
            else
            {
                audioSource.GetComponent<AudioSource>().loop = false;
                if (LudoManager.Instance.gameInfo.gameSettings.sound == false)
                {
                    play = false;
                }
            }
            if (play)
                audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio not found: " + audioName);
        }
    }

    public void PlayNext()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= audioClips.Count)
        {
            currentTrackIndex = 0;
        }
        PlayAudio(audioClips[currentTrackIndex].name);
    }

    public void PlayPrevious()
    {
        currentTrackIndex--;
        if (currentTrackIndex < 0)
        {
            currentTrackIndex = audioClips.Count - 1;
        }
        PlayAudio(audioClips[currentTrackIndex].name);
    }
    public void PlayAudioOnSource(string audioName, AudioSource source)
    {
        if (audioDictionary.ContainsKey(audioName))
        {
            //source.Stop();
            source.clip = audioDictionary[audioName];
            //Debug.Log("Playing " + audioName);
            source.Play();
        }
        else
        {
            Debug.LogWarning("Audio not found: " + audioName);
        }
    }
}
