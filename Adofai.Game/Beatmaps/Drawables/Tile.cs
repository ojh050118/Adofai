using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Adofai.Game.Beatmaps.Drawables
{
    public class Tile : CompositeDrawable
    {
        public float TileSize = 50.0f;

        public Tile()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
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
                CornerRadius = 2.5f,
                BorderColour = Color4.White,
                BorderThickness = 2.5f,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Size = new Vector2(TileSize * 2, TileSize),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Gray,
                    },
                },
            };
        }
    }
}
