using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Framework.Graphics;

namespace adofai.Game.Tests.Visual
{
    public class TestSceneFAI2 : TestScene
    {
        private DecoupledFireandIce fireandIce;

        [BackgroundDependencyLoader]
        private void load()
        {
            fireandIce = new DecoupledFireandIce
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            Add(fireandIce);
            AddStep("Init", () => fireandIce.Init());
            AddStep("Rotate Container", () => fireandIce.RotateContainer());
            AddStep("Loop Rotate", () => fireandIce.Loop(b => b.RotateTo(0).RotateTo(360, 2000)));
            AddStep("Change Origin", () => fireandIce.Test2ChangeOrigin());
            AddStep("Get Angle", () => fireandIce.GetAngle());
            //AddStep("Rotate + 45", () => fireandIce.Rotat);
        }
    }
}
