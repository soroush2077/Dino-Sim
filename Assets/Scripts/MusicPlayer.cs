using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance;

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] string menuSceneName = "MainMenu";

    AudioSource source;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayFor(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayFor(scene.name);
    }

    void PlayFor(string sceneName)
    {
        AudioClip wanted = (sceneName == menuSceneName) ? menuMusic : gameMusic;

        if (wanted == null) return;
        if (source.clip == wanted && source.isPlaying) return;   // already playing the right track

        source.clip = wanted;
        source.Play();
    }
}