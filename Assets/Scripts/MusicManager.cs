using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music Tracks")]
    public AudioClip mainMenuMusic;
    public AudioClip roomMusic;
    public AudioClip LastRoomMusic;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool loopMusic = true;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.loop = loopMusic;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartScene":
                PlayMusic(mainMenuMusic);
                break;
            case "Room1":
                PlayMusic(roomMusic);
                break;
            case "Room2":
                PlayMusic(roomMusic);
                break;
            case "Room3":
                PlayMusic(roomMusic);
                break;
            case "LastRoom":
                PlayMusic(LastRoomMusic);
                break;
            default:
                PlayMusic(mainMenuMusic);
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }

    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }
}