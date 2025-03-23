using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void OnEnable()
    {
        Debug.Log("[Entrance] OnEnable called");
        Debug.Log($"[Entrance] Expected transition: {transitionName}");
        Debug.Log($"[Entrance] Actual transition: {SceneManagement.Instance.SceneTransitionName}");

        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.transform.position = transform.position;
                Debug.Log($"[Entrance] Player moved to {transform.position}");
            }
            else
            {
                Debug.LogError("[Entrance] PlayerController.Instance is null!");
            }

            if (CameraController.Instance != null)
            {
                CameraController.Instance.SetPlayerCameraFollow();
            }

            if (UIFade.Instance != null)
            {
                UIFade.Instance.FadeToClear();
            }
        }
        else
        {
            Debug.LogWarning("[Entrance] Transition name does not match. Player not moved.");
        }
    }
}
