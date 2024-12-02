using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PatternElement : MonoBehaviour
{

    public int corpseNum;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaceWithDelay());
    }

    public IEnumerator PlaceWithDelay()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.setGridCellFromWorldPos(corpseNum, transform.position);
        yield return new WaitForSeconds(0.1f);

    }

}
