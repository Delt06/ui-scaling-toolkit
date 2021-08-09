using JetBrains.Annotations;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DELTation.UIScalingToolkit
{
    public abstract class SafeAreaBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool _overrideSettings;
#if ODIN_INSPECTOR
        [ShowIf(nameof(_overrideSettings))]
#endif
        [SerializeField]
        private bool _updateEveryFrame;
#if ODIN_INSPECTOR
        [ShowIf(nameof(_overrideSettings))]
#endif
        [SerializeField]
        private bool _ignoreBottom;

        [SerializeField]
        private bool _ignoreTopPadding;

        [SerializeField]
        private bool _ignoreBottomPadding;

        private Rect? _lastSafeArea;
        [CanBeNull]
        private SafeAreaSettings _settings;

        protected virtual void Awake()
        {
            InvalidateCache();
            EnsureInitialized();
            ResizeToSafeArea();
        }

        protected virtual void Update()
        {
            if (ShouldUpdateEveryFrame() || Application.isEditor) ResizeToSafeArea();
        }

        protected virtual void OnValidate()
        {
            InvalidateCache();
        }

        private bool ShouldUpdateEveryFrame()
        {
            if (_overrideSettings)
                return _updateEveryFrame;
            return TryGetSettings(out var settings) && settings.UpdateEveryFrame;
        }

        protected abstract void EnsureInitialized();

        private void ResizeToSafeArea()
        {
            var shouldIgnoreBottom = ShouldIgnoreBottom();
            var topPadding = GetTopPadding();
            var bottomPadding = GetBottomPadding();
            var safeArea = SafeAreaUtils.CalculateAdjustedSafeArea(shouldIgnoreBottom, topPadding, bottomPadding);
            if (safeArea == _lastSafeArea) return;

#if UNITY_EDITOR
            EnsureInitialized();
#endif
            ApplySafeArea(ref safeArea);
            _lastSafeArea = safeArea;
        }

        private bool ShouldIgnoreBottom()
        {
            if (_overrideSettings)
                return _ignoreBottom;
            return TryGetSettings(out var settings) && settings.IgnoreBottom;
        }

        private float GetTopPadding() =>
            !_ignoreTopPadding && TryGetSettings(out var settings) ? settings.TopPaddingInScreenSpace : 0f;

        private float GetBottomPadding() =>
            !_ignoreBottomPadding && TryGetSettings(out var settings) ? settings.BottomPaddingInScreenSpace : 0f;

        private bool TryGetSettings(out SafeAreaSettings settings)
        {
            if (_settings != null)
            {
                settings = _settings;
                return true;
            }

            _settings = settings = transform.root.GetComponentInChildren<SafeAreaSettings>();
            return settings != null;
        }

        protected abstract void ApplySafeArea(ref Rect safeArea);

        protected virtual void InvalidateCache()
        {
            _lastSafeArea = null;
        }
    }
}