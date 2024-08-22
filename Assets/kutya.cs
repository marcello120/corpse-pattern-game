using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class kutya : MonoBehaviour
{
    public float kecske;
    public Image spimage;

    public string ressurectSceneName;
    public string quitSceneName;

    public void RessurectButtonPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitButtonPress()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        spimage.material.SetFloat("_Progress", kecske);
    }
}
