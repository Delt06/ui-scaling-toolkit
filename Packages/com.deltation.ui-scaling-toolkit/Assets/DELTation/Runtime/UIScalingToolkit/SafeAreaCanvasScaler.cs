using UnityEngine;
using UnityEngine.UI;

namespace DELTation.UIScalingToolkit
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class SafeAreaCanvasScaler : SafeAreaBehaviour
    {
        [SerializeField]
        private Vector2 _referenceResolution = new Vector2(1080, 1920);

        [SerializeField] [Range(0f, 1f)] private float _matchWidthOrHeight = 1f;

        private CanvasScaler _canvasScaler;

        protected override void EnsureInitialized()
        {
            if (_canvasScaler == null)
                _canvasScaler = GetComponent<CanvasScaler>();
        }

        protected override void ApplySafeArea(ref Rect safeArea)
        {
            if (!SafeAreaUtils.CalculateSafeRectAnchors(safeArea, out var anchors)) return;
            
            var (anchorMin, anchorMax) = anchors;
            var inverseScale = anchorMax - anchorMin;
            var resolution = _referenceResolution / inverseScale;
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.matchWidthOrHeight = _matchWidthOrHeight;
            _canvasScaler.referenceResolution = resolution;
        }
    }
}