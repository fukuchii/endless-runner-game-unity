using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float Parallaxfx;

     void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }
     void FixedUpdate()
    {
        float dist = (cam.transform.position.x * Parallaxfx);
        transform.position = new Vector3(startpos+dist,transform.position.y, transform.position.z);
    }
}
