using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneMode());
        }
    }


    private IEnumerator LoadSceneMode()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }

}
