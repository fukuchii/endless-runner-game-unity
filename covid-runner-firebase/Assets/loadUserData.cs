using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadUserData : MonoBehaviour
{
    public GameObject Auth_Manager;
    GameObject objToSpawn;
    GameObject rank;
    GameObject username;
    GameObject Score;
    public Dictionary<string, int> leaderboard = new Dictionary<string, int>();

    private void Update()
    {
        foreach (KeyValuePair<string, int> boss in leaderboard)
        {
            //Debug.Log(boss.Key + " " + boss.Value);
        }

    }
    void RefreshLeaderBoard()
    {

    }
}