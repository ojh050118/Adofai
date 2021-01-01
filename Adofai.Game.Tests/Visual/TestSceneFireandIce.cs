using Adofai.Game.Screens.Play;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual
{
    public class TestSceneFireandIce : TestScene
    {
        private FireandIce fireandIce;

        [BackgroundDependencyLoader]
        private void load()
        {
            fireandIce = new FireandIce(128.0f)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Add(fireandIce);
            AddStep("Init", () => fireandIce.Init());
            AddStep("Show", () => fireandIce.Show());
            AddStep("Rotate Container", () => fireandIce.RotateContainer());
            AddStep("Change Origin", () => fireandIce.ChangeOrigin());
        }
    }
}
