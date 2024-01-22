using System.Collections;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Camera mainCam;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public float maxGrapplingDistance = 5f; // Set your maximum grappling distance here
    public float grapplingSpeed = 5f; // Set your grappling speed here
    public float pauseDuration = 1f;

    private Coroutine grapplingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 mousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);

            // Limiting the distance
            float distance = Vector2.Distance(transform.position, mousePos);
            if (distance > maxGrapplingDistance)
            {
                mousePos = (mousePos - (Vector2)transform.position).normalized * maxGrapplingDistance + (Vector2)transform.position;
            }

            if (grapplingCoroutine != null)
            {
                StopCoroutine(grapplingCoroutine);
            }

            grapplingCoroutine = StartCoroutine(GrapplingAnimation(mousePos));
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (grapplingCoroutine != null)
            {
                StopCoroutine(grapplingCoroutine);
            }

            grapplingCoroutine = StartCoroutine(ShrinkLine());
        }

        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    IEnumerator GrapplingAnimation(Vector2 targetPos)
    {
        lineRenderer.enabled = true;

        float t = 0f;
        Vector2 startPos = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * grapplingSpeed;
            lineRenderer.SetPosition(0, Vector2.Lerp(startPos, targetPos, t));
            yield return null;
        }

        distanceJoint.connectedAnchor = targetPos;
        distanceJoint.enabled = true;

        yield return new WaitForSeconds(pauseDuration);
    }

    IEnumerator ShrinkLine()
    {
        float t = 0f;
        Vector2 endPos = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * grapplingSpeed;
            lineRenderer.SetPosition(0, Vector2.Lerp(distanceJoint.connectedAnchor, endPos, t));
            yield return null;
        }

        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }
}