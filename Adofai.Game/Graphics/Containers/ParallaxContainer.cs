﻿using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Utils;
using osuTK;

namespace Adofai.Game.Graphics.Containers
{
    public class ParallaxContainer : Container, IRequireHighFrequencyMousePosition
    {
        public const float DEFAULT_PARALLAX_AMOUNT = 0.02f;
        public float ParallaxAmount = DEFAULT_PARALLAX_AMOUNT;

        public ParallaxContainer()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        private readonly Container content;
        private InputManager input;

        protected override Container<Drawable> Content => content;

        [BackgroundDependencyLoader]
        private void load()
        {
            content.MoveTo(Vector2.Zero, firstUpdate ? 0 : 1000, Easing.OutQuint);
            content.Scale = new Vector2(1 + Math.Abs(ParallaxAmount));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            input = GetContainingInputManager();
        }

        private bool firstUpdate = true;

        protected override void Update()
        {
            base.Update();

            Vector2 offset = (input.CurrentState.Mouse == null ? Vector2.Zero : ToLocalSpace(input.CurrentState.Mouse.Position) - DrawSize / 2) * ParallaxAmount;

            const float parallax_duration = 100;

            double elapsed = Math.Clamp(Clock.ElapsedFrameTime, 0, parallax_duration);

            content.Position = Interpolation.ValueAt(elapsed, content.Position, offset, 0, parallax_duration, Easing.OutQuint);
            content.Scale = Interpolation.ValueAt(elapsed, content.Scale, new Vector2(1 + Math.Abs(ParallaxAmount)), 0, 1000, Easing.OutQuint);

            firstUpdate = false;
        }
    }
}
