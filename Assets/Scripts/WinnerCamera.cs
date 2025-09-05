using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WinnerCamera : MonoBehaviour
{
    public CinemachineVirtualCamera canvas;
    public CinemachineVirtualCamera wide;
    public float transitionDelay = 2f;
    
    // Start is called before the first frame update
    void Awake()
    {
        wide.Priority = 20;
        canvas.Priority = 10;
        StartCoroutine(TransitionToCanvas());
    }

    IEnumerator TransitionToCanvas()
    {
        yield return new WaitForSeconds(transitionDelay);
        wide.Priority = 10;
        canvas.Priority = 20;
    }
}
