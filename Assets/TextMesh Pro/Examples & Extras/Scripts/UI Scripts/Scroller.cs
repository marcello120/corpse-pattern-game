using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private RawImage rawImg;
    [SerializeField] private float _x, _y;

    // Update is called once per frame
    void Update()
    {
        rawImg.uvRect = new Rect(rawImg.uvRect.position + new Vector2(_x, _y) * Time.unscaledDeltaTime, rawImg.uvRect.size);
    }
}
