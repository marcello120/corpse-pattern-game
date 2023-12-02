using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PowerUpOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    public PowerUp powerUp;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image background;
    public Image icon;
    public Button button;
    public GameObject PowerUpContainer;

    //-----------------------------------

    [SerializeField] private float verticalMoveAmount = 10f;
    [SerializeField] private float moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float scaleAmount = 1.2f;

    private Vector3 startPos;
    private Vector3 startScale;

    public PowerUpSelection powerUpSelection;


    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    private IEnumerator moveCard(bool animStart)
    {
        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;
        while(elapsedTime < moveTime)
        {
            elapsedTime+= Time.unscaledDeltaTime;

            if (animStart)
            {
                endPosition = startPos + new Vector3(0f, verticalMoveAmount, 0f);
                endScale = startScale * scaleAmount;
            }
            else
            {
                endPosition = startPos;
                endScale = startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / moveTime));

            transform.position = lerpedPos;
            transform.localScale= lerpedScale;


            yield return null;

        }



    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        background.color = powerUp.powerUpColor;
        title.text = powerUp.powerUpName;
        description.text = powerUp.powerUpDescription;
        button.name = powerUp.powerUpName;
        button.interactable = true;
        GetComponent<Button>().onClick.AddListener(delegate { choose(); });
        button.onClick.AddListener(delegate { choose(); });

    }

    public void choose()
    {
        button.interactable = false;
        GameObject newPowerUp = Instantiate(PowerUpContainer,new Vector3(0,0,0), Quaternion.identity);
        newPowerUp.GetComponent<PowerUpObject>().init(powerUp);
        powerUpSelection.hide();
    }

 

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.fullyExited)
        {
            eventData.selectedObject = null;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(moveCard(true));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(moveCard(false));
    }
}
