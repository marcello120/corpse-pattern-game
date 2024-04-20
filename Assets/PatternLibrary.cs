using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PatternLibrary : MonoBehaviour
{
    public PatternGrid patternGrid;
    RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        List<int[,]> patterns = PatternStore.Instance.corpsePatterns.Select(c => c.getOtherPatternFrom2DArray()).ToList();;

        for (int i = 0; i < patterns.Count; i++)
        {
            PatternGrid newPatternGrid = Instantiate(patternGrid);
            newPatternGrid.setPattern(patterns[i]);
            newPatternGrid.transform.parent = this.transform;

                newPatternGrid.rectTransform.anchoredPosition = new Vector3((50 * patterns.Count) - 115 *i, 0, 0);
            
        }

        rectTransform.sizeDelta = new Vector2(100*(patterns.Count -1), 200);
}

// Update is called once per frame
    void Update()
    {
        
    }
}
