using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuliText : MonoBehaviour
{
    public string text;
    public float textSize;
    public GameObject muliLetter;
    public float swithTime;
    public float minResolveTime;
    public float maxResolveTime;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < text.Length; i++)
        {
            Debug.Log(text[i]);
            GameObject newLetter = Instantiate(muliLetter, Vector3.zero, Quaternion.identity);
            newLetter.transform.parent = transform;
            newLetter.GetComponent<RectTransform>().anchoredPosition = Vector3.zero + new Vector3(textSize*i/2,0);
            newLetter.transform.localScale = Vector3.one;
            MuliLetter newMuliLetter = newLetter.GetComponent<MuliLetter>();
            newMuliLetter.text.SetText(text[i].ToString());
            newMuliLetter.switchTime = swithTime;
            newMuliLetter.resolveCount = Random.Range(minResolveTime, maxResolveTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
