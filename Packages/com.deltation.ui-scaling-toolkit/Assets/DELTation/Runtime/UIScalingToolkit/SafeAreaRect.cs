using UnityEngine;

namespace DELTation.UIScalingToolkit
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaRect : SafeAreaBehaviour
    {
        private RectTransform _rectTransform;

        protected override void EnsureInitialized()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
        }

        protected override void ApplySafeArea(ref Rect safeArea)
        {
            if (!SafeAreaUtils.CalculateSafeRectAnchors(safeArea, out var anchors)) return;
            
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = Vector2.zero;

            var (anchorMin, anchorMax) = anchors;
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }
    }
}