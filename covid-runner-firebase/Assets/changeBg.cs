using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeBg : MonoBehaviour
{
    public GameObject sky;
    public GameObject building;
    public GameObject mountain;

    //Night Texttures
    [Header("Night Textures")]
    public Texture nightSky;
    public Texture buildingNight;
    public Texture mountainNight;

    //Morning Texttures
    [Header("Morning Textures")]
    public Texture morningtSky;
    public Texture buildingMorning;
    public Texture mountainMorning;
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(1, 3);
        Debug.Log(random);
        if (random == 1)
         {
             sky.GetComponent<RawImage>().texture = nightSky;
             building.GetComponent<RawImage>().texture = buildingNight;
             mountain.GetComponent<RawImage>().texture = mountainNight;
        }
         else
         {
             sky.GetComponent<RawImage>().texture = morningtSky;
             building.GetComponent<RawImage>().texture = buildingMorning;
             mountain.GetComponent<RawImage>().texture = mountainMorning;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
