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

    public bool win = false;

    public void RessurectButtonPress()
    {
        string scene_name = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.GetFloat("Story " + scene_name + " Part 2", 0) == 0.5f)
        {
            if (StaticData.storyDictionary.ContainsKey(scene_name + " Part 2"))
            {
                StaticData.StoryPojo story = StaticData.storyDictionary[scene_name + " Part 2"];
                PlayerPrefs.SetFloat("Story " + scene_name + " Part 2", 1);
                StaticData.story = story;
                StaticData.story.targetScene = scene_name;
                SceneManager.LoadScene("Story");
                return;

            }
        }
        if (PlayerPrefs.GetFloat("Story " + scene_name + " Part 3", 0) != 1 && win)
        {
            if (StaticData.storyDictionary.ContainsKey(scene_name + " Part 3"))
            {
                StaticData.StoryPojo story = StaticData.storyDictionary[scene_name + " Part 3"];
                PlayerPrefs.SetFloat("Story " + scene_name + " Part 3", 1);
                StaticData.story = story;
                StaticData.story.targetScene = scene_name;
                SceneManager.LoadScene("Story");
                return;

            }
        }


        SceneManager.LoadScene(scene_name);


    }
    public void QuitButtonPress()
    {
        Time.timeScale = 1;
        string scene_name = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.GetFloat("Story " + scene_name + " Part 2", 0) == 0.5f)
        {
            if (StaticData.storyDictionary.ContainsKey(scene_name + " Part 2"))
            {
                StaticData.StoryPojo story = StaticData.storyDictionary[scene_name + " Part 2"];
                PlayerPrefs.SetFloat("Story " + scene_name + " Part 2", 1);
                StaticData.story = story;
                StaticData.story.targetScene = quitSceneName;
                SceneManager.LoadScene("Story");
                return;

            }
        }
        if (PlayerPrefs.GetFloat("Story " + scene_name + " Part 3", 0) != 1 && win)
        {
            if (StaticData.storyDictionary.ContainsKey(scene_name + " Part 3"))
            {
                StaticData.StoryPojo story = StaticData.storyDictionary[scene_name + " Part 3"];
                PlayerPrefs.SetFloat("Story " + scene_name + " Part 3", 1);
                StaticData.story = story;
                StaticData.story.targetScene = quitSceneName;
                SceneManager.LoadScene("Story");
                return;
            }
        }
        SceneManager.LoadScene(quitSceneName);


    }

    // Update is called once per frame
    void Update()
    {
        spimage.material.SetFloat("_Progress", kecske);
    }
}
