using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    float maxTime;
    float timer;
    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;
    public GameObject obstacle4;
    public GameObject obstacle5;
    public GameObject obstacle6;
    public GameObject powerup;
    public GameObject powerup1;
    private int ChooseObstacle;

    // Start is called before the first frame update
    void Start()
    {
        maxTime = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= maxTime)
        {
            
            timer = 0;
            GenerateObstacle();
        }
    }
    void GenerateObstacle()
    {
        ChooseObstacle = Random.Range(1,9);
        if(ChooseObstacle == 1) { GameObject obstacle = Instantiate(obstacle1); }
        if (ChooseObstacle == 2) { GameObject obstacle = Instantiate(obstacle2); }
        if (ChooseObstacle == 3) { GameObject obstacle = Instantiate(obstacle3); }
        if (ChooseObstacle == 4) { GameObject obstacle = Instantiate(obstacle4); }
        if (ChooseObstacle == 5) { GameObject obstacle = Instantiate(obstacle5); }
        if (ChooseObstacle == 6) { GameObject obstacle = Instantiate(obstacle6); }
        if (ChooseObstacle == 7) { GameObject obstacle = Instantiate(powerup); }
        if (ChooseObstacle == 8) { GameObject obstacle = Instantiate(powerup1); }
    }
}
