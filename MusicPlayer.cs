using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public List<string> musicFiles = new List<string>();
    public int currentTrack = 0;

    public Button playButton;
    public Button stopButton;
    public Button nextButton;
    public Button prevButton;
    public Button refreshButton;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Проверяем наличие папки Music и создаем ее, если она не существует
        string musicPath = Application.dataPath + "/Music";
        if (!Directory.Exists(musicPath))
        {
            Directory.CreateDirectory(musicPath);
        }

        // Загружаем файлы в папке Music и добавляем их в список музыкальных файлов
        string[] files = Directory.GetFiles(musicPath, "*.mp3");
        foreach (string file in files)
        {
            musicFiles.Add(file);
        }

        files = Directory.GetFiles(musicPath, "*.wav");
        foreach (string file in files)
        {
            musicFiles.Add(file);
        }

        // Проверяем наличие музыкальных файлов
        if (musicFiles.Count == 0)
        {
            Debug.LogWarning("No music files found in " + musicPath);
            return;
        }

        // Воспроизводим первый файл
        audioSource.clip = GetAudioClip(musicFiles[currentTrack]);
        audioSource.Play();

        // Добавляем обработчики событий кнопок
        playButton.onClick.AddListener(PlayTrack);
        stopButton.onClick.AddListener(StopTrack);
        nextButton.onClick.AddListener(NextTrack);
        prevButton.onClick.AddListener(PrevTrack);
        refreshButton.onClick.AddListener(RefreshTracks);
    }

    void PlayTrack()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopTrack()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void NextTrack()
    {
        currentTrack++;
        if (currentTrack >= musicFiles.Count)
        {
            currentTrack = 0;
        }

        audioSource.clip = GetAudioClip(musicFiles[currentTrack]);
        audioSource.Play();
    }

    void PrevTrack()
    {
        currentTrack--;
        if (currentTrack < 0)
        {
            currentTrack = musicFiles.Count - 1;
        }

        audioSource.clip = GetAudioClip(musicFiles[currentTrack]);
        audioSource.Play();
    }

    void RefreshTracks()
    {
        musicFiles.Clear();

        // Загружаем файлы в папке Music и добавляем их в список музыкальных файлов
        string musicPath = Application.dataPath + "/Music";
        string[] files = Directory.GetFiles(musicPath, "*.mp3");
        foreach (string file in files)
        {
            musicFiles.Add(file);
        }

        files = Directory.GetFiles(musicPath, "*.wav");
        foreach (string file in files)
        {
            musicFiles.Add(file);
        }

        // Проверяем наличие музыкальных файлов
            if (musicFiles.Count == 0)
    {
        Debug.LogWarning("No music files found in " + musicPath);
        return;
    }

    currentTrack = 0;
    audioSource.clip = GetAudioClip(musicFiles[currentTrack]);
    audioSource.Play();
}

AudioClip GetAudioClip(string filePath)
{
    // Загружаем аудио-клип из файла
    string audioPath = "file://" + filePath;
    WWW audioLoader = new WWW(audioPath);
    while (!audioLoader.isDone) { }

    return audioLoader.GetAudioClip();
}
}
