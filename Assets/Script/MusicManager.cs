using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    [Header("Musique par scène")]
    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;

    [Header("Musique spéciale")]
    public AudioClip bossMusic;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[MusicManager] Instance créée et persistante.");
        }
        else
        {
            Debug.Log("[MusicManager] Instance déjà existante : suppression de ce GameObject.");
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        Debug.Log("[MusicManager] AudioSource trouvé : " + (audioSource != null));

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[MusicManager] Nouvelle scène chargée : " + scene.name);
        PlayMusicForScene(scene.name);
    }

    public void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = null;

        switch (sceneName)
        {
            case "menuPrincipal": clip = menuMusic; break;
            case "loadingScene": clip = menuMusic; break;
            case "level_One": clip = level1Music; break;
            case "level_Two": clip = level2Music; break;
            case "level_Three": clip = level3Music; break;
            default: Debug.LogWarning("[MusicManager] Aucune musique définie pour cette scène."); break;
        }

        if (clip != null)
        {
            Debug.Log("[MusicManager] Démarrage du fade vers : " + clip.name);
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeToMusic(clip));
        }
    }

    public void PlayBossMusic()
    {
        if (bossMusic == null)
        {
            Debug.LogWarning("[MusicManager] BossMusic non assignée !");
            return;
        }

        Debug.Log("[MusicManager] Lancement musique Boss : " + bossMusic.name);
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToMusic(bossMusic));
    }

    public void StopBossMusicAndResumeLevel()
    {
        Debug.Log("[MusicManager] Fin combat Boss, reprise musique normale");
        // On récupère la scène actuelle et on relance la musique de niveau
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private System.Collections.IEnumerator FadeToMusic(AudioClip newClip)
    {
        float t = 0f;
        float fadeTime = 1f;
        float startVolume = audioSource.volume > 0 ? audioSource.volume : 1f;

        // Fade out
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }

        Debug.Log("[MusicManager] Musique jouée : " + newClip.name);
    }
}
