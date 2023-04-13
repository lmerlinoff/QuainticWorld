using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;
    public Text songTitleText;
    public Slider volumeSlider;
    public Button playButton;
    public Button nextButton;
    public Button prevButton;

    public AudioSource audioSource;
    public int currentSongIndex = 0;
    public bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playButton.onClick.AddListener(PlayButtonOnClick);
        nextButton.onClick.AddListener(NextButtonOnClick);
        prevButton.onClick.AddListener(PrevButtonOnClick);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        EnablePlayer(false);
    }

    void Update()
    {
        if (!audioSource.isPlaying && isPlaying)
        {
            currentSongIndex = (currentSongIndex + 1) % songs.Length;
            PlayCurrentSong();
        }
    }

    void SetVolume(float volume)
    {
        audioSource.volume = volume / 100.0f;
    }

    void PlayButtonOnClick()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
            playButton.GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            audioSource.UnPause();
            isPlaying = true;
            playButton.GetComponentInChildren<Text>().text = "Pause";
        }
    }

    void NextButtonOnClick()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        PlayCurrentSong();
    }

    void PrevButtonOnClick()
    {
        currentSongIndex--;
        if (currentSongIndex < 0)
        {
            currentSongIndex = songs.Length - 1;
        }
        PlayCurrentSong();
    }

    void PlayCurrentSong()
    {
        audioSource.clip = songs[currentSongIndex];
        audioSource.Play();
        songTitleText.text = audioSource.clip.name;
        isPlaying = true;
        playButton.GetComponentInChildren<Text>().text = "Pause";
    }
    void EnablePlayer(bool enable)
{
    audioSource.enabled = enable;
    playButton.enabled = enable;
    nextButton.enabled = enable;
    prevButton.enabled = enable;
    volumeSlider.enabled = enable;
}

public void TogglePlayer()
{
    if (audioSource.enabled)
    {
        audioSource.Stop();
        EnablePlayer(false);
    }
    else
    {
        EnablePlayer(true);
    }
}
}