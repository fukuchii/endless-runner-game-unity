using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerActions : MonoBehaviour
{
    Rigidbody2D RB;
    bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        isJumping = false;
        Debug.Log("jump1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount ==1 && isJumping==false) {
            RB.velocity = new Vector3(0,20,0);
            isJumping = true;
            Debug.Log("jump");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
}
