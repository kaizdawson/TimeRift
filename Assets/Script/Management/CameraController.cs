using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = Object.FindFirstObjectByType<CinemachineVirtualCamera>();
        //cinemachineVirtualCamera = Object.FindFirstObjectByType<CinemachineCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
