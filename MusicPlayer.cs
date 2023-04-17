using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;


public class MusicPlayer : MonoBehaviour
{
    public Text songTitle;
    public Slider volumeSlider;
    public Button playButton;
    public Button nextButton;
    public Button prevButton;
    public Button updateButton;
    public AudioSource audioSource;

    private List<string> musicList = new List<string>();
    private int currentSongIndex = -1;

    void Start()
    {
        // Create the "My music" folder if it doesn't exist
        if (!Directory.Exists(Application.persistentDataPath + "/My music"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/My music");
        }

        // Find all music files in the "My music" folder
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/My music", "*.mp3");

        // Add the music files to the list
        foreach (string filePath in filePaths)
        {
            musicList.Add(filePath);
        }

        // Set the initial volume and play the first song in the list
        audioSource.volume = volumeSlider.value;
        PlaySong(0);
    }

    void Update()
    {
        // Update the volume as the slider changes
        audioSource.volume = volumeSlider.value;
    }

    public void PlayPause()
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

    public void NextSong()
    {
        if (currentSongIndex < musicList.Count - 1)
        {
            currentSongIndex++;
            PlaySong(currentSongIndex);
        }
    }

    public void PrevSong()
    {
        if (currentSongIndex > 0)
        {
            currentSongIndex--;
            PlaySong(currentSongIndex);
        }
    }

    public void UpdateMusic()
    {
        // Clear the current music list
        musicList.Clear();

        // Find all music files in the "My music" folder
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/My music", "*.mp3");

        // Add the music files to the list
        foreach (string filePath in filePaths)
        {
            musicList.Add(filePath);
        }
    }

    private void PlaySong(int index)
    {
        // Stop the current song and update the song title
        audioSource.Stop();
        currentSongIndex = index;
        songTitle.text = Path.GetFileNameWithoutExtension(musicList[currentSongIndex]);

        // Play the new song
        audioSource.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(musicList[currentSongIndex]));
        audioSource.Play();
        playButton.GetComponentInChildren<Text>().text = "Pause";
    }
}
