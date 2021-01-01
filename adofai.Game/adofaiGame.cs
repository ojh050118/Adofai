using Adofai.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Adofai.Game
{
    public class AdofaiGame : AdofaiGameBase
    {
        private ScreenStack screenStack;

        //private MainMenuScreen mainMenu;
        private GameplayScreen game;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
            //mainMenu = new MainMenuScreen();
            game = new GameplayScreen();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            screenStack.Push(game);
        }
    }
}
