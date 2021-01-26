using System;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;

namespace Adofai.Game.Audio
{
    public class SampleManager : Component
    {
        [Resolved]
        private AudioManager audio { get; set; }

        private DrawableSample sample;
        private readonly string sampleConfig;
        private float lastVolume;
        public float CurrentVolume = 25.0f;

        /// <summary>
        /// 초기 샘플을 지정합니다.
        /// </summary>
        /// <param name="name"></param>
        public SampleManager(string name = "hit.wav")
        {
            sampleConfig = name;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            sample = new DrawableSample(audio.Samples.Get($"{sampleConfig}"));
        }

        protected override void Update()
        {
            base.Update();

            if (CurrentVolume != lastVolume)
            {
                Console.WriteLine($"Sample Volume: {CurrentVolume}%");
                lastVolume = CurrentVolume;
            }
        }

        public void Play()
        {
            sample.VolumeTo(CurrentVolume / 100);
            sample.Play();
        }

        /// <summary>
        /// 샘플의 볼륨을 조정합니다. 최대는 1.0(100%) 입니다.
        /// </summary>
        /// <param name="action">볼륨을 높일지 낮출지 결정합니다.</param>
        /// <param name="percent">조정수치입니다. 범위는 0에서 100까지 입니다.</param>
        public void Volume(VolumeAction action, float percent = 10.0f)
        {
            if (action == VolumeAction.Up && CurrentVolume != 100.0f)
            {
                if (CurrentVolume + percent > 100.0f)
                {
                    sample.VolumeTo(1.0f, 100);
                    CurrentVolume = 100.0f;
                    return;
                }

                sample.VolumeTo((CurrentVolume += percent) / 100, 500, Easing.Out);
            }

            if (action == VolumeAction.Down && CurrentVolume != 0.0f)
            {
                if (CurrentVolume - percent < 0.0f)
                {
                    sample.VolumeTo(0.0f, 100);
                    CurrentVolume = 0.0f;
                    return;
                }

                sample.VolumeTo((CurrentVolume -= percent) / 100, 500, Easing.In);
            }
        }

        public enum VolumeAction
        {
            Up,
            Down
        }
    }
}
