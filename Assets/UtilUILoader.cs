using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UtilUILoader : MonoBehaviour
{

    public Image image;
    public RiggedPlayerController player;
    public bool loading = false;
    public float flashDuration = 0.15f;  // Duration of the flash
    public float scaleMultiplier = 1.1f; // The scale multiplier for the flash effect
    private Vector3 originalScale;




    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RiggedPlayerController>();
        originalScale = image.rectTransform.localScale;
        setLoadingFalse();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (loading)
        {
            if (player.utilTimer.isDone())
            {
                setLoadingFalse();
                return;
            }
            float fill = player.utilTimer.getLoading();
            image.fillAmount = fill;
        }

        if (!loading)
        {
            if (!player.utilTimer.isDone())
            {
                setLoadingTrue();
                return;
            }
        }

    }

    void setLoadingTrue()
    {
        loading = true;
        image.color = new Color(0.3f, 0.3f, 0.3f, 0.3f);

    }

    void setLoadingFalse()
    {
        loading = false;
        //image.color = new Color(1, 1, 1, 0.6f);
        StartCoroutine(Flash(new Color(1, 1, 1, 0.7f)));

    }

    IEnumerator Flash(Color finalColor)
    {
        // Set the image color to white
        image.color = Color.white;
        image.rectTransform.localScale = originalScale * scaleMultiplier;

        // Wait for the specified duration
        yield return new WaitForSeconds(flashDuration);

        // Revert the image color to its original state
        image.color = finalColor;
        image.rectTransform.localScale = originalScale;
    }

}

