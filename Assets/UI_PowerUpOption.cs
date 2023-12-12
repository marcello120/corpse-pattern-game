using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class UI_PowerUpOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    public PowerUp powerUp;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image background;
    public Image icon;
    public Button button;
    public GameObject powerUpContainer;
    public Image glow;

    //-----------------------------------

    [SerializeField] private float verticalMoveAmount = 10f;
    [SerializeField] private float moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float scaleAmount = 1.2f;

    private Vector3 startPos;
    private Vector3 startScale;

    public PowerUpSelection powerUpSelection;

    private bool chosen = false;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
        chosen = false;
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
        glow.color= powerUp.powerUpColor;
        title.text = powerUp.powerUpName;
        description.text = powerUp.powerUpDescription;
        button.name = powerUp.powerUpName;
        button.interactable = true;
        GetComponent<Button>().onClick.AddListener(delegate { choose(); });
        button.onClick.AddListener(delegate { choose(); });
        icon.sprite = powerUp.sprite;
        chosen = false;
        StartCoroutine(moveCard(false));

    }

    public void choose()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        button.onClick.RemoveAllListeners();
        if (chosen)
        {
            return;
        }
        chosen = true;
        button.interactable = false;
        RiggedPlayerController player = (RiggedPlayerController)GameObject.FindFirstObjectByType(typeof(RiggedPlayerController));

        Vector3 targetPos = GameManager.Instance.getSpawnPoint(player.transform.position, 8, 10);

        GameObject newPowerUp = Instantiate(powerUpContainer, targetPos, Quaternion.identity);
        newPowerUp.GetComponent<PowerUpObject>().init(powerUp);

        powerUpSelection.hide();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(moveCard(true));
        icon.GetComponent<Animator>().SetBool("selected", true);
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(moveCard(false));
        icon.GetComponent<Animator>().SetBool("selected", false);

    }
}
