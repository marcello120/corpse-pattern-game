using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryViewController : MonoBehaviour
{
    public GameObject itemPrefab;
    public TextMeshProUGUI textDisplay;
    public Image right;
    public GameObject container;


    public List<StoryItemUi> items;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        container.transform.parent.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        textDisplay.gameObject.SetActive(false);
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void DontShow() {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, 1f, () => {
            Hide();
        }));
    }

    public void Show()
    {
        canvasGroup.alpha = 0f;
        container.transform.parent.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        textDisplay.gameObject.SetActive(true);

        //destroy all children of container
        if (container != null)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
                items.Clear();
            }
        }

        //StaticData.storyDictionary
        //iterate dictionary
        foreach (KeyValuePair<string, StaticData.StoryPojo> entry in StaticData.storyDictionary)
        {
            StoryItemUi item = Instantiate(itemPrefab, container.transform).GetComponent<StoryItemUi>();
            StaticData.StoryPojo pojo = entry.Value;
            pojo.unlocked = PlayerPrefs.GetFloat("Story " + entry.Key,0)!=0;
            if(entry.Key == "Intro")
            {
                pojo.unlocked = true;
            }
            item.Init(this,pojo,entry.Key);
            items.Add(item);
        }

        container.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1f;
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 1f));

    }

    public void setText(string text, string key, bool unlocked)
    {
        if (unlocked)
        {
            textDisplay.text = text;
        }
        else
        {
            textDisplay.text = "Story not yet discovered";
        }
        foreach (StoryItemUi item in items)
        {
            item.setNotChosen();
            if(key == item.key)
            {
                item.setChosen();
            }
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (onComplete != null)
        {
            onComplete();
        }
    }
}
