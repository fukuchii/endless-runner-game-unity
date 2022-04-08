using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditShow : MonoBehaviour
{
    public GameObject editCanvas;
    public GameObject success;
    public GameObject ShowInfoCanvas;
    public void DisableCanvas()
    {
        editCanvas.SetActive(true);
        ShowInfoCanvas.SetActive(false);
    }
    public void EnableCanvas()
    {
        editCanvas.SetActive(false);
        ShowInfoCanvas.SetActive(true);
    }
    public void Success()
    {
        success.SetActive(false);
        ShowInfoCanvas.SetActive(true);
    }
}
