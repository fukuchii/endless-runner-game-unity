using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    bool isJumping;
    public new bool enabled;
    public Rigidbody2D RB;
    public SpriteRenderer spriteRenderer;
   
    void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }
    
            
       
    
    void Start()
    {
  
        animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        isJumping = false;
        RB.freezeRotation = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
        animator.SetBool("jump", false);
        
    }
    void HandleFingerTap(LeanFinger finger)
    {
        if (isJumping == false)
        {
            SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.Jump);
            RB.velocity = new Vector3(0, 22, 0);
            isJumping = true;
            animator.SetBool("jump", true);
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "obstacle")
        {
            SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.collide);
            Color newColor = new Color(0.9f, 0.6f, 0.6f);
            Game_Manager.GameOver();
            spriteRenderer.color = newColor;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.color = Color.white;
            GameControl.toggleShield(-1);
        }
        else if(collision.tag == "mask")
        {
            SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.pickUp);
            GameControl.toggleShield(1);
            Game_Manager.updateShield(1);
        }
        else if (collision.tag == "vaccine")
        {
            SfxManager.sfxInstance.audio.PlayOneShot(SfxManager.sfxInstance.pickUp);
            Game_Manager.updateLives(1);
        }
    }
   
}
