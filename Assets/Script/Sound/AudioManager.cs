using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource; 
    public AudioSource sfxSource; 

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
  
        if (backgroundMusic != null)
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
}
