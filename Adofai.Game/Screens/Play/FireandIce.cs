using System;
using Adofai.Game.Player;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Adofai.Game.Screens.Play
{
    public class FireandIce : CompositeDrawable
    {
        private Fire fire;
        private Ice ice;
        private const float size = 50.0f;
        private readonly float bpm;
        public const int DISTANCE = 100;
        public Drawable Container;

        /// <summary>
        /// 현재 원점.
        /// </summary>
        public OriginState CurrentOrigin = OriginState.Fire;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newBpm">현재트랙의 BPM</param>
        public FireandIce(float newBpm)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Masking = true;
            AutoSizeAxes = Axes.Both;
            bpm = newBpm;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            fire = new Fire
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(size),
            };
            ice = new Ice
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(size),
            };

            Container = InternalChild = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                Children = new Drawable[]
                {
                    ice,
                    fire
                }
            };
        }

        public void RotateContainer()
        {
            // 360도를 회전합니다. 하나의 타일은 1비트이고 360도는 2비트이기에 60000 / BPM으로 계산하고 2를 곱합니다.
            InternalChild.Loop(b => b.RotateTo(0).RotateTo(360, (60000 / bpm) * 2));
            // 두 행성(fire, ice)사이의 거리.
            ice.MoveToX(DISTANCE, (60000 / bpm), Easing.OutQuint);
        }

        public void ChangeOrigin()
        {
            float xPos;
            float yPos;
            float radian;

            switch (CurrentOrigin)
            {
                case OriginState.Fire:
                    radian = MathHelper.DegreesToRadians(getAngle());
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));

                    float width = InternalChild.Width - ice.OriginPosition.X;
                    float height = InternalChild.Height - ice.OriginPosition.Y;

                    InternalChild.OriginPosition = new Vector2(width, height);

                    if (InternalChild.Rotation > 0 && InternalChild.Rotation < 90)
                        InternalChild.MoveToOffset(new Vector2(xPos, yPos));

                    else if (InternalChild.Rotation > 90 && InternalChild.Rotation < 180)
                        InternalChild.MoveToOffset(new Vector2(-xPos, yPos));

                    else if (InternalChild.Rotation > 180 && InternalChild.Rotation < 270)
                        InternalChild.MoveToOffset(new Vector2(-xPos, -yPos));

                    else if (InternalChild.Rotation > 270 && InternalChild.Rotation < 360)
                        InternalChild.MoveToOffset(new Vector2(xPos, -yPos));

                    else
                    {
                        if (InternalChild.Rotation == 0 || InternalChild.Rotation == 360)
                            InternalChild.MoveToOffset(new Vector2(xPos, 0));

                        else if (InternalChild.Rotation == 90)
                            InternalChild.MoveToOffset(new Vector2(0, -yPos));

                        else if (InternalChild.Rotation == 180)
                            InternalChild.MoveToOffset(new Vector2(-xPos, 0));

                        else if (InternalChild.Rotation == 270)
                            InternalChild.MoveToOffset(new Vector2(0, yPos));
                    }

                    CurrentOrigin = OriginState.Ice;
                    break;

                case OriginState.Ice:
                    radian = MathHelper.DegreesToRadians(getAngle());
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));

                    InternalChild.Origin = Anchor.Centre;

                    if (InternalChild.Rotation > 0 && InternalChild.Rotation < 90)
                        InternalChild.MoveToOffset(new Vector2(-xPos, -yPos));

                    else if (InternalChild.Rotation > 90 && InternalChild.Rotation < 180)
                        InternalChild.MoveToOffset(new Vector2(xPos, -yPos));

                    else if (InternalChild.Rotation > 180 && InternalChild.Rotation < 270)
                        InternalChild.MoveToOffset(new Vector2(xPos, yPos));

                    else if (InternalChild.Rotation > 270 && InternalChild.Rotation < 360)
                        InternalChild.MoveToOffset(new Vector2(-xPos, yPos));

                    else
                    {
                        if (InternalChild.Rotation == 0 || InternalChild.Rotation == 360)
                            InternalChild.MoveToOffset(new Vector2(xPos, 0));

                        else if (InternalChild.Rotation == 90)
                            InternalChild.MoveToOffset(new Vector2(0, -yPos));

                        else if (InternalChild.Rotation == 180)
                            InternalChild.MoveToOffset(new Vector2(-xPos, 0));

                        else if (InternalChild.Rotation == 270)
                            InternalChild.MoveToOffset(new Vector2(0, yPos));
                    }

                    CurrentOrigin = OriginState.Fire;
                    break;
            }
        }

        public void Move()
        {
            InternalChild.MoveToOffset(new Vector2(-size * 2, 0), (60000 / bpm), Easing.OutQuint);
        }

        public void Init()
        {
            Hide();
            CurrentOrigin = OriginState.Fire;
            InternalChild.Rotation = 0;
            InternalChild.Anchor = Anchor.Centre;
            InternalChild.Origin = Anchor.Centre;
            InternalChild.MoveTo(new Vector2(0, 0));
            ice.MoveToX(0);
            InternalChild.ClearTransforms();
        }

        private float getDistance()
        {
            double width = 0;
            double height = InternalChild.OriginPosition.Y - (size / 2);

            if (CurrentOrigin == OriginState.Ice)
                width = InternalChild.OriginPosition.X - (InternalChild.Width / 2);

            if (CurrentOrigin == OriginState.Fire)
                width = InternalChild.Width - ice.OriginPosition.X - InternalChild.OriginPosition.X;

            float distance = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));

            return distance;
        }

        private float getAngle()
        {
            double radian = MathHelper.DegreesToRadians(InternalChild.Rotation);
            double width = Math.Abs(getDistance() * Math.Cos(radian));
            double height = Math.Abs(getDistance() * Math.Sin(radian));
            radian = (float)Math.Atan2(height, width);

            return (float)MathHelper.RadiansToDegrees(radian);
        }
    }
}
