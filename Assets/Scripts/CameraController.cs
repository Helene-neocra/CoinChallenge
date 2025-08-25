using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera skyCamera;
    public CinemachineVirtualCamera playerCamera;
    public float transitionDelay = 2f;
    
    void Awake()
    {
        skyCamera.Priority = 20;
        playerCamera.Priority = 10;
        StartCoroutine(TransitionToPlayer());
    }

    IEnumerator TransitionToPlayer()
    {
        yield return new WaitForSeconds(transitionDelay);
        skyCamera.Priority = 10;
        playerCamera.Priority = 20;
    }

}
