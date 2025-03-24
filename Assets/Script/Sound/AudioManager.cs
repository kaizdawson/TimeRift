using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource; 
    public AudioSource sfxSource;
    public AudioClip bossMusicClip;

    public AudioClip backgroundMusic; 

    void Awake()
    {
     
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Scene3")
        {
            PlayMusicWithFade(bossMusicClip, 1.5f);
        }
        else if (backgroundMusic != null)
        {
            PlayBGM(backgroundMusic);
        }
    }

 
    public void PlayBGM(AudioClip music)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = music;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip sound)
    {
        sfxSource.PlayOneShot(sound);
    }

    public void PlayMusicWithFade(AudioClip newMusic, float fadeDuration = 1.0f)
    {
        StartCoroutine(FadeMusicCoroutine(newMusic, fadeDuration));
    }

    private IEnumerator FadeMusicCoroutine(AudioClip newMusic, float duration)
    {
        if (bgmSource.isPlaying)
        {
       
            float startVolume = bgmSource.volume;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
                yield return null;
            }
            bgmSource.Stop();
        }

        bgmSource.clip = newMusic;
        bgmSource.Play();

   
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, 1.0f, t / duration);
            yield return null;
        }

        bgmSource.volume = 1.0f;
    }


}
