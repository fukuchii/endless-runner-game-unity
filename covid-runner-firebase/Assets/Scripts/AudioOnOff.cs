using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioOnOff : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Sprite spriteA;
    [SerializeField] Sprite spriteB;
    public void MusicToggle()
    {
        button.image.sprite = button.image.sprite == spriteA ? spriteB : spriteA;
        if (BGSound.BGInstance.Audio.isPlaying)
        {
            BGSound.BGInstance.Audio.Pause();

        }
        else
        {
            BGSound.BGInstance.Audio.Play();
           
        }
    }
}
