using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private AudioClip exitSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private float waitToLoadTime = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            PlayExitSound();
            if (UIFade.Instance != null)
            {
                UIFade.Instance.FadeToBlack();
            }
            else
            {
                Debug.LogError("UIFade.Instance is null! Hãy kiểm tra xem UIFade có tồn tại trong scene không.");
            }

            StartCoroutine(LoadSceneMode());
        }
    }
    private void PlayExitSound()
    {
        if (exitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(exitSound, 0.7f);
        }
    }


    private IEnumerator LoadSceneMode()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }

}
