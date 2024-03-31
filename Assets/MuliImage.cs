using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MuliImage : MonoBehaviour
{
        public Image targetImage;
        public Image image;

        public float switchTime;
        public float resolveCount;

        public bool settled = false;

        public Sprite[] sprites;


        // Start is called before the first frame update
        void Start()
        {
            sprites = Resources.LoadAll<Sprite>("Glifs");
            targetImage.enabled = false;
            image.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
            StartCoroutine(doTheThing());

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
            targetImage.enabled = true;
        }
    }

