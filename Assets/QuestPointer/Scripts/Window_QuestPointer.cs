
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_QuestPointer : MonoBehaviour {

    [SerializeField] private Camera uiCamera;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    public GameObject targetObj;

    private RectTransform pointerRectTransform;
    private Image pointerImage;

    private bool inited = false;

    private void Awake() {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();

    }

    public void init(GameObject target, Color color)
    {
        targetObj = target;
        pointerImage.color = color;
        inited = true;
        uiCamera = GameObject.Find("UI_Camera").GetComponent<Camera>();

        StartCoroutine(PulsePointer(6, 0.6f, 1.2f));

    }


    private void Update() {
        if(targetObj == null && inited)
        {
            Debug.Log("SELFDESTRUCT");
            Destroy(gameObject);
            return;
        }

        float borderSize = -100f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetObj.transform.position);
        bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width - 0 || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height - 0;

        if (isOffScreen) {
            pointerImage.enabled = true;
            RotatePointerTowardsTargetPosition();

            pointerImage.sprite = arrowSprite;
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        } else {
           pointerImage.enabled = false;
        }
    }

    private void RotatePointerTowardsTargetPosition() {
        Vector3 toPosition = targetObj.transform.position;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show(Vector3 targetPosition) {
        gameObject.SetActive(true);
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private IEnumerator PulsePointer(int pulseCount, float pulseDuration, float scaleMultiplier)
    {
        Vector3 originalScale = pointerRectTransform.localScale;

        for (int i = 0; i < pulseCount; i++)
        {
            // Scale up
            float elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(1f, scaleMultiplier, elapsed / (pulseDuration / 2));
                pointerRectTransform.localScale = originalScale * scale;
                yield return null;
            }

            // Scale down
            elapsed = 0f;
            while (elapsed < pulseDuration / 2)
            {
                elapsed += Time.deltaTime;
                float scale = Mathf.Lerp(scaleMultiplier, 1f, elapsed / (pulseDuration / 2));
                pointerRectTransform.localScale = originalScale * scale;
                yield return null;
            }
        }

        // Ensure scale returns to normal
        pointerRectTransform.localScale = originalScale;
    }


}
