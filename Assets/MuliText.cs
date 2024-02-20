using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuliText : MonoBehaviour
{
    public string text;
    public float textSize;
    public GameObject muliLetter;
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
            newMuliLetter.resolveTimer = new MuliTimer(Random.Range(3f,7f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
