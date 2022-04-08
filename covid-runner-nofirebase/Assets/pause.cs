using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    //public GameObject UI;
    public GameObject PauseCanvas;
    float _previousTimeScale;
    public void Paused()
    {
        _previousTimeScale = Time.timeScale;
        Time.timeScale=0;
        PauseCanvas.SetActive(true);
    }
    public void Resume()
    {
        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
       
        yield return new WaitForSecondsRealtime(0.1f);
        PauseCanvas.SetActive(false);
        Time.timeScale = _previousTimeScale;
    }
}
