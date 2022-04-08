using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public Text LifeText;
    public Text shieldText;
    private static int lives;
    public static int shield;
    public GameObject Game_Over;
    public GameObject AuthManager;
    public Text ScoreObject;
    private void Start()
    {
        lives = 3;
        shield = 0;
        Time.timeScale = 1;
        LifeText.GetComponent<Text>();
        shieldText.GetComponent<Text>();
        Game_Over.GetComponent<Canvas>();  
        
    }
    public void Update()
    {
        int currentScore = int.Parse(ScoreObject.GetComponent<Text>().text.ToString());
        int highscore = int.Parse(PlayerPrefs.GetString("Highscore", null));
        if (shield < 0)
        {
            shield = 0;
        }
        if (lives < 0) {
            if (currentScore > highscore)
            {
              StartCoroutine(AuthManager.GetComponent<AuthManager>().UpdateScore(currentScore));
            }
            
            GameOverToggle();
        }
        shieldText.text = " X" + shield;
        LifeText.text = " X" + lives;
    }
 
    public static void GameOver()
    {
      
        if (shield > 0)
        {
            shield--;

        }
        else
        {
            lives--;
        }

        if (lives < 0)
        {
            
       
            Time.timeScale = 0;
        }
    }
    public static void updateShield(int shield_update)
    {
        shield += shield_update;
        if (shield > 5)
        {
            shield = 5;
        }
    }
    public static void updateLives(int live_update)
    {
        lives += live_update;
        if (lives > 10)
        {
            lives = 10;
        }
    }
    public void GameOverToggle()
    {
        Game_Over.SetActive(true);
    }

}
