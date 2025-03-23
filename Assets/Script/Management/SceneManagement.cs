using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Debug.Log("Scene loaded: " + scene.name);

    if (UIFade.Instance != null)
    {
        UIFade.Instance.FadeToClear();
        Debug.Log("FadeToClear() called after scene load.");
    }
    else
    {
        Debug.LogWarning("UIFade.Instance is null after scene load.");
    }
}

}

