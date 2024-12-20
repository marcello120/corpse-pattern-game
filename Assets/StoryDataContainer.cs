using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDataContainer : MonoBehaviour
{
    // Start is called before the first frame update

    public float storyTime;
    [TextArea(10, 10)]
    public string storyText;
    public AudioClip storyClip;
    public string targetScene;
}
