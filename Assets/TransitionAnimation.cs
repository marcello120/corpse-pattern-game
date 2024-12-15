using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class TransitionAnimation : MonoBehaviour
{
    public Material transitionMaterial;
    public float transitionTime = 1f;
    public float transitionSpeed = 1f;
    private string materialName = "_MaskAmount";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        float currentTime = 0f;

        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;

            // Calculate normalized time (t) for Lerp
            float t = currentTime / transitionTime;

            // Smoothly transition the material property
            transitionMaterial.SetFloat(materialName, Mathf.Lerp(1f, -1f, t));

            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        transitionMaterial.SetFloat(materialName, -1f);
    }
}
