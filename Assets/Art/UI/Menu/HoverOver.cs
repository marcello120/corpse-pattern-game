using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI theText;
    public Shader shader;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (theText == null)
        {
            
        }
        else
        {
            theText.color = Color.red; //Or however you do your color
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (theText == null)
        {

        }
        else
        {
            theText.color = Color.yellow; //Or however you do your color
        }
    }
}
