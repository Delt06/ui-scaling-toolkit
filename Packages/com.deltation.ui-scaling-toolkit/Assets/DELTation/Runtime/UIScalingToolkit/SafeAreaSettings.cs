using System;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.UIScalingToolkit
{
    public sealed class SafeAreaSettings : MonoBehaviour
    {
        public enum PaddingMode
        {
            ScreenSpace,
            CanvasSpace,
        }

        [SerializeField]
        private bool _updateEveryFrame;
        [SerializeField]
        private bool _ignoreBottom;

        [SerializeField] private PaddingMode _topPaddingMode = PaddingMode.CanvasSpace;
        [SerializeField]
        [Min(0f)] private float _topPadding;

        [SerializeField] private PaddingMode _bottomPaddingMode = PaddingMode.CanvasSpace;
        [SerializeField]
        [Min(0f)] private float _bottomPadding;

        [CanBeNull]
        private Canvas _canvas;

        public bool UpdateEveryFrame => _updateEveryFrame;

        public bool IgnoreBottom => _ignoreBottom;

        public PaddingMode TopPaddingMode
        {
            get => _topPaddingMode;
            set => _topPaddingMode = value;
        }

        public float TopPaddingInScreenSpace => ConvertPaddingToScreenSpace(_topPadding, _topPaddingMode);

        public PaddingMode BottomPaddingMode
        {
            get => _bottomPaddingMode;
            set => _bottomPaddingMode = value;
        }

        public float BottomPaddingInScreenSpace => ConvertPaddingToScreenSpace(_bottomPadding, _bottomPaddingMode);

        private float ConvertPaddingToScreenSpace(float padding, PaddingMode paddingMode) =>
            paddingMode switch
            {
                PaddingMode.CanvasSpace => padding * GetCanvasScaleFactor(),
                PaddingMode.ScreenSpace => padding,
                _ => throw new ArgumentOutOfRangeException(nameof(paddingMode), paddingMode, null),
            };

        // Should be called before safe area initialization.
        public void SetTopPadding(float topPadding)
        {
            _topPadding = topPadding;
        }

        // Should be called before safe area initialization.
        public void SetBottomPadding(float bottomPadding)
        {
            _bottomPadding = bottomPadding;
        }

        private float GetCanvasScaleFactor()
        {
            var canvas = GetCanvas();
            return canvas != null ? canvas.scaleFactor : 1f;
        }

        [CanBeNull]
        private Canvas GetCanvas()
        {
            if (_canvas)
                return _canvas;
            _canvas = transform.root.GetComponentInChildren<Canvas>();
            return _canvas;
        }
    }
}