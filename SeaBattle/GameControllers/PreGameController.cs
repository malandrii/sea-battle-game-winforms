using System;

namespace SeaBattle
{
    public class PreGameController
    {
        private readonly MainForm _mainForm;
        private HumanPlayer _humanPlayer;
        private Enemy _enemy;
        private int _chosenShipSize;

        public PreGameController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public int ChosenShipSize 
        {
            get => _chosenShipSize;
            set
            {
                if (value >= Field.StartingCoordinate && value <= GameController.ShipSizesAmount)
                    _chosenShipSize = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public bool ChosenShipIsHorizontal { get; set; } = false;

        public bool ShipHasBeenPlaced { get; set; }

        public void StartPreGame()
        {
            const string clearFieldTitle = "Clear Field";
            _mainForm.ContextMenuStrip.Items[Field.StartingCoordinate].Text = clearFieldTitle;
            _mainForm.CreateShipSizeChoosingPanel();
            DeclarePlayers();
            _mainForm.SetButtonsEvents();
            _mainForm.GameController.RefreshEnemyMovesMarking();
        }

        public void DeclarePlayers()
        {
            _humanPlayer = new HumanPlayer(_mainForm);
            _enemy = new Enemy(_mainForm, _humanPlayer);
            _mainForm.GameController.SetPlayers(_humanPlayer, _enemy);
            _humanPlayer.SetEnemy(_enemy);
            _humanPlayer.DeclareField();
        }

        public void StartGame(bool enemyRandomMoves)
        {
            const string restartGameTitle = "Restart Game";
            _mainForm.ContextMenuStrip.Items[Field.StartingCoordinate].Text = restartGameTitle;
            _mainForm.GameController.RefreshEnemyMovesMarking();
            _enemy.RandomMoves = enemyRandomMoves;
            _enemy.DeclareField();
            _enemy.SpawnRandomShips();
        }

        public void ArrangeHumanPlayerShipsRandomly()
        {
            _mainForm.GameController.RestartGame();
            _humanPlayer.SpawnRandomShips();
            _mainForm.UnableShipsArrangePanel();
            FieldController.ColorShips(_humanPlayer.Field, humanField: true);
        }

        public void TryToChangeChosenShipSize()
        {
            if (ShipHasBeenPlaced)
            {
                ResetProperties();
            }
        }

        public void RotateShipHorizontality()
        {
            ChosenShipIsHorizontal = !ChosenShipIsHorizontal;
        }

        public void ResetProperties()
        {
            ChosenShipSize = Field.StartingCoordinate;
            ShipHasBeenPlaced = false;
        }
    }
}
