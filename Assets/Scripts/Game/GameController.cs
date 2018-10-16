namespace ExperimentFight
{
    public class GameController
    {
        public static GameState gameState = GameState.Normal;

        private static bool isGameStart = false;
        public static bool IsGameStart { get { return isGameStart; } }

        public delegate void _Func();

        public static event _Func OnGameStart;
        public static event _Func OnGameOver;

        public static void GameStart()
        {
            if (isGameStart)
                return;

            isGameStart = true;

            if (OnGameStart != null)
                OnGameStart();
        }

        public static void GameStop()
        {
            if (!isGameStart)
                return;

            isGameStart = false;

            if (OnGameOver != null)
                OnGameOver();
        }
    }
}

