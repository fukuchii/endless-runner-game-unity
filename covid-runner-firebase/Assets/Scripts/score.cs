using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    private int Score;
    public Text scoreText;

    float timer;
    float maxTime;


   

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        scoreText.GetComponent<Text>();
        maxTime = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTime)
        {
            Score++;
            scoreText.text = Score.ToString("00000");
            timer = 0;
        }
        if (Score % 1000  == 0)
        {
            if (Time.timeScale == 1.4f)
            {
                Time.timeScale = 1.4f;
            }
            else
            {
                Time.timeScale += 0.05f;
            }
        }
    }
    public int GetScore() { return Score; }
}
