using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class animator : MonoBehaviour
{
    public Animator anim;
    public static string lvl = "";
    public void FadeToLevel(string lvlname)
    {

        anim.SetTrigger("FadeOut");
        Debug.Log(lvlname);
        lvl = lvlname.ToString();
        Debug.Log(lvl);
    }

    public void onFadeout() {
        Debug.Log(lvl);
        SceneManager.LoadScene(lvl);
    }
    public void Navigate(string lvl) {
        Time.timeScale = 1;
        SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.click);
        string lbl = lvl.ToString();
        SceneManager.LoadScene(lbl);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Home()
    {
        AuthManager auth = new AuthManager();
        auth.SignOut();
        Navigate("StartScreen");
    }
    public void clickSound(){
        SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.click);
    }

}
