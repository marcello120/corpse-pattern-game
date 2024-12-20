using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StaticData;

public class StoryItemUi : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image background;
    public Button button;

    public Color activeColor;
    public Color purchasedColor;
    public Color poorColor;

    public bool active = false;

    public StoryPojo storyPojo;
    public string key;

    public StoryViewController storyViewController;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { choose(); });
    }

    public void choose()
    {
        storyViewController.setText(storyPojo.storyText, key, storyPojo.unlocked);
    }

    public void LoadScene()
    {
            StaticData.story = storyPojo;
            StaticData.story.targetScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene("Story");   
    }

    public void setChosen()
    {
        if (background == null)
        {
            return;
        }
        background.color = activeColor;
        active = true;
    }

    public void setNotChosen()
    {
        active = false;
        if (background == null)
        {
            return;
        }
        if (storyPojo.unlocked)
        {
            background.color = purchasedColor;

        }
        else
        {
            background.color = poorColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(StoryViewController storyViewController,StoryPojo storyPojo, string key)
    {
        this.storyViewController = storyViewController;
        this.storyPojo = storyPojo;
        this.key = key;
        title.text = key;
        description.text = key;
        if (storyPojo.unlocked)
        {
            background.color = purchasedColor;
            button.onClick.AddListener(delegate { LoadScene(); });

        }
        else
        {
            background.color = poorColor;
            button.gameObject.SetActive(false);
        }

    }
}
