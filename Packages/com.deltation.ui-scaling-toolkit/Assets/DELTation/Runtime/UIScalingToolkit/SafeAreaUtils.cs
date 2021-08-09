using UnityEngine;

namespace DELTation.UIScalingToolkit
{
    public static class SafeAreaUtils
    {
        public static Rect CalculateAdjustedSafeArea(bool ignoreBottom = false, float topPadding = 0f,
            float bottomPadding = 0f)
        {
            var safeArea = Screen.safeArea;

            if (ignoreBottom)
            {
                var min = safeArea.min;
                min.y = 0f;
                safeArea.min = min;
            }

            if (!Mathf.Approximately(topPadding, 0f))
            {
                var max = safeArea.max;
                max.y -= topPadding;
                safeArea.max = max;
            }

            if (!Mathf.Approximately(bottomPadding, 0f))
            {
                var min = safeArea.min;
                min.y += bottomPadding;
                safeArea.min = min;
            }

            return safeArea;
        }

        public static (Vector2 anchorMin, Vector2 anchorMax) CalculateSafeRectAnchors(in Rect safeArea)
        {
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            return (anchorMin, anchorMax);
        }
    }
}