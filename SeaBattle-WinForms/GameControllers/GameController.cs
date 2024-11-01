namespace SeaBattle
{
    public class GameController
    {
        public const int ShipSizesAmount = 4;
        private readonly MainForm _mainForm;
        private Enemy _enemy;

        public GameController(MainForm mainForm) => _mainForm = mainForm;

        public HumanPlayer HumanPlayer { get; private set; }

        public bool EnemyTurn => _mainForm.ComputerTurnLabelVisible;

        public void SetPlayers(HumanPlayer humanPlayer, Enemy enemy)
        {
            HumanPlayer = humanPlayer;
            _enemy = enemy;
        }

        public void RefreshEnemyMovesMarking()
        {
            _enemy.MarkMoves = _mainForm.MarkComputerMovesToolStripMenuItemChecked;
        }

        public void FinishGame()
        {
            _mainForm.HumanPlayerStatus.SetFinishGameStatus(humanWon: _enemy.ShipPartsAlive == 0);
            FieldController.ColorShips(_enemy.Field, humanField: false);
            _enemy.Field.Unable();
            _mainForm.SetRestartButtonVisibile();
        }

        public void RestartGame()
        {
            ResetGame();
            _mainForm.PreGameController.StartPreGame();
        }

        private void ResetGame()
        {
            _mainForm.ResetControls();
            HumanPlayer.Field.Dispose();
            _enemy.Field.Dispose();
        }
    }
}
