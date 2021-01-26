using Adofai.Game.Beatmaps.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Adofai.Game.Screens.Play
{
    public class Beatmap : CompositeDrawable
    {
        private FireandIce fireandIce;
        public FillFlowTile Tile;
        private int tapCount;
        public int Count;
        public GameState State = GameState.Ready;
        public const int DEFAULT_JUDGEMENT_ANGLE = 20;
        public float BPM;
        public bool DisableInput;
        public bool FillRequird;
        public int Hit;

        /// <summary>
        /// 각도 판정 오차 범위입니다. 기본값은 20 입니다.
        /// </summary>
        public int JudgementAngle = DEFAULT_JUDGEMENT_ANGLE;

        /// <summary>
        /// 타일과 행성, 두 요소를 합친 하나의 요소입니다. 게의의 대부분을 관리합니다. 타일과 행성을 관리할 수 있습니다.
        /// </summary>
        /// <param name="newBpm">초기 BPM을 설정합니다.</param>
        public Beatmap(float newBpm)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;
            BPM = newBpm;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            fireandIce = new FireandIce(BPM)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            Tile = new FillFlowTile(() => new Tile(), BPM);
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    Tile,
                    fireandIce
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            if (BPM != fireandIce.BPM && BPM != Tile.BPM)
            {
                fireandIce.BPM = BPM;
                Tile.BPM = BPM;
                ChangeGameState(GameState.Ready);
            }
        }

        private void init()
        {
            fireandIce.Init();
            Tile.Init();
        }

        private void start()
        {
            fireandIce.FadeTo(1, 60000 / BPM);
            fireandIce.RotateContainer();
            Tile.AddTile();
            tapCount = 0;
            Count = 1;
            Hit = 0;
        }

        private void stop()
        {
            fireandIce.ClearTransforms(true);
            Tile.ClearTransforms(true);
            ClearTransforms(true);
        }

        public void AddTile()
        {
            if (State == GameState.Ready || State == GameState.GameOver)
                return;

            Tile.AddTile();
            Count += 1;
        }

        /// <summary>
        /// 첫 타일을 제거한뒤 움직입니다. 만약 첫타일의 좌표가 fireandice의 좌표가 같다면 타일을 제거하지 않습니다.
        /// </summary>
        public void DelTile(bool revert = false)
        {
            if (State == GameState.Ready || State == GameState.GameOver)
                return;

            if (fireandIce.Container.Position == Tile.GetTilePosition(0))
                return;

            Tile.DelTile(revert);
            Count -= 1;

            // tapCount가 0 미만일 때, tap 메서드를 사용한다면 의도치않은 동작을 할 수 있습니다.
            if (tapCount != 0)
                tapCount -= 1;
        }

        /// <summary>
        /// fireandIce가 특정각도 범위에 들어오면 원점을 바꿀수 있습니다.
        /// 각도 범위 수정이 가능하여 판정을 느슨하게할지 엄격하게할지 정수(양수)로 조정할 수 있습니다.
        /// 타일에서 벗어나지 않게 좌표를 수정합니다.
        /// </summary>
        public void Tap()
        {
            if (tapCount >= Count)
            {
                ChangeGameState(GameState.GameOver);
                return;
            }

            if (fireandIce.CurrentOrigin == OriginState.Fire && (fireandIce.Container.Rotation >= 360 - JudgementAngle || fireandIce.Container.Rotation < 0 + JudgementAngle))
            {
                tapCount += 1;
                fireandIce.ChangeOrigin();
                fireandIce.Container.Position = Tile.GetTilePosition(tapCount);
                Move();
                AddTile();
                Hit += 1;
            }

            else if (fireandIce.CurrentOrigin == OriginState.Ice && fireandIce.Container.Rotation >= 180 - JudgementAngle && fireandIce.Container.Rotation <= 180 + JudgementAngle)
            {
                tapCount += 1;
                fireandIce.ChangeOrigin();
                fireandIce.Container.Position = Tile.GetTilePosition(tapCount);
                Move();
                AddTile();
                Hit += 1;
            }

            else
                ChangeGameState(GameState.GameOver);

            if (Tile.GetTilePosition(0).X <= -600)
                DelTile();
        }

        public void Move()
        {
            Tile.Move();
            fireandIce.Move();
        }

        public void ChangeGameState(GameState newState)
        {
            if (newState == State)
                return;

            State = newState;

            switch (State)
            {
                case GameState.Ready:
                    init();
                    break;

                case GameState.Playing:
                    start();
                    break;

                case GameState.GameOver:
                    stop();
                    break;
            }
        }

        public void Autoplay(bool state = false, bool directNextPlay = true)
        {
            if (state && State == GameState.Playing)
            {
                DisableInput = true;

                if (fireandIce.CurrentOrigin == OriginState.Fire && fireandIce.Container.Rotation >= 350)
                {
                    Tap();
                }

                else if (fireandIce.CurrentOrigin == OriginState.Ice && fireandIce.Container.Rotation >= 170 && fireandIce.Container.Rotation <= 190)
                {
                    Tap();
                }
            }

            if (State == GameState.Ready && directNextPlay)
            {
                ChangeGameState(GameState.Playing);
                FillRequird = true;
            }
        }
    }
}
