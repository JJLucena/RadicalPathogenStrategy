using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(LineRenderer))]
public class RayDrawer : MonoBehaviour {
    public float displayDuration = 2f;
    public float laserSpeed = 0.0001f;

    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
    }

    public void DrawRay(Transform startPoint, Transform endPoint, Color rayColor) {
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;

        // Initial positions
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);

        StartCoroutine(DrawRayCorrutine(startPoint, endPoint));
    }

    IEnumerator DrawRayCorrutine(Transform startPoint, Transform endPoint) {
        float elapsedTime = 0f;

        while (elapsedTime < displayDuration) {
            // Lerp the end position of the laser line
            float step = elapsedTime / laserSpeed;
            if (startPoint == null || endPoint == null) {
                StopAllCoroutines();
                break;
            }
            Vector3 endPosition = Vector3.Lerp(startPoint.position, endPoint.position, step);
            lineRenderer.SetPosition(1, endPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the line is fully extended
        if (startPoint == null || endPoint == null)
            StopAllCoroutines();
        else
            lineRenderer.SetPosition(1, endPoint.position);

        yield return new WaitForSeconds(displayDuration);

        lineRenderer.enabled = false;
    }
}
