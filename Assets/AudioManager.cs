using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "MainMenu")
            {
                musicSource.clip = menuMusic;
                musicSource.volume = 1f;
                musicSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeToMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip && musicSource.isPlaying)
            yield break;

        float startVolume = musicSource.volume;
        float t = 0f;

        // Fade out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
    }
}
