using Adofai.Game.Graphics.UserInterface;
using osu.Framework.Testing;

namespace Adofai.Game.Tests.Visual.UserInterface
{
    public class TestSceneLoadingSpinner : TestScene
    {
        public TestSceneLoadingSpinner()
        {
            LoadingSpinner loading = new LoadingSpinner(true, true);
            loading.Show();
            Add(loading);
        }
    }
}
