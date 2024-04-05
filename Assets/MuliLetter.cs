using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MuliLetter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;

    public float switchTime;
    public float resolveCount;

    public bool settled = false;

    public Sprite[] sprites;


    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Glifs");
        text.enabled = false;
        image.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        StartCoroutine(doTheThing());

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable(){
        //StartCoroutine(doTheThing());
    }

    IEnumerator doTheThing()
    {
            for (int i = 0; i < resolveCount; i++)
            {
                image.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
                yield return new WaitForSecondsRealtime(switchTime);
            }
            settled = true;
            image.enabled = false;
            text.enabled = true;
    }
}
