using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setter : MonoBehaviour
{
    [Header("DISPLAYING INFO")]
    public Text Username;
    public Text Email;
    public Text Score;

    [Header("EDITING INFO")]
    public TMP_InputField editUsername;
    public TMP_InputField editEmail;
    public GameObject authman;
    private void Update()
    {
        StartCoroutine(doSequentialStuff());
    }
    private void Start()
    {
        EditUpdateField();
    }
    IEnumerator doSequentialStuff()
    {
        //Do first request then wait for it to return
        yield return StartCoroutine(authman.GetComponent<AuthManager>().LoadUserData());
        UpdateFields();
    }
    void UpdateFields()
    {
        Email.text = PlayerPrefs.GetString("Email", null);
        Username.text = PlayerPrefs.GetString("username", null);
        Score.text = PlayerPrefs.GetString("Highscore", null);
    }
    void EditUpdateField()
    {
        string username = PlayerPrefs.GetString("username"); 
        string email = PlayerPrefs.GetString("Email");
        editUsername.text = username;
        editUsername.IsInteractable();
        editEmail.text = email;
        editEmail.IsInteractable();
    }
}
