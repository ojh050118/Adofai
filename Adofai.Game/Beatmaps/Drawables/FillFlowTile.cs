using System;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Adofai.Game.Beatmaps.Drawables
{
    public class FillFlowTile : CompositeDrawable
    {
        private readonly Func<Tile> createTile;
        private float beforeTileX;
        private float offset;
        public float BPM;

        public FillFlowTile(Func<Tile> createTile, float newBpm)
        {
            this.createTile = createTile;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
            BPM = newBpm;
        }

        /// <summary>
        /// 타일을 오른쪽 방향으로 생성합니다. 원점이 중앙이라는것에 기반.
        /// </summary>
        public void AddTile()
        {
            if (InternalChildren.Count == 0)
            {
                offset = 0.0f;
                beforeTileX = 0.0f;
            }

            AddInternal(createTile());

            var lastChild = InternalChildren.Last();
            var width = (float)Math.Truncate(lastChild.DrawWidth * lastChild.Scale.X);

            lastChild.Alpha = 0;
            lastChild.Position = new Vector2(beforeTileX, lastChild.Position.Y);
            lastChild.MoveTo(new Vector2(offset, lastChild.Position.Y), 60000 / BPM, Easing.OutQuint);
            lastChild.FadeTo(1, (60000 / BPM) / 2);

            offset += width;
            beforeTileX = offset - width;
        }

        /// <summary>
        /// InternalChildren의 첫 인덱스를 제거합니다.
        /// 파라미터를 true로 설정하면 마지막 인덱스를 제거합니다.
        /// </summary>
        public void DelTile(bool revert = false)
        {
            if (InternalChildren.Count == 0)
                return;

            if (revert)
            {
                RemoveInternal(InternalChildren.Last());
                return;
            }

            RemoveInternal(InternalChildren.First());
        }

        /// <summary>
        /// InternalChildren[idx]의 좌표를 Vector2 형식으로 반환합니다.
        /// </summary>
        /// <param name="idx">범위안에서 인덱스 값을 넣어주세요.</param>
        /// <returns>좌표</returns>
        public Vector2 GetTilePosition(int idx)
        {
            int x = (int)InternalChildren[idx].X;
            int y = (int)InternalChildren[idx].Y;

            return new Vector2(x, y);
        }

        public void Move()
        {
            int width = 0;

            foreach (var childTile in InternalChildren)
            {
                // Math.Truncate는 소수점을 버립니다. Math.Ceiling를 사용하게 된다면 1씩 증가하여 서서히 타일이 밀립니다.
                width = (int)Math.Truncate(childTile.Width * (int)childTile.Scale.X);
                childTile.MoveToOffset(new Vector2(-width, childTile.Y), (60000 / BPM), Easing.OutQuint).Then();
            }

            offset -= width;
            beforeTileX = offset - width;
        }

        public void Init()
        {
            // 내머리로는 이게 한계
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                RemoveInternal(InternalChildren.First());

                for (int e = 0; e < InternalChildren.Count; i++)
                    RemoveInternal(InternalChildren.First());
            }

            offset = 0.0f;
            beforeTileX = 0.0f;
            Position = new Vector2(0, 0);
        }
    }
}
