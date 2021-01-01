using Adofai.Game.Beatmaps.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual
{
    public class TestSceneFillFlowTile : TestScene
    {
        private FillFlowTile tile;

        [BackgroundDependencyLoader]
        private void load()
        {
            tile = new FillFlowTile(() => new Tile(), 128.0f);

            Add(tile);
            AddStep("FillFlow Tile", () => tile.Show());
            AddStep("Add Tile", () => tile.AddTile());
            AddStep("Remove Tile", () => tile.DelTile());
            AddStep("Init", () => tile.Init());
        }
    }
}
