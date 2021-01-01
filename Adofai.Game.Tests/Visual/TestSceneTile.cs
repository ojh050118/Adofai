using Adofai.Game.Beatmaps.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual
{
    public class TestSceneTile : TestScene
    {
        private Tile tile;

        [BackgroundDependencyLoader]
        private void load()
        {
            tile = new Tile
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            Add(tile);
            AddStep("Tile", () => tile.Show());
        }
    }
}
