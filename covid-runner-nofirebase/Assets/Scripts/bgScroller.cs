using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bgScroller : MonoBehaviour
{
    [SerializeField] public RawImage img;
    [SerializeField] public float x, y;
    public void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x,0) * Time.deltaTime, img.uvRect.size);
    }
}
