using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameInputController : MonoBehaviour
{
    public TMP_InputField input;
    public Button button;


    // Start is called before the first frame update
    void Start()
    {
        button.interactable = false;
    }

    public void UpdateTextBox(TMP_InputField textbox)
    {
        if (textbox.text.Length > 1)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;

        }
    }

    public void ButtonPressed()
    {
        PlayerPrefs.SetString("Username", input.text);
        SceneManager.LoadScene(StaticData.sceneNames[GameManager.Level.ENDLESS]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
