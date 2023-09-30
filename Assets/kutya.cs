using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kutya : MonoBehaviour
{
    public float kecske;
    //public Shader shader;
    public Image spimage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spimage.material.SetFloat("_Progress", kecske);
    }
}
