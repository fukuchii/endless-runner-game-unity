using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
   
    public float speed;
    public float start;
    public float end;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        if (transform.position.x <= end)
        {
            if(gameObject.tag == "obstacle"||gameObject.tag == "vaccine"||gameObject.tag == "mask")
            {
                Destroy(gameObject);
            }
            else {
                transform.position = new Vector2(start, transform.position.y);
            }

           
        }
    }
   

}
