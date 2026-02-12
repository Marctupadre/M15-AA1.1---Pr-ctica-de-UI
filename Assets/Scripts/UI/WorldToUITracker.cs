using UnityEngine;
using UnityEngine.UI;

public class WorldToUITracker : MonoBehaviour
{
    public enum IndicatorType
    {
        Ball,
        Target
    }

    [Header("References")]
    public Transform target;
    public Camera cam;
    public RectTransform iconUI;
    public Canvas canvas;
    public Image iconImage;

    [Header("Settings")]
    public IndicatorType type;
    public float screenMargin = 0.05f;
    public LayerMask occlusionMask;

    void Update()
    {
        if (target == null) return;

        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);
        bool isInFront = viewportPos.z > 0;

        if (!isInFront)
            viewportPos *= -1;

        bool isInsideViewport =
            isInFront &&
            viewportPos.x > 0 && viewportPos.x < 1 &&
            viewportPos.y > 0 && viewportPos.y < 1;

        bool isOccluded = false;
        Vector3 dir = target.position - cam.transform.position;
        float distance = dir.magnitude;

        Ray ray = new Ray(cam.transform.position, dir.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, occlusionMask))
        {
            if (hit.transform != target)
                isOccluded = true;
        }
        Vector3 clampedViewport = viewportPos;
        clampedViewport.x = Mathf.Clamp(clampedViewport.x, screenMargin, 1 - screenMargin);
        clampedViewport.y = Mathf.Clamp(clampedViewport.y, screenMargin, 1 - screenMargin);

        Vector3 screenPos = cam.ViewportToScreenPoint(clampedViewport);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out localPoint
        );

        iconUI.localPosition = localPoint;

        Vector2 dirToTarget = new Vector2(
            viewportPos.x - 0.5f,
            viewportPos.y - 0.5f
        );
        float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
        iconUI.rotation = Quaternion.Euler(0, 0, angle);

        float alpha = CalculateAlpha(isInsideViewport, isOccluded);

        Color c = iconImage.color;
        c.a = alpha;
        iconImage.color = c;
    }

    float CalculateAlpha(bool inside, bool occluded)
    {
        if (type == IndicatorType.Ball)
        {
            if (inside && !occluded) return 0f;
            if (inside && occluded) return 0.5f;
            return 1f;
        }
        else
        {
            if (inside && !occluded) return 0.5f;
            if (inside && occluded) return 0.75f;
            return 1f;
        }
    }
}
