using Adofai.Game.Screens;
using Adofai.Game.Screens.Play;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual
{
    public class TestSceneBeatmap : TestScene
    {
        private Beatmap beatmap;

        [BackgroundDependencyLoader]
        private void load()
        {
            beatmap = new Beatmap(128.0f)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Add(beatmap);
            AddLabel("GameState");
            AddStep("GameState.Ready", () => beatmap.ChangeGameState(GameState.Ready));
            AddStep("GameState.Playing", () => beatmap.ChangeGameState(GameState.Playing));
            AddStep("Add Tile", () => beatmap.AddTile());
            AddWaitStep("Waiting for 1000ms", 5);
            AddStep("Change Origin", () => beatmap.Tap());
            AddStep("Move Camera", () => beatmap.Move());
            AddStep("Remove Tile", () => beatmap.DelTile());
            AddStep("GameState.GameOver", () => beatmap.ChangeGameState(GameState.GameOver));
        }
    }
}
