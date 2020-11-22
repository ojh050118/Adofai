using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Input.Events;
using adofai.Game.Player;

namespace adofai.Game
{
    public class DecoupledFireandIce : CompositeDrawable
    {
        private Fire fire;
        private Ice ice;
        private float size = 50.0f;

        /// <summary>
        /// 현재 원점.
        /// </summary>
        private OriginState origin = OriginState.Fire;

        public DecoupledFireandIce()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
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

            InternalChild = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    fire,
                    ice,
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();

            //Console.WriteLine("Distance: " + getDistance().ToString() + " | " + "Angle: " + getAngle().ToString());
        }
        public void RotateContainer()
        {
            //ice.MoveToOffset(new Vector2(100, Y), 2000, Easing.OutQuint);
            //InternalChild.Loop(b => b.RotateTo(0).RotateTo(360, 10000));
            ice.MoveToOffset(new Vector2(100, Y), 1000, Easing.OutQuint);
        }

        public void TestChangeOrigin()
        {
            changeOrigin();
        }


        private void changeOrigin()
        {
            float width;
            float height;

            float xPos;
            float yPos;
            float radian;

            switch (origin)
            {
                case OriginState.Fire:
                    radian = (float)(getAngle() * Math.PI / 180);
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));

                    InternalChild.RotateTo(90, 1000);

                    width = InternalChild.Width - ice.OriginPosition.X;
                    height = InternalChild.Height - ice.OriginPosition.Y;

                    InternalChild.OriginPosition = new Vector2(width, height);
                    InternalChild.MoveToOffset(new Vector2(xPos, yPos));

                    origin = OriginState.Ice;
                    break;

                case OriginState.Ice:
                    radian = (float)(getAngle() * Math.PI / 180);
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));

                    InternalChild.RotateTo(0, 1000);

                    width = InternalChild.Width / 2;
                    height = InternalChild.Height - ice.OriginPosition.Y;

                    InternalChild.OriginPosition = new Vector2(width, height);
                    InternalChild.MoveToOffset(new Vector2(xPos, -yPos));

                    origin = OriginState.Fire;
                    break;
            }
        }

        /// <summary>
        /// 삼각비를 이용하여 좌표를 바꾸는것을 여기서 구현 했습니다.
        /// 이제 논리를 확인합니다. 
        /// 그 다음, 사분면에 따른 좌표 변경을 만들어야 합니다. 
        /// 그리고 계속 360도를 회전하면서 클릭할때마다 이것이 작동해야 합니다.
        /// </summary>
        public void Test2ChangeOrigin()
        {
            // 다음 두개의 변수들은 컨테이너의 원점을 바꾸어 주는것만 사용됩니다.
            float width;
            float height;

            // 다음 두개의 변수들은 움직이던 오브젝트가 중심으로 바뀔때 즉시 그 위치의 좌표를 구해줍니다.
            float xPos;
            float yPos;
            float radian;

            switch (origin)
            {
                case OriginState.Fire:
                    //InternalChild.RotateTo(180, 1000).Then();
                    //xPos = getDistance() * (float)Math.Cos(InternalChild.Rotation);
                    //yPos = getDistance() * (float)Math.Sin(InternalChild.Rotation);
                    radian = (float)(getAngle() * Math.PI / 180);
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));
                    InternalChild.RotateTo(90, 1000);

                    // ice를 원점으로 설정합니다. 컨테이너의 폭에서 Ice의 반지름을 뺀 값과 높이의 중간값을 설정하면 됩니다.
                    width = InternalChild.Width - ice.OriginPosition.X;
                    height = InternalChild.Height - ice.OriginPosition.Y;

                    Console.WriteLine("현재 중심: Ice, 더한 좌표: (" + xPos.ToString() + ", " + yPos.ToString() + ")");

                    // 원점을 Ice로 바뀌게 합니다.
                    InternalChild.OriginPosition = new Vector2(width, height);

                    // 이것이 의미가 있는지 검증 필요.
                    InternalChild.MoveToOffset(new Vector2(xPos, yPos));

                    origin = OriginState.Ice;
                    break;

                case OriginState.Ice:
                    //InternalChild.RotateTo(360, 1000).Then().RotateTo(0).Then();
                    //xPos = getDistance() * (float)Math.Cos(90 - InternalChild.Rotation);
                    //yPos = getDistance() * (float)Math.Sin(90 - InternalChild.Rotation);
                    radian = (float)(getAngle() * Math.PI / 180);
                    xPos = (float)(getDistance() * Math.Cos(radian));
                    yPos = (float)(getDistance() * Math.Sin(radian));
                    InternalChild.RotateTo(0, 1000);

                    // Fire를 원점으로 합니다. 컨테이너의 중점과 높이의 줌간값을 설정하면 됩니다.
                    width = InternalChild.Width / 2;
                    height = InternalChild.Height - ice.OriginPosition.Y;

                    Console.WriteLine("현재 중심: FIre, 더한 좌표: (" + xPos.ToString() + ", " + yPos.ToString() + ")");
                    Console.WriteLine("Rotation: " + InternalChild.Rotation.ToString());

                    // 원점을 Fire로 바뀌게 합니다.
                    InternalChild.OriginPosition = new Vector2(width, height);

                    InternalChild.MoveToOffset(new Vector2(xPos, -yPos));

                    origin = OriginState.Fire;
                    break;
            }
        }

        public void Init()
        {
            origin = OriginState.Fire;
            InternalChild.Rotation = 0;
            InternalChild.Anchor = Anchor.Centre;
            InternalChild.Origin = Anchor.Centre;
            InternalChild.MoveTo(new Vector2(0, 0));
            InternalChild.ClearTransforms();
        }

        private float getDistance()
        {
            double width = 0;
            double height = InternalChild.OriginPosition.Y - 25;

            if (origin == OriginState.Ice)
                width = InternalChild.OriginPosition.X - (InternalChild.Width / 2);

            if (origin == OriginState.Fire)
                width = InternalChild.Width - ice.OriginPosition.X - InternalChild.OriginPosition.X;

            float distance = (float)Math.Sqrt((width * width) + (height * height));

            return distance;
        }

        public void GetAngle()
        {
            Console.WriteLine("Angle: " + getAngle());
        }

        private float getAngle()
        {
            double radian = InternalChild.Rotation * Math.PI / 180;
            radian = double.Parse(String.Format("{0:F5}", radian));

            double width = Math.Abs((getDistance() * Math.Cos(radian))); // 폭 모서리 * Cos(radian)
            double height = Math.Abs((getDistance() * Math.Sin(radian))); // 높이 모서리 * Sin(radian)

            float angle = (float)Math.Atan2(height, width);

            return (angle * 180) / (float)Math.PI;
        }
    }
}
