namespace SeaBattle
{
    public class GameController
    {
        private readonly MainForm _mainForm;
        private HumanPlayer _humanPlayer;
        private Enemy _enemy;

        public GameController(MainForm mainForm) 
        {
            _mainForm = mainForm;
        }

        public ShipButton[,] HumanPlayerField { get => _humanPlayer.Field; }

        public HumanPlayer HumanPlayer { get => _humanPlayer; }

        public void RefreshEnemyMovesMarking(bool markEnemyMoves)
        {
            _enemy.MarkMoves = markEnemyMoves;
        }

        public void ArrangeHumanPlayerShipsRandomly()
        {
            RestartGame();
            _humanPlayer.SpawnRandomShips();
            _mainForm.UnableShipsArrangePanel();
            FieldController.ColorHumanPlayerShips(_humanPlayer.Field);
        }

        public void StartGame()
        {
            const string clearFieldButtonText = "Clear Field";
            _mainForm.ContextMenuStrip.Items[FieldController.StartingCoordinate].Text = clearFieldButtonText;
            _mainForm.CreateShipSizeChoosingPanel();
            DeclarePlayers();
            _mainForm.SetButtonsEvents();
            RefreshEnemyMovesMarking(_mainForm.EnemyMarkMoves);
        }

        public void DeclarePlayers()
        {
            _humanPlayer = new HumanPlayer(_mainForm);
            _enemy = new Enemy(_mainForm, _humanPlayer);
            _humanPlayer.SetEnemy(_enemy);
            _humanPlayer.Field = new ShipButton[FieldController.FieldSize, FieldController.FieldSize];
            _humanPlayer.DeclareField();
        }

        public void EnemyStartGame(bool enemyRandomMoves)
        {
            RefreshEnemyMovesMarking(_mainForm.EnemyMarkMoves);
            _enemy.RandomMoves = enemyRandomMoves;
            _enemy.DeclareField();
            _enemy.SpawnRandomShips();
        }

        public void FinishGame()
        {
            _mainForm.HumanPlayerStatus.SetFinishGameStatus(humanWon: _enemy.ShipPartsAlive == 0);
            FieldController.RevealEnemyShips(_enemy.Field);
        }

        public void RestartGame()
        {
            ResetGame();
            StartGame();
        }

        private void ResetGame()
        {
            _mainForm.ResetControls();
            FieldController.ClearField(_humanPlayer.Field);
            FieldController.ClearField(_enemy.Field);
        }
    }
}
