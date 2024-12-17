using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoInterfaceController : MonoBehaviour
{
    public static InfoInterfaceController Instance;

    public Image leftImage;
    public Image rightImage;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI descText;
    public Button button;

    CanvasGroup canvasGroup;
    CinemachineVirtualCamera cinemachineCamera;
    MidPoint midpoint;


    private void Awake()
    {
        // Ensure that there is only one instance of the class
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();
        cinemachineCamera = GameObject.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>();
        midpoint = GameObject.Find("MidPoint").GetComponent<MidPoint>();
        Hide();
        canvasGroup.alpha = 0;
    }

    public void FadeInHintAndZoomAndMove(Vector3? targetPos, string? textMain = "", string? textSecond = "", string? description = "", string? buttonText = "")
    {
        FadeInHintAndZoomAndMove(null, targetPos, textMain, textSecond, description, buttonText);
    }

    public void FadeInHintAndZoomAndMove(Sprite? sprite, Vector3? targetPos,string? textMain = "", string? textSecond = "", string? description = "", string? buttonText = "")
    {
        FadeInHint(sprite,textMain, textSecond, description);
        StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 1.5f, 0.75f));
        if (targetPos.HasValue)
        {
            StartCoroutine(CinemachineCameraMove(targetPos.Value, 1f, false));
        }
    }


    public void FadeInHint(Sprite? sprite, string? textMain = "", string? textSecond = "", string? description = "", string? buttonText = "")
    {
        mainText.text = textMain;
        secondText.text = textSecond;
        descText.text = description;
        descText.enabled = true;
        mainText.enabled = true;
        secondText.enabled = true;
        rightImage.enabled = true;
         if (sprite != null)
        {
            leftImage.enabled = true;
            leftImage.sprite = sprite;
        }
        if (buttonText != "")
        {
            button.gameObject.active = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        }
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 1f));
    }


    public void FadeOut()
    {

        StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 2f, 1f));
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, 1f, () => {
            Hide();
        }));
        RiggedPlayerController player = (RiggedPlayerController)FindFirstObjectByType(typeof(RiggedPlayerController));
        StartCoroutine(CinemachineCameraMove(player.transform, 1f,true));

    }

    public void Show()
    {
        leftImage.enabled = true;
        rightImage.enabled = true;
        mainText.enabled = true;
        secondText.enabled = true;
        descText.enabled = true;
        button.gameObject.active = true;
    }


    public void Hide()
    {
        leftImage.enabled = false;
        rightImage.enabled = false;
        mainText.enabled = false;
        secondText.enabled = false;
        descText.enabled = false;
        button.gameObject.active = false;
    }


    // Helper method for fading CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (onComplete != null)
        {
            onComplete();
        }
    }

    private IEnumerator CinemachineCameraZoom(CinemachineVirtualCamera virtualCamera, float targetZoom, float duration)
    {
        if (virtualCamera == null) yield break;

        float startZoom = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }

    public IEnumerator CinemachineCameraMove(Transform target, float duration, bool release)
    {
        Vector3 initialPosition = midpoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(initialPosition, target.position, elapsedTime / duration);
            midpoint.setPos(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        midpoint.setPos(target.position);

        if (release)
        {
            midpoint.release();
        }
    }

    public IEnumerator CinemachineCameraMove(Vector3 targetPos, float duration, bool release)
    {
        Vector3 initialPosition = midpoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPos, elapsedTime / duration);
            midpoint.setPos(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        midpoint.setPos(targetPos);

        if (release)
        {
            midpoint.release();
        }
    }
}

