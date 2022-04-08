using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSound : MonoBehaviour
{
   public static BGSound BGInstance;
    public AudioSource Audio;
    private void Awake()
    {
        if(BGInstance != null && BGInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        BGInstance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
}
