using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Adofai.Game.Player
{
    public class Fire : CompositeDrawable
    {
        public Fire()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Masking = true;
            BorderColour = Color4.White;
            BorderThickness = 2.5f;
            Size = new Vector2(50.0f);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Masking = true,
                BorderColour = Color4.White,
                BorderThickness = 2.5f,
                Children = new Drawable[]
                {
                    new Circle
                    {
                        Colour = Color4.Red,
                        Size = new Vector2(50.0f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }
                }
            };
        }
    }
}
