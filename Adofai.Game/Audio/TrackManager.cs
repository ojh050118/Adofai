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

        /// <summary>
        /// 리소스 프로젝트에 있는 트랙리스트입니다. 확장자까지 포함되어 있습니다.
        /// </summary>
        private readonly string[] trackList =
        {
            "Alter Ego.mp3",
            "crystallized.mp3",
            "Light it up.mp3",
            "Maelstrom.mp3",
            "Once again.mp3",
            "R.ogg",
            "Tempest.mp3",
            "Upgrade.mp3"
        };

        private readonly float[] trackBPMList =
        {
            125.0f,
            87.0f,
            87.5f,
            97.5f,
            230.0f,
            180.0f,
            420.0f,
            128.0f
        };

        //private readonly float[] trackOffsetList;

        [Resolved]
        private AudioManager audio { get; set; }

        /// <summary>
        /// 현재 트랙 이름입니다.
        /// </summary>
        public string CurrentTrack;

        /// <summary>
        /// 해당 트랙의 BPM입니다.
        /// </summary>
        public float CurrentBPM;

        /// <summary>
        /// 현재 트랙의 인덱스입니다.
        /// </summary>
        public int CurrentTrackNum;

        private DrawableTrack track;
        public GameState State;
        public float CurrentVolume = 25.0f;
        private float lastVolume;

        /// <summary>
        /// 처음 재생하려는 트랙을 설정하면 BPM과 인덱스를 지정해줍니다.
        /// </summary>
        /// <param name="trackName">처음 재생할 트랙 이름.</param>
        public TrackManager(string trackName = "Alter Ego.mp3")
        {
            foreach (var t in trackList)
            {
                if (t.Equals(trackName))
                {
                    CurrentTrack = trackName;
                    CurrentTrackNum = Array.IndexOf(trackList, t);
                    CurrentBPM = trackBPMList[CurrentTrackNum];
                }
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            track = new DrawableTrack(audio.Tracks.Get($"{CurrentTrack}"));
        }

        // 게임 상태를 따라 트랙을 관리하는것은 잘못 되었습니다. 트랙관리자는 트랙 로드, 트랙의 볼륨설정, 플레이와 정지만을 제공하기 떄문입니다.
        protected override void Update()
        {
            base.Update();

            if (track.HasCompleted && State == GameState.Playing)
                loopTrack();

            if (CurrentVolume != lastVolume)
            {
                Console.WriteLine($"Track Volume: {CurrentVolume}%");
                lastVolume = CurrentVolume;
            }

            if (track.TrackLoaded && State == GameState.Ready)
            {
                track.Stop();
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

        private void loopTrack()
        {
            CurrentTrackNum += 1;

            if (CurrentTrackNum > trackList.Length - 1)
            {
                CurrentTrackNum = 0;
            }

            track = new DrawableTrack(audio.Tracks.Get($"{trackList[CurrentTrackNum]}"));
            CurrentBPM = trackBPMList[CurrentTrackNum];
            CurrentTrack = trackList[CurrentTrackNum];
        }

        /// <summary>
        /// 트랙의 볼륨을 조정합니다. 최대는 1.0(100%) 입니다.
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
