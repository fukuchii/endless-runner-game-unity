using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider loadingbar;
    public void LoadLevel(int index) {
        StartCoroutine(LoadAsynchronously(index));
    }
    IEnumerator LoadAsynchronously(int index) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        LoadingScreen.SetActive(true);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/.9f);
            //Debug.Log(progress);
            loadingbar.value=progress;
            yield return null;
        }
    }
}
