using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public DaniText daniText;
    public TextMeshProUGUI skipText;

    [Header("debug")]
    public float storyTime;
    [TextArea(10, 10)]
    public string storyText;
    public AudioClip storyClip;
    public string targetScene;

    public bool primed;

    // Start is called before the first frame update
    void Start()
    {
        if (StaticData.story != null)
        {
            storyTime = StaticData.story.storyTime;
            storyText = StaticData.story.storyText;
            storyClip = StaticData.story.storyClip;
            targetScene = StaticData.story.targetScene;
            if (storyText != null)
            {
                daniText.setText(storyText);
            }
            if(storyClip == null)
            {
                GetComponent<AudioSource>().Stop();
            }
            else
            {
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = storyClip;
                GetComponent<AudioSource>().Play();
            }
            StartCoroutine(goToScene());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("HELLO");
            if (primed)
            {
                SceneManager.LoadScene(targetScene);
            }
            else
            {
                primed = true;
                skipText.enabled = true;

            }
        }
    }

    private IEnumerator goToScene()
    {
        if(targetScene != null)
        {
            yield return new WaitForSeconds(storyTime);
            primed = true;
            skipText.enabled = true;
            yield return new WaitForSeconds(storyTime/2);
            SceneManager.LoadScene(targetScene);
        }
    }
}
