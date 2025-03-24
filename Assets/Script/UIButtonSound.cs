using UnityEngine;
using System;
using System.Collections;

public class UIButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void PlayClickSoundThen(Action onComplete)
    {
        StartCoroutine(PlayAndWait(onComplete));
    }

    private IEnumerator PlayAndWait(Action onComplete)
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(clickSound.length);
        }

        onComplete?.Invoke(); 
    }
}
