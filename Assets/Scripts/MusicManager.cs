using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip bgMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bgMusic != null) {
            PlayBackgroundMusic(false, bgMusic);
        }
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume) {
        Instance.audioSource.volume = volume;
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null) {
        if (audioClip != null) {
            audioSource.clip = audioClip;
        }
        if (audioSource.clip != null) {
            if (resetSong) {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

}
