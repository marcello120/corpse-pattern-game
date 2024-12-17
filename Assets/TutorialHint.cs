using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialHint : MonoBehaviour
{
    public string mainText;
    public string secondaryText;
    [TextArea(10, 10)]
    public string description;
    public string buttonText;
    public Sprite sprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HintList hintList = GetComponent<HintList>();
            if ( hintList != null)
            {
                description = hintList.hintList[Random.Range(0,hintList.hintList.Count)];
            }
            InfoInterfaceController.Instance.FadeInHintAndZoomAndMove(sprite,transform.position, mainText, secondaryText, description, buttonText);

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InfoInterfaceController.Instance.FadeOut();

        }
    }
}
