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

        public static bool CalculateSafeRectAnchors(in Rect safeArea, out (Vector2 anchorMin, Vector2 anchorMax) anchors)
        {
            var width = Screen.width;
            var height = Screen.height;

            if (Mathf.Approximately(width, 0f) || Mathf.Approximately(height, 0f))
            {
                anchors = default;
                return false;
            }
            
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= width;
            anchorMin.y /= height;
            anchorMax.x /= width;
            anchorMax.y /= height;
            anchors = (anchorMin, anchorMax);
            return true;
        }
    }
}