using System;
using Adofai.Game.Audio;
using Adofai.Game.Beatmaps.Drawables;
using Adofai.Game.Screens.Play;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace Adofai.Game.Screens
{
    public class GameplayScreen : AdofaiScreen
    {
        private readonly DrawSizePreservingFillContainer gameScreen = new DrawSizePreservingFillContainer();
        private Beatmap beatmap;
        private readonly Tile tile = new Tile();
        private Vector2 lastSize;
        public GameState CurrentState;
        public TrackManager Track;
        public SampleManager Sample;
        private int lastHit;
        private int lastTrackNum;

        [BackgroundDependencyLoader]
        private void load()
        {
            Track = new TrackManager("Tempest.mp3");
            Sample = new SampleManager();
            beatmap = new Beatmap(Track.CurrentBPM)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            gameScreen.Children = new Drawable[]
            {
                beatmap,
                Track,
                Sample
            };

            Track.CurrentVolume = 25.0f;

            AddInternal(gameScreen);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            beatmap.JudgementAngle = 30;
            beatmap.State = GameState.Ready;
            beatmap.ChangeGameState(CurrentState = GameState.Ready);
        }

        protected override void Update()
        {
            base.Update();

            CurrentState = beatmap.State;
            Track.State = CurrentState;

            if (lastTrackNum != Track.CurrentTrackNum)
            {
                beatmap.BPM = Track.CurrentBPM;

                lastTrackNum = Track.CurrentTrackNum;
            }

            if (!gameScreen.DrawSize.Equals(lastSize) && beatmap.State == GameState.Playing)
            {
                fillTile();
                lastSize = gameScreen.DrawSize;
            }

            beatmap.Autoplay(true);

            if (beatmap.FillRequird)
            {
                fillTile();
                beatmap.FillRequird = false;
            }

            if (lastHit != beatmap.Hit)
            {
                Sample.Play();
                lastHit = beatmap.Hit;
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.D || e.Key == Key.F || e.Key == Key.J || e.Key == Key.K && !beatmap.DisableInput)
            {
                if (beatmap.State == GameState.Ready)
                {
                    beatmap.ChangeGameState(CurrentState = GameState.Playing);
                    fillTile();
                    return base.OnKeyDown(e);
                }

                if (beatmap.State == GameState.GameOver)
                {
                    beatmap.ChangeGameState(CurrentState = GameState.Ready);
                    beatmap.ChangeGameState(CurrentState = GameState.Playing);
                    fillTile();
                    return base.OnKeyDown(e);
                }

                beatmap.Tap();
            }

            if (e.Key == Key.Up)
                Track.Volume(TrackManager.VolumeAction.Up, 5);

            if (e.Key == Key.Down)
                Track.Volume(TrackManager.VolumeAction.Down, 5);

            if (e.Key == Key.Left)
                Sample.Volume(SampleManager.VolumeAction.Down, 5);

            if (e.Key == Key.Right)
                Sample.Volume(SampleManager.VolumeAction.Up, 5);

            return base.OnKeyDown(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left || e.Button == MouseButton.Right && !beatmap.DisableInput)
            {
                if (beatmap.State == GameState.Ready)
                {
                    beatmap.ChangeGameState(CurrentState = GameState.Playing);
                    fillTile();
                    return base.OnMouseDown(e);
                }

                if (beatmap.State == GameState.GameOver)
                {
                    beatmap.ChangeGameState(CurrentState = GameState.Ready);
                    beatmap.ChangeGameState(CurrentState = GameState.Playing);
                    fillTile();
                    return base.OnMouseDown(e);
                }

                beatmap.Tap();
            }

            return base.OnMouseDown(e);
        }

        private void fillTile()
        {
            var tileNum = (int)Math.Round(gameScreen.DrawWidth / (tile.TileSize * 2)) / 2 - 1;

            if (beatmap.Count != tileNum)
            {
                while (beatmap.Count > tileNum)
                    beatmap.DelTile(true);

                while (beatmap.Count < tileNum)
                    beatmap.AddTile();
            }
        }
    }
}
