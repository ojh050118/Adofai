using System;
using Adofai.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;

namespace Adofai.Game.Audio
{
    public class TrackManager : Component
    {
        // R.ogg: 180.0, Upgrade.mp3: 128.0, Once again.mp3: 230
        private readonly string[] extension = { "mp3", "ogg" };

        [Resolved]
        private AudioManager audio { get; set; }

        /// <summary>
        /// 재생할 트랙 이름입니다.
        /// </summary>
        public string TrackName;

        /// <summary>
        /// 확장자를 설정하는 배열이 있습니다. 0은 mp3, 1은 ogg입니다.
        /// </summary>
        public int ExtensionIdx;

        /// <summary>
        /// 해당 트랙의 BPM입니다.
        /// </summary>
        public float BPM;

        private DrawableTrack track;
        public GameState State;
        public float CurrentVolume = 25.0f;
        private float lastVolume;

        public TrackManager()
        {
            TrackName = "Upgrade";
            ExtensionIdx = 0;
            BPM = 128.0f;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            track = new DrawableTrack(audio.Tracks.Get($"{TrackName}.{extension[ExtensionIdx]}"));
        }

        protected override void Update()
        {
            base.Update();

            if (CurrentVolume != lastVolume)
            {
                Console.WriteLine($"Volume: {CurrentVolume}%");
                lastVolume = CurrentVolume;
            }

            if (track.TrackLoaded && State == GameState.Playing)
            {
                track.Start();
                track.VolumeTo(CurrentVolume / 100);
            }

            if (track.TrackLoaded && State == GameState.GameOver)
            {
                track.Stop();
                track.Reset();
            }
        }

        /// <summary>
        /// 현재 트랙의 볼륨을 조정합니다. 최대는 1.0(100%) 입니다.
        /// </summary>
        /// <param name="action">볼륨을 높일지 낮출지 결정합니다.</param>
        /// <param name="percent">조정수치입니다. 범위는 0에서 100까지 입니다.</param>
        public void Volume(VolumeAction action, float percent = 10.0f)
        {
            if (action == VolumeAction.Up && CurrentVolume != 100.0f)
            {
                if (CurrentVolume + percent > 100.0f)
                {
                    track.VolumeTo(1.0f, 100);
                    CurrentVolume = 100.0f;
                    return;
                }

                track.VolumeTo((CurrentVolume += percent) / 100, 500, Easing.Out);
            }

            if (action == VolumeAction.Down && CurrentVolume != 0.0f)
            {
                if (CurrentVolume - percent < 0.0f)
                {
                    track.VolumeTo(0.0f, 100);
                    CurrentVolume = 0.0f;
                    return;
                }

                track.VolumeTo((CurrentVolume -= percent) / 100, 500, Easing.In);
            }
        }

        public enum VolumeAction
        {
            Up,
            Down
        }
    }
}
