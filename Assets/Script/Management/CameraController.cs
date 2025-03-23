
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private void Start()
    {
        SetPlayerCameraFollow();
    }
    public void SetPlayerCameraFollow()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        cinemachineCamera.Follow = PlayerController.Instance.transform;

        if (cinemachineCamera != null && PlayerController.Instance != null)
        {
            cinemachineCamera.Follow = PlayerController.Instance.transform; 
        }
        else
        {
            Debug.LogError("CinemachineCamera or PlayerController is NULL!");
        }
    }
}

