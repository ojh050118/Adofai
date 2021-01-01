using Adofai.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual.Screens
{
    public class TestSceneGameplayScreen : TestScene
    {
        private GameplayScreen game;
        private SpriteText textbox;

        [BackgroundDependencyLoader]
        private void load()
        {
            ScreenStack screen = new ScreenStack();
            game = new GameplayScreen();
            textbox = new SpriteText
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Text = $"State: {game.CurrentState}"
            };

            screen.Push(game);
            Add(screen);
            Add(textbox);
        }

        protected override void Update()
        {
            base.Update();

            textbox.Text = $"State: {game.CurrentState}";
        }
    }
}
