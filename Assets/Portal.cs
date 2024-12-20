using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Portal : MonoBehaviour
{

    public string scene_name;
    public Material transitionMaterial;
    public float transitionTime = 1f;
    public float transitionSpeed = 1f;
    private string materialName = "_MaskAmount";

    void Start()
    {
        
    }

    private void load()
    {
        //StoryDataContainer storyData = GetComponent<StoryDataContainer>();
        if (PlayerPrefs.GetFloat("Story " + scene_name, 0) != 1)
        {
            if(StaticData.storyDictionary.ContainsKey(scene_name + " Part 1"))
            {
                StaticData.StoryPojo story = StaticData.storyDictionary[scene_name + " Part 1"];
                PlayerPrefs.SetFloat("Story " + scene_name, 1);
                StaticData.story = story;
                StaticData.story.targetScene = scene_name;
                StartCoroutine(LoadLevel("Story"));
            }
            else
            {
                StartCoroutine(LoadLevel(scene_name));
            }
        }
        else
        {
            StartCoroutine(LoadLevel(scene_name));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            load();
        }
    }

    IEnumerator LoadLevel(string scene_name)
    {
        float currentTime = 0f;

        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;

            // Calculate normalized time (t) for Lerp
            float t = currentTime / transitionTime;

            // Smoothly transition the material property
            transitionMaterial.SetFloat(materialName, Mathf.Lerp(-1f, 1f, t));

            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        transitionMaterial.SetFloat(materialName, 1f);

        // Wait before loading the scene (optional)
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(scene_name);
    }

}
