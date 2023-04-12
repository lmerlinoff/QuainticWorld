using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> songs;
    public Text songTitleText;
    public Slider volumeSlider;
    public Button playButton;
    public Button nextButton;
    public Button prevButton;

    private int currentSongIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        // Установка начальных значений элементов
        songTitleText.text = songs[currentSongIndex].name;
        volumeSlider.value = audioSource.volume;
        playButton.onClick.AddListener(PlayButtonOnClick);
        nextButton.onClick.AddListener(NextButtonOnClick);
        prevButton.onClick.AddListener(PrevButtonOnClick);
    }

    void Update()
    {
        if (isPlaying && !audioSource.isPlaying)
        {
            // Автоматически переходить на следующую песню после окончания текущей
            currentSongIndex = (currentSongIndex + 1) % songs.Count;
            PlayCurrentSong();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayButtonOnClick()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
            playButton.GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            audioSource.Play();
            isPlaying = true;
            playButton.GetComponentInChildren<Text>().text = "Pause";
        }
    }

    public void NextButtonOnClick()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Count;
        PlayCurrentSong();
    }

    public void PrevButtonOnClick()
    {
        currentSongIndex--;
        if (currentSongIndex < 0)
        {
            currentSongIndex = songs.Count - 1;
        }
        PlayCurrentSong();
    }

    public void PlayCurrentSong()
    {
        audioSource.Stop();
        audioSource.clip = songs[currentSongIndex];
        audioSource.Play();
        isPlaying = true;
        playButton.GetComponentInChildren<Text>().text = "Pause";
        songTitleText.text = songs[currentSongIndex].name;
    }

    public void SelectSong(int index)
    {
        currentSongIndex = index;
        PlayCurrentSong();
    }

    public void EnablePlayer(bool enable)
    {
        audioSource.enabled = enable;
        volumeSlider.enabled = enable;
        playButton.enabled = enable;
        nextButton.enabled = enable;
        prevButton.enabled = enable;
    }
}