﻿using UnityEngine;
using ViewSystem;
using ViewSystem.Animation;
using ViewSystem.Attributes;
using ViewSystem.Base;

namespace Features.UI.View
{
    [AttributeViewType(ViewType.Window)]
    public class StandbyWindow : BaseAnimatedView<FadeViewAnimation>
    {
        [Header("Animation")] [SerializeField] private CanvasGroup canvasGroup;
        public override ViewLayer ViewLayer => ViewLayer.Window;

        protected override void InitializeAnimation()
        {
            Animation = new FastFadeViewAnimation(canvasGroup);
        }
    }
}