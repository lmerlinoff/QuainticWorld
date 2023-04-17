using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Text musicTitle;
    public Slider volumeSlider;
    public Button playButton;
    public Button nextButton;
    public Button prevButton;
    public Button updateButton;

    private List<string> musicList;
    private int currentSongIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        musicList = new List<string>();
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        playButton.onClick.AddListener(delegate { OnPlayButtonClick(); });
        nextButton.onClick.AddListener(delegate { OnNextButtonClick(); });
        prevButton.onClick.AddListener(delegate { OnPrevButtonClick(); });
        updateButton.onClick.AddListener(delegate { OnUpdateButtonClick(); });
        OnUpdateButtonClick();
    }

    private void OnPlayButtonClick()
    {
        if (musicList.Count > 0)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                audioSource.Play();
                playButton.GetComponentInChildren<Text>().text = "Pause";
            }
            else
            {
                isPlaying = false;
                audioSource.Pause();
                playButton.GetComponentInChildren<Text>().text = "Play";
            }
        }
    }

    private void OnNextButtonClick()
    {
        if (musicList.Count > 0)
        {
            currentSongIndex = (currentSongIndex + 1) % musicList.Count;
            PlayCurrentSong();
        }
    }

    private void OnPrevButtonClick()
    {
        if (musicList.Count > 0)
        {
            currentSongIndex--;
            if (currentSongIndex < 0)
            {
                currentSongIndex = musicList.Count - 1;
            }
            PlayCurrentSong();
        }
    }

    private void OnUpdateButtonClick()
    {
        string path = Application.dataPath + "/My music";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles();
        musicList.Clear();
        foreach (FileInfo file in files)
        {
            if (file.Extension.ToLower() == ".mp3" || file.Extension.ToLower() == ".wav")
            {
                musicList.Add(file.FullName);
            }
        }
        if (musicList.Count > 0)
        {
            currentSongIndex = 0;
            PlayCurrentSong();
        }
        else
        {
            musicTitle.text = "No music found";
            audioSource.Stop();
            isPlaying = false;
            playButton.GetComponentInChildren<Text>().text = "Play";
        }
    }

    private void OnVolumeChange()
    {
        audioSource.volume = volumeSlider.value;
    }

    private void PlayCurrentSong()
    {
        string currentSong = musicList[currentSongIndex];
        audioSource.Stop();
        audioSource.clip = NAudioPlayer.FromMp3Data(File.ReadAllBytes(currentSong));
        musicTitle.text = Path.GetFileNameWithoutExtension(currentSong);
        audioSource.Play();
        isPlaying = true;
        playButton.GetComponentInChildren<Text>().text = "Pause";
    }
}
