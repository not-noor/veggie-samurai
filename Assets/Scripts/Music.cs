using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip MainMenuMusic; 
    public AudioClip GameStartMusic; 
    public AudioClip BGM;         
    private AudioSource mainAudioSource; 
    private AudioSource secondaryAudioSource; 

    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);

        mainAudioSource = gameObject.AddComponent<AudioSource>();
        secondaryAudioSource = gameObject.AddComponent<AudioSource>();

        // Play the main menu music by default
        PlayMusic(mainAudioSource, MainMenuMusic, true);
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene") 
        {
            PlayMusic(secondaryAudioSource, GameStartMusic, false);

            PlayMusic(mainAudioSource, BGM, true);
        }

        if (scene.name == "MainMenu")
        {
            mainAudioSource.Stop();
            PlayMusic(mainAudioSource, MainMenuMusic, true);
        }
    }

    private void PlayMusic(AudioSource audioSource, AudioClip clip, bool loop)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip;
        audioSource.loop = loop; // Set looping based on the parameter
        audioSource.Play();
    }
}