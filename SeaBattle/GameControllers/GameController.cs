namespace SeaBattle
{
    public class GameController
    {
        public const int ShipSizesAmount = 4;
        private readonly MainForm _mainForm;
        private HumanPlayer _humanPlayer;
        private Enemy _enemy;

        public GameController(MainForm mainForm) 
        {
            _mainForm = mainForm;
        }

        public HumanPlayer HumanPlayer { get => _humanPlayer; }

        public int ComputerMoveSpeed { get => _mainForm.ComputerMoveSpeedSelectedIndex; }

        public bool ComputerTurn { get => _mainForm.ComputerTurnLabelVisible; }

        public void RefreshPlayers(HumanPlayer humanPlayer, Enemy enemy)
        {
            _humanPlayer = humanPlayer;
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
            FieldController.UnableField(_enemy.Field);
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
            FieldController.DisposeField(_humanPlayer.Field);
            FieldController.DisposeField(_enemy.Field);
        }
    }
}
