using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GenerateLeaderboard : MonoBehaviour
{
    public GameObject Auth_Manager;
    private Dictionary<string, int> leaderboard = new Dictionary<string, int>();
    private Transform entryContainer;
    private Transform entryTemplate;
    private int isLoaded = 0;
    private void Start()
    {
        var AuthMan = Auth_Manager.GetComponent<AuthManager>();
        StartCoroutine(AuthMan.LoadScoreboardData());
        leaderboard = AuthMan.list;
    }
    private void Update()
    {
        this.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        this.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        if (isLoaded < 1)
        {
            entryContainer = transform.Find("highscoreEntryContainer");
            entryTemplate = entryContainer.Find("highscoreEntryTemplate");
            entryTemplate.gameObject.SetActive(false);
            float templateHeight = 10f;
            int rankString = 1;
            foreach (KeyValuePair<string, int> boss in leaderboard.Reverse())
            {
                if (boss.Key != null && rankString <= 10)
                {
                    Transform entryTransform = Instantiate(entryTemplate, entryContainer);
                    RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
                    entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * 8);
                    entryTransform.gameObject.SetActive(true);
                    entryTransform.Find("rank lbl").GetComponent<Text>().text = rankString.ToString();
                    entryTransform.Find("username lbl").GetComponent<Text>().text = boss.Key.ToString();
                    entryTransform.Find("score lbl").GetComponent<Text>().text = boss.Value.ToString();
                    rankString++;
                    isLoaded++;
                }

            }
        }
    }

}

