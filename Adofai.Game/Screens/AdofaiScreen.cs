﻿using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace Adofai.Game.Screens
{
    public class AdofaiScreen : Screen
    {
        public AdofaiScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        OnExit();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            this.FadeInFromZero(200, Easing.Out);
        }

        public override bool OnExiting(IScreen next)
        {
            this.FadeOut(200, Easing.Out);
            this.ScaleTo(0.9f, 200, Easing.Out);
            return base.OnExiting(next);
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);
            this.ScaleTo(1.1f, 200, Easing.Out);
            this.FadeOut(200, Easing.Out);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            this.ScaleTo(1, 200, Easing.Out);
            this.FadeIn(200, Easing.Out);
        }

        protected virtual void OnExit() => this.Exit();
    }
}
