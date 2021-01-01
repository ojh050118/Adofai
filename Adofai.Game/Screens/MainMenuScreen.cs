using Adofai.Game.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Adofai.Game.Screens
{
    public class MainMenuScreen : AdofaiScreen
    {
        private LoadingSpinner load1;
        private LoadingSpinner load2;
        private SpriteText text;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "A Dance of Fire and Ice",
                    Font = FontUsage.Default.With(size: 50),
                    Y = -100,
                    Shadow = true,
                    ShadowOffset = new Vector2(0, 0.15f)
                },
                new FillFlowContainer<LoadingSpinner>
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(20, 0),
                    Children = new[]
                    {
                        load1 = new LoadingSpinner(true, true),
                        load2 = new LoadingSpinner(true, true),
                    }
                }
            });

            load1.Size = new Vector2(70);
            load2.Size = new Vector2(70);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            load1.Show();
            load2.Show();

            text.Loop(b => b.ScaleTo(1.25f, 0, Easing.None).ScaleTo(1.0f, 1000f, Easing.OutQuint).Then());
        }
    }
}
