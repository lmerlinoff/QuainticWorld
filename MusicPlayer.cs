using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Text songTitle;
    public Slider volumeSlider;
    public Button playButton;
    public Button nextButton;
    public Button prevButton;

    private List<string> songPaths;
    private int currentSongIndex = 0;

    void Start()
    {
        string[] musicExtensions = new string[] { ".wav", ".mp3" };
        string musicFolderPath = Application.dataPath + "/My Music/";
        songPaths = new List<string>();

        foreach (string file in Directory.GetFiles(musicFolderPath))
        {
            if (musicExtensions.Contains(Path.GetExtension(file)))
            {
                songPaths.Add(file);
            }
        }

        audioSource.clip = GetAudioClip(songPaths[currentSongIndex]);
        audioSource.Play();

        playButton.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(NextSong);
        prevButton.onClick.AddListener(PrevSong);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            playButton.GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            audioSource.Play();
            playButton.GetComponentInChildren<Text>().text = "Pause";
        }
    }

    private void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songPaths.Count;
        audioSource.clip = GetAudioClip(songPaths[currentSongIndex]);
        audioSource.Play();
        songTitle.text = Path.GetFileNameWithoutExtension(songPaths[currentSongIndex]);
    }

    private void PrevSong()
    {
        currentSongIndex = (currentSongIndex - 1 + songPaths.Count) % songPaths.Count;
        audioSource.clip = GetAudioClip(songPaths[currentSongIndex]);
        audioSource.Play();
        songTitle.text = Path.GetFileNameWithoutExtension(songPaths[currentSongIndex]);
    }

    private void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private AudioClip GetAudioClip(string path)
    {
        WWW www = new WWW("file://" + path);
        while (!www.isDone) { }
        return www.GetAudioClip(false, false);
    }
}
